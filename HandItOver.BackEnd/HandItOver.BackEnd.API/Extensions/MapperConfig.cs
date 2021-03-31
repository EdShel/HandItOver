using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace HandItOver.BackEnd.API.Extensions
{

    public static class MapperConfig
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<DAL.Entities.Mailbox, BLL.Models.MailboxGroup.MailboxViewResult>();
                CreateMap<DAL.Entities.MailboxGroup, BLL.Models.MailboxGroup.MailboxGroupViewResult>();
                CreateMap<DAL.Entities.MailboxRent, BLL.Models.MailboxRent.RentResult>()
                    .ForMember(r => r.MailboxSize, opt => opt.MapFrom(r => r.Mailbox.Size));
                CreateMap<DAL.Entities.WhitelistJoinToken, BLL.Models.MailboxAccessControl.JoinTokenModel>();
                CreateMap<DAL.Entities.Auth.AppUser, BLL.Models.Users.UserPublicInfoResult>();
                CreateMap<DAL.Entities.Delivery, BLL.Models.Delivery.ActiveDeliveryResult>();
                CreateMap<DAL.Entities.Delivery, BLL.Models.Delivery.DeliveryResult>();
                CreateMap<DAL.Entities.Mailbox, BLL.Models.Mailbox.MailboxViewResult>();
                CreateMap<BLL.Models.MailboxGroup.MailboxGroupEditRequest, DAL.Entities.MailboxGroup>();
                CreateMap<BLL.Models.Mailbox.MailboxEditRequest, DAL.Entities.Mailbox>();
            }
        }
    }

}
