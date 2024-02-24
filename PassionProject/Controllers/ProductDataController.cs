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
    public class ProductDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Products in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Products in the database,
        /// </returns>
        /// <example>
        /// GET: api/ProductData/ListProducts
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult ListProducts()
        {
            List<Product> Product = db.Products.ToList();
            List<ProductDto> ProductDtos = new List<ProductDto>();

            Product.ForEach(p => ProductDtos.Add(new ProductDto()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                BaseQuantity = p.BaseQuantity
            }));

            return Ok(ProductDtos);
        }

        /// <summary>
        /// Returns all Products in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A product in the system matching up to the product ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Product</param>
        /// <example>
        /// GET: api/ProductData/FindProduct/5
        /// </example>
        [ResponseType(typeof(ProductDto))]
        [HttpGet]
        public IHttpActionResult FindProduct(int id)
        {
            Product Product= db.Products.Find(id);
            ProductDto ProductDto = new ProductDto()
            {
                ProductId = Product.ProductId,
                ProductName = Product.ProductName,
                BaseQuantity = Product.BaseQuantity
            };
            if (Product == null)
            {
                return NotFound();
            }

            return Ok(ProductDto);
        }

        /// <summary>
        /// Updates a particular Product in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Product ID primary key</param>
        /// <param name="Product">JSON FORM DATA of an Species</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/UpdateProduct/5
        /// FORM DATA: Species JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {

                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        /// Adds a product to the system
        /// </summary>
        /// <param name="Product">JSON FORM DATA of an Species</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Product ID, Product Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/AddProduct
        /// FORM DATA: Product JSON Object
        /// </example>
        [ResponseType(typeof(Product))]
        [HttpPost]
        public IHttpActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        }

        /// <summary>
        /// Deletes a product from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Product</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/DeleteProduct/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Product))]
        [HttpPost]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
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

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}
