﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Machine.Specifications;

namespace Relax.Lucene.Tests
{
    public class when_searching_against_family : with_indexed_people
    {
        protected static Tuple<ScoreDoc,Document> result;
        protected static List<Tuple<ScoreDoc,Document>> results;
        
        private Because of = () =>
                                 {
                                     results = provider.GetDocumentsForQuery<Person>(x => 
                                         x.Cars.Count > 0 &&
                                         x.Cars.Any(c => c.Year == 2010)
                                         ).ToList();
                                     result = results[0];
                                 };
        
        private It should_produce_1_result = () => 
            results.Count.ShouldEqual(1);
    }
}