﻿using Machine.Specifications;

namespace Couch.Tests.URI
{
    [Subject("Couch URI")]
    public class when_retrieving_design_document : with_basic_uri
    {
        private Because of = () => uri.Design("myDesign");

        private It should_append_design_document_to_uri
            = () => uri.ToString().ShouldEqual(@"http://localhost:5984/symbiotecouch/_design/myDesign");
    }
}