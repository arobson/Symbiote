﻿using System;
using Machine.Specifications;
using Symbiote.Core;
using Symbiote.Core.Serialization;

namespace Couch.Tests.Document
{
    public class when_working_with_complex_couch_document
    {
        protected static string newRev = "01";
        protected static Guid newId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        protected static TestDocument simpleDocument;

        private Establish context = () =>
                                        {
                                            Assimilate
                                                .Initialize();
                                            simpleDocument = new TestDocument() {Message = "test"};
                                        };

        private Because of = () =>
                                 {
                                     simpleDocument.UpdateKeyFromJson(newId.ToJson(false));
                                     simpleDocument.UpdateRevFromJson(newRev);
                                 };

        private It get_document_id_should_return_id = () => 
            simpleDocument.GetDocumentId().ShouldEqual(simpleDocument.DocumentId);

        private It get_doc_id_as_json_should_return_id_to_string =
            () => simpleDocument.DocumentId.ToString().ShouldEqual(simpleDocument.GetDocumentIdAsJson());

        private It should_have_new_id = () => simpleDocument.DocumentId.ShouldEqual(newId);
        private It should_have_new_rev = () => simpleDocument.DocumentRevision.ShouldEqual(newRev);
    }
}
