using FluentValidation;
using MyRecipebook.Domain.Entities;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.Recipe
{
    public class RecipeValidator : AbstractValidator<RequestRecipeJson>
    {

        public RecipeValidator()
        {
            RuleFor(recipe => recipe.Title).NotEmpty().WithMessage(ResourceMenssageException.RECIPY_TITLE_EMPTY);
            RuleFor(recipe => recipe.CookingTime).IsInEnum().WithMessage(ResourceMenssageException.COOKING_TIME_NOT_SUPORTED);
            RuleFor(recipe => recipe.Difficulty).IsInEnum().WithMessage(ResourceMenssageException.DIFUCULT_LEVEL_NOT_SUPORT);
            RuleFor(recipe => recipe.Ingredients.Count).GreaterThan(0).WithMessage(ResourceMenssageException.AT_LEAST_ONE_INGREDIENTES);
            RuleFor(recipe => recipe.Instructions.Count).GreaterThan(0).WithMessage(ResourceMenssageException.AT_LEAST_ONE_INSTRUCTION);
            RuleForEach(recipe => recipe.DishTypes).IsInEnum().WithMessage(ResourceMenssageException.DISH_TYPE_NOT_SUPORTED);
            RuleForEach(recipe => recipe.Ingredients).NotEmpty().WithMessage(ResourceMenssageException.INGREDIENT_EMPTY);
            // Define uma validação para cada item da coleção "Instructions" dentro de um objeto "recipe".
            RuleForEach(recipe => recipe.Instructions).ChildRules(InstructionRule =>
            {
                // Define uma validação para a propriedade "Step" de cada instrução.
                // "Step" deve ser maior que 0, ou seja, o passo da instrução não pode ser negativo ou zero.
                InstructionRule.RuleFor(instruction => instruction.Step)
                    .GreaterThan(0)
                    .WithMessage(ResourceMenssageException.NON_NEGATIVE_INSTRUCTION_STEP);

                // Define uma validação para a propriedade "Tex" de cada instrução.
                InstructionRule
                    .RuleFor(instruction => instruction.Text) // Verifica a propriedade "Tex" (texto da instrução).
                    .NotEmpty() // Garante que o texto da instrução não esteja vazio.
                    .WithMessage(ResourceMenssageException.INSTRUCTION_EMPTY) // Mensagem de erro caso esteja vazio.
                    .MaximumLength(2000) // Limita o texto a no máximo 2000 caracteres.
                    .WithMessage(ResourceMenssageException.INGREDIENT_EXCEED_LIMIT_CHARACTERS); // Mensagem de erro caso ultrapasse o limite.
            });

            // Define uma validação para a coleção "Instructions" como um todo.
            RuleFor(recipe => recipe.Instructions)
                // Garante que os valores de "Step" (ordem das instruções) sejam únicos.
                // Usa LINQ para contar os passos únicos e verifica se é igual ao número total de instruções.
                .Must(instructions => instructions.Select(i => i.Step).Distinct().Count() == instructions.Count)
                // Mensagem de erro caso existam dois ou mais passos com o mesmo valor de ordem.
                .WithMessage(ResourceMenssageException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER);

        }


    }
}
