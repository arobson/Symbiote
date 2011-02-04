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
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;

namespace Symbiote.Core.Memento
{
    public class Memoizer
        : IMemoizer
    {
        protected ConcurrentDictionary<Type, Func<IMemento>> Factories { get; set; }

        public IMemento<T> GetMemento<T>( T instance )
        {
            var memento = Assimilate.GetInstanceOf<IMemento<T>>();
            memento.Capture( instance );
            return memento;
        }

        public T GetFromMemento<T>( IMemento<T> memento )
        {
            return memento.Retrieve();
        }

        public void ResetToMemento<T>( T instance, IMemento<T> memento )
        {
            memento.Reset( instance );
        }

        public Func<IMemento> GetFactoryFor<T>()
        {
            return Factories.GetOrAdd( typeof( T ), BuildFactoryFor<T>() );
        }

        public Func<IMemento> BuildFactoryFor<T>()
        {
            var type = Assimilate.Assimilation.DependencyAdapter.GetTypesRegisteredFor<IMemento<T>>().First();
            var constructor = Expression.New( type );
            var lambda = Expression.Lambda<Func<IMemento>>( constructor, null );
            return lambda.Compile();
        }
    }
}