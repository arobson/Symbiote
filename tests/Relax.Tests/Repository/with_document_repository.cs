﻿using Machine.Specifications;
using Moq;
using Symbiote.Relax;
using Symbiote.Relax.Impl;

namespace Relax.Tests.Repository
{
    public abstract class with_document_repository : with_configuration
    {
        protected static IDocumentRepository repository;
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
                                            repository = new DocumentRepository(configuration, new CouchCommandFactory());
                                        };
    }
}