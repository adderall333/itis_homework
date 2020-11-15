using System;
using System.Net.Http;
using homework_1;

namespace ClientExpressions
{
    public class RemoteCalculator : ICalculator
    {
        public double Calculate(double val1, string operation, double val2)
        {
            var client = new HttpClient();
            var response = client.GetAsync(("http://localhost:5000/calculate?" +
                                                  $"val1={val1}&" +
                                                  $"operation={operation}&" +
                                                  $"val2={val2}")
                .Replace("+", "%2B")).Result.Content.ReadAsStringAsync().Result;
            var result = 0d;
            var isDouble = Double.TryParse(response, out result);
                        
            if (!isDouble)
                throw new Exception(response);

            return result;
        }
    }
}