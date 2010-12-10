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


namespace Symbiote.Redis.Impl.Command.Hash
{
    class HSetCommand<TValue>
        : RedisCommand<bool>
    {
        protected const string VALUE_EXCEEDS_1GB = "Value must not exceed 1 GB";
        protected const string HSET = "HSET {0} {1} {2}\r\n";
        protected string Key { get; set; }
        protected string Field {get; set; }
        protected TValue Value { get; set; }

        public bool HSet(IRedisConnection connection)
        {
            var data = Serialize(Value);
            if (data.Length > 1073741824)
                throw new ArgumentException(VALUE_EXCEEDS_1GB, "value");
            return connection.SendExpectSuccess(data, HSET.AsFormat(Key, Field, data.Length));
        }

        public HSetCommand(string key, string field, TValue value)
        {
            Key = key;
            Field = field;
            Value = value;
            Command = HSet;
        }
    }
}
