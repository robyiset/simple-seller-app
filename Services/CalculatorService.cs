
using System.Text;

namespace simple_seller_app.Services
{

    public class CalculatorService
    {
        private readonly string _baseUrl = "http://www.dneonline.com/calculator.asmx";

        private async Task<string> SendSoapRequest(string operation, string param1, string param2)
        {
            using (var client = new HttpClient())
            {
                // Build the SOAP request body
                var soapEnvelope = $@"
                <?xml version='1.0' encoding='utf-8'?>
                <soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
                               xmlns:xsd='http://www.w3.org/2001/XMLSchema'
                               xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                    <soap:Body>
                        <{operation} xmlns='http://tempuri.org/'>
                            <intA>{param1}</intA>
                            <intB>{param2}</intB>
                        </{operation}>
                    </soap:Body>
                </soap:Envelope>";

                // Create HTTP content with the SOAP envelope
                var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

                // Send the POST request to the web service
                var response = await client.PostAsync(_baseUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
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