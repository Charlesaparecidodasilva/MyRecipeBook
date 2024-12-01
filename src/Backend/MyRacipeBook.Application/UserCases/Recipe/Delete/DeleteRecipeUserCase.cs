using MyRecipebook.Domain.Repositories;
using MyRecipebook.Domain.Repositories.Recipe;
using MyRecipebook.Domain.Services.LoggedUser;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;


namespace MyRacipeBook.Application.UserCases.Recipe.Delete
{
    public class DeleteRecipeUserCase : IDeleteRecipeUserCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeWriteOnlyRespository _repository;
        private readonly IRecipeReadOnlyRespository _readRepository;
        private readonly IUnitiOfWork  _unitiOfWork;

        public DeleteRecipeUserCase(
            ILoggedUser loggedUser,
            IRecipeWriteOnlyRespository repository,
            IRecipeReadOnlyRespository readRepository,
            IUnitiOfWork unitiOfWork )           
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _readRepository = readRepository;
            _unitiOfWork = unitiOfWork;
        }


        public async Task Execute(long recipeId)
        {
            var userlogger = await _loggedUser.User();

            var recipe = await _readRepository.GetById(userlogger, recipeId);

            if (recipe is null)
            {
                throw new NotFoundException(ResourceMenssageException.RECIPE_NOT_FOUND);
            }

            await _repository.Delete(recipeId);
            await _unitiOfWork.Commit();
        }       
    }
}


