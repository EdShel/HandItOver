﻿using HandItOver.BackEnd.BLL.Models.Mailbox;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using HandItOver.BackEnd.Infrastructure.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Entities
{
    public class MailboxService
    {
        private readonly UserRepository userRepository;

        private readonly MailboxRepository mailboxRepository;

        private readonly ITokenService tokenService;

        private readonly IRefreshTokenFactory refreshTokenFactory;

        public MailboxService(
            UserRepository userRepository,
            MailboxRepository mailboxRepository,
            ITokenService tokenService,
            IRefreshTokenFactory refreshTokenFactory)
        {
            this.userRepository = userRepository;
            this.mailboxRepository = mailboxRepository;
            this.tokenService = tokenService;
            this.refreshTokenFactory = refreshTokenFactory;
        }

        public async Task<MailboxAuthResult> AuthorizeMailbox(MailboxAuthRequest request)
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
                Size = request.Size
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
    }
}
