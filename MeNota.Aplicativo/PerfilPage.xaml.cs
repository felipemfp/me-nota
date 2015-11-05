using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace MeNota.Aplicativo
{
    public partial class PerfilPage : PhoneApplicationPage
    {
        private Models.Usuario usuario = (Application.Current as App).Usuario;

        public PerfilPage()
        {
            InitializeComponent();
            txtNomeUsuario.Text = usuario.Nome;
            txtUsuario.Text = "@" + usuario.Nome;
        }

        private async void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            var nome = txtNomeUsuario.Text;
            if (!String.IsNullOrWhiteSpace(nome))
            {
                var httpClient = Servico.Instanciar();

                usuario.Nome = nome;

                string json = "=" + JsonConvert.SerializeObject(usuario);

                var content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");
                
                await httpClient.PutAsync("api/usuario/" + usuario.Id, content);

                (Application.Current as App).Usuario = usuario;
                txtUsuario.Text ="@"+ usuario.Nome;
            }
        }

        private async void btnApagar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja excluir seu usuário?", "Excluir usuário", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                var httpClient = Servico.Instanciar();
                await httpClient.DeleteAsync("api/usuario/" + usuario.Id);
                (Application.Current as App).Usuario = null;
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }
    }
}