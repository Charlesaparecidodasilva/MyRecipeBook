using MyRecipebook.Domain.Extensions;
using System.Globalization;

namespace MyRecipeBook.API.Middleware
{
    public class CutureMiddleware
    {
        private readonly RequestDelegate _next;

        public CutureMiddleware(RequestDelegate next) 
        {
            _next = next;
        }
        public async Task Invoke (HttpContext context)
        {
            //Aqui estou buscando todas as linguagens suportadas e armazenando elas variável supportedLinguages. 
            var supportedLinguages = CultureInfo.GetCultures(CultureTypes.AllCultures);

            // Aqui estou pegando a cultura local do usuário e inserindo ela na variável  "requestedCulture".
            var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            //Aqui ja estou inserindo o ingles como indioma padrão previamente na variavel cultureInfo.
            var cultureInfo = new CultureInfo("en");


            //Logo a baixo vamos validar a lingua do usuario que esta na "requestedCulture". ///

            //Nesse if estou verificando se "requestedCulture" esta vazia ou em branco
            if (requestedCulture.NotEmpaty()
                // E se ela alguma lingua suportada é igual a que esta nela.
                && supportedLinguages.Any( c => c.Name.Equals(requestedCulture)))  
            {
                //Aqui ja verificamos "requestedCulture" nao é vazia e é semelhante as linguas suportadas.

                //Agora é so inserir a lingua que ja foi validade na variáavel cultureInfo. 
                cultureInfo = new CultureInfo(requestedCulture);
            }
            
            //Aqui estou inserindo o que ficou no "cultureInfo" para mudar o idioma das mensagens(Caso o idimo nao for validado n IF, o ingles é o padrao).   
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;


            // estou liberando o processo.
            await _next(context);

        }
    }
}
