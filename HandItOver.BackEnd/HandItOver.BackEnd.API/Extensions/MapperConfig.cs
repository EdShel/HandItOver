using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

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
                CreateMap<DAL.Entities.MailboxRent, BLL.Models.MailboxRent.RentResult>();
            }
        }
    }

}
