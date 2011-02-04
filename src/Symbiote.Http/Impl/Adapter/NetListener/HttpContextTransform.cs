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
using System.Net;

namespace Symbiote.Http.Impl.Adapter.NetListener
{
    public class HttpContextTransform : IConextTransformer<HttpListenerContext>
    {
        public HttpRequestTransform RequestTransform { get; set; }

        #region IConextTransformer<HttpListenerContext> Members

        public Context From<T>( T context )
        {
            return From( context as HttpListenerContext );
        }

        public Context From( HttpListenerContext context )
        {
            var responseAdapter = new HttpResponseAdapter( context );
            return new Context
                (
                RequestTransform.Transform( context.Request ),
                responseAdapter
                );
        }

        #endregion

        public HttpContextTransform()
        {
            RequestTransform = new HttpRequestTransform();
        }
    }
}