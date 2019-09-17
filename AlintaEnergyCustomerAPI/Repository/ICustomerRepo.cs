using AlintaEnergyCustomerAPI.Models;
using System.Collections.Generic;

namespace AlintaEnergyCustomerAPI.Repository
{
    public interface ICustomerRepo
    {
        IEnumerable<Customer> GetCustomers();

        Customer GetCustomerById(int Id);

        Customer GetCustomerByPartialName(string firstName, string lastName);

        bool DeleteCustomer(int Id);

        Customer UpdateCustomer(Customer existingCustomer, Customer customer);

        int AddCustomer(Customer customer);

        bool CustomerExists(Customer customer);
    }
}
