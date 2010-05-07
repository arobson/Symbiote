﻿using System;
using Relax;
using Relax.Impl;
using Symbiote.Core;
using Symbiote.Core.Cache;

namespace Symbiote.Relax
{
    public static class RelaxAssimilation
    {
        public static IAssimilate Relax(this IAssimilate assimilate, Action<CouchConfigurator> configure)
        {
            var config = new CouchConfigurator();
            configure(config);
            var configuration = config.GetConfiguration();
            RelaxConfiguration.Configure(configuration);
            return assimilate;
        }
    }
}
