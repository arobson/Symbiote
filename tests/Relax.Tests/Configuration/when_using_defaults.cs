﻿using Machine.Specifications;

namespace Relax.Tests.Configuration
{
    [Subject("Couch Configuration")]
    public class when_using_defaults : with_couch_configuration
    {
        private It should_use_http_as_protocol = () => configuration.Protocol.ShouldEqual("http");
        private It should_use_localhost_as_server = () => configuration.Server.ShouldEqual("localhost");
        private It should_use_port_5984 = () => configuration.Port.ShouldEqual(5984);
        private It should_not_require_preauthorization = () => configuration.Preauthorize.ShouldBeFalse();
        private It should_have_six_second_timeout = () => configuration.TimeOut.ShouldEqual(6000);
        private It should_not_use_cache = () => configuration.Cache.ShouldBeFalse();

        private It should_provide_default_couch_name_for_type = () =>
                                                                configuration
                                                                    .GetDatabaseNameForType<TestDocument>()
                                                                    .ShouldEqual("testingdoc");
    }
}