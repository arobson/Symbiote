// /* 
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
using System.Linq.Expressions;
using Symbiote.Couch.Impl.Commands;
using Symbiote.Couch.Impl.Model;

namespace Symbiote.Couch
{
    public interface IDocumentRepository : IDisposable
    {
        void DeleteDocument<TModel>( object id );

        void DeleteDocument<TModel>( object id, string rev );

        void DeleteAttachment<TModel>( TModel model, string attachmentName )
            where TModel : IHaveAttachments;

        IList<TModel> FromView<TModel>( string designDocument, string viewName, Action<ViewQuery> query );

        TModel Get<TModel>( object id, string revision );

        TModel Get<TModel>( object id );

        IList<TModel> GetAll<TModel>();

        IList<TModel> GetAll<TModel>( int pageSize, int pageNumber );

        IList<TModel> GetAllByKeys<TModel>( object[] ids );

        IList<TModel> GetAllBetweenKeys<TModel>( object startingWith, object endingWith );

        Tuple<string, byte[]> GetAttachment<TModel>( object id, string attachmentName )
            where TModel : IHaveAttachments;

        IList<TModel> GetAllByCriteria<TModel>( Expression<Func<TModel, bool>> criteria );

        void Save<TModel>( TModel model );

        void SaveAll<TModel>( IEnumerable<TModel> list );

        void SaveAttachment<TModel>( TModel model, string attachmentName, string contentType, byte[] content )
            where TModel : IHaveAttachments;

        void HandleUpdates<TModel>( int since, Action<string, ChangeRecord> onUpdate, AsyncCallback updatesInterrupted );

        void HandleUpdates( string database, int since, Action<string, ChangeRecord> onUpdate,
                            AsyncCallback updatesInterrupted );

        void StopChangeStreaming( string database );

        void StopChangeStreaming<TModel>();
    }
}