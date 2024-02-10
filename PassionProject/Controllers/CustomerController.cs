﻿using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PassionProject.Controllers
{
    public class CustomerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CustomerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44310/api/customerdata/");
        }

        // GET: Customer/List
        public ActionResult List()
        {
            //objective: communicate with our customer data api to retrieve a list of customers
            //curl https://localhost:44310/api/customerdata/listcustomers


            string url = "listcustomers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<CustomerDto> customers = response.Content.ReadAsAsync<IEnumerable<CustomerDto>>().Result;
            //Debug.WriteLine("Number of customers received : ");
            //Debug.WriteLine(customers.Count());


            return View(customers);
        }
        

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our customer data api to retrieve one customer
            //curl https://localhost:44310/api/customerdata/findanimal/{id}

            string url = "findcustomer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            CustomerDto selectedcustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
            Debug.WriteLine("customer received : ");
            Debug.WriteLine(selectedcustomer.CustomerName);


            return View(selectedcustomer);
        }

        public ActionResult Error()
        {

            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(customer.CustomerName);
            //objective: add a new customer into our system using the API
            //curl -H "Content-Type:application/json" -d @customer.json https://localhost:44310/api/customerdata/addcustomer 
            string url = "addcustomer";


            string jsonpayload = jss.Serialize(customer);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Customer/Edit/2
        public ActionResult Edit(int id)
        {
            //grab the customer information

            //objective: communicate with our customer data api to retrieve one customer
            //curl https://localhost:44310/api/customerdata/findcustomer/{id}

            string url = "findcustomer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            CustomerDto selectedcustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
            //Debug.WriteLine("customer received : ");
            //Debug.WriteLine(selectedcustomer.CustomerName);

            return View(selectedcustomer);
        }

        // POST: Customer/Update/2
        [HttpPost]
        public ActionResult Update(int id, Customer customer)
        {
            try
            {
                Debug.WriteLine("The new customer info is:");
                Debug.WriteLine(customer.CustomerName);

                //serialize into JSON
                //Send the request to the API

                string url = "updatecustomer/" + id;


                string jsonpayload = jss.Serialize(customer);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/customerdata/UpdateCustomer/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;




                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


    }
}