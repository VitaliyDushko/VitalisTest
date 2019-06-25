using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BookStoreUtilities
{
    public class BookApiService
    {
        private HttpClient Client { get; }

        public BookApiService(HttpClient client, IConfiguration configuration, IHttpContextAccessor httpContextAccessor = null)
        {
            //get access token from http context
            if (httpContextAccessor != null)
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("BookStoreApiEndpoint"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result);
                Client = client;
            }
            //use client credentials
            else
            {
                var disco = client.GetDiscoveryDocumentAsync(configuration.GetValue<string>("AuthenticationServerEndpoint")).Result;

                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                // request token
                var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "bookstorecmdclient",
                    ClientSecret = "BillizzzCMDTool",

                    Scope = "bookstore"
                }).Result;

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                Console.WriteLine(tokenResponse.Json);
                Console.WriteLine("\n\n");
                Client = new HttpClient();
                Client.BaseAddress = new Uri(configuration.GetValue<string>("BookStoreApiEndpoint"));
                Client.SetBearerToken(tokenResponse.AccessToken);
            }
        }

        public async Task<GenericResponseFromApiService<List<Book>>> GetBooks()
        {
            return await ExtractResponse<List<Book>>(await Client.GetAsync("Books"));
        }

        public async Task<GenericResponseFromApiService<Book>> GetBookById(int id)
        {
            return await ExtractResponse<Book>(await Client.GetAsync($"Books/{id}"));
        }

        public async Task<GenericResponseFromApiService<int?>> CreateBook(Book book)
        {
            return await ExtractResponse<int?>(await Client.PostAsync($"Books", ConvertBookToByteArrayContent(book)));
        }

        public async Task<GenericResponseFromApiService<Book>> EditBook(Book book)
        {
            return await ExtractResponse<Book>(await Client.PutAsync($"Books/{book.Id}", ConvertBookToByteArrayContent(book)));
        }

        public async Task<GenericResponseFromApiService<bool>> DeleteBookById(int id)
        {

            return await ExtractResponse<bool>(await Client.DeleteAsync($"Books/{id}"));
        }

        public async Task<string> GetBooksInStringFormat()
        {
            return await ExtractStringResponse(await Client.GetAsync("Books"));
        }

        public async Task<string> GetBookByIdInStringFormat(int id)
        {
            return await ExtractStringResponse(await Client.GetAsync($"Books/{id}"));
        }

        public async Task<string> CreateBookInStringFormat(Book book)
        {
            return await ExtractStringResponse(await Client.PostAsync($"Books", ConvertBookToByteArrayContent(book)));
        }

        public async Task<string> EditBookInStringFormat(Book book)
        {
            return await ExtractStringResponse(await Client.PutAsync($"Books/{book.Id}", ConvertBookToByteArrayContent(book)));
        }

        public async Task<string> DeleteBookByIdInStringFormat(int id)
        {

            return await ExtractStringResponse(await Client.DeleteAsync($"Books/{id}"));
        }
        private async Task<GenericResponseFromApiService<T>> ExtractResponse<T>(HttpResponseMessage response)
        {
            GenericResponseFromApiService<T> convertedResponse = new GenericResponseFromApiService<T>();
            try
            {
                if (response.IsSuccessStatusCode)
                {              
                    convertedResponse.Content = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    convertedResponse.Errors = ExtractErrors(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
                convertedResponse = new GenericResponseFromApiService<T> { Errors = new List<string> { "Internal Server Error" } };
            }
            return convertedResponse;
        }

        private async Task<string> ExtractStringResponse(HttpResponseMessage response)
        {
            try
            { 
                return await response.Content.ReadAsStringAsync();            
            }
            catch (Exception ex)
            {
                return $"{{{ex.Message}}}";
            }
        }

        private ByteArrayContent ConvertBookToByteArrayContent(Book book)
        {
            string serializedBook = JsonConvert.SerializeObject(book);
            var buffer = System.Text.Encoding.UTF8.GetBytes(serializedBook);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }

        private List<string> ExtractErrors(string stringResponse)
        {
            var jsonResponse = JObject.Parse(stringResponse);
            return jsonResponse.ToObject<ErrorResponse>().Errors;
        }
    }
}
