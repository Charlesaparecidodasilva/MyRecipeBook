

namespace MyRecipeBook.Exception.ExceptionBase
{

    //Essa classe so vai ser reconhecida se ele ter herança com a SystemException.
    public class MyRecipeBookException : SystemException
    {

        public MyRecipeBookException(string message) : base(message) { }
        
    }
}


