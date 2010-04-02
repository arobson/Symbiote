﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Moq;
using Symbiote.Relax;
using Symbiote.Relax.Impl;

namespace Relax.Tests.Server
{
    public abstract class with_couch_server : with_configuration
    {
        protected static ICouchServer server;
        protected static CouchUri uri;
        protected static Mock<ICouchCommand> commandMock;
        protected static CouchUri couchUri
        {
            get
            {
                return Moq.It.Is<CouchUri>(u => u.ToString().Equals(uri.ToString()));
            }
        }

        private Establish context = () =>
        {
            commandMock = new Mock<ICouchCommand>();
            server = new CouchDbServer(
                configuration, 
                new CouchCommandFactory(), 
                new DocumentRepository(configuration, new CouchCommandFactory()));
        };
    }
}
