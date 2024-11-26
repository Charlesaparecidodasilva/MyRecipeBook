using AutoMapper;
using MyRecipebook.Comunication.Enum;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;
using Sqids;

namespace MyRecipeBook.Application.Services.AutoMapper;

//A classe profile é nativa do proprio Mapper//
public class AutoMapping : Profile
{
      ///instalo a bibliotetca para configurar isso
    private readonly SqidsEncoder<long> _idEncoder;

    public AutoMapping(SqidsEncoder<long> idEncoder) 
    {
        _idEncoder = idEncoder;
        RequestToDomain();
        DomaimToResponse();
    }

    private void RequestToDomain()
    {

        CreateMap<RequestRegisterUserJson, MyRecipebook.Domain.Entities.User>()
          .ForMember(dest => dest.Password, opt => opt.Ignore());

        // Cria um mapeamento entre a classe RequestRecipeJson (DTO) e MyRecipebook.Domain.Entities.Recipe (entidade do domínio)
        CreateMap<RequestRecipeJson, MyRecipebook.Domain.Entities.Recipe>()

            // Configura a propriedade Instructions do destino (Recipe) para ser ignorada durante o mapeamento.
            // Isso significa que Instructions não será automaticamente preenchida pelo AutoMapper.
            .ForMember(dest => dest.Instructions, opt => opt.Ignore())

            // Configura o mapeamento para a propriedade Ingredients do destino (Recipe).
            // Essa propriedade será preenchida a partir da lista Ingredients do objeto de origem (RequestRecipeJson),
            // mas apenas com elementos distintos, ou seja, duplicatas serão removidas.
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))

            // Configura o mapeamento para a propriedade DishTypes do destino (Recipe).
            // Essa propriedade será preenchida a partir da lista DishTypes do objeto de origem (RequestRecipeJson),
            // mas também com elementos distintos, eliminando duplicatas.
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

        // Cria um mapeamento entre o tipo string (origem) e a entidade MyRecipebook.Domain.Entities.Ingredient (destino).
        CreateMap<string, MyRecipebook.Domain.Entities.Ingredient>()

            // Configura o mapeamento para a propriedade Item do destino (Ingredient).
            // O valor do destino será preenchido diretamente pelo valor da string de origem (source).
            .ForMember(dest => dest.Item, opt => opt.MapFrom(source => source));

        CreateMap<DishType, MyRecipebook.Domain.Entities.DishType>()
           .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

        CreateMap<RequestInstructionJson, MyRecipebook.Domain.Entities.Instruction>();
    }

    private void DomaimToResponse()
    {

        CreateMap<MyRecipebook.Domain.Entities.User, ResponseUserProfileJson>();

        // mapeio a entidade recipe com a resposta (Não esquecer de configurar isso na injeção de dependencia do Mapper)  
        CreateMap<MyRecipebook.Domain.Entities.Recipe, ResponseRegisteredRecipeJason>()
            .ForMember(dest => dest.Id, config => config.MapFrom(sourse => _idEncoder.Encode(sourse.Id)));

        CreateMap<MyRecipebook.Domain.Entities.Recipe, ResponseShortRecipeJson>()
            .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEncoder.Encode(source.Id)))
            .ForMember(dest => dest.AmountIngredients, config => config.MapFrom(source => source.Ingredients.Count));

    }

}






