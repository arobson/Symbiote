﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Machine.Specifications;

namespace Lucene.Tests
{
    public class TestModel
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int Age { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
    }

    public class when_searching_against_stored_index : with_index_on_disk
    {
        protected static List<Tuple<ScoreDoc, Document>> documents;
        protected static Stopwatch watch;
        protected static int age;

        private Because of = () =>
                                 {
                                     watch = Stopwatch.StartNew();
                                     documents = searchProvider.GetDocumentsForQuery<TestModel>(x => x.DateOfBirth <= DateTime.Parse("01/01/1980")).ToList();
                                     watch.Stop();
                                     age = int.Parse(documents[0].Item2.Get("Age"));
                                 };
        
        private It should_return_one_document = () =>
                                                documents.Count.ShouldEqual(1);
        
        private It should_have_age_31 = () => age.ShouldEqual(31);
    }
}