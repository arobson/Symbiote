﻿using System.Diagnostics;
using Machine.Specifications;
using Symbiote.Core.Serialization;
using Symbiote.Couch.Impl.Json;
using Newtonsoft.Json.Linq;

namespace Couch.Tests.Commands.SaveCommand
{
    public class when_scrubbing_bulk_persist_type_tokens : with_serialized_bulk_persist
    {
        protected static BulkPersist deserialized;
        protected static string scrubbedJson;
        protected static JObject scrubbedJsonObject;
        protected static ParentDoc deserializedDocument;
        protected static Stopwatch timer;

        private Because of = () =>
                                 {
                                     var saveDocumentsCommand = factory.CreateSaveDocumentOnlyListCommand();

                                     timer = Stopwatch.StartNew();
                                     scrubbedJson = saveDocumentsCommand.ScrubBulkPersistOfTypeTokens(json);
                                     timer.Stop();

                                     scrubbedJsonObject = JObject.Parse(scrubbedJson);
                                     deserialized = scrubbedJson.FromJson<BulkPersist>();
                                     deserializedDocument = deserialized.Documents[0] as ParentDoc;
                                 };

        private It should_not_produce_null = () => deserialized.ShouldNotBeNull();
        private It should_correctly_deserialize_documents = () => deserializedDocument.ShouldNotBeNull();
        private It should_have_simple_docs_array = () => scrubbedJsonObject["docs"].ShouldNotContain(new [] {new JProperty("$values")});
        private It should_take_less_than_25ms = () => timer.ElapsedMilliseconds.ShouldBeLessThan(25);
    }
}