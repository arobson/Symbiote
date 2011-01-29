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

using System.IO;

namespace Symbiote.Core.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadToEnd(this Stream stream, int timeOut)
        {
            int read;
            var buffer = new byte[8 * 1024];
            if(stream.CanTimeout)
                stream.ReadTimeout = timeOut;
            using (var memoryStream = new MemoryStream())
            {
                while ( ( ( read = stream.Read( buffer, 0, buffer.Length ) ) > 0 ) )
                {
                    memoryStream.Write( buffer, 0, read );
                }
                return memoryStream.ToArray();
            }
        }
    }
}
