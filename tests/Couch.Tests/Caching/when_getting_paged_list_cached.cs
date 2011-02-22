﻿using System;
using System.Collections.Generic;
using Machine.Specifications;
using Moq;
using Symbiote.Couch;
using Symbiote.Couch.Impl.Cache;
using Symbiote.Core;
using Symbiote.StructureMapAdapter;
using It = Machine.Specifications.It;

namespace Couch.Tests.Caching
{
    public class when_getting_paged_list_cached : with_couch_cache_provider
    {
        protected static ICouchCacheProvider couchCacheProvider;
        protected static bool called_delegate;
        protected static IList<TestDocument> documents;
        protected static IList<TestDocument> result;
        protected static Guid _documentId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
        protected static Guid _documentId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");

        private Because of = () =>
                                 {
                                     Assimilate.Initialize();

                                     documents = new List<TestDocument>()
                                                     {
                                                         new TestDocument
                                                             {
                                                                 Message = "Doc 1", 
                                                                 DocumentId = _documentId1, 
                                                                 DocumentRevision = "1"
                                                             },
                                                         new TestDocument
                                                             {
                                                                 Message = "Doc 2", 
                                                                 DocumentId = _documentId2,
                                                                 DocumentRevision = "1"
                                                             }
                                                     };

                                     cacheKeyBuilderMock
                                         .Setup(x => x.GetListKey<TestDocument>(3, 10))
                                         .Returns("TestDocument_list_10_3");

                                     cacheProviderMock
                                         .Setup(x => x.Get<IList<TestDocument>>("TestDocument_list_10_3"))
                                         .Returns(documents);

                                     couchCacheProvider = CouchCacheProvider;
                                     result = couchCacheProvider.GetAllPaged(10, 3, (x, y) =>
                                                                                        {
                                                                                            called_delegate = true;
                                                                                            return documents;
                                                                                        });
                                 };

        private It should_build_cache_keys_correctly = () => cacheKeyBuilderMock.VerifyAll();
        private It should_request_list_from_cache_provider = () => cacheProviderMock.VerifyAll();
        private It should_not_store_result_in_cache_provider =
            () => cacheProviderMock.Verify(x => x.Store("TestDocument_list_10_3", documents), Times.Never());
        private It should_not_call_delegate = () => called_delegate.ShouldBeFalse();
        private It should_have_correct_result = () => result.ShouldEqual(documents);
    }
}