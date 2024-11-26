using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MyRecipeBook.API.Converter
{
    // aqui eu defino o tipo de propriedade eu quero converter, e nesse caso, uma string
    public partial class StringConverter : JsonConverter<string>
    {                              
        
                                      //reader é a string enviada pelo usuário
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Obtenho a string do reader removo os espeços do inicioe do fim usando o Trim
            var value = reader.GetString()?.Trim();

           if (value is null) 
                return null;

            return RemoveExtraWhiteSpaces().Replace(value, " ");

            
        }
        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) => writer.WriteStringValue(value);
        [GeneratedRegex(@"\s+")]
        private static partial Regex RemoveExtraWhiteSpaces();

        //public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        //{
        //    writer.WriteStringValue(value);
        //}

        //// Implementação alternativa para remover espaços extras
        //private static Regex RemoveExtraWhiteSpaces()
        //{
        //    return new Regex(@"\s+");
        //}


    }
}
