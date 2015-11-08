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

                    HttpNotificationChannel pushChannel;

                    string channelName = "MeNotaChannel";

                    InitializeComponent();

                    pushChannel = HttpNotificationChannel.Find(channelName);

                    if (pushChannel == null)
                    {
                        pushChannel = new HttpNotificationChannel(channelName);

                        pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                        pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                        pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                        pushChannel.Open();

                        pushChannel.BindToShellToast();
                    }
                    else
                    {
                        pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                        pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                        pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);
                        
                        (Application.Current as App).Usuario.Url = pushChannel.ChannelUri.ToString();
                        AtualizarUsuario((Application.Current as App).Usuario);
                    }

                    NavigationService.Navigate(new Uri("/PainelPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Falha na autenticação.");
                }
            }
        }

        private async void AtualizarUsuario(Models.Usuario usuario)
        {
            var httpClient = Servico.Instanciar();
            string json = "=" + JsonConvert.SerializeObject(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");
            await httpClient.PutAsync("api/usuario/" + usuario.Id, content);
        }

        private void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                (Application.Current as App).Usuario.Url = e.ChannelUri.ToString();
                AtualizarUsuario((Application.Current as App).Usuario);
            });
        }

        private void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show("Um erro com o sistema de Notificação ocorreu.", "Reinicie o aplicativo", MessageBoxButton.OK)
            );
        }

        private void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            string message = String.Empty;
            string relativeUri = string.Empty;            

            foreach (string key in e.Collection.Keys)
            {
                if (string.Compare(
                    key,
                    "wp:Param",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.CompareOptions.IgnoreCase) == 0)
                {
                    relativeUri = e.Collection[key];
                }
            }

            message = String.Format("{0} diz: {1}", e.Collection["Text1"], e.Collection["Text2"]);

            Dispatcher.BeginInvoke(() => {
                if (MessageBox.Show(message, "Nova mensagem", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    NavigationService.Navigate(new Uri(relativeUri, UriKind.Relative));
                }
            });
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