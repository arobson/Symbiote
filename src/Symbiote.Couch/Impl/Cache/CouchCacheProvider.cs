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
using Symbiote.Core.Cache;
using Symbiote.Core.Extensions;

namespace Symbiote.Couch.Impl.Cache
{
    public class CouchCacheProvider : ICouchCacheProvider
    {
        protected IKeyAssociationManager _associationManager;
        protected ICacheProvider _cache;
        protected ICacheKeyBuilder _keyBuilder;

        public void AddCrossReference( string key, string cacheKey )
        {
            _associationManager.AddKeyAssociation( key, cacheKey );
        }

        public virtual void InvalidateItem( string affectedKey )
        {
            _associationManager
                .GetAssociations( affectedKey )
                .ForEach( x => _cache.Remove( x ) );
        }

        public virtual bool Delete<TModel>( object key, Func<object, bool> delete )
        {
            var cacheKey = _keyBuilder.GetKey<TModel>( key );
            InvalidateItem( cacheKey );
            return delete( key );
        }

        public virtual bool Delete<TModel>( object key, string rev, Func<object, string, bool> delete )
        {
            var cacheKey = _keyBuilder.GetKey<TModel>( key, rev );
            InvalidateItem( cacheKey );
            return delete( key, rev );
        }

        public virtual TModel Get<TModel>( object key, string rev, Func<object, string, TModel> retrieve )
        {
            var cacheKey = _keyBuilder.GetKey<TModel>( key, rev );
            var result = _cache.Get<TModel>( cacheKey );
            if ( result == null )
            {
                result = retrieve( key, rev );
                _cache.Store( cacheKey, result );
            }
            return result;
        }

        public virtual TModel Get<TModel>( object key, Func<object, TModel> retrieve )
        {
            var cacheKey = _keyBuilder.GetKey<TModel>( key );
            var result = _cache.Get<TModel>( cacheKey );
            if ( result == null )
            {
                result = retrieve( key );
                _cache.Store( cacheKey, result );
            }
            return result;
        }

        public virtual IList<TModel> GetAll<TModel>( Func<IList<TModel>> retrieve )
        {
            var listCacheKey = _keyBuilder.GetListKey<TModel>();
            var result = _cache.Get<IList<TModel>>( listCacheKey );
            if ( result == null )
            {
                result = retrieve();
                _cache.Store( listCacheKey, result );
                result.ForEach( x =>
                                    {
                                        var cacheKey = _keyBuilder.GetKey<TModel>( x.GetDocumentId() );
                                        var cacheRevKey = _keyBuilder.GetKey<TModel>( x.GetDocumentId(),
                                                                                      x.GetDocumentRevision() );
                                        AddCrossReference( cacheKey, listCacheKey );
                                        AddCrossReference( cacheRevKey, listCacheKey );
                                    } );
            }
            return result;
        }

        public virtual IList<TModel> GetAllPaged<TModel>( int pageSize, int pageNumber,
                                                          Func<int, int, IList<TModel>> retrieve )
        {
            var listCacheKey = _keyBuilder.GetListKey<TModel>( pageNumber, pageSize );
            var result = _cache.Get<IList<TModel>>( listCacheKey );
            if ( result == null )
            {
                result = retrieve( pageSize, pageNumber );
                _cache.Store( listCacheKey, result );
                result.ForEach( x =>
                                    {
                                        var cacheKey = _keyBuilder.GetKey<TModel>( x.GetDocumentId() );
                                        var cacheRevKey = _keyBuilder.GetKey<TModel>( x.GetDocumentId(),
                                                                                      x.GetDocumentRevision() );
                                        AddCrossReference( cacheKey, listCacheKey );
                                        AddCrossReference( cacheRevKey, listCacheKey );
                                    } );
            }
            return result;
        }

        public IList<TModel> GetAllInRange<TModel>( object startingWith, object endingWith,
                                                    Func<object, object, IList<TModel>> retrieve )
        {
            var listCacheKey = _keyBuilder.GetRangeKey<TModel>( startingWith, endingWith );
            var result = _cache.Get<IList<TModel>>( listCacheKey );
            if ( result == null )
            {
                result = retrieve( startingWith, endingWith );
                _cache.Store( listCacheKey, result );
                result.ForEach( x =>
                                    {
                                        var cacheKey = _keyBuilder.GetKey<TModel>( x.GetDocumentId() );
                                        var cacheRevKey = _keyBuilder.GetKey<TModel>( x.GetDocumentId(),
                                                                                      x.GetDocumentRevision() );
                                        AddCrossReference( cacheKey, listCacheKey );
                                        AddCrossReference( cacheRevKey, listCacheKey );
                                    } );
            }
            return result;
        }

        public virtual bool Save<TModel>( TModel model, Func<TModel, bool> save )
        {
            var cacheKey = _keyBuilder.GetKey<TModel>( model.GetDocumentId() );
            var cacheRevKey = _keyBuilder.GetKey<TModel>( model.GetDocumentId(), model.GetDocumentRevision() );
            InvalidateItem( cacheKey );
            InvalidateItem( cacheRevKey );
            var success = save( model );
            CacheSavedModel( model, cacheKey, cacheRevKey );
            return success;
        }

        public virtual bool SaveAll<TModel>( IEnumerable<TModel> list, Func<IEnumerable<TModel>, bool> save )
        {
            var success = save( list );
            list.ForEach( x =>
                              {
                                  var cacheKey = _keyBuilder.GetKey<TModel>( x.GetDocumentId() );
                                  var cacheRevKey = _keyBuilder.GetKey<TModel>( x.GetDocumentId(),
                                                                                x.GetDocumentRevision() );
                                  InvalidateItem( cacheKey );
                                  InvalidateItem( cacheRevKey );
                                  CacheSavedModel( x, cacheKey, cacheRevKey );
                              } );
            return success;
        }

        protected virtual void CacheSavedModel<TModel>( TModel model, string cacheKey, string cacheRevKey )
        {
            _cache.Store( cacheKey, model );
            _cache.Store( cacheRevKey, model );
        }

        public CouchCacheProvider( ICacheProvider cache, IKeyAssociationManager keyAssociationManager,
                                   ICacheKeyBuilder keyBuilder )
        {
            _cache = cache;
            _keyBuilder = keyBuilder;
            _associationManager = keyAssociationManager;
        }
    }
}