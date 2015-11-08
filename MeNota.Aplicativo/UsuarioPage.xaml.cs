using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace MeNota.Aplicativo
{
    public partial class UsuarioPage : PhoneApplicationPage
    {
        
        private Models.Usuario usuario = (Application.Current as App).Usuario;
        private Models.Usuario usuarioAlvo;

        public UsuarioPage()
        {
            
            InitializeComponent();
            lblUsuario.Text = "@" + usuario.Nome;
        }

        private async Task<bool> RecuperarUsuarioAlvo(int id)
        {
            var httpClient = Servico.Instanciar();
            var response = await httpClient.GetAsync("api/usuario/" + id);
            var strJson = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> lst = JsonConvert.DeserializeObject<List<Models.Usuario>>(strJson);
            if (lst.Count == 1)
            {
                usuarioAlvo = lst[0];
                return true;
            }
            return false;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("usuario"))
            {
                int id = int.Parse(dic["usuario"]);
                if (await RecuperarUsuarioAlvo(id))
                {
                    lblUsuarioAlvo.Text = "@" + usuarioAlvo.Nome;
                }
                else
                {
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }
            else
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }

            if (dic.ContainsKey("mensagem"))
            {
                lblMensagem.Text = dic["mensagem"];
            }

            base.OnNavigatedTo(e);
        }

        private async void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            var mensagem = txtMensagem.Text;
            if (String.IsNullOrWhiteSpace(mensagem))
            {
                MessageBox.Show("Digite uma mensagem...");
            }
            else
            {
                try
                {
                    string xmlMensagem =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<wp:Notification xmlns:wp=\"WPNotification\">" +
                        "<wp:Toast>" +
                            "<wp:Text1>" + $"@{usuario.Nome}" + "</wp:Text1>" +
                            "<wp:Text2>" + mensagem + "</wp:Text2>" +
                            "<wp:Param>/UsuarioPage.xaml?usuario=" + usuarioAlvo.Id + "&mensagem="
                                + mensagem + "</wp:Param>" +
                        "</wp:Toast>" +
                    "</wp:Notification>";

                    byte[] msgBytes = Encoding.UTF8.GetBytes(xmlMensagem);

                    string uri = usuarioAlvo.Url;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "POST";
                    request.ContentType = "text/xml";
                    request.ContentLength = xmlMensagem.Length;
                    request.Headers["X-MessageID"] = Guid.NewGuid().ToString();
                    request.Headers["X-WindowsPhone-Target"] = "toast";
                    request.Headers["X-NotificationClass"] = "2";

                    // Envia a requisição
                    using (Stream requestStream = await request.GetRequestStreamAsync())
                    {
                        requestStream.Write(msgBytes, 0, msgBytes.Length);
                    }

                    HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                    string notificationStatus = response.Headers["X-NotificationStatus"];
                    string notificationChannelStatus = response.Headers["X-SubscriptionStatus"];
                    string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];

                    if (notificationStatus == "Received" && notificationChannelStatus == "Active" && deviceConnectionStatus == "Connected")
                    {
                        MessageBox.Show("Mensagem enviada.");
                        txtMensagem.Text = String.Empty;
                    }
                    else
                    {
                        MessageBox.Show($"@{usuarioAlvo.Nome} está desconectado.");
                    }
                }
                catch
                {
                    MessageBox.Show("Ocorreu um erro.");
                }
            }
        }
    }
}