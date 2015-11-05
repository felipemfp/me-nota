using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MeNota.Aplicativo.Resources;
using Newtonsoft.Json;
using Microsoft.Phone.Notification;
using System.Net.Http;
using System.Text;

namespace MeNota.Aplicativo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
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
                var response = await httpClient.GetAsync("api/usuario?nome="+nome);
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
            // Delegates para atualização, erro e recebimento de mensagem
            Notificacao.HttpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
            Notificacao.HttpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ErrorOccurred);
            Notificacao.HttpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);

            // Abre o canal e efetiva a ligação dos recebimentos
            Notificacao.HttpChannel.Open();
            Notificacao.HttpChannel.BindToShellToast();
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
            Dispatcher.BeginInvoke(() => MessageBox.Show("Um erro com o sistema de Notificação ocorreu.", "Reinicie o aplicativo", MessageBoxButton.OK));            
        }

        private void httpChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            string relativeUri = string.Empty;

            // Parse out the information that was part of the message.
            foreach (string key in e.Collection.Keys)
            {
                if (string.Compare(key, "wp:Param",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.CompareOptions.IgnoreCase) == 0)
                {
                    relativeUri = e.Collection[key];
                }
            }

            // Display a dialog of all the fields in the toast.
            Dispatcher.BeginInvoke(() =>
            {
                //MessageBox.Show(message.ToString());
                //listMsg.Items.Add(e.Collection["wp:Text1"] + ": " + e.Collection["wp:Text2"]);
                NavigationService.Navigate(new Uri(relativeUri, UriKind.Relative));
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

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}