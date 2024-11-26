using CommomTestUtilities.Request;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using MyRacipeBook.Application.UserCases.Recipe;
using MyRecipebook.Comunication.Enum;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.Test.Recipe
{
    public class RecipeValidatorTest
    {
        [Fact]

        public void Success()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();

        }

        [Fact]

        public void Erro_Invalid_Cooking_Time()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.CookingTime = (MyRecipebook.Comunication.Enum.CookingTime?)1000;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.COOKING_TIME_NOT_SUPORTED));

        }


        [Fact]

        public void Erro_Invalid_Difficulty()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Difficulty = (MyRecipebook.Comunication.Enum.Difficulty?)500;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.DIFUCULT_LEVEL_NOT_SUPORT));

        }

        [Theory]
        [InlineData(null)]
        [InlineData("      ")]
        [InlineData("")]
        public void Erro_Title_Empaty(string title)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Title = title;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.RECIPY_TITLE_EMPTY));
        }

        [Fact]
        public void Success_Cooking_Time_Null()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.CookingTime = null;

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }


        [Fact]
        public void Success_Difficulty_Null()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Difficulty = null;

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }
        //**************************************************************

        [Fact]
        public void Success_Difficulty_Empty()
        {
            var request = RequestRecipeJsonBuilder.Build();

            request.DishTypes.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        public void Error_Invalid_DishTypes()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Add((DishType)1000);
            var validator = new RecipeValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.DISH_TYPE_NOT_SUPORTED));
        }


        public void Error_Empaty_Ingrediente()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Clear();
            var validator = new RecipeValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.INGREDIENT_EMPTY));
        }




        public void Error_Empaty_Instructios()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.Clear();
            var validator = new RecipeValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.INSTRUCTION_EMPTY));
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public void Error_Empaty_Value_Ingredients(string ingredientes)
        {
            var request = RequestRecipeJsonBuilder.Build();

            request.Ingredients.Add(ingredientes);
            var validator = new RecipeValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.INGREDIENT_EMPTY));               
        }

        [Fact]  
        public void Error_Same_Step_Instrutions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = request.Instructions.Last().Step;
            var validator = new RecipeValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER));
        }

        [Fact]

        public void Error_Negative_Step_Instruction()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = -1;
            var validator = new RecipeValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.NON_NEGATIVE_INSTRUCTION_STEP));           
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void Error_Empaty_Value_Instruction(string instruction)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Text = instruction;
            var validator = new RecipeValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.INSTRUCTION_EMPTY));
        }

        [Fact]
        public void Error_Instruction_Too_Long()
        {
            var request = RequestRecipeJsonBuilder.Build();

            request.Instructions.First().Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);
            var validator = new RecipeValidator();
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.INSTRUCTION_EMPTY));


        }
    }
}
