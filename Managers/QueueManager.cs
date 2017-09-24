using System.Text;
using RabbitMQ.Client;
using System.Configuration;
using Newtonsoft.Json;
using SocketServer.Handlers.Queue.Messages;
using System;
using System.Threading;

namespace Managers
{
    public class QueueManager : ManagerBase<QueueManager>
    {
        public ConnectionFactory Factory { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            Factory = new ConnectionFactory();
            Factory.HostName = ConfigurationManager.AppSettings["rabbit_host"];
            Factory.Port = int.Parse(ConfigurationManager.AppSettings["rabbit_port"]);
            Factory.UserName = ConfigurationManager.AppSettings["rabbit_username"];
            Factory.Password = ConfigurationManager.AppSettings["rabbit_password"];
            Factory.Protocol = Protocols.AMQP_0_9_1;
            Factory.VirtualHost = "/";
        }

        public void Queue(string exchange, byte[] data, string route = "")
        {
            try
            {
                using (var connection = Factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        properties.DeliveryMode = 1;
                        channel.BasicPublish(exchange, route, properties, data);
                    }
                }
            }
            catch (Exception)
            {
                Thread.Sleep(100);
                Queue(exchange, data, route);
            }
        }

        public void Queue(string exchange, string data, string route = "")
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            Queue(exchange, bytes, route);
        }

        public void Queue(string exhange, BaseMessage data, string route = "")
        {
            var content = JsonConvert.SerializeObject(data);
            Queue(exhange, content, route);
        }
    }
}
