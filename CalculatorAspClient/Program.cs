using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalculatorAspClient
{
    class Program
    {
        static readonly HttpClient Client = new HttpClient();

        static async Task Main()
        {
            var val1 = Console.ReadLine();
            var operation = Console.ReadLine();
            operation = operation == "+" ? "%2B" : operation;
            var val2 = Console.ReadLine();
            try	
            {
                HttpResponseMessage response = await Client.GetAsync($"http://localhost:5000/calculate?" +
                                                                     $"val1={val1}&" +
                                                                     $"operation={operation}&" +
                                                                     $"val2={val2}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseBody);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
        }
    }
}