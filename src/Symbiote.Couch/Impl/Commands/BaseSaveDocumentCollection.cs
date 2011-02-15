﻿// /* 
// Copyright 2008-2011 Alex Robson
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// */
using System;
using System.Collections.Generic;
using System.Linq;
using Symbiote.Core.Extensions;
using Symbiote.Couch.Config;
using Symbiote.Couch.Impl.Http;
using Symbiote.Couch.Impl.Json;
using Newtonsoft.Json.Linq;
using Symbiote.Couch.Impl.Metadata;

namespace Symbiote.Couch.Impl.Commands
{
    public abstract class BaseSaveDocumentCollection : BaseCouchCommand
    {
        protected CommandResult SaveEnumerable( IEnumerable<object> models )
        {
            try
            {
                var list = new BulkPersist( true, false, models );
                var body = list.ToString();

                var result = Post( body );
                var updates = result.GetResultAs<SaveResponse[]>().ToDictionary( x => x.Id, x => x.Revision );
                models
                    .ForEach( x =>
                                  {
                                      var documentId = x.GetDocumentIdAsJson();
                                      string newRev = null;
                                      if ( updates.TryGetValue( documentId, out newRev ) )
                                      {
                                          x.SetDocumentRevision( newRev );
                                      }
                                  } );
                return result;
            }
            catch ( Exception ex )
            {
                throw Exception(
                    ex,
                    "An exception occurred trying to save a collection documents at {0}. \r\n\t {1}",
                    Uri.ToString(),
                    ex
                    );
            }
        }

        public virtual string ScrubBulkPersistOfTypeTokens( string body )
        {
            var jBlob = JObject.Parse( body );
            var docs = jBlob["docs"]["$values"];
            jBlob.Property( "docs" ).Value = docs;
            return jBlob.ToString();
        }

        protected BaseSaveDocumentCollection( IHttpAction action, ICouchConfiguration configuration, ISerializeDocument serializer ) 
            : base( action, configuration, serializer )
        {
        }
    }
}