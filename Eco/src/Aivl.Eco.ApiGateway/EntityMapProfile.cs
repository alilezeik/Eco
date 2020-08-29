namespace Eco.ApiGateway
{
    using Eco.Sql.Models;
    using AutoMapper;
    using System.Text.RegularExpressions;

    public class EntityMapProfile : Profile
    {
        public EntityMapProfile()
        {
            //CreateMap<StartupUserEntity, StartupUser>().ReverseMap();
        }
    }
}
