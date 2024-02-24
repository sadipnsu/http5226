using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class UpdateCustomer
    {
        //This viewmodel is a class which stores information that we need to present to /Customer/Update/{}

        //the existing customer information

        public CustomerDto SelectedCustomer { get; set; }

        // all products to choose from when updating this customer

        public IEnumerable<TransactionDto> TransactionsOptions { get; set; }
    }
}