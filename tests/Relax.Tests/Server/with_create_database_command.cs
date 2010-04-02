﻿using Machine.Specifications;
using Relax.Tests.Repository;
using Symbiote.Relax.Impl;

namespace Relax.Tests.Server
{
    public abstract class with_create_database_command : with_couch_server
    {
        private Establish context = () =>
                                        {
                                            uri = new CouchUri("http", "localhost", 5984, "testdocument");
                                            commandMock.Setup(x => x.Put(couchUri));
                                            WireUpCommandMock(commandMock.Object);
                                        };
    }
}