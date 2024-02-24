using PassionProject.Models;
using PassionProject.Models.ViewModels;
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
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44310/api/");
        }

        

        // GET: Customer/List
        public ActionResult List()
        {
            //objective: communicate with our customer data api to retrieve a list of customers
            //curl https://localhost:44310/api/customerdata/listcustomers

            //We are going to borrow the token from this request
            string url = "customerdata/listcustomers";
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
            DetailsCustomer ViewModel = new DetailsCustomer();

            //objective: communicate with our customer data api to retrieve one customer
            //curl https://localhost:44310/api/customerdata/findcustomer/{id}

            string url = "customerdata/findcustomer/" + id;
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
        // GET: Customer/New
        public ActionResult New()
        {
            //information about all customers in the system.
            //GET api/customerdata/listcustomers

            string url = "customerdata/listcustomers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CustomerDto> CustomersOptions = response.Content.ReadAsAsync<IEnumerable<CustomerDto>>().Result;

            return View(CustomersOptions);
        }


        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(customer.CustomerName);
            //objective: add a new customer into our system using the API
            //curl -H "Content-Type:application/json" -d @customer.json https://localhost:44310/api/customerdata/addcustomer 
            string url = "customerdata/addcustomer/";


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

            UpdateCustomer ViewModel = new UpdateCustomer();

            //grab the customer information

            //objective: communicate with our customer data api to retrieve one customer
            //curl https://localhost:44310/api/customerdata/findcustomer/{id}

            string url = "customerdata/findcustomer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            CustomerDto Selectedcustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
            //Debug.WriteLine("customer received : ");
            //Debug.WriteLine(selectedcustomer.CustomerName);

            ViewModel.SelectedCustomer = Selectedcustomer;


            return View(ViewModel);
        }

        // POST: Customer/Update/2
        [HttpPost]
        public ActionResult Update(int id, Customer customer)
        {
                Debug.WriteLine("The new customer info is:");
                Debug.WriteLine(customer.CustomerName);

                //serialize into JSON
                //Send the request to the API

                string url = "/customerdata/updatecustomer/" + id;


                string jsonpayload = jss.Serialize(customer);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/customerdata/UpdateCustomer/{id}
                //Header : Content-Type: application/json
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
        // GET: Customer/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "customerdata/findcustomer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CustomerDto selectedcustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
            return View(selectedcustomer);
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            //GetApplicationCookie();//get token credentials
            string url = "customerdata/deletecustomer/" + id;
            HttpContent content = new StringContent("");
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


    }
}