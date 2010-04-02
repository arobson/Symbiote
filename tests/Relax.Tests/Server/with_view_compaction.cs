﻿using Machine.Specifications;
using Relax.Tests.Repository;
using Symbiote.Relax.Impl;

namespace Relax.Tests.Server
{
    public abstract class with_view_compaction : with_couch_server
    {
        private Establish context = () =>
                                        {
                                            uri = new CouchUri("http", "localhost", 5984, "testdocument")
                                                .CompactView("testView");
                                            commandMock.Setup(x => x.Post(couchUri));
                                            WireUpCommandMock(commandMock.Object);
                                        };
    }
}