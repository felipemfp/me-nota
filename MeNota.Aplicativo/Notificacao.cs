using Microsoft.Phone.Notification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MeNota.Aplicativo
{
    public class Notificacao
    {
        private const string channelName = "channelMeNota";

        private static HttpNotificationChannel _HttpChannel { get; set; } 

        public static HttpNotificationChannel HttpChannel
        {
            get
            {
                if (_HttpChannel == null)
                {
                    _HttpChannel = HttpNotificationChannel.Find(channelName);
                    if (_HttpChannel == null)
                    {
                        _HttpChannel = new HttpNotificationChannel(channelName);
                    }
                }
                return _HttpChannel;
            }
            set
            {
                _HttpChannel = value;
            }
        }        
    }
}
