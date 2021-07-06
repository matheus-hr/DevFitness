using AutoMapper;
using DevFitness.API.Core.Entities;
using DevFitness.API.Models.InputModels;
using DevFitness.API.Models.ViewModels;

namespace DevFitness.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>(); //Mapeia do banco de dados para uma ViewModel (Classe de Saida do Usuario)
            CreateMap<CreateUserInputModel, User>(); //Mapeia a  Classe de entrada de dados para uma Classe de Tabela
        }
    }
}
