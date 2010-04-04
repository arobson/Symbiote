﻿using Machine.Specifications;

namespace Relax.Tests.URI
{
    [Subject("Couch URI")]
    public class when_getting_attachment_including_revision : with_basic_uri
    {
        private Because of = () => uri.Key("id").Attachment("foo.txt").Revision("1-A");

        private It should_append_attachment
            = () => uri.ToString().ShouldEqual(@"http://localhost:5984/test/id/foo.txt?rev=1-A");
    }
}