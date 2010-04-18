﻿using System;
using Machine.Specifications;
using Symbiote.Core.Extensions;
using Symbiote.Relax.Impl;

namespace Relax.Tests.Repository
{
    public abstract class with_save_model_command : with_test_document
    {
        protected static Guid id;
        protected static string originalDocument;

        private Establish context = () =>
                                        {
                                            id = Guid.NewGuid();
                                            document = new TestDocument()
                                                           {
                                                               DocumentId = id,
                                                               Message = "Hello",
                                                               DocumentRevision = "2"
                                                           };
                                            originalDocument = document.ToJson(false);

                                            uri = new CouchUri("http", "localhost", 5984, "testdocument")
                                                .Id(id);
                                            commandMock.Setup(x => x.Put(couchUri, document.ToJson(false)))
                                                .Returns("{{ ok : \"true\", id : \"{0}\", rev : \"3\" }}".AsFormat(id));
                                            WireUpCommandMock(commandMock.Object);
                                        };
    }
}