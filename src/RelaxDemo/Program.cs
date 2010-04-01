﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Symbiote.Core;
using Symbiote.Core.Extensions;
using Symbiote.Daemon;
using Symbiote.Relax;
using Symbiote.Log4Net;

namespace RelaxDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Assimilate
                .Core()
                .Daemon<RelaxDemoService>(x => x
                                 .Arguments(args)
                                 .Name("relaxdemo")
                                 .DisplayName("Relax Demo")
                                 .Description("Relax Integration Testing")
                    )
                .Relax(x => x.UseDefaults())
                .AddColorConsoleLogger<ChangeWatcher>(x => x
                                .Info()
                                .DefineColor()
                                    .Text.IsGreen().ForAllOutput()
                                .MessageLayout(m => m.Message().Date().Newline())
                    )
                .AddConsoleLogger<RelaxDemoService>(x => x
                                .Info()
                                .MessageLayout(m => m.Message().Newline())
                    )
                .RunDaemon<RelaxDemoService>();
        }
    }

    public class RelaxDemoService : IDaemon
    {
        private IDocumentRepository<TestDocument> _couch;
        private DatabaseDeleter _databaseDeleter;
        private BulkDataPersister _bulkPersister;
        private DocumentSaver _documentSaver;
        private DocumentRetriever _retriever;
        private PagingDataLoader _pager;
        private BulkDataLoader _bulkLoader;
        private ChangeWatcher _watcher;

        public void Start()
        {
            "Starting service ..."
                .ToInfo<RelaxDemoService>();

            // start watcher
            "Starting change watcher."
                .ToInfo<RelaxDemoService>();
            _watcher.Start();

            // create bulk documents
            "Creating 10 documents via bulk insertion ..."
                .ToInfo<RelaxDemoService>();

            _bulkPersister.SaveDocuments();
            
            "... Done"
                .ToInfo<RelaxDemoService>();

            // retrieving all documents
            "Retrieve all documents at once ..."
                .ToInfo<RelaxDemoService>();

            var list = _bulkLoader.GetAllDocuments();

            "Retrieved {0} documents"
                .ToInfo<RelaxDemoService>(list.Count);

            // save each document once
            "Resaving each document (testing change updates) ..."
                .ToInfo<RelaxDemoService>();

            list.ForEach(_documentSaver.Save);

            "... Done"
                .ToInfo<RelaxDemoService>();

            // loading each doc by id
            "Loading each document by id ..."
                .ToInfo<RelaxDemoService>();

            list.ForEach(x => _retriever.GetById(x.DocumentId));

            "... Done"
                .ToInfo<RelaxDemoService>();

            // deleting each document
            "Deleting all documents ..."
                .ToInfo<RelaxDemoService>();

            list.ForEach(x => _couch.DeleteDocument(x.DocumentId, x.DocumentRevision));

            "... Done"
                .ToInfo<RelaxDemoService>();

            // adding 10 new documents one by one
            "Adding 10 new documents one at a time ..."
                .ToInfo<RelaxDemoService>();

            list.ForEach(_documentSaver.Save);

            "... Done"
                .ToInfo<RelaxDemoService>();

            // getting all documents by paging
            "Testing paging by retrieving page sizes of 3 ..."
                .ToInfo<RelaxDemoService>();

            for (int i = 1; i < 5; i++)
            {
                var docs = _pager.GetNext3Documents();
                "Page {0} has {1} records"
                    .ToInfo<RelaxDemoService>(i, docs.Count);
            }
        }

        public void Stop()
        {
            "Stopping service..."
                .ToInfo<RelaxDemoService>();
            _watcher.Stop();
            _databaseDeleter.Nuke();
        }

        public RelaxDemoService(IDocumentRepository<TestDocument> couch)
        {
            _couch = couch;
            _databaseDeleter = new DatabaseDeleter(couch);
            _bulkPersister = new BulkDataPersister(couch);
            _documentSaver = new DocumentSaver(couch);
            _retriever = new DocumentRetriever(couch);
            _pager = new PagingDataLoader(couch);
            _bulkLoader = new BulkDataLoader(couch);
            _watcher = new ChangeWatcher(couch);
        }
    }

    public class ChangeWatcher
    {
        private IDocumentRepository<TestDocument> _couch;

        public void Start()
        {
            _couch.HandleUpdates(0, Update, null);
        }

        private void Update(ChangeRecord obj)
        {
            "An update was posted to couch: \r\n\t {0}"
                .ToInfo<ChangeWatcher>(obj.Document);
        }

        public void Stop()
        {
            _couch.StopChangeStreaming();
        }

        public ChangeWatcher(IDocumentRepository<TestDocument> couch)
        {
            _couch = couch;
        }
    }

    public class BulkDataLoader
    {
        private IDocumentRepository<TestDocument> _couch;

        public IList<TestDocument> GetAllDocuments()
        {
            return _couch.GetAll();
        }

        public BulkDataLoader(IDocumentRepository<TestDocument> couch)
        {
            _couch = couch;
        }
    }

    public class PagingDataLoader
    {
        private IDocumentRepository<TestDocument> _couch;
        private int _page = 0;

        public IList<TestDocument> GetNext3Documents()
        {
            return _couch.GetAll(3, ++_page);
        }

        public PagingDataLoader(IDocumentRepository<TestDocument> couch)
        {
            _couch = couch;
        }
    }

    public class DocumentRetriever
    {
        private IDocumentRepository<TestDocument> _couch;

        public TestDocument GetById(string id)
        {
            return _couch.Get(id);
        }

        public DocumentRetriever(IDocumentRepository<TestDocument> couch)
        {
            _couch = couch;
        }
    }

    public class DocumentSaver
    {
        private IDocumentRepository<TestDocument> _couch;

        public void Save(TestDocument document)
        {
            _couch.Save(document);
        }

        public DocumentSaver(IDocumentRepository<TestDocument> couch)
        {
            _couch = couch;
        }
    }

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

    public class DatabaseDeleter
    {
        private IDocumentRepository<TestDocument> _couch;

        public void Nuke()
        {
            _couch.DeleteDatabase();
        }

        public DatabaseDeleter(IDocumentRepository<TestDocument> couch)
        {
            _couch = couch;
        }
    }

    public class TestDocument : DefaultCouchDocument
    {
        public virtual string Message { get; set; }

        public TestDocument()
        {
        }

        public TestDocument(string message)
        {
            Message = message;
        }
    }
}
