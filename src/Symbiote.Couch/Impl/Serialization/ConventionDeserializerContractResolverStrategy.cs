﻿/* 
Copyright 2008-2010 Alex Robson

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using Symbiote.Core;
using Symbiote.Core.Extensions;
using Newtonsoft.Json.Serialization;
using Symbiote.Core.Serialization;

namespace Symbiote.Couch.Impl.Serialization
{
    public class ConventionDeserializerContractResolverStrategy : IContractResolverStrategy
    {
        public bool ResolverAppliesForSerialization(Type type)
        {
            return false;
        }

        public bool ResolverAppliesForDeserialization(Type type)
        {
            if (type.IsGenericType)
                if (type.GetGenericArguments()[0].GetInterface("ICouchDocument`1") == null)
                    return true;

            return type.GetInterface("ICouchDocument`1") == null;
        }

        public IContractResolver Resolver
        {
            get { return Assimilate.GetInstanceOf<ConventionDeserializationContractResolver>(); }
        }
    }
}