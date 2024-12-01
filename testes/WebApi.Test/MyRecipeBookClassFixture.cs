using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test
{
    public class MyRecipeBookClassFixture : IClassFixture<CustonWebAplicationFactory>
    {
        private readonly HttpClient _httpClient;
        public MyRecipeBookClassFixture (CustonWebAplicationFactory factory) => _httpClient = factory.CreateClient();
        
      
        
        protected async Task<HttpResponseMessage> DoPost(
            string method, 
            object requesicao,
            string token = "",
            string culture = "en")
        {          
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);
            return await _httpClient.PostAsJsonAsync(method, requesicao);
        }

        protected async Task<HttpResponseMessage> DoPut( string method, object request, string token, string culture = "en" )
        {
            ChangeRequestCulture (culture);
            AuthorizeRequest(token);

            return await _httpClient.PutAsJsonAsync(method, request);

        }
     
        protected  async  Task<HttpResponseMessage> DoGet( string method, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);

            return await _httpClient.GetAsync(method);
        }
       
        protected async Task<HttpResponseMessage> DoDelete(string method, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);
            return await _httpClient.DeleteAsync(method);
        }
        
        private void ChangeRequestCulture(string culture)
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

            //Aqui estamos  implementando a cultura do Theory para o  cliente HTTP
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

        }

        private void AuthorizeRequest(string token)
        {
            // se o token for vazio, saia da função
            if (string.IsNullOrWhiteSpace(token))
                return;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        }
    }



}
