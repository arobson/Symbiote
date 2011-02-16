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
using Symbiote.Core.Collections;
using Symbiote.Couch.Impl.Model;

namespace Symbiote.Couch.Impl.Metadata
{
    public class DocumentMetadataProvider : IProvideDocumentMetadata
    {
        public MruDictionary<object, DocumentMetadata> Cache { get; set; }

        public DocumentMetadata GetMetadata( string key )
        {
            return Cache[ key ];
        }

        public void SetMetadata( string key, DocumentMetadata metadata )
        {
            Cache[ key ] = metadata;
        }

        public DocumentMetadataProvider()
        {
            Cache = new MruDictionary<object, DocumentMetadata>( 100000 );
        }
    }
}