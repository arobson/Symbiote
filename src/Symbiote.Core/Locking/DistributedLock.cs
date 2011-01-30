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

namespace Symbiote.Core.Locking
{
    public class DistributedLock
        : IDisposable
    {
        protected ILockManager Manager { get; set; }
        protected object LockId { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            Manager.ReleaseLock( LockId );
        }

        #endregion

        public static DistributedLock Create<T>( T lockId )
        {
            var manager = Assimilate.GetInstanceOf<ILockManager>();
            var lockInstance = new DistributedLock( manager, lockId );
            manager.AcquireLock( lockId );
            return lockInstance;
        }

        public DistributedLock( ILockManager lockManager, object lockId )
        {
            Manager = lockManager;
            LockId = lockId;
        }
    }
}