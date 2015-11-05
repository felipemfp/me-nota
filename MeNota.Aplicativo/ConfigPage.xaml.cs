using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;

namespace MeNota.Aplicativo
{
    public partial class ConfigPage : PhoneApplicationPage
    {
        public ConfigPage()
        {
            InitializeComponent();
            ListarUsuarios();
        }

        public async void ListarUsuarios()
        {
            var httpClient = Servico.Instanciar();
            var response = await httpClient.GetAsync("api/usuario");
            var strJson = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> lstUsuario = JsonConvert.DeserializeObject<List<Models.Usuario>>(strJson);
            //lbxUsuarios.ItemsSource = lstUsuario.Select(u => new ListBoxItem { Content = "@" + u.Nome });
            lbxUsuarios.ItemsSource = lstUsuario;
        }

        private async void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            var nome = txtNomeUsuario.Text;
            if (!String.IsNullOrWhiteSpace(nome))
            {
                var httpClient = Servico.Instanciar();

                Models.Usuario u = new Models.Usuario {
                    Nome = nome
                };

                List<Models.Usuario> lst = new List<Models.Usuario>();

                lst.Add(u);

                string json = "=" + JsonConvert.SerializeObject(lst);

                var content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");

                await httpClient.PostAsync("api/usuario", content);

                txtNomeUsuario.Text = String.Empty;
                pnrConfig.DefaultItem = (PanoramaItem)pnrConfig.Items[0];
                ListarUsuarios();
            }
        }
    }
}