﻿using Machine.Specifications;
using Symbiote.Core.Serialization;
using Symbiote.Couch.Impl.Commands;
using Symbiote.Couch.Impl.Json;

namespace Couch.Tests.Commands.SaveCommand
{
    public abstract class with_serialized_bulk_persist : with_single_parent_document
    {
        protected static BulkPersist persist;
        protected static string json;
        protected static ISaveDocuments command;

        private Establish context = () =>
                                        {
                                            persist = new BulkPersist(true, false, new [] {testDoc});
                                            json = persist.ToJson();
                                            command = factory.CreateSaveDocumentsCommand();
                                        };
    }
}
