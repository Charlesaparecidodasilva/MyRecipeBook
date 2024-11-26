using Bogus;
using MyRecipebook.Domain.Entities;
using MyRecipebook.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Entities
{
    // Define a classe responsável por criar objetos fictícios (fake) do tipo Recipe.
    // Utiliza a biblioteca Bogus para gerar dados falsos, ideal para testes unitários.
    public class RecipeBuilder
    {
        // Método estático que cria e retorna uma lista de objetos Recipe associados a um usuário específico.
        // Parâmetros:
        //   - user: O usuário ao qual as receitas serão associadas.
        //   - count: O número de receitas a serem geradas. O padrão é 2.
        public static IList<Recipe> Collection(User user, uint count = 2)
        {
            // Inicializa uma lista vazia para armazenar as receitas geradas.
            var list = new List<Recipe>();

            // Se o número de receitas (count) for zero, define como 1 para garantir que pelo menos uma receita seja criada.
            if (count == 0)
                count = 1;

            // Variável para controlar o ID das receitas geradas.
            var recipeId = 1;

            // Loop para gerar a quantidade especificada de receitas.
            for (int i = 0; i < count; i++)
            {
                // Cria uma receita fictícia associada ao usuário fornecido.
                var fakeRecipe = Build(user);

                // Define o ID da receita atual.
                fakeRecipe.Id = recipeId;

                // Adiciona a receita criada à lista de receitas.
                list.Add(fakeRecipe);

                // Incrementa o ID para a próxima receita.
                recipeId++;
            }

            // Retorna a lista de receitas geradas.
            return list;
        }

        // Método estático que constrói e retorna uma única instância fictícia de Recipe.
        // Parâmetros:
        //   - user: O usuário ao qual a receita será associada.
        public static Recipe Build(User user)
        {
            // Utiliza a biblioteca Bogus para gerar dados falsos para um objeto Recipe.
            return new Faker<Recipe>()

                // Define o ID da receita como 1 (pode ser ajustado conforme necessário).
                .RuleFor(r => r.Id, () => 1)

                // Gera um título aleatório para a receita usando uma palavra Lorem.
                .RuleFor(r => r.Title, (f) => f.Lorem.Word())

                // Seleciona aleatoriamente um valor do enum CookingTime (tempo de cozimento).
                .RuleFor(r => r.CookingTime, (f) => f.PickRandom<CookingTime>())

                // Seleciona aleatoriamente um valor do enum Difficulty (nível de dificuldade).
                .RuleFor(r => r.Difficulty, (f) => f.PickRandom<Difficulty>())

                // Gera uma lista de ingredientes aleatórios.
                .RuleFor(r => r.Ingredients, (f) => f.Make(1, () => new Ingredient
                {
                    Id = 1,  // Define o ID do ingrediente como 1.
                    Item = f.Commerce.ProductName()  // Gera um nome de produto aleatório para o ingrediente.
                }))

                // Gera uma lista de instruções aleatórias.
                .RuleFor(r => r.Instructions, (f) => f.Make(1, () => new Instruction
                {
                    Id = 1,  // Define o ID da instrução como 1.
                    Step = 1,  // Define o número da etapa como 1.
                    Text = f.Lorem.Paragraph()  // Gera um parágrafo aleatório para a instrução.
                }))

                // Gera uma lista de tipos de prato aleatórios.
                .RuleFor(r => r.DishTypes, (f) => f.Make(1, () => new MyRecipebook.Domain.Entities.DishType
                {
                    Id = 1,  // Define o ID do tipo de prato como 1.
                    Type = f.PickRandom<MyRecipebook.Domain.Enum.DishType>()  // Seleciona aleatoriamente um tipo de prato.
                }))

                // Define o ID do usuário associado à receita com base no usuário fornecido.
                .RuleFor(r => r.UserId, () => user.Id);
        }
    }

}


