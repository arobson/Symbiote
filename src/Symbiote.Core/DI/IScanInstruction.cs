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
using System.Reflection;

namespace Symbiote.Core.DI
{
    public interface IScanInstruction
    {
        void Assembly( Assembly assembly );
        void TheCallingAssembly();
        void AssemblyContainingType<T>();
        void AssemblyContainingType( Type type );
        void AssembliesFromApplicationBaseDirectory();
        void AssembliesFromApplicationBaseDirectory( Predicate<Assembly> assemblyFilter );
        void AddAllTypesOf<TPlugin>();
        void AddAllTypesOf( Type pluginType );
        void AddSingleImplementations();
        void ConnectImplementationsToTypesClosing( Type openGenericType );
        void Exclude( Predicate<Type> exclude );
        void ExcludeNamespace( string nameSpace );
        void ExcludeNamespaceContainingType<T>();
        void ExcludeType<T>();
        void Execute( IDependencyRegistry registry );

#if !SILVERLIGHT
        void AssembliesFromPath( string path );
        void AssembliesFromPath( string path, Predicate<Assembly> assemblyFilter );
#endif
    }
}