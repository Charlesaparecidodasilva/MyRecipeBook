using Moq;
using MyRecipebook.Domain.Entities;
using MyRecipebook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Repositories
{
    //Define uma classe que cria mocks de IUserReadOnlyRepository
    public class UserReadOnlyRepositoryBuilder
    {
        //Declara um campo privado para armazenar o mock de IUserReadOnlyRepository.

        private readonly Mock<IUserReadOnlyRepository> _repository;      
      
        //Construtor da classe.
        public UserReadOnlyRepositoryBuilder()
        {
           // No construtor, criamos o mock de IUserReadOnlyRepository
            _repository = new Mock<IUserReadOnlyRepository>();
        }      
        public void ExistActiveUserWhitEmail(string email)
        {
            // aqui estu configurando para retornar True quando essa função for chamada com esse email.
            _repository.Setup(repository => repository.ExistActiveUserWhitEmail(email)).ReturnsAsync(true);       
        }

        public void GetByEmailAndPassword(User user)
        {

            _repository.Setup(respository => respository.GetEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user);
        }





        // O método Build retorna o mock criado, para ser usado em testes.
        public IUserReadOnlyRepository Build() => _repository.Object;       
    }
}

