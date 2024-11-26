using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRacipeBook.Application.UserCases.Login.DoLogin;
using MyRacipeBook.Application.UserCases.Recipe;
using MyRacipeBook.Application.UserCases.Recipe.Filter;
using MyRacipeBook.Application.UserCases.User.ChangePassword;
using MyRacipeBook.Application.UserCases.User.Profile;
using MyRacipeBook.Application.UserCases.User.Register;
using MyRacipeBook.Application.UserCases.User.Update;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Infrastruture.Services.LoggedUser;
using Sqids;

namespace MyRacipeBook.Application
{
    public static class DependencyInjectionExtencion
    {
        ///Essa classe injeta as depedencias.
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMapper(services, configuration);
            AddUseCase(services);        
        }
        private static void AddAutoMapper(IServiceCollection services, IConfiguration configuration)
        {
            var sqids = new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
            });          
            services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping(sqids));
            }).CreateMapper());
        }

        //Vejas só qui,  
        private static void AddUseCase(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUserCase, DoLoginUserCase>();           
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IChangePasswordUserCase, ChangePasswordUserCase>();
            services.AddScoped<IRegisteRecipeUseCase, RegisterRecipeUseCase>();
            services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();           
        }

      

    }
}

