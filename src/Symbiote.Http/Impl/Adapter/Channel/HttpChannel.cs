﻿// /* 
// Copyright 2008-2011 Alex Robson
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// */
using System;
using Symbiote.Core.Futures;
using Symbiote.Messaging;
using Symbiote.Messaging.Impl.Channels;

namespace Symbiote.Http.Impl.Adapter.Channel
{
    public class HttpChannel :
        IChannel
    {
        public IChannelAdapter Adapter { get; set; }
        public ChannelDefinition ChannelDefinition { get; set; }

        #region IChannel Members

        public string Name { get; set; }

        public Future<TReply> ExpectReply<TReply, TMessage>( TMessage message )
        {
            return Adapter.ExpectReply<TReply, TMessage>( message, x => { } );
        }

        public Future<TReply> ExpectReply<TReply, TMessage>( TMessage message, Action<IEnvelope> modifyEnvelope )
        {
            return Adapter.ExpectReply<TReply, TMessage>( message, modifyEnvelope );
        }

        public void Send<TMessage>( TMessage message )
        {
            Adapter.Send( message );
        }

        public void Send<TMessage>( TMessage message, Action<IEnvelope> modifyEnvelope )
        {
            Adapter.Send( message, modifyEnvelope );
        }

        #endregion

        public HttpChannel( ChannelDefinition channelDefinition )
        {
            ChannelDefinition = channelDefinition;
            Adapter = new WebClientChannelAdapter( channelDefinition );
        }
    }
}