
using System.Text;
using System.Xml.Linq;

namespace simple_seller_app.Services
{
    public class CalculatorService
    {

        private async Task<string> SendSoapRequest(string operation, string param1, string param2)
        {
            using (var client = new HttpClient())
            {
                var soapEnvelope = $"<?xml version='1.0' encoding='utf-8'?><soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'><soap:Body><{operation} xmlns='http://tempuri.org/'><intA>{param1}</intA><intB>{param2}</intB></{operation}></soap:Body></soap:Envelope>";

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("SOAPAction", "\"http://tempuri.org/" + operation + "\"");
                client.DefaultRequestHeaders.Connection.Add("keep-alive");

                var response = await client.PostAsync("http://www.dneonline.com/calculator.asmx", new StringContent(soapEnvelope, Encoding.UTF8, "text/xml"));
                var responseString = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(responseString);
                var result = doc.Descendants(XName.Get($"{operation}Result", "http://tempuri.org/"))
                                        .FirstOrDefault()?.Value;

                if (int.TryParse(result, out _))
                {
                    return result;
                }
                else
                {
                    return "An error occured";
                }


            }
        }


        public async Task<string> Add(string a, string b)
        {
            return await SendSoapRequest("Add", a, b);
        }

        public async Task<string> Subtract(string a, string b)
        {
            return await SendSoapRequest("Subtract", a, b);
        }

        public async Task<string> Multiply(string a, string b)
        {
            return await SendSoapRequest("Multiply", a, b);
        }

        public async Task<string> Divide(string a, string b)
        {
            return await SendSoapRequest("Divide", a, b);
        }
    }
}
