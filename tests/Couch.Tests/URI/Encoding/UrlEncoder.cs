﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Relax.Impl.Json;
using Symbiote.Core.Extensions;

namespace Relax.Tests.URI.Encoding
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

    public class when_encoding_json_for_url : with_test_json
    {
        protected static string result;
        protected static string expected = @"[""a"",12,""ab%2b%3d.%2c%5b%5d%3c%3e"",""123-123-4556"",""1979-06-17T00%3a00%3a00"",14.5,true]";

        private Because of = () =>
                                 {
                                     var encoder = new JsonUrlEncoder();
                                     result = encoder.Encode(testJson);
                                 };

        private It should_provide_expected_format = () => result.ShouldEqual(expected);
    }
}
