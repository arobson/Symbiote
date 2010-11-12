﻿/* 
Copyright 2008-2010 Alex Robson

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using Symbiote.Messaging.Impl.Channels;
using Symbiote.Messaging.Impl.Transform;
using Symbiote.Rabbit.Impl.Transform;

namespace Symbiote.Rabbit.Impl.Channels
{
    public class RabbitChannelDefinition
        : BaseChannelDefinition
    {
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public override Type ChannelType { get { return typeof(RabbitChannel); } }
        public override Type FactoryType { get { return typeof(RabbitChannelFactory); } }
        
        public RabbitChannelDefinition() : base()
        {
            OutgoingTransform = new Transformer().Then<RabbitSerializerTransform>();
            IncomingTransform = new Transformer().Then<RabbitSerializerTransform>();
        }
    }

    public class RabbitChannelDefinition<TMessage>
        : BaseChannelDefinition<TMessage>
    {
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public override Type ChannelType { get { return typeof(RabbitChannel<TMessage>); } }
        public override Type FactoryType { get { return typeof(RabbitChannelFactory<TMessage>); } }

        public RabbitChannelDefinition()
            : base()
        {
            OutgoingTransform = new Transformer().Then<RabbitSerializerTransform>();
            IncomingTransform = new Transformer().Then<RabbitSerializerTransform>();
        }
    }
}
