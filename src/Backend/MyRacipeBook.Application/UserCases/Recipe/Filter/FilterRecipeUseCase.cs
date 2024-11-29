using AutoMapper;
using MyRecipebook.Domain.Dtos;
using MyRecipebook.Domain.Repositories.Recipe;
using MyRecipebook.Domain.Services.LoggedUser;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;
using MyRecipeBook.Infrastruture.DataAcess;
using MyRecipeBook.Infrastruture.Services.LoggedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.Recipe.Filter
{
    public class FilterRecipeUseCase : IFilterRecipeUseCase
    {
        private readonly  ILoggedUser _loggedUser;     
        private readonly IMapper _mapper;
        private readonly IRecipeReadOnlyRespository _repository;

        public FilterRecipeUseCase(
            ILoggedUser loggedUser,         
            IMapper mapper,
            IRecipeReadOnlyRespository repository)
        { 
            _loggedUser = loggedUser;         
            _mapper = mapper;
            _repository = repository;
        }
        
        public async Task<ResponseRecipesJson> Execulte(RequestFilterRecipeJson request)
        {
            Validate(request);
            
            var loggedUser = await _loggedUser.User();

            var filter = new MyRecipebook.Domain.Dtos.FilterRecipesDto
            {
                RecipeTitle_Ingredient = request.RecipeTitle_Ingredientes,
                CookingTimes = request.CookingTimes.Distinct().Select(c => (MyRecipebook.Domain.Enum.CookingTime)c).ToList(),
                Difficulties = request.Difficulties.Distinct().Select(c => (MyRecipebook.Domain.Enum.Difficulty)c).ToList(),
                DishTypes = request.DishTypes.Distinct().Select(c => (MyRecipebook.Domain.Enum.DishType)c).ToList()    
            };

            var recipe = await _repository.Filter(loggedUser, filter);

            return new ResponseRecipesJson
            {
                Recipes = _mapper.Map<List<ResponseShortRecipeJson>>(recipe)
            };
            
        }



        private static void Validate(RequestFilterRecipeJson request)
        {
            var validator = new FilterRecipeValidator();

            var result = validator.Validate(request);
        }

         //{
         //       Recipes = _mapper.Map<List<ResponseShortRecipeJson>>(recipe)
         //};







}
}

