using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProject.Models
{
    public class Transaction
    {

        [Key]
        public int TransactionId { get; set; }

        [ForeignKey("Customers")]
        public int CustomerId { get; set; }
        public virtual Customer Customers { get; set; }

        [ForeignKey("Products")]
        public int ProductId { get; set; }

        // a transaction can have many products 
        public ICollection<Product> Products { get; set; }
        public int OrderQuantity { get; set; }
    }

    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int OrderQuantity { get; set; }

    }

}