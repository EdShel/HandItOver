using HandItOver.BackEnd.BLL.Models.Delivery;
using HandItOver.BackEnd.BLL.Models.Mailbox;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using HandItOver.BackEnd.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class MailboxService
    {
        private readonly UserRepository userRepository;

        private readonly MailboxRepository mailboxRepository;

        private readonly DeliveryRepository deliveryRepository;

        private readonly RentRepository rentRepository;

        private readonly ITokenService tokenService;

        private readonly IRefreshTokenFactory refreshTokenFactory;

        public MailboxService(
            UserRepository userRepository,
            MailboxRepository mailboxRepository,
            ITokenService tokenService,
            IRefreshTokenFactory refreshTokenFactory,
            DeliveryRepository deliveryRepository,
            RentRepository rentRepository)
        {
            this.userRepository = userRepository;
            this.mailboxRepository = mailboxRepository;
            this.tokenService = tokenService;
            this.refreshTokenFactory = refreshTokenFactory;
            this.deliveryRepository = deliveryRepository;
            this.rentRepository = rentRepository;
        }

        public async Task<MailboxAuthResult> AuthorizeMailboxAsync(MailboxAuthRequest request)
        {
            AppUser owner = await this.userRepository.FindByIdOrNullAsync(request.OwnerId)
                ?? throw new NotFoundException("Mailbox owner");

            Mailbox? alreadyRegistered = await this.mailboxRepository.FindByPhysicalIdOrNullAsync(request.PhysicalId);
            if (alreadyRegistered != null)
            {
                throw new RecordAlreadyExistsException("Registered mailbox");
            }

            Mailbox newMailbox = new Mailbox
            {
                OwnerId = owner.Id,
                PhysicalId = request.PhysicalId,
                Size = request.Size,
                Address = request.Address
            };
            this.mailboxRepository.CreateMailbox(newMailbox);

            IRefreshToken refreshToken = this.refreshTokenFactory.GenerateRefreshToken();
            RefreshToken refreshTokenRecord = new RefreshToken
            {
                Value = refreshToken.Value,
                Expires = refreshToken.Expires
            };
            this.userRepository.CreateRefreshToken(owner, refreshTokenRecord);
            await this.userRepository.SaveChangesAsync();

            string authToken = this.tokenService.GenerateAuthToken(GetClaimsForMailbox(newMailbox));
            return new MailboxAuthResult(
                MailboxId: newMailbox.Id,
                AuthToken: authToken,
                RefreshToken: refreshToken.Value
            );
        }

        private static ClaimsIdentity GetClaimsForMailbox(Mailbox mailbox)
        {
            var claims = new Claim[]
            {
                new Claim(AuthConstants.Claims.ID, mailbox.Id),
                new Claim(AuthConstants.Claims.OWNER_ID, mailbox.OwnerId),
                new Claim(AuthConstants.Claims.ROLE, AuthConstants.Roles.MAILBOX)
            };
            return new ClaimsIdentity(claims);
        }


        public async Task RequestOpening(string mailboxId)
        {
            Delivery currentDelivery = await this.deliveryRepository.GetCurrentDeliveryOrNullAsync(mailboxId)
                ?? throw new NotFoundException("Delivery");

            currentDelivery.Mailbox.IsOpen = true;
            this.mailboxRepository.UpdateMailbox(currentDelivery.Mailbox);

            currentDelivery.Taken = DateTime.UtcNow;
            this.deliveryRepository.UpdateDelivery(currentDelivery);
            await this.deliveryRepository.SaveChangesAsync();
        }

        public async Task<MailboxStatus> GetMailboxStatus(string mailboxId)
        {
            Mailbox mailbox = await this.mailboxRepository.FindByIdOrNullAsync(mailboxId)
                ?? throw new NotFoundException("Mailbox");
            Delivery? currentDelivery = await this.deliveryRepository.GetCurrentDeliveryOrNullAsync(mailboxId);
            if (!mailbox.IsOpen)
            {
                if (currentDelivery == null)
                {
                    MailboxRent? currentRent = await this.rentRepository.FindForTimeOrNull(mailboxId, DateTime.UtcNow);
                    if (currentRent != null)
                    {
                        mailbox.IsOpen = true;
                        this.mailboxRepository.UpdateMailbox(mailbox);
                        await this.mailboxRepository.SaveChangesAsync();
                    }
                }
                else if (currentDelivery.TerminalTime != null
                        && DateTime.UtcNow >= currentDelivery.TerminalTime)
                {
                    mailbox.IsOpen = true;
                    this.mailboxRepository.UpdateMailbox(mailbox);
                    await this.mailboxRepository.SaveChangesAsync();
                }
            }
            return new MailboxStatus(
                MailboxId: mailbox.Id,
                IsOpen: mailbox.IsOpen
            );
        }

        // TODO: create DTO
        public async Task<IEnumerable<Mailbox>> GetOwnedMailboxes(string userId)
        {
            return await this.mailboxRepository.FindByOwnerAsync(userId);
        }
    }
}
