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
using System.Collections.Generic;
using Symbiote.Couch.Impl.Http;

namespace Symbiote.Couch
{
    public interface ICouchServer
        : IDisposable
    {
        void CleanViews<TModel>();
        void CreateDatabase<TModel>();
        void CompactDatabase<TModel>();
        void CompactView<TModel>(string testview);
        void CopyDatabase<TModel>(CouchUri targetUri);
        void CopyDatabase(CouchUri sourceUri, CouchUri targetUri);
        bool DatabaseExists<TModel>();
        IList<string> DatabaseList { get; }
        void DeleteDatabase<TModel>();
        void Replicate<TModel>(CouchUri targetUri);
        void Replicate(CouchUri sourceUri, CouchUri targetUri);

        IDocumentRepository Repository { get; }
    }
}