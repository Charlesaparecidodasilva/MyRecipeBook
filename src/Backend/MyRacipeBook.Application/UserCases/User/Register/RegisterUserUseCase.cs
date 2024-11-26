using AutoMapper;
using MyRecipebook.Domain.Repositories;
using MyRecipebook.Domain.Repositories.User;
using MyRecipebook.Domain.Security.Cryptography;
using MyRecipebook.Domain.Security.Tokens;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;

namespace MyRacipeBook.Application.UserCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPaswordEncripter _passwordEncripter;
        private readonly IUnitiOfWork _unitiOfWork;
        private readonly IAcessTokenGenerator _accesstokenGenerator;

        public RegisterUserUseCase(
            IUserWriteOnlyRepository userWriteOnlyRepository, 
            IUserReadOnlyRepository userReadOnlyRepository,                   
            IUnitiOfWork unitOfWork,
            IPaswordEncripter passwordEncripter,
            IAcessTokenGenerator accesstokenGenerator,
        IMapper mapper
            )
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _mapper = mapper;
            _unitiOfWork = unitOfWork;
            _passwordEncripter = passwordEncripter;
            _accesstokenGenerator = accesstokenGenerator;
        }


        // A request é jason com os dados enviado pelo usuario.
        public async Task <ResponseRegisteredUserJason> Execute (RequestRegisterUserJson request)
        {
            try
            {
                // Validar a request
                await Validate(request);

                // Mapear a request em uma entidade(EntyBase e User criado em Domain)
                var user = _mapper.Map<MyRecipebook.Domain.Entities.User>(request);

                //Criptografia da senha

                user.Password = _passwordEncripter.Encrypt(request.Password);

                user.UserIdentifier = Guid.NewGuid();

                //Salvar no Banco de dados///
                //adicionando o usuario
                await _userWriteOnlyRepository.Add(user);

                await _unitiOfWork.Commit();

                //_userReadOnlyRepository.ExistActiveUserWhitEmail(user.Email); //verificado so o email é igual.


                ///Retorna onome do usuário.
                return new ResponseRegisteredUserJason
                {
                    Name = request.Name,
                    Tokens = new ResponseTokensJson
                    {
                        AccessToken = _accesstokenGenerator.Generate(user.UserIdentifier)
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }

                                              
          
        }
                                 ///O "rquest" tras os dados do arquivo jason.
        private async Task Validate (RequestRegisterUserJson request)
        {         
            //validator é uma instancia da classe que tem as regras de validação, e as menssagens de erro.
            var validator = new RegisteUserValidator();

            //Aqui estamos  validando  jason usando a instancia do validadtor e armazenando na classe result.
            var result = validator.Validate (request);
            var exisEmail = await _userReadOnlyRepository.ExistActiveUserWhitEmail(request.Email);

            if (exisEmail)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMenssageException.EMAIL_ALREADY_REGISTERED));
            }

            //Aqui verificamos se o resultado é valido.
            if (result.IsValid == false) 
            {
                           //<< joga em erro//
              //mensagem de erro//  //Selecioando cada mensagem dentro de result e cria uma lista//
                var errorMenssagens = result.Errors.Select(e => e.ErrorMessage).ToList();
                       
                throw new ErroOnValidationException(errorMenssagens);
            }
        }
    }
}
