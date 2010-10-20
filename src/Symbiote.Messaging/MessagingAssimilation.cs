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
using System.Linq;
using System.Reflection;
using Symbiote.Core;
using Symbiote.Core.Extensions;
using Symbiote.Messaging.Impl;
using Symbiote.Messaging.Impl.Actors;
using Symbiote.Messaging.Impl.Channels;
using Symbiote.Messaging.Impl.Dispatch;
using Symbiote.Messaging.Impl.Serialization;
using Symbiote.Messaging.Impl.Subscriptions;

namespace Symbiote.Messaging
{
    public static class MessagingAssimilation
    {
        public static IAssimilate Messaging(this IAssimilate assimilate)
        {
            assimilate.Dependencies(x =>
                                        {
                                            x.Scan(
                                                s =>
                                                    {
                                                        IEnumerableExtenders.ForEach<Assembly>(AppDomain
                                                                                   .CurrentDomain
                                                                                   .GetAssemblies()
                                                                                   .Where(
                                                                                       a =>
                                                                                       a.GetReferencedAssemblies().Any(
                                                                                           r => r.FullName.Contains("Symbiote.Messaging"))), s.Assembly);
                                                        s.ConnectImplementationsToTypesClosing(
                                                            typeof (IHandle<>));
                                                        s.ConnectImplementationsToTypesClosing(
                                                            typeof (IHandle<,>));
                                                        s.ConnectImplementationsToTypesClosing(
                                                            typeof (IActorFactory<>));
                                                    });
                                            x.For<IBus>().Use<Bus>();
                                            x.For<IChannelManager>().Use<ChannelManager>().AsSingleton();
                                            x.For<IDispatcher>().Use<DispatchManager>().AsSingleton();
                                            x.For<ISubscriptionManager>().Use<SubscriptionManager>().AsSingleton();
                                            x.For<IAgency>().Use<Agency>().AsSingleton();
                                            x.For<IActorCache>().Use<InMemoryActorCache>().AsSingleton();

                                            x.For<IKeyAccessor>().Use<DefaultKeyAccessor>();
                                            //x.For<IMessageSerializer>().Use<JsonMessageSerializer>();
                                            //x.For<IMessageSerializer>().Use<NetBinarySerializer>();
                                            x.For<IMessageSerializer>().Use<ProtobufMessageSerializer>();
                                            x.For(typeof (IAgent<>)).Use(typeof (DefaultAgent<>));
                                            x.For(typeof (IActorStore<>)).Use(typeof (NullActorStore<>));
                                            x.For(typeof (IActorFactory<>)).Use(typeof (DefaultActorFactory<>));
                                        });

            var handlers =
                Assimilate
                    .Assimilation
                    .DependencyAdapter
                    .RegisteredPluginTypes
                    .Where(x => typeof(IHandle).IsAssignableFrom(x) || x.IsAssignableFrom(typeof(IHandle)));

            var dispatcherPairs = handlers
                .SelectMany(h =>
                                {
                                    var interfaces = h.GetInterfaces().Where(t => t.Name == "IHandle`1");
                                    return interfaces
                                        .Select(handler =>
                                                    {
                                                        var messageType = handler.GetGenericArguments()[0];
                                                        var dispatchInterface = typeof(IDispatchMessage<>).MakeGenericType(messageType);
                                                        var dispatchType = typeof(MessageDispatcher<>).MakeGenericType(messageType);
                                                        return Tuple.Create(dispatchInterface, dispatchType);
                                                    });
                                });
            var actorDispatcherPairs = handlers
                .SelectMany(h =>
                                {
                                    var interfaces = h.GetInterfaces().Where(t => t.Name == "IHandle`2");
                                    return interfaces
                                        .Select(handler =>
                                                    {
                                                        var messageType = handler.GetGenericArguments()[1];
                                                        var actorType = handler.GetGenericArguments()[0];
                                                        var dispatcherInterface =
                                                            typeof (IDispatchMessage<,>).MakeGenericType(actorType,
                                                                                                         messageType);
                                                        var dispatcherType =
                                                            typeof (ActorMessageDispatcher<,>).MakeGenericType(
                                                                actorType,
                                                                messageType);
                                                        return Tuple.Create(dispatcherInterface, dispatcherType);
                                                    });
                                });

            var simpleInterface = typeof(IDispatchMessage);
            Assimilate.Dependencies(x =>
                                        {
                                            dispatcherPairs.ForEach(p =>
                                                                        {
                                                                            x.For(p.Item1).Use(p.Item2);
                                                                            x.For(simpleInterface).Add(p.Item2);
                                                                        });
                                            actorDispatcherPairs.ForEach(p =>
                                                                             {
                                                                                 x.For(p.Item1).Use(p.Item2);
                                                                                 x.For(simpleInterface).Add(p.Item2);
                                                                             });
                                        });

            return assimilate;
        }
    }
}