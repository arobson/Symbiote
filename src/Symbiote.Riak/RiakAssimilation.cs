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
using Symbiote.Core;
using Symbiote.Riak.Config;
using Symbiote.Riak.Impl;
using Symbiote.Riak.Impl.ProtoBuf;
using Symbiote.Riak.Impl.ProtoBuf.Connection;

namespace Symbiote.Riak
{
    public static class RiakAssimilation
    {
        public static IAssimilate Riak( this IAssimilate assimilate, Action<RiakConfigurator> configurate )
        {
            var configurator = new RiakConfigurator();
            configurate( configurator );

            Assimilate.Dependencies( x =>
                                         {
                                             x.For<IRiakConfiguration>().Use( configurator.Configuration );
                                             x.For<IConnectionFactory>().Use<ConnectionFactory>();
                                             x.For<IConnectionProvider>().Use<PooledConnectionProvider>().AsSingleton();
                                             x.For<ICommandFactory>().Use<ProtoBufCommandFactory>().AsSingleton();
                                             x.For<IConnectionPool>().Use<LockingConnectionPool>();
                                             x.For<IRiakClient>().Use<RiakClient>();
                                             x.For<IDocumentRepository>().Use<RiakClient>();
                                             x.For<IKeyValueStore>().Use<RiakClient>();
                                             x.For<ITrackVectors>().Use<VectorRegistry>().AsSingleton();
                                         } );

            return assimilate;
        }
    }
}