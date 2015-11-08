using Microsoft.Phone.Controls;
using Microsoft.Phone.Notification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Navigation;

namespace MeNota.Aplicativo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ConfigPage.xaml", UriKind.Relative));
        }

        private async void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            var nome = txtNomeUsuario.Text;
            if (!String.IsNullOrWhiteSpace(nome))
            {
                var httpClient = Servico.Instanciar();
                var response = await httpClient.GetAsync("api/usuario?nome=" + nome);
                var strJson = response.Content.ReadAsStringAsync().Result;
                List<Models.Usuario> lstUsuario = JsonConvert.DeserializeObject<List<Models.Usuario>>(strJson);
                if (lstUsuario.Count == 1 && lstUsuario[0].Nome == nome)
                {
                    txtNomeUsuario.Text = String.Empty;
                    (Application.Current as App).Usuario = lstUsuario[0];
                    CanalEntrar();
                    NavigationService.Navigate(new Uri("/PainelPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Falha na autenticação.");
                }
            }
        }

        public void CanalEntrar()
        {
            HttpNotificationChannel httpChannel = new Notificacao().HttpChannel;

            // Delegates para atualização, erro e recebimento de mensagem
            httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
            httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ErrorOccurred);
            httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);

            // Abre o canal e efetiva a ligação dos recebimentos
            httpChannel.Open();
            httpChannel.BindToShellToast();
        }

        private async void AtualizarUsuario(Models.Usuario usuario)
        {
            var httpClient = Servico.Instanciar();

            string json = "=" + JsonConvert.SerializeObject(usuario);

            var content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");

            await httpClient.PutAsync("api/usuario/" + usuario.Id, content);
        }

        private void httpChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                // Mostra dados do canal
                (Application.Current as App).Usuario.Url = e.ChannelUri.ToString();
                AtualizarUsuario((Application.Current as App).Usuario);
            });
        }

        private void httpChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Mostra dados do erro
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show("Um erro com o sistema de Notificação ocorreu.", "Reinicie o aplicativo", MessageBoxButton.OK)
            );
        }

        private void httpChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            string remetente = string.Empty;
            string mensagem = string.Empty;
            string relativeUri = string.Empty;

            if (e.Collection.ContainsKey("wp:Param") && e.Collection.ContainsKey("wp:Remetente") && e.Collection.ContainsKey("wp:Mensagem"))
            {
                relativeUri = e.Collection["wp:Param"];
                remetente = e.Collection["wp:Remetente"];
                mensagem = e.Collection["wp:Mensagem"];

                // Display a dialog of all the fields in the toast.
                Dispatcher.BeginInvoke(() =>
                {
                    if (MessageBox.Show(String.Format("@{0} diz: {1}", remetente, mensagem), "Nova mensagem", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        NavigationService.Navigate(new Uri(relativeUri, UriKind.Relative));
                    }
                });
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((Application.Current as App).Usuario != null)
            {
                NavigationService.Navigate(new Uri("/PainelPage.xaml", UriKind.Relative));
            }
            else
            {
                base.OnNavigatedTo(e);
            }
        }
    }
}