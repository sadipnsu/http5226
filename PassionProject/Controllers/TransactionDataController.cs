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
    public class TransactionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all transaction in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all transaction in the database, including their associated customers and products.
        /// </returns>
        /// <example>
        /// GET: api/TransactionData/ListTransactions
        /// </example>
        [HttpGet]
        [ResponseType(typeof(TransactionDto))]
        public IHttpActionResult ListTransactions()
        {
            List<Transaction> Transactions = db.Transactions.ToList();
            List<TransactionDto> TransactionDtos = new List<TransactionDto>();

            Transactions.ForEach(t => TransactionDtos.Add(new TransactionDto()
            {
                TransactionId = t.TransactionId,
                ProductId = t.ProductId,
                CustomerId = t.CustomerId,
                OrderQuantity = t.OrderQuantity,
            }));

            return Ok(TransactionDtos);
        }


        /// <summary>
        /// Returns all transaction in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A transaction in the system matching up to the transaction ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Transaction</param>
        /// <example>
        /// GET: api/TransactionData/FindTransaction/5
        /// </example>
        [ResponseType(typeof(TransactionDto))]
        [HttpGet]
        public IHttpActionResult FindTransaction(int id)
        {
            Transaction Transaction = db.Transactions.Find(id);
            TransactionDto TransactionDto = new TransactionDto()
            {
                TransactionId = Transaction.TransactionId,
                ProductId = Transaction.ProductId,
                CustomerId = Transaction.CustomerId,
                OrderQuantity = Transaction.OrderQuantity,
            };
            if (Transaction == null)
            {
                return NotFound();
            }

            return Ok(TransactionDto);
        }


        //// <summary>
        /// Updates a particular Transaction in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Transaction ID primary key</param>
        /// <param name="Transaction">JSON FORM DATA of a transaction</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/TransactionData/UpdateTransaction/5
        /// FORM DATA: Transaction JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTransaction(int id, Transaction Transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Transaction.TransactionId)
            {

                return BadRequest();
            }

            db.Entry(Transaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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
        /// Adds a Transaction to the system
        /// </summary>
        /// <param name="Transaction">JSON FORM DATA of a </param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Transaction ID, Transaction Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/TransactionData/AddTransaction
        /// FORM DATA: Transaction JSON Object
        /// </example>
        [ResponseType(typeof(Transaction))]
        [HttpPost]
        public IHttpActionResult AddTransaction(Transaction Transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transactions.Add(Transaction);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Transaction.TransactionId }, Transaction);
        }

        /// <summary>
        /// Deletes an transaction from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the transaction</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/TransactionData/DeleteTransaction/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Transaction))]
        [HttpPost]
        public IHttpActionResult DeleteTransaction(int id)
        {
            Transaction Transaction = db.Transactions.Find(id);
            if (Transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(Transaction);
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

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(t => t.TransactionId == id) > 0;
        }
    }
}
