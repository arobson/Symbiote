﻿using System;
using Machine.Specifications;
using Symbiote.Core.Extensions;
using Symbiote.Relax.Impl;

namespace Relax.Tests.Repository
{
    public abstract class with_save_models_command : with_test_document
    {
        protected static Guid id;
        protected static string originalDocument;
        protected static string bulkSave;

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
                                            bulkSave = new BulkPersist<TestDocument>(true, false, new[] {document}).ToJson(false);
                                            uri = new CouchUri("http", "localhost", 5984, "testdocument").BulkInsert();
                                            var saveResponse = 
                                                new []
                                                    {
                                                        new SaveResponse() {Id = id.ToString(), Revision = "3", Success = true}
                                                    };

                                            commandMock.Setup(x => x.Post(couchUri, bulkSave))
                                                .Returns(saveResponse.ToJson(false));
                                            WireUpCommandMock(commandMock.Object);
                                        };
    }
}