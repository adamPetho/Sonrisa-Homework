using Sonrisa_Homework_Webhooks.Models;
using System.Threading.Channels;

namespace Sonrisa_Homework_Webhooks.Services
{

    public sealed class MessageQueue : IMessageQueue
    {
        private readonly Channel<Notification> _channel =
            Channel.CreateUnbounded<Notification>();

        public ValueTask EnqueueAsync(
            Notification notification,
            CancellationToken cancellationToken)
        {
            return _channel.Writer.WriteAsync(
                notification,
                cancellationToken);
        }

        public ValueTask<Notification> DequeueAsync(
            CancellationToken cancellationToken)
        {
            return _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
