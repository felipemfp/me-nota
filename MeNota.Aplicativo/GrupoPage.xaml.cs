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
    public partial class GrupoPage : PhoneApplicationPage
    {
        private Models.Usuario usuario = (Application.Current as App).Usuario;
        private Models.Grupo grupo;
        private List<Models.Usuario> membros;

        public GrupoPage()
        {
            InitializeComponent();
        }

        private async Task<bool> RecuperarGrupo(int id)
        {
            var httpClient = Servico.Instanciar();
            var response = await httpClient.GetAsync("api/grupo/" + id);
            var strJson = response.Content.ReadAsStringAsync().Result;
            List<Models.Grupo> lst = JsonConvert.DeserializeObject<List<Models.Grupo>>(strJson);
            if (lst.Count == 1)
            {
                grupo = lst[0];
                return true;
            }
            return false;
        }

        private async Task<string> RecuperarNomeUsuario(int id)
        {
            var httpClient = Servico.Instanciar();
            var response = await httpClient.GetAsync("api/usuario/" + id);
            var strJson = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> lst = JsonConvert.DeserializeObject<List<Models.Usuario>>(strJson);
            if (lst.Count == 1)
            {
                return lst[0].Nome;
            }
            return String.Empty;
        }

        private async Task<List<Models.Usuario>> RecuperarMembros()
        {
            var httpClient = Servico.Instanciar();
            var response = await httpClient.GetAsync("api/usuario?grupo=" + grupo.Id);
            var strJson = response.Content.ReadAsStringAsync().Result;
            membros = JsonConvert.DeserializeObject<List<Models.Usuario>>(strJson);
            membros.Remove(membros.Single(m=>m.Id == usuario.Id));
            return membros;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("grupo"))
            {
                var id = int.Parse(dic["grupo"]);

                if (await RecuperarGrupo(id))
                {
                    pnrPrincipal.Title = grupo.Descricao;
                    txtDescricao.Text = grupo.Descricao;
                    lblDescricao.Text = grupo.Descricao;
                    lbxUsuarios.ItemsSource = await RecuperarMembros();
                    if (grupo.IdAdm == usuario.Id)
                    {
                       lblAdministrador.Text = "@" + usuario.Nome;
                        lbxConfigUsuarios.ItemsSource = lbxUsuarios.ItemsSource;
                    }
                    else
                    {
                        pnrPrincipal.Items.Remove(pnrItemConfigGrupo);
                        pnrPrincipal.Items.Remove(pnrItemConfigMembros);
                        lblAdministrador.Text = "@" + await RecuperarNomeUsuario(grupo.IdAdm.Value);
                    }
                }
                else
                {
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }

                base.OnNavigatedTo(e);
            }
            else
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
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
                lbxConfigUsuarios.ItemsSource = lstUsuario;
            }
            else
            {
                lbxConfigUsuarios.ItemsSource = membros;
            }
        }

        private async void lbxConfigUsuarios_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var lbx = (ListBox)sender;
            var usuarioAlvo = lbx.SelectedItem as Models.Usuario;

            var acao = !membros.Contains(usuarioAlvo) ? "adicionar" : "remover";

            if (MessageBox.Show("Tem certeza que deseja " + acao + " @" + usuarioAlvo.Nome + "?", (char.ToUpper(acao[0]) + acao.Substring(1)), MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                var httpClient = Servico.Instanciar();
                Models.GrupoUsuario g = new Models.GrupoUsuario
                {
                    IdGrupo = grupo.Id,
                    IdUsuario = usuarioAlvo.Id
                };

                switch (acao)
                {
                    case "adicionar":
                        List<Models.GrupoUsuario> lst = new List<Models.GrupoUsuario>();
                        lst.Add(g);
                        string json = "=" + JsonConvert.SerializeObject(lst);
                        var content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");
                        await httpClient.PostAsync("api/grupousuario", content);
                        break;
                    case "remover":
                        await httpClient.DeleteAsync("api/grupousuario?grupo=" + g.IdGrupo + "&usuario=" + g.IdUsuario);
                        break;
                    default:
                        break;
                }

                lbxUsuarios.ItemsSource = await RecuperarMembros();
            }
        }

        private void txtNomeUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            var nome = txtNomeUsuario.Text;
            if (String.IsNullOrWhiteSpace(nome))
            {
                lbxConfigUsuarios.ItemsSource = membros;
            }
        }

        private async void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            var descricao = txtDescricao.Text;
            if (!String.IsNullOrWhiteSpace(descricao))
            {
                var httpClient = Servico.Instanciar();

                grupo.Descricao = descricao;

                string json = "=" + JsonConvert.SerializeObject(grupo);

                var content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");

                await httpClient.PutAsync("api/grupo/"+grupo.Id, content);

                pnrPrincipal.Title = grupo.Descricao;
                txtDescricao.Text = grupo.Descricao;
                lblDescricao.Text = grupo.Descricao;
            }
        }

        private async void btnApagar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja excluir o grupo " + grupo.Descricao + "?", "Excluir", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                var httpClient = Servico.Instanciar();
                await httpClient.DeleteAsync("api/grupo/" + grupo.Id);
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void lbxUsuarios_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var lbx = (ListBox)sender;
            var usuarioAlvo = (Models.Usuario)lbx.SelectedItem;

            NavigationService.Navigate(new Uri("/UsuarioPage.xaml?usuario="+usuarioAlvo.Id, UriKind.Relative));
        }
    }
}