﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbiote.Core.Hashing;
using Symbiote.Core.Utility;

namespace Symbiote.Messaging.Impl.Node
{
    public interface INodeConfiguration
    {
        string StandardExchange { get; set; }
    }

    public class NodeConfiguration
        : INodeConfiguration
    {
        public string StandardExchange { get; set; }
    }

    public interface INode
    {
        void Publish<T>( T message );
        Future<R> Request<T, R>( T message );
    }

    public class Node
        : INode
    {
        public INodeRegistry Nodes { get; set; }
        public IBus Bus { get; set; }
        public INodeConfiguration Configuration { get; set; }

        public void Initialize()
        {
            
        }

        public void Publish<T>( T message )
        {
            
        }

        public Future<R> Request<T, R>( T message )
        {
            return default( Future<R> );
        }

        public Node( INodeRegistry nodes, IBus bus, INodeConfiguration configuration )
        {
            Nodes = nodes;
            Bus = bus;
            Configuration = configuration;
            Initialize();
        }
    }

    public class NodeUp
    {
        public string NodeId { get; set; }
    }

    public class NodeDown
    {
        public string NodeId { get; set; }
    }

    public class NodeChangeHandler :
        IHandle<NodeUp>,
        IHandle<NodeDown>
    {
        protected INodeRegistry Registry { get; set; }

        public void Handle( IEnvelope<NodeUp> envelope )
        {
            Registry.AddNode( envelope.Message.NodeId );
        }

        public void Handle( IEnvelope<NodeDown> envelope )
        {
            Registry.RemoveNode(envelope.Message.NodeId);
        }

        public NodeChangeHandler( INodeRegistry registry )
        {
            Registry = registry;
        }
    }

    public interface INodeRegistry
    {
        void AddNode( string NodeId );
        void RemoveNode( string NodeId );
        string GetNodeFor<T>( T value );
    }

    public class NodeRegistry
        : INodeRegistry
    {
        protected Distributor<string> Nodes { get; set; }

        public void AddNode( string NodeId )
        {
            Nodes.AddNode( NodeId, NodeId );
        }

        public void RemoveNode( string NodeId )
        {
            Nodes.RemoveNode( NodeId );
        }

        public string GetNodeFor<T>( T value )
        {
            return Nodes.GetNode( value );
        }

        public NodeRegistry()
        {
            Nodes = new Distributor<string>( 1000 );
        }
    }
}
