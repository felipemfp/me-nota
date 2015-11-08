using Microsoft.Phone.Notification;

namespace MeNota.Aplicativo
{
    public class Notificacao
    {
        private const string channelName = "channelMeNota";

        private HttpNotificationChannel _HttpChannel { get; set; }

        public HttpNotificationChannel HttpChannel
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
