using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MeNota.ServicoRest.Controllers
{
    public class NotificacaoController : Controller
    {
        // GET: Notificacao
        public ActionResult Index()
        {
            return View();
        }

        // GET: Notificacao/Enviar?remetente=5&destinatario=1&mensagem=HelloWorld
        public ActionResult Enviar(int remetente, int destinatario, string mensagem) {
            Models.Usuario usuario;
            Models.Usuario usuarioAlvo;

            using (var c = new Models.DbMeNotaDataContext())
            {
                usuario = c.Usuarios.SingleOrDefault(u => u.Id == remetente);
                usuarioAlvo = c.Usuarios.SingleOrDefault(u => u.Id == destinatario);
            }
            
            if (String.IsNullOrWhiteSpace(mensagem))
            {
                Debug.WriteLine("[ME_NOTA] Mensagem vazia");
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
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(msgBytes, 0, msgBytes.Length);
                    }

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string notificationStatus = response.Headers["X-NotificationStatus"];
                    string notificationChannelStatus = response.Headers["X-SubscriptionStatus"];
                    string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];

                    if (notificationStatus == "Received" && notificationChannelStatus == "Active" && deviceConnectionStatus == "Connected")
                    {
                        Debug.WriteLine("[ME_NOTA] Mensagem enviada.");
                    }
                    else
                    {
                        Debug.WriteLine($"[ME_NOTA] @{usuarioAlvo.Nome} está desconectado.");
                    }
                }
                catch
                {
                    Debug.WriteLine("[ME_NOTA] Ocorreu um erro.");
                }
            }
            return RedirectToAction("Index");
        }

    }
}