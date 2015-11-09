using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
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
            if (usuario != null)
            {
                lblUsuario.Text = "@" + usuario.Nome;
            }
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
            if (usuario == null)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            if (String.IsNullOrWhiteSpace(mensagem))
            {
                MessageBox.Show("Digite uma mensagem...");
            }
            else
            {
                try
                {
                    HttpClient httpClient = Servico.Instanciar();
                    var response = await httpClient.GetAsync($"notificacao/enviar?remetente={usuario.Id}&destinatario={usuarioAlvo.Id}&mensagem={mensagem}");
                    var strJson = response.Content.ReadAsStringAsync().Result;
                    dynamic retorno = JsonConvert.DeserializeObject(strJson);
                    if (retorno["Flag"] == true)
                    {
                        MessageBox.Show((string)retorno["Mensagem"]);
                        txtMensagem.Text = String.Empty;
                    }
                    else
                    {
                        MessageBox.Show((string)retorno["Mensagem"]);
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