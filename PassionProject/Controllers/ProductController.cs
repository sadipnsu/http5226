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
    public class ProductController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ProductController()
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

        // GET: Product/List
        public ActionResult List()
        {
            //objective: communicate with our product data api to retrieve a list of products
            //curl https://localhost:44310/api/productdata/listproducts

            //We are going to borrow the token from this request
            string url = "productdata/listproducts";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ProductDto> products = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
            //Debug.WriteLine("Number of products received : ");
            //Debug.WriteLine(products.Count());


            return View(products);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            DetailsProduct ViewModel = new DetailsProduct();

            //objective: communicate with our customer data api to retrieve one customer
            //curl https://localhost:44310/api/productdata/findproduct/{id}

            string url = "productdata/findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            ProductDto selectedproduct = response.Content.ReadAsAsync<ProductDto>().Result;
            Debug.WriteLine("customer received : ");
            Debug.WriteLine(selectedproduct.ProductName);


            return View(selectedproduct);
        }
        public ActionResult Error()
        {

            return View();
        }

        // GET: Product/New
        public ActionResult New()
        {
            //information about all products in the system.
            //GET api/productdata/listproducts

            string url = "productdata/listproducts";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ProductDto> ProductsOptions = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;

            return View(ProductsOptions);
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(product.ProductName);
            //objective: add a new product into our system using the API
            //curl -H "Content-Type:application/json" -d @customer.json https://localhost:44310/api/productdata/addproduct 
            string url = "/productdata/addproduct";


            string jsonpayload = jss.Serialize(product);

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

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "productdata/findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProductDto selectedproducts = response.Content.ReadAsAsync<ProductDto>().Result;
            return View(selectedproducts);
        }

        // POST: Product/Update/5
        [HttpPost]
        public ActionResult Update(int id, Product product)
        {

            string url = "productdata/updateproduct/" + id;
            string jsonpayload = jss.Serialize(product);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: Product/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "productdata/findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProductDto selectedproduct = response.Content.ReadAsAsync<ProductDto>().Result;
            return View(selectedproduct);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //GetApplicationCookie();//get token credentials
            string url = "productdata/deleteproduct/" + id;
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