using FirebirdSql.Data.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRacipeBook.Application.UserCases.Login.DoLogin;
using MyRacipeBook.Application.UserCases.Recipe;
using MyRacipeBook.Application.UserCases.Recipe.Delete;
using MyRacipeBook.Application.UserCases.Recipe.Filter;
using MyRacipeBook.Application.UserCases.Recipe.GetById;
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
            AddAutoMapper(services);
            AddIdEnconder(services, configuration);
            AddUseCase(services);        
        }
        private static void AddAutoMapper(IServiceCollection services)
        {
                  
            services.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOptions =>
            {
                var sqids = option.GetService<SqidsEncoder<long>>()!;

                autoMapperOptions.AddProfile(new AutoMapping(sqids));
            }).CreateMapper()); 
        }

        private static void AddIdEnconder(IServiceCollection services, IConfiguration configuration)
        {
            var sqids = new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
            });

            services.AddSingleton(sqids);
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
            services.AddScoped<IGetRecipeByIdUserCase, GetRecipeByIdUserCase>();
            services.AddScoped<IDeleteRecipeUserCase, DeleteRecipeUserCase>();
        }

      

    }
}

