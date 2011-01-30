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
using Lucene.Net.Store;
using Symbiote.Lucene.Config;

namespace Symbiote.Lucene.Impl
{
    public class DefaultDirectoryFactory : IDirectoryFactory
    {
        protected ILuceneConfiguration configuration;

        #region IDirectoryFactory Members

        public Directory CreateDirectoryFor( string indexName )
        {
            string indexDirectory = null;
            configuration.DirectoryPaths.TryGetValue( indexName, out indexDirectory );
            indexDirectory = indexDirectory ?? System.IO.Path.Combine( configuration.IndexPath, indexName );
            var directoryInfo = System.IO.Directory.CreateDirectory( indexDirectory );
            var qualifiedDirectoryPath = System.IO.Path.GetFullPath( directoryInfo.ToString() );

            var simpleFsDirectory = new SimpleFSDirectory(
                directoryInfo,
                new SimpleFSLockFactory() );

            simpleFsDirectory.EnsureOpen();

            return simpleFsDirectory;
        }

        #endregion

        public DefaultDirectoryFactory( ILuceneConfiguration configuration )
        {
            this.configuration = configuration;
        }
    }
}