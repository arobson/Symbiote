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
using System.Collections.Generic;

namespace Symbiote.Redis.Impl.Command.Set
{
    public class SDiffStoreCommand<TValue> : SEnumInputBoolOutCommand<TValue>
    {
        protected override void SetCmd()
        {
            CMD = "SDIFFSTORE";
        }

        public SDiffStoreCommand( IEnumerable<string> key, string destKey )
            : base( key, destKey )
        {
        }
    }
}