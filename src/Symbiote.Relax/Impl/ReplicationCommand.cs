﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Symbiote.Relax.Impl
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class ReplicationCommand
    {
        [JsonProperty("source")]
        public string SourceUri { get; set; }
        [JsonProperty("target")]
        public string TargetUri { get; set; }
        [JsonProperty("continuous")]
        public bool ContinuousReplication { get; set; }
        [JsonProperty("create_target")]
        public bool CreateTarget { get; set; }

        public static ReplicationCommand Once(CouchUri source, CouchUri target)
        {
            return new ReplicationCommand()
                       {
                           SourceUri = source.ToString(),
                           TargetUri = target.ToString(),
                           CreateTarget = true
                       };
        }

        public static ReplicationCommand Continuous(CouchUri source, CouchUri target)
        {
            return new ReplicationCommand()
                       {
                           SourceUri = source.ToString(),
                           TargetUri = target.ToString(),
                           ContinuousReplication = true,
                           CreateTarget = true
                       };
        }
    }
}
