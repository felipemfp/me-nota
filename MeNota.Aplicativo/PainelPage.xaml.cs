using Microsoft.Phone.Controls;
using Microsoft.Phone.Notification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MeNota.Aplicativo
{
    public partial class PainelPage : PhoneApplicationPage
    {      
        private Models.Usuario usuario = (Application.Current as App).Usuario;

        public PainelPage()
        {
            InitializeComponent();
            lblUsuario.Text = "@" + usuario.Nome;
            ListarGrupos();
            ListarUsuarios();

            // Iniciar canal
            HttpNotificationChannel pushChannel;
            string channelName = "MeNotaChannel";
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

            message = $"{e.Collection["wp:Text1"]} diz: {e.Collection["wp:Text2"]}";

            Dispatcher.BeginInvoke(() => {
                if (MessageBox.Show(message, "Nova mensagem", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    NavigationService.Navigate(new Uri(relativeUri, UriKind.Relative));
                }
            });
        }

        public async void ListarUsuarios()
        {
            var httpClient = Servico.Instanciar();
            var response = await httpClient.GetAsync("api/usuario");
            var strJson = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> lst = JsonConvert.DeserializeObject<List<Models.Usuario>>(strJson);

            lst.Remove(lst.Single(u => u.Id == usuario.Id));

            lbxUsuarios.ItemsSource = lst;
        }

        public async void ListarGrupos()
        {
            var httpClient = Servico.Instanciar();
            var response = await httpClient.GetAsync("api/grupo?usuario=" + usuario.Id);
            var strJson = response.Content.ReadAsStringAsync().Result;
            List<Models.Grupo> lstGrupo = JsonConvert.DeserializeObject<List<Models.Grupo>>(strJson);

            var responseAdmin = await httpClient.GetAsync("api/grupo?admin=" + usuario.Id);
            var strJsonAdmin = responseAdmin.Content.ReadAsStringAsync().Result;
            List<Models.Grupo> lstGrupoAdmin = JsonConvert.DeserializeObject<List<Models.Grupo>>(strJsonAdmin);

            lbxGrupos.ItemsSource = lstGrupo;
            lbxMeusGrupos.ItemsSource = lstGrupoAdmin;
        }

        private async void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            var descricao = txtDescricaoGrupo.Text;
            if (!String.IsNullOrWhiteSpace(descricao))
            {
                var httpClient = Servico.Instanciar();

                Models.Grupo g = new Models.Grupo
                {
                    Descricao = descricao,
                    IdAdm = usuario.Id
                };

                List<Models.Grupo> lst = new List<Models.Grupo>();

                lst.Add(g);

                string json = "=" + JsonConvert.SerializeObject(lst);

                var content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");

                await httpClient.PostAsync("api/grupo", content);

                txtDescricaoGrupo.Text = String.Empty;
                pnrPrincipal.DefaultItem = (PanoramaItem)pnrPrincipal.Items[0];
                ListarGrupos();
            }
        }

        private void lbx_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var lbx = (ListBox)sender;
            NavigationService.Navigate(new Uri("/GrupoPage.xaml?grupo=" + (lbx.SelectedItem as Models.Grupo).Id, UriKind.Relative));
        }

        private void btnPerfil_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PerfilPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            usuario = (Application.Current as App).Usuario;
            lblUsuario.Text = "@" + usuario.Nome;
            base.OnNavigatedTo(e);
        }

        private void lbxUsuario_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var lbx = (ListBox)sender;
            var usuarioAlvo = (Models.Usuario)lbx.SelectedItem;

            NavigationService.Navigate(new Uri("/UsuarioPage.xaml?usuario=" + usuarioAlvo.Id, UriKind.Relative));
        }

        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            var nome = txtNomeUsuario.Text;
            if (!String.IsNullOrWhiteSpace(nome))
            {
                var httpClient = Servico.Instanciar();
                var response = await httpClient.GetAsync("api/usuario?nome=" + nome);
                var strJson = response.Content.ReadAsStringAsync().Result;
                List<Models.Usuario> lstUsuario = JsonConvert.DeserializeObject<List<Models.Usuario>>(strJson);
                lbxUsuarios.ItemsSource = lstUsuario;
            }
        }

        private void txtNomeUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtNomeUsuario.Text))
            {
                ListarUsuarios();
            }
        }
    }
}