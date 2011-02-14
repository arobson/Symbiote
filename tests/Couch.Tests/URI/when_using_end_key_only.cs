﻿using System;
using Machine.Specifications;

namespace Couch.Tests.URI
{
    [Subject("Couch URI")]
    public class when_using_end_key_only : with_basic_uri
    {
        private static DateTime date = DateTime.Now;
        private static object complexKey;

        private Because of = () =>
                                 {
                                     complexKey = new object[] {"test", 10, "+"};
                                     uri.EndKey(complexKey);
                                 };

        private It should_append_reduce_false
            = () => uri.ToString().ShouldEqual(@"http://localhost:5984/symbiotecouch?endkey=[""test"",10,""%2b""]");
    }
}