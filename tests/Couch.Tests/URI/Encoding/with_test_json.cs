﻿using System;
using Machine.Specifications;
using Symbiote.Core.Serialization;

namespace Couch.Tests.URI.Encoding
{
    public abstract class with_test_json
    {
        protected static object[] keySource;
        protected static string testJson;

        private Establish context = () =>
                                        {
                                            keySource = new object[] {"a", 12, "ab+=.,[]<>", "123-123-4556", DateTime.Parse("6/17/1979"), 14.5,true};
                                            testJson = keySource.ToJson(false);
                                        };
    }
}
