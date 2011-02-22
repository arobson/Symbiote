﻿using System.Collections.Generic;

namespace Relax.Lucene.Tests
{
    public class DomainHelper
    {
        public static Person Create_Single_Person_With_One_Car()
        {
            return new Person()
                       {
                           FirstName = "Alex",
                           LastName = "Robson",
                           Age = 31,
                           Cars = new List<Car>()
                                      {
                                          new Car() {Make="Honda",Model="Crosstour",Year=2010}
                                      }
                       };
        }

        public static List<Person> Create_Family_With_Two_Cars()
        {
            return new List<Person>()
                       {
                           new Person()
                               {
                                   FirstName = "Alex",
                                   LastName = "Robson",
                                   Age = 31,
                                   Cars = new List<Car>()
                                              {
                                                  new Car() {Make="Honda",Model="Crosstour",Year=2010}
                                              }
                               },
                           new Person()
                               {
                                   FirstName = "Rebekah",
                                   LastName = "Robson",
                                   Age = 28,
                                   Cars = new List<Car>()
                                              {
                                                  new Car() {Make="Honda",Model="Civic",Year=2008}
                                              }
                               },
                           new Person()
                               {
                                   FirstName = "Sean",
                                   LastName = "Robson",
                                   Age = 28,
                                   Cars = new List<Car>()
                                              {
                                                  new Car() {Make="Honda",Model="Accord",Year=1999}
                                              }
                               },
                           new Person()
                               {
                                   FirstName = "Rebekah",
                                   LastName = "Robson",
                                   Age = 28,
                                   Cars = new List<Car>()
                                              {
                                                  new Car() {Make="Honda",Model="Civic",Year=2008}
                                              }
                               },
                       };
        }
    }
}