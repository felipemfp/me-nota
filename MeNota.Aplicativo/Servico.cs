using System;
using System.Net.Http;

namespace MeNota.Aplicativo
{
    public class Servico
    {
        private const string baseAddress = "http://10.22.0.189/20121011110064/";//"http://localhost:62539/";

        public static HttpClient Instanciar()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            return httpClient;
        }
    }
}
