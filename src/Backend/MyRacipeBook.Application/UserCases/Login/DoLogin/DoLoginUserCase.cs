
using MyRecipebook.Domain.Repositories.User;
using MyRecipebook.Domain.Security.Cryptography;
using MyRecipebook.Domain.Security.Tokens;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;
using MyRecipeBook.Exception.ExceptionBase;

namespace MyRacipeBook.Application.UserCases.Login.DoLogin
{
    public class DoLoginUserCase : IDoLoginUserCase
    {     
        private readonly IUserReadOnlyRepository _repository;
        private readonly IPaswordEncripter _passwordEncripter;
        private readonly IAcessTokenGenerator _accesstokenGenerator;

        public DoLoginUserCase (
            IUserReadOnlyRepository repository,
            IAcessTokenGenerator accesstokenGenerator,
            IPaswordEncripter passwordEncripter)
        {
            _repository = repository;
            _passwordEncripter = passwordEncripter;
            _accesstokenGenerator = accesstokenGenerator;
        }
          
                          ///tipo de resposta                //Paramento da requesição
        public async Task<ResponseRegisteredUserJason>Execute(RequestLoguinJason request)
        {

             var pasworEncript = _passwordEncripter.Encrypt(request.Password);
           
            var user = await _repository.GetEmailAndPassword(request.Email, pasworEncript) ?? throw new InvalidLoginException();
            return new ResponseRegisteredUserJason
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accesstokenGenerator.Generate(user.UserIdentifier)

                }

            };
        }
    }
}
