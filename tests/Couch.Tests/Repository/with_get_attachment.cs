﻿using System;
using System.Text;
using Machine.Specifications;
using Moq;
using Symbiote.Core;
using Symbiote.Couch;
using Symbiote.Couch.Impl.Http;
using Symbiote.Couch.Impl.Repository;

namespace Couch.Tests.Repository
{
    public abstract class with_get_attachment : with_configuration
    {
        protected static Tuple<string, byte[]> attachment;
        protected static string content;
        protected static Mock<IHttpAction> commandMock;
        protected static CouchUri uri;
        protected static IDocumentRepository repository;
        protected static CouchUri couchUri 
        {
            get
            {
                return Moq.It.Is<CouchUri>(u => u.ToString().Equals(uri.ToString()));
            }
        }

        private Establish context = () =>
                                        {
                                            repository = Assimilate.GetInstanceOf<DocumentRepository>();
                                            uri = new CouchUri("http", "localhost", 5984, "symbiotecouch")
                                                .Id("1")
                                                .Attachment("myattachment");

                                            content = "This is some content. Huzzah.";
                                            var bytes = Encoding.UTF8.GetBytes(content);
                                            attachment = Tuple.Create("myattachment", bytes);
                                            commandMock = new Mock<IHttpAction>();
                                            commandMock.Setup(x => x.GetAttachment( couchUri ))
                                                .Returns(attachment);

                                            WireUpCommandMock(commandMock.Object);
                                        };
    }
}