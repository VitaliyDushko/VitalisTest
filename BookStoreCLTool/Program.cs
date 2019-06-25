using BookStoreUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookStoreCLTool
{
    class Program
    {
        private static async Task Main()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var bookApiService = new BookApiService(new HttpClient(), configuration);

            TypeTip();

            var command = Console.ReadLine().Trim();
            while (command != "finish")
            {
                if (command.StartsWith("-get"))
                {
                    var parameters = command.Split(' ');
                    int bookId;
                    if (int.TryParse(parameters[1], out bookId))
                    {

                        Console.WriteLine(JObject.Parse(await bookApiService.GetBookByIdInStringFormat(bookId)));
                    }
                    else
                    {
                        TypeTip();
                    }
                }
                else if (command.StartsWith("-title"))
                {
                    var parameters = command.Split(' ');
                    if (parameters.Length == 4)
                    {
                        Console.WriteLine(JsonConvert.SerializeObject(await bookApiService.CreateBookInStringFormat(new Book { Title = parameters[1], Author = parameters[3] }), Formatting.Indented));
                    }
                    else
                    {
                        TypeTip();
                    }
                }
                else if (command.StartsWith("-edit"))
                {
                    var parameters = command.Split(' ');
                    int bookId;
                    if (parameters.Length == 6 && int.TryParse(parameters[1], out bookId))
                    {
                        var response = await bookApiService.GetBookById(bookId);
                        if (response.Errors == null || response.Errors.Count == 0)
                        {
                            response.Content.Title = parameters[3];
                            response.Content.Author = parameters[4];
                            var result = await bookApiService.EditBook(response.Content);
                            Console.WriteLine(JsonConvert.SerializeObject(result.Content, Formatting.Indented));
                        }
                        Console.WriteLine("Book with this id was not found");
                    }
                    else
                    {
                        TypeTip();
                    }
                }
                else if (command.StartsWith("-delete"))
                {
                    var parameters = command.Split(' ');
                    int bookId;
                    if (int.TryParse(parameters[1], out bookId))
                    {
                        string response = await bookApiService.DeleteBookByIdInStringFormat(bookId);
                        if (response == "true")
                        {
                            Console.WriteLine($"Book was deleted");
                        }
                        else
                        {
                            Console.WriteLine(JArray.Parse(response));
                        }
                    }
                    else
                    {
                        TypeTip();
                    }
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(await bookApiService.GetBooksInStringFormat(), Formatting.Indented));
                }
                command = Console.ReadLine().Trim();
            }

            Console.ReadLine();
        }

        private static void TypeTip()
        {
            Console.WriteLine("Choose the operation you want to do:\n1.Display all books info - press \"Enter\"" +
                "\n2.Display a specific book info - type \"-get <%book id%>\" and press enter\n" +
                "3.Create a new book - type \"-title <%Your title%> author -<%Author name%>\"\n" +
                "4.Edit book - type \"-edit <%book id%>\" -title <%Your title%> author -<%Author name%>\"\n" +
                "5.Delete book - type \"-delete <%book id%>\n" +
                "6.Terminate app - type \"finish");
        }
    }
}
