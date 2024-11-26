using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Exception.ExceptionBase
{
    public class ErroOnValidationException : MyRecipeBookException
    {

        //crio uma lista para colocar os erros de menssagens//
        public IList<string> MenssageErroValidada { get; set; }
     
        // Pego a lista de erro que sera instanciada e jogo dento da lista que acabei de criar.
        public ErroOnValidationException(IList<string> erros) : base(string.Empty)
        {
            MenssageErroValidada = erros;
        }  
    }
}
