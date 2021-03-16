using HandItOver.BackEnd.BLL.Models.MailboxGroup;
using HandItOver.BackEnd.DAL.Entities;
using HandItOver.BackEnd.DAL.Repositories;
using HandItOver.BackEnd.Infrastructure.Exceptions;
using HandItOver.BackEnd.Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.ResourceAccess
{
    public interface IOwnedResource
    {
        string OwnerId { get; }
    }

    public class ResourceOwnerRequirement : IAuthorizationRequirement
    {
    }

    public abstract class ResourceAccessAuthorizationHandler<TResource> : AuthorizationHandler<ResourceOwnerRequirement, TResource>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ResourceOwnerRequirement requirement,
            TResource resource)
        {
            string? userId = context.User.FindFirst(AuthConstants.Claims.ID)?.Value;
            if (userId != null && await IsOwnerAsync(userId, resource))
            {
                context.Succeed(requirement);
            }
        }

        protected abstract Task<bool> IsOwnerAsync(string userId, TResource resource);
    }

    public sealed class MailboxGroupAuthorizationHandler : ResourceAccessAuthorizationHandler<IMailboxGroupRequest>
    {
        private readonly MailboxGroupRepository mailboxGroupRepository;

        public MailboxGroupAuthorizationHandler(MailboxGroupRepository mailboxGroupRepository)
        {
            this.mailboxGroupRepository = mailboxGroupRepository;
        }

        protected override async Task<bool> IsOwnerAsync(string userId, IMailboxGroupRequest resource)
        {
            MailboxGroup? mailboxGroup = await this.mailboxGroupRepository.FindByIdOrNullAsync(resource.GroupId)
                ?? throw new NotFoundException("Mailbox group");
            return mailboxGroup.OwnerId == userId;
        }
    }

}
