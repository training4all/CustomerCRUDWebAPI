using System;
using System.Collections.Generic;
using System.Linq;
using AlintaEnergyCustomerAPI.Models;

namespace AlintaEnergyCustomerAPI.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        CustomerDbContext _context;
        public CustomerRepo(CustomerDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            if (_context == null)
                return null;

            return _context.Customers;
        }

        public Customer GetCustomerById(int Id)
        {
            if (_context == null)
                return null;

            return _context.Customers.Where(customer => customer.Id == Id).FirstOrDefault<Customer>();
        }

        public Customer GetCustomerByPartialName(string firstName, string lastName)
        {
            if (_context == null)
                return null;

            return _context.Customers.Where(customer => customer.FirstName.Contains(firstName, StringComparison.InvariantCultureIgnoreCase)
                                                    || customer.LastName.Contains(lastName, StringComparison.InvariantCultureIgnoreCase)
                                                    ).FirstOrDefault<Customer>();
        }

        public int AddCustomer(Customer customer)
        {
            if (_context == null)
                return 0;

            _context.Customers.Add(customer);
            _context.SaveChanges();
            return customer.Id;
        }

        public Customer UpdateCustomer(Customer existingCustomer, Customer customer)
        {
            if (_context != null)
            {
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.DateOfBirth = customer.DateOfBirth;

                _context.Customers.Update(existingCustomer);
                var isUpdated = _context.SaveChanges();
                return isUpdated == 0 ? null : customer;
            }

            return null;
        }

        public bool DeleteCustomer(int Id)
        {
            if (_context != null)
            {
                var customerToDelete = _context.Customers.Where(customer => customer.Id == Id).FirstOrDefault<Customer>();
                if (customerToDelete != null)
                {
                    _context.Customers.Remove(customerToDelete);
                    var isDeleted = _context.SaveChanges();
                    return isDeleted == 0 ? false : true;
                }
            }

            return false;
        }

        public bool CustomerExists(Customer customer)
        {
            if (_context == null)
                return false;

           Customer existingCustomer = _context.Customers.Where(cust => string.Equals(cust.FirstName, customer.FirstName, StringComparison.InvariantCultureIgnoreCase)
                                            && string.Equals(cust.LastName, customer.LastName, StringComparison.InvariantCultureIgnoreCase)
                                            ).FirstOrDefault<Customer>();

            return existingCustomer!=null ? true : false;            
        }
    }
}
