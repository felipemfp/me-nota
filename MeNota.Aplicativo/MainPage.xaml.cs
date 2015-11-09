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



                    NavigationService.Navigate(new Uri("/PainelPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Falha na autenticação.");
                }
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