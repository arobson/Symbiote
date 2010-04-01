﻿using System;
using Machine.Specifications;

namespace Relax.Tests.Repository
{
    public class when_getting_a_document_by_key : with_get_document_by_key_command
    {
        private static Exception exception = null;
        private static TestDocument record;
        private Because of = () =>
                                 {
                                     exception = Catch.Exception(
                                         () => record = repository.Get<TestDocument>(id)
                                         );
                                 };

        private It should_get_document_without_exception = () => exception.ShouldBeNull();
        private It should_get_record = () =>
                                           {
                                               record.DocumentId.ShouldEqual(id.ToString());
                                               record.Message.ShouldEqual("Hello");
                                               record.DocumentRevision.ShouldEqual("1");
                                           };
        private It should_call_get_correctly = () => commandMock.Verify(x => x.Get(couchUri));
    }
}