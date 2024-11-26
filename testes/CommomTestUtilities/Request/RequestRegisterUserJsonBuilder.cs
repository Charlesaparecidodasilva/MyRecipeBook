using Bogus;
using Bogus.DataSets;
using MyRecipeBook.Comunication.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommomTestUtilities.Request
{
    public class RequestRegisterUserJsonBuilder 
    {

        public static RequestRegisterUserJson Build(int passwordLength = 10)
        {
            return new Faker<RequestRegisterUserJson>()
                .RuleFor(user => user.Name, (f) => f.Person.FirstName)
                .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
                .RuleFor(user => user.Password, (f) => f.Internet.Password(passwordLength));
        }

        //EXPLICAÇÃO DO MÉTODO
        //        O método Build() cria e retorna uma instância de RequestRegisterUserJson com valores gerados dinamicamente.
        // Ele usa a classe Faker<T>, que é parte da biblioteca Bogus, para gerar esses dados de forma automática e realista(embora fictícia).
        //Uso de Faker<RequestRegisterUserJson>:

        //Faker<RequestRegisterUserJson> é um gerador de dados que cria um objeto RequestRegisterUserJson com regras específicas para preencher cada uma das propriedades.
        //Regra para o Name: O nome do usuário é gerado usando f.Person.FirstName, que utiliza o gerador de nomes da classe Person da biblioteca Bogus.
        //Regra para o Email: O e-mail é gerado usando f.Internet.Email(user.Name), que cria um e-mail baseado no nome gerado.
        //Regra para o Password: A senha é gerada com f.Internet.Password(), que cria uma senha aleatória.

    }
}
