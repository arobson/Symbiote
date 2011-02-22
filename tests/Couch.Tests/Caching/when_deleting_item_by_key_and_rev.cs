﻿using Machine.Specifications;
using Symbiote.Couch.Impl.Cache;

namespace Couch.Tests.Caching
{
    public class when_deleting_item_by_key_and_rev : with_couch_cache_provider
    {
        protected static ICouchCacheProvider couchCacheProvider;
        protected static bool called_delegate;

        private Because of = () =>
                                 {
                                     cacheKeyBuilderMock
                                         .Setup(x => x.GetKey<TestDocument>("item_1","rev_1"))
                                         .Returns("TestDocument_item_1_rev_1");

                                     keyAssociationManagerMock
                                         .Setup(x => x.GetAssociations("TestDocument_item_1_rev_1"))
                                         .Returns(new string[] { "cache_key1", "cache_key2" });

                                     cacheProviderMock
                                         .Setup(x => x.Remove("cache_key1"))
                                         .AtMostOnce();

                                     cacheProviderMock
                                         .Setup(x => x.Remove("cache_key2"))
                                         .AtMostOnce();

                                     couchCacheProvider = CouchCacheProvider;
                                     couchCacheProvider.Delete<TestDocument>("item_1","rev_1", (x,y) => called_delegate = true);
                                 };

        private It should_call_delegate = () => called_delegate.ShouldBeTrue();
        private It should_get_associations_from_key_association_manager = () => keyAssociationManagerMock.VerifyAll();
        private It should_remove_associated_keys_from_the_cache = () => cacheProviderMock.VerifyAll();
    }
}