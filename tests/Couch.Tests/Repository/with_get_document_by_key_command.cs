﻿using System;
using Machine.Specifications;
using Symbiote.Couch.Impl.Http;
using Symbiote.Core.Extensions;

namespace Couch.Tests.Repository
{
    public abstract class with_get_document_by_key_command : with_document_repository
    {
        protected static Guid id;

        private Establish context = () =>
                                        {
                                            id = Guid.NewGuid();
                                            uri = new CouchUri("http", "localhost", 5984, "symbiotecouch").Id(id);
                                            commandMock
                                                .Setup(x => x.Get(couchUri))
                                                .Returns(@"{{_id : ""{0}"", _rev : ""1"", Message : ""Hello"" }}".AsFormat(id));
                                            WireUpCommandMock(commandMock.Object);
                                        };
    }
}