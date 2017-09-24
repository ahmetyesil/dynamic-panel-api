using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Managers;
using RabbitMQ.Client.Events;
using SocketServer.Handlers.Queue.Messages;
using Managers.Messages;
using System;
using System.Threading;
using System.Collections.Generic;

namespace SocketServer.Handlers.Queue
{
    public abstract class BaseQueueHandler<T> : BaseHandler where T : BaseMessage
    {
        private bool WaitingForStop = false;
        public bool IsWorking { get; set; }
        public abstract string QueueName { get; }

        public void StartListen()
        {
            IsWorking = true;
            while (!WaitingForStop)
            {
                try
                {
                    using (var connection = QueueManager.Instance.Factory.CreateConnection())
                    {
                        using (var channel = connection.CreateModel())
                        {
                            Queue<BasicGetResult> queue = new Queue<BasicGetResult>();
                            while (true)
                            {
                                try
                                {
                                    var message = channel.BasicGet(QueueName, false);
                                    if (message != null)
                                    {
                                        queue.Enqueue(message);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                catch
                                {
                                    break;
                                }
                            }
                            try
                            {
                                while (queue.Count > 0)
                                {
                                    var message = queue.Dequeue();
                                    var body = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
                                    DataReceivedEventArgs arg = new DataReceivedEventArgs();
                                    try
                                    {
                                        DataReceived(body, arg);
                                    }
                                    catch
                                    {

                                    }

                                    if (arg.IsCompleted)
                                    {
                                        channel.BasicAck(message.DeliveryTag, false);
                                    }
                                    else
                                    {
                                        channel.BasicNack(message.DeliveryTag, false, true);
                                    }
                                }
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                }
                catch
                {
                    continue;
                }
                Thread.Sleep(100);
            }
            IsWorking = false;
        }

        protected abstract void DataReceived(T message, DataReceivedEventArgs arg);

        public override void Dispose()
        {
            WaitingForStop = true;
        }
    }
}
