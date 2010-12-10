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
using Symbiote.Redis.Impl.Connection;


namespace Symbiote.Redis.Impl.Command.List
{
    class LPopCommand<TValue>
        : RedisCommand<TValue>
    {
        protected const string LPOP = "LPOP {0}\r\n";
        protected string Key { get; set; }

        public TValue LPop(IRedisConnection connection)
        {
            var data = connection.SendExpectData(null, LPOP.AsFormat(Key));
            return Deserialize<TValue>(data);
        }

        public LPopCommand(string key)
        {
            Key = key;
            Command = LPop;
        }


    }
}