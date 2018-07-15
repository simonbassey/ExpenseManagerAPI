using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ExpenseMgr.Services.Helpers
{
    public interface ICurrencyConverter
    {
        Task<double> Convert(double amount, string from, string to);
    }

    public class CurrencyConverter : ICurrencyConverter
    {
        public async Task<double> Convert(double amount, string from, string to)
        {
            try
            {
                using (var _httpClient = new HttpClient())
                {
                    var requestUrl = BuildRequestUrl(from, to);
                    var response = await _httpClient.GetStringAsync(requestUrl);
                    JObject responseObject = JObject.Parse(response);
                    var resultNode = responseObject.GetValue($"{from}_{to}");
                    float rate = -1;
                    if (resultNode == null)
                        return rate;
                    float.TryParse(resultNode.Value<string>("val"), out rate);
                    return rate > -1 ? rate * amount : rate;
                }
            }
            catch (Exception exception)
            {
                var errorMsg = string.Format("Error occured while while requesting to convert {0} from {1} to {2} => \n{3}",
                                amount, from, to, exception);
                Debug.WriteLine(errorMsg);
                throw new InvalidOperationException(errorMsg, exception);
            }
        }

        private string BuildRequestUrl(string baseCurrency, string targetCurrency)
        {
            var query = $"?q={baseCurrency}_{targetCurrency}&compact=y";
            return "https://free.currencyconverterapi.com/api/v6/convert" + query;
        }
    }
}
