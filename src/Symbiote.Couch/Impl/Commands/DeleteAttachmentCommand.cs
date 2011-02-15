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
using Symbiote.Couch.Config;
using Symbiote.Couch.Impl.Http;
using Symbiote.Couch.Impl.Metadata;
using Symbiote.Couch.Impl.Model;

namespace Symbiote.Couch.Impl.Commands
{
    public class DeleteAttachmentCommand : BaseCouchCommand
    {
        public virtual CommandResult DeleteAttachment<TModel>( TModel model, string attachmentName )
            where TModel : IHaveAttachments
        {
            try
            {
                CreateUri<TModel>()
                    .Id( model.GetDocumentIdAsJson() )
                    .Attachment( attachmentName )
                    .Revision( model.GetDocumentRevision() );

                var result = Delete();
                model.SetDocumentRevision( result.JsonObject.Value<string>( "rev" ) );
                model.RemoveAttachment( attachmentName );
                return result;
            }
            catch ( Exception ex )
            {
                throw Exception( ex,
                                 "An exception occurred trying to delete attachment {0} from document of type {1} with id {2} and rev {3} at {4}. \r\n\t {5}",
                                 attachmentName,
                                 typeof( TModel ).FullName,
                                 model.GetDocumentIdAsJson(),
                                 model.GetDocumentRevision(),
                                 Uri.ToString(),
                                 ex );
            }
        }

        public DeleteAttachmentCommand( IHttpAction action, ICouchConfiguration configuration, ISerializeDocument serializer ) 
            : base( action, configuration, serializer )
        {
        }
    }
}