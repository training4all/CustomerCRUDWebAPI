using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AlintaEnergyCustomerAPI.Models;
using AlintaEnergyCustomerAPI.Repository;

namespace AlintaEnergyCustomerAPI.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/[controller]")]    
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo _customerRepo;
        public CustomersController(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        /// <summary>
        /// This will return all customers
        /// </summary>
        /// <returns>List of all existing customers</returns>
        /// <remarks>
        /// Sample request (this request will get all the customers) \
        /// GET /api/customers/getcustomers
        /// </remarks>
        /// <response code="404">no customers found</response>       
        [HttpGet("getcustomers")]
        public ActionResult<IEnumerable<Customer>> GetAllCustomers()
        {
            try
            {
                var customers = _customerRepo.GetCustomers();

                if (customers == null)
                    return NotFound();

                return Ok(customers);
            }
            catch (Exception)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Unexpected database error.");
            }
        }

        /// <summary>
        /// This will get only 1 customer based on customer id
        /// </summary>
        /// <param name="Id"> Id of the customer to get</param>
        /// <returns>single customer with same id</returns>
        /// <remarks>
        /// Sample request (this request will only 1 customer with id=1) \
        /// GET /api/customers/getcustomerbyid/1 
        /// </remarks>
        /// /// <response code="400">id is not a valid customer id</response>       
        [HttpGet("getcustomerbyid/{Id}")]
        public ActionResult<Customer> GetCustomerById(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    return BadRequest();
                }

                var customer = _customerRepo.GetCustomerById(Id);

                if (customer == null)
                    return NotFound();

                return Ok(customer);
            }
            catch (Exception)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Unexpected database error.");
            }
        }

        /// <summary>
        /// It will get customer by partial search on First Name or Last Name
        /// </summary>
        /// <param name="firstName">first name of customer</param>
        /// <param name="lastName">last name of customer</param>
        /// <returns>single customer which matches search creteria</returns>
        /// <remarks>
        /// Sample request (this request will get only 1 customer with firstName=charu and lastName=jain) \
        /// GET /api/customers/searchcustomersbyname?firstName=charu&amp;lastName=jain
        /// </remarks>        
        [HttpGet("searchcustomersbyname")]
        public ActionResult<Customer> SearchCustomersByName(string firstName, string lastName)
        {
            try
            {
                if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                {
                    return BadRequest();
                }

                var customer = _customerRepo.GetCustomerByPartialName(firstName, lastName);

                if (customer == null)
                    return NotFound();

                return Ok(customer);
            }
            catch (Exception)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Unexpected database error.");
            }
        }

        /// <summary>
        /// This will delete customer with particular Id
        /// </summary>
        /// <param name="Id">id of the customer to delete</param>
        /// <returns>status indicating wheather a customer is deleted or not</returns>
        /// <remarks>
        /// Sample request (this request will delete customer with id=2) \
        /// DELETE /api/customers/deletecustomer?Id=2
        /// </remarks>      
        [HttpDelete("deletecustomer")]
        public ActionResult DeleteCustomer(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return BadRequest();
                }

                var isDeleted = _customerRepo.DeleteCustomer(Id);

                if (isDeleted)
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Unexpected database error.");
            }
        }

        /// <summary>
        /// This will create new customer
        /// </summary>
        /// <param name="customer">customer details to be created</param>
        /// <returns>newly created customer</returns>
        /// <remarks>
        /// Sample request (this request will create new customer) \
        /// POST /api/customers/addcustomer \
        ///  { \
        ///        "firstName": "new", \
        ///        "lastName": "customer", \
        ///       "dateOfBirth": "1986-09-10T00:00:00" \
        ///  } \
        /// </remarks>      
        [Consumes("application/json")]
        [HttpPost("addcustomer")]
        public ActionResult<Customer> Post([FromBody] Customer customer)
        {
            try
            {
                if (_customerRepo.CustomerExists(customer))
                {
                    return BadRequest("customer already exists.");
                }

                var customerId = _customerRepo.AddCustomer(customer);
                if (customerId != 0)
                {
                    return Created($"api/customers/getcustomerbyid/{customerId}", customer);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Unexpected database error.");
            }
        }

        /// <summary>
        /// This will update entire customer object with given Id
        /// </summary>
        /// <param name="Id">Id of customer to update</param>
        /// <param name="customer">customer information to update with</param>
        /// <returns>updated customer</returns>
        /// <remarks>
        /// Sample request (this request will update customer with Id=3) \
        /// PUT /api/customers/updatecustomer/3 \
        ///  { \
        ///        "firstName": "updated", \
        ///        "lastName": "customer", \
        ///       "dateOfBirth": "1986-09-10T00:00:00" \
        ///  } \
        /// </remarks>       
        [Consumes("application/json")]
        [HttpPut("updatecustomer/{Id}")]
        public ActionResult<Customer> Put(int Id, [FromBody] Customer customer)
        {
            try
            {
                var existingCustomer = _customerRepo.GetCustomerById(Id);
                if (existingCustomer == null)
                    return NotFound("no customer with given Id exists.");

                var updatedCustomer = _customerRepo.UpdateCustomer(existingCustomer, customer);
                if (updatedCustomer != null)
                {
                    return Ok(updatedCustomer);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Unexpected database error.");
            }
        }

    }
}
