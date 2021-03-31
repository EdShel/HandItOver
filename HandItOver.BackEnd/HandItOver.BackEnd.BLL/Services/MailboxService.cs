using AutoMapper;
using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.Mailbox;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using HandItOver.BackEnd.Infrastructure.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class MailboxService : IMailboxService
    {
        private readonly UserRepository userRepository;

        private readonly MailboxRepository mailboxRepository;

        private readonly ITokenService tokenService;

        private readonly IRefreshTokenFactory refreshTokenFactory;

        private readonly IMapper mapper;

        public MailboxService(
            UserRepository userRepository,
            MailboxRepository mailboxRepository,
            ITokenService tokenService,
            IRefreshTokenFactory refreshTokenFactory,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mailboxRepository = mailboxRepository;
            this.tokenService = tokenService;
            this.refreshTokenFactory = refreshTokenFactory;
            this.mapper = mapper;
        }

        public async Task<MailboxAuthResult> AuthorizeMailboxAsync(MailboxAuthRequest request)
        {
            AppUser owner = await this.userRepository.FindByIdOrNullAsync(request.OwnerId)
                ?? throw new NotFoundException("Mailbox owner");

            Mailbox? mailbox = await this.mailboxRepository.FindByPhysicalIdOrNullAsync(request.PhysicalId);
            if (mailbox == null)
            {
                mailbox = new Mailbox
                {
                    OwnerId = owner.Id,
                    PhysicalId = request.PhysicalId,
                    Size = request.Size,
                    Address = request.Address,
                    IsOpen = true
                };
                this.mailboxRepository.CreateMailbox(mailbox);
            }
			else
			{
				mailbox.Address = request.Address;
				mailbox.Size = request.Size;
				this.mailboxRepository.UpdateMailbox(mailbox);
			}
            IRefreshToken refreshToken = this.refreshTokenFactory.GenerateRefreshToken();
            RefreshToken refreshTokenRecord = new RefreshToken
            {
                Value = refreshToken.Value,
                Expires = refreshToken.Expires
            };
            this.userRepository.CreateRefreshToken(owner, refreshTokenRecord);
            await this.userRepository.SaveChangesAsync();

            string authToken = this.tokenService.GenerateAuthToken(GetClaimsForMailbox(mailbox));
            return new MailboxAuthResult(
                MailboxId: mailbox.Id,
                AuthToken: authToken,
                RefreshToken: refreshToken.Value
            );
        }

        private static ClaimsIdentity GetClaimsForMailbox(Mailbox mailbox)
        {
            var claims = new Claim[]
            {
                new Claim(AuthConstants.Claims.ID, mailbox.OwnerId),
                new Claim(AuthConstants.Claims.MAILBOX_ID, mailbox.Id),
                new Claim(AuthConstants.Claims.ROLE, AuthConstants.Roles.MAILBOX)
            };
            return new ClaimsIdentity(claims);
        }

        public async Task<IEnumerable<MailboxViewResult>> GetOwnedMailboxesAsync(string userId)
        {
            var mailboxes = await this.mailboxRepository.FindByOwnerAsync(userId);
            return this.mapper.Map<IEnumerable<MailboxViewResult>>(mailboxes);
        }

        public async Task<MailboxViewResult> GetMailboxAsync(string mailboxId)
        {
            var mailbox = await this.mailboxRepository.FindByIdOrNullAsync(mailboxId)
                   ?? throw new NotFoundException("Mailbox");
            return this.mapper.Map<MailboxViewResult>(mailbox);
        }

        public async Task<MailboxViewResult> EditMailboxAsync(string mailboxId, MailboxEditRequest changes)
        {
            var mailbox = await this.mailboxRepository.FindByIdOrNullAsync(mailboxId)
                ?? throw new NotFoundException("Mailbox");
            this.mapper.Map(changes, mailbox);
            this.mailboxRepository.UpdateMailbox(mailbox);
            await this.mailboxRepository.SaveChangesAsync();
            return this.mapper.Map<MailboxViewResult>(mailbox);
        }
    }
}
