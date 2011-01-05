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
using Symbiote.Core.Extensions;
using Symbiote.Core.Impl.Serialization;
using Symbiote.Couch.Config;
using Symbiote.Couch.Impl.Http;
using Symbiote.Couch.Impl.Json;

namespace Symbiote.Couch.Impl.Commands
{
    public class GetDocumentsByIdsCommand : BaseCouchCommand
    {
        public CommandResult GetDocuments<TModel>(object[] ids)
        {
            try
            {
                CreateUri<TModel>()
                    .ListAll()
                    .IncludeDocuments();

                var keys = new KeyList() { keys = ids };
                var jsonKeyList = keys.ToJson(false);

                var commandResult = Post(jsonKeyList);
                commandResult.ApplyDesignDocumentFilter();
                return commandResult;
            }
            catch (Exception ex)
            {
                var couchEx = Exception(ex,
                                        "An exception occurred trying to retrieve a list of documents of type {0} by keys at {1}. \r\n\t {2}",
                                        typeof(TModel).FullName,
                                        Uri.ToString(),
                                        ex
                    );

                if (configuration.Throw404Exceptions)
                    throw couchEx;

                return new CommandResult("");
            }
        }

        public GetDocumentsByIdsCommand(IHttpAction action, ICouchConfiguration configuration) : base(action, configuration)
        {
        }
    }
}