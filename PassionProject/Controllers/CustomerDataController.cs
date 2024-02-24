using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;


namespace PassionProject.Controllers
{
    public class CustomerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all customers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all customers in the database.
        /// </returns>
        /// <example>
        /// GET: api/CustomerData/ListCustomers
        /// </example>
        [HttpGet]
        public IEnumerable<CustomerDto> ListCustomers()
        {
            List<Customer> Customers = db.Customers.ToList();
            List<CustomerDto> CustomerDtos = new List<CustomerDto>();

            Customers.ForEach(c => CustomerDtos.Add(new CustomerDto()
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
            }));


            return CustomerDtos;
        }

        /// <summary>
        /// Returns all customers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An customer in the system matching up to the customer ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Customer</param>
        /// <example>
        // GET: api/CustomerData/FindCustomer/5
        /// </example>
        [ResponseType(typeof(Customer))]
        [HttpGet]
        public IHttpActionResult FindCustomer(int id)
        {
            Customer Customer = db.Customers.Find(id);
            CustomerDto CustomerDto = new CustomerDto()
            {
                CustomerId = Customer.CustomerId,
                CustomerName = Customer.CustomerName

            };
            if (Customer == null)
            {
                return NotFound();
            }

            return Ok(CustomerDto);
        }

        /// <summary>
        /// Updates a particular Customer in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Customer ID primary key</param>
        /// <param name="Customer">JSON FORM DATA of an Customer</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        // POST: api/CustomerData/UpdateCustomer/5
        /// FORM DATA: Customer JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {

                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }



        /// <summary>
        /// Adds an Customer to the system
        /// </summary>
        /// <param name="Customer">JSON FORM DATA of an Customer</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Customer ID, Customer Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        // POST: api/CustomerData/AddCustomer
        /// FORM DATA: Customer JSON Object
        /// </example>
        [ResponseType(typeof(Customer))]
        [HttpPost]
        public IHttpActionResult AddCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerId }, customer);
        }

        /// <summary>
        /// Deletes an Customer from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Customer</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        // POST: api/CustomerData/DeleteCustomer/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Customer))]
        [HttpPost]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerId == id) > 0;
        }

    }
}
