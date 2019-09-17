using AlintaEnergyCustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlintaEnergyCustomerAPI.Providers
{
    public class CustomerDataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                using (var context = new CustomerDbContext(serviceProvider.GetRequiredService<DbContextOptions<CustomerDbContext>>()))
                {
                    if (context.Customers.Any())
                    {
                        return;
                    }

                    context.Customers.AddRange(
                        new Customer()
                        {
                            FirstName = "John",
                            LastName = "Papa",
                            DateOfBirth = DateTime.Parse("Jan 10, 1980")
                        },
                        new Customer()
                        {                         
                            FirstName = "Will",
                            LastName = "Smith",
                            DateOfBirth = DateTime.Parse("Dec 30, 1986")
                        },
                        new Customer()
                        {                         
                            FirstName = "Charu",
                            LastName = "Jain",
                            DateOfBirth = DateTime.Parse("Sep 10, 1986")
                        },
                        new Customer()
                        {                         
                            FirstName = "Villy",
                            LastName = "Martin",
                            DateOfBirth = DateTime.Parse("Mar 23, 1982")
                        },
                        new Customer()
                        {                         
                            FirstName = "Micheal",
                            LastName = "Kor",
                            DateOfBirth = DateTime.Parse("Oct 2, 1985")
                        }
                    );

                    context.SaveChanges();
                }
            }
            catch(Exception)
            {
                // log error here for reference
            }
        }
    }
}
