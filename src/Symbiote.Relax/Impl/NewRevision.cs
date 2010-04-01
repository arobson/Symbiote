﻿using Newtonsoft.Json;

namespace Symbiote.Relax.Impl
{
    public class NewRevision
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "rev")]
        public string Revision { get; set; }
    }
}