using System;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace Burrow.Extras.Internal
{
    internal class PriorityMessageHandler : DefaultMessageHandler
    {
        public PriorityMessageHandler(IConsumerErrorHandler consumerErrorHandler,
                                      Func<BasicDeliverEventArgs, Task> jobFactory,
                                      IRabbitWatcher watcher) : base(consumerErrorHandler, jobFactory, watcher)
        {
        }

        internal static int GetMsgPriority(BasicDeliverEventArgs eventArg)
        {
            var priority = -1;
            if (eventArg.BasicProperties.Headers != null && eventArg.BasicProperties.Headers.Contains("Priority"))
            {
                //It's a byte, has to convert to char
                var enc = new System.Text.UTF8Encoding();
                string str = enc.GetString((byte[]) eventArg.BasicProperties.Headers["Priority"]);
                int.TryParse(str, out priority);
            }
            return priority;
        }
    }
}