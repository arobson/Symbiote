﻿using Symbiote.Relax;

namespace RelaxDemo
{
    public class BulkDataPersister
    {
        private IDocumentRepository<TestDocument> _couch;

        private TestDocument[] documents = new TestDocument[]
                                               {
                                                   new TestDocument("Document 1"),                                   
                                                   new TestDocument("Document 2"),                                   
                                                   new TestDocument("Document 3"),                                   
                                                   new TestDocument("Document 4"),                                   
                                                   new TestDocument("Document 5"),                                   
                                                   new TestDocument("Document 6"),                                   
                                                   new TestDocument("Document 7"),                                   
                                                   new TestDocument("Document 8"),                                   
                                                   new TestDocument("Document 9"),                                   
                                                   new TestDocument("Document 10"),                                   
                                               };

        public void SaveDocuments()
        {
            _couch.Save(documents);
        }

        public BulkDataPersister(IDocumentRepository<TestDocument> couch)
        {
            _couch = couch;
        }
    }
}