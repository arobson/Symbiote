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

namespace Symbiote.Couch.Impl.Cache
{
    public interface ICouchCacheProvider
    {
        void AddCrossReference(string key, string cacheKey);

        void InvalidateItem(string affectedKey);

        void Delete<TModel>(object key, Action<object> delete);

        void Delete<TModel>(object key, string rev, Action<object, string> delete);

        TModel Get<TModel>(object key, string rev, Func<object, string, TModel> retrieve);

        TModel Get<TModel>(object key, Func<object, TModel> retrieve);

        IList<TModel> GetAll<TModel>(Func<IList<TModel>> retrieve);

        IList<TModel> GetAllPaged<TModel>(int pageSize, int pageNumber, Func<int, int, IList<TModel>> retrieve);

        IList<TModel> GetAllInRange<TModel>(object startingWith, object endingWith, Func<object,object,IList<TModel>> retrieve);

        void Save<TModel>(TModel model, Action<TModel> save);

        void SaveAll<TModel>(IEnumerable<TModel> list, Action<IEnumerable<TModel>> save);
    }
}