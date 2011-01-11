﻿using Machine.Specifications;
using Mikado.Tests.Domain.Model;

namespace Mikado.Tests.TestSetup
{
    public abstract class with_Manager : with_ioc_configuration
    {
        public static Manager Manager;

        private Establish context = () =>
                                        {
                                            Manager = new Manager()
                                                          {
                                                              Age = 37,
                                                              FirstName = "Jim",
                                                              LastName = "Cowart",
                                                              Department = "Development"
                                                          };
                                        };
    }
}