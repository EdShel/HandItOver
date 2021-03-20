using AutoMapper;
using HandItOver.BackEnd.BLL.Models.MailboxAccessControl;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Entities.Auth;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services
{
    public class MailboxAccessControlService
    {
        private readonly MailboxGroupRepository mailboxGroupRepository;

        private readonly WhitelistJoinTokenRepository whitelistJoinTokenRepository;

        private readonly UserRepository userRepository;

        private readonly IMapper mapper;

        public MailboxAccessControlService(
            MailboxGroupRepository mailboxGroupRepository,
            UserRepository userRepository,
            WhitelistJoinTokenRepository whitelistJoinTokenRepository,
            IMapper mapper)
        {
            this.mailboxGroupRepository = mailboxGroupRepository;
            this.userRepository = userRepository;
            this.whitelistJoinTokenRepository = whitelistJoinTokenRepository;
            this.mapper = mapper;
        }

        public async Task<WhitelistInfo> GetMailboxWhitelist(string groupId)
        {
            MailboxGroup mailboxGroup = await this.mailboxGroupRepository.GetWhitelistByIdAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");

            return new WhitelistInfo(
                MailboxGroupId: mailboxGroup.GroupId,
                Entries: mailboxGroup.Whitelisted.Select(
                    u => new WhitelistEntry(
                        Email: u.Email,
                        Id: u.Id
                    )
                )
            );
        }

        public async Task AddUserToWhitelistAsync(string groupId, string userEmail)
        {
            AppUser user = await this.userRepository.FindByEmailOrNullAsync(userEmail)
                ?? throw new NotFoundException("User");
            MailboxGroup mailboxGroup = await this.mailboxGroupRepository.GetWhitelistByIdAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");

            if (mailboxGroup.OwnerId == user.Id)
            {
                throw new OperationException("Owner of the group can't be whitelisted.");
            }
            if (mailboxGroup.Whitelisted.Contains(user))
            {
                throw new RecordAlreadyExistsException("User in whitelist");
            }

            mailboxGroup.Whitelisted.Add(user);
            await this.mailboxGroupRepository.SaveChangesAsync();
        }

        public async Task RemoveUserFromWhitelistAsync(string groupId, string userEmail)
        {
            AppUser user = await this.userRepository.FindByEmailOrNullAsync(userEmail)
                ?? throw new NotFoundException("User");
            MailboxGroup mailboxGroup = await this.mailboxGroupRepository.GetWhitelistByIdAsync(groupId)
                ?? throw new NotFoundException("Mailbox group");

            if (!mailboxGroup.Whitelisted.Contains(user))
            {
                throw new WrongValueException("Mailbox group whitelist");
            }

            mailboxGroup.Whitelisted.Remove(user);
            await this.mailboxGroupRepository.SaveChangesAsync();
        }

        public async Task<JoinTokenModel> CreateWhitelistJoinTokenAsync(string groupId)
        {
            const int tokenSize = 36;
            byte[] tokenData = new byte[tokenSize];
            RandomNumberGenerator.Create().GetBytes(tokenData);
            string tokenValue = Convert.ToBase64String(tokenData);

            var token = new WhitelistJoinToken
            {
                GroupId = groupId,
                Token = tokenValue
            };
            this.whitelistJoinTokenRepository.AddToken(token);
            await this.whitelistJoinTokenRepository.SaveChangesAsync();

            return this.mapper.Map<JoinTokenModel>(token);
        }

        public async Task<IEnumerable<JoinTokenModel>> GetAllTokensAsync(string groupId)
        {
            var tokens = await this.whitelistJoinTokenRepository.GetTokensOfGroup(groupId);
            return this.mapper.Map<IEnumerable<JoinTokenModel>>(tokens);
        }

        public async Task DeleteToken(string groupId, string tokenId)
        {
            WhitelistJoinToken token = await this.whitelistJoinTokenRepository.FindByIdOrNull(tokenId)
                ?? throw new NotFoundException("Whitelist join token");
            this.whitelistJoinTokenRepository.DeleteToken(token);
            await this.whitelistJoinTokenRepository.SaveChangesAsync();
        }

        public async Task JoinWhitelistByToken(string groupId, string tokenValue, string userEmail)
        {
            WhitelistJoinToken token = await this.whitelistJoinTokenRepository.FindByGroupAndValueOrNull(groupId, tokenValue)
                ?? throw new NotFoundException("Whitelist join token");
            await AddUserToWhitelistAsync(groupId, userEmail);
        }
    }
}
