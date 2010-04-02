﻿using Machine.Specifications;
using Symbiote.Core.Extensions;
using Symbiote.Relax.Impl;

namespace Relax.Tests.Repository
{
    public abstract class with_continuous_replication : with_document_repository
    {
        protected static CouchUri targetUri;
        protected static CouchUri sourceUri;
        protected static string replication;

        private Establish context = () =>
                                        {
                                            uri = new CouchUri("http", "localhost", 5984).Replicate();
                                            sourceUri = new CouchUri("http", "localhost", 5984, "testdocument");
                                            targetUri = new CouchUri("admin", "password", "http", "remotehost", 5984);

                                            replication = ReplicationCommand.Continuous(sourceUri, targetUri).ToJson(false);

                                            commandMock
                                                .Setup(x => x.Post(couchUri, replication));
                                            WireUpCommandMock(commandMock.Object);
                                        };
    }
}