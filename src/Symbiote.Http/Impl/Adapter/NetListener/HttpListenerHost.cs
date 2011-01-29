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
using System.Net;
using Symbiote.Http.Config;
using Symbiote.Http.Owin;

namespace Symbiote.Http.Impl.Adapter.NetListener
{
    public class HttpListenerHost
        : IHost, IDisposable
    {
        public IRouteRequest RequestRouter { get; set; }
        public HttpListenerConfiguration Configuration { get; set; }
        public HttpListener Listener { get; set; }
        public HttpContextTransform ContextTransformer { get; set; }
        public bool Running { get; set; }

        public void Start()
        {
            Listener.Start();
            Listener.BeginGetContext(GetRequest, null);
        }

        public void Stop()
        {
            Listener.Stop();
        }

        public void GetRequest(IAsyncResult result)
        {
            try
            {
                var context = Listener.EndGetContext(result);
                ProcessRequest(context);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Well, this is bad: \r\n {0}", exception);
                throw;
            }
            Listener.BeginGetContext(GetRequest, null);
        }

        public void ProcessRequest(HttpListenerContext listenerContext)
        {
            var context = ContextTransformer.From(listenerContext);
            var application = RequestRouter.GetApplicationFor(context.Request);
            application.Process( 
                context.Request.Items,  
                context.Response.Respond,
                OnApplicationException
                );
        }

        public void OnApplicationException(Exception exception)
        {
            Console.WriteLine("Well, this is bad: \r\n {0}", exception);
        }

        public HttpListenerHost(HttpListenerConfiguration configuration, IRouteRequest router)
        {
            RequestRouter = router;
            Configuration = configuration;
            ContextTransformer = new HttpContextTransform();
            Listener = new HttpListener();
            Listener.AuthenticationSchemes = Configuration.AuthSchemes;
            Configuration.HostedUrls.ForEach(x =>
            {
                Listener.Prefixes.Add( x );
            } );
            
        }

        public void Dispose()
        {
            if(Listener != null && Listener.IsListening)
            {
                Listener.Stop();
                Listener.Abort();
            }
        }
    }
}
