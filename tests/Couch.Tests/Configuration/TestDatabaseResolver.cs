﻿using Symbiote.Couch;

namespace Couch.Tests.Configuration
{
    public class TestDatabaseResolver : IResolveDatabaseNames
    {
        public string GetDatabaseNameFor<TModel>()
        {
            if (typeof(TModel).Equals(typeof(object)))
                return null;    
            return typeof (TModel).Name.ToLower();
        }
    }
}
