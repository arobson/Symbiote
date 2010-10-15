﻿using RabbitMQ.Client;
using Symbiote.Core;
using Symbiote.Messaging.Impl.Dispatch;
using Symbiote.Messaging.Impl.Serialization;
using Symbiote.Rabbit.Impl.Channels;

namespace Symbiote.Rabbit.Impl.Adapter
{
    public class RabbitQueueListener
        : DefaultBasicConsumer
    {
        protected IChannelProxy Proxy { get; set; }
        protected IDispatcher Dispatch { get; set; }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            Dispatch.Send(RabbitEnvelope.Create(
                Proxy, 
                consumerTag, 
                deliveryTag, 
                redelivered, 
                exchange, 
                routingKey, 
                properties,
                Assimilate.GetInstanceOf<IMessageSerializer>().Deserialize(body)));
        }

        public RabbitQueueListener(IChannelProxy proxy, IDispatcher dispatch)
        {
            Proxy = proxy;
            Dispatch = dispatch;
            proxy.InitConsumer(this);
        }
    }
}