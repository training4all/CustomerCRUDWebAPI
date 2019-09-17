using NSubstitute;
using System.Collections.Generic;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using AlintaEnergyCustomerAPI.Controllers;
using AlintaEnergyCustomerAPI.Models;
using AlintaEnergyCustomerAPI.Repository;

namespace AlintaEnergyCustomerAPITests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly ICustomerRepo _customerRepo;

        public CustomerControllerTests()
        {
            _customerRepo = Substitute.For<ICustomerRepo>();
        }

        public class GetAllCustomersTests : CustomerControllerTests
        {
            private readonly CustomersController _customersController;

            public GetAllCustomersTests()
            {
                _customersController = new CustomersController(_customerRepo);
            }

            [Fact]
            public void ShouldSuccessfullyReturnCustomerList()
            {
                // arrange                
                var mockCustomers = new List<Customer>()
                {
                    new Customer(){ FirstName="first-name", LastName="last-name"}
                };
                _customerRepo.GetCustomers()
                       .Returns(mockCustomers);

                // act
                var actionResult = _customersController.GetAllCustomers();

                // assert
                Assert.IsType<OkObjectResult>(actionResult.Result);
            }

            [Fact]
            public void ShouldReturnNotFound()
            {
                // arrange   
                List<Customer> mockCustomers = null;
                _customerRepo.GetCustomers()
                       .Returns(mockCustomers);

                // act
                var actionResult = _customersController.GetAllCustomers();

                // assert
                Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundResult>(actionResult.Result);
            }
        }

        public class GetCustomerByIdTests : CustomerControllerTests
        {
            private readonly CustomersController _customersController;

            public GetCustomerByIdTests()
            {
                _customersController = new CustomersController(_customerRepo);
            }

            [Theory, MemberData(nameof(MockCustomersData))]
            public void ShouldReturnCustomerBasedOnId(int customerId, Customer expectedCustomer)
            {
                // arrange                   
                _customerRepo.GetCustomerById(1)
                       .Returns(GetFirstMockCustomer());
                _customerRepo.GetCustomerById(2)
                       .Returns(GetSecondMockCustomer());

                // act
                var actionResult = _customersController.GetCustomerById(customerId);
                var okObjectResult = actionResult.Result as OkObjectResult;
                var actualCustomer = okObjectResult.Value as Customer;

                // assert
                Assert.IsType<OkObjectResult>(okObjectResult);
                Assert.Equal(actualCustomer.FirstName, expectedCustomer.FirstName);
                Assert.Equal(actualCustomer.LastName, expectedCustomer.LastName);
            }

            public static IEnumerable<object[]> MockCustomersData()
            {
                yield return new object[] { 1, GetFirstMockCustomer() };
                yield return new object[] { 2, GetSecondMockCustomer() };
            }

            private static Customer GetFirstMockCustomer()
            {
                return new Customer()
                {
                    Id = 1,
                    FirstName = "first",
                    LastName = "customer"
                };
            }

            private static Customer GetSecondMockCustomer()
            {
                return new Customer()
                {
                    Id = 2,
                    FirstName = "second",
                    LastName = "customer"
                };
            }

        }

}
}
