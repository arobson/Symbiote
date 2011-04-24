﻿using System;
using Machine.Specifications;
using Moq;
using Symbiote.Couch.Impl.Http;

namespace Couch.Tests.Repository
{
    public abstract class with_delete_document_command_by_id_and_rev : with_document_repository
    {
        protected static Guid id;
        protected static Mock<IHttpAction> commandMock;

        private Establish context = () =>
                                        {
                                            commandMock = new Mock<IHttpAction>();
                                            id = Guid.NewGuid();
                                            uri = new CouchUri("http", "localhost", 5984, "symbiotecouch").IdAndRev(id, "1");
                                            commandMock.Setup(x => x.Delete(couchUri));
                                            WireUpCommandMock(commandMock.Object);
                                        };
    }

    public abstract class with_get_by_keys : with_document_repository
    {
        
    }
}