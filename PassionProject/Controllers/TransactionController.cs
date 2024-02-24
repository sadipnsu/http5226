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
    public class TransactionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TransactionController()
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

        // GET: Transaction/List
        public ActionResult List()
        {
            //objective: communicate with our transaction data api to retrieve a list of transaction
            //curl https://localhost:44310/api/transactiondata/listtransactions

            //We are going to borrow the token from this request
            string url = "transactiondata/listtransactions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<TransactionDto> transactions = response.Content.ReadAsAsync<IEnumerable<TransactionDto>>().Result;
            //Debug.WriteLine("Number of transactions received : ");
            //Debug.WriteLine(transaction.Count());


            return View(transactions);
        }
        public ActionResult Error()
        {

            return View();
        }

        // GET: Transaction/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Transaction/Create
        [HttpPost]
        public ActionResult Create(Transaction transaction)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Transaction.CustomerName);
            //objective: add a new Transaction into our system using the API
            //curl -H "Content-Type:application/json" -d @Keeper.json https://localhost:44324/api/transactiondata/addtransaction 
            string url = "transactiondata/addtransaction";


            string jsonpayload = jss.Serialize(transaction);
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
        // GET: Transaction/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "transactiondata/findtransaction/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TransactionDto selectedTransaction = response.Content.ReadAsAsync<TransactionDto>().Result;
            return View(selectedTransaction);
        }

        // POST: Transaction/Update/5
        [HttpPost]
        public ActionResult Update(int id, Transaction transaction)
        {

            string url = "transactiondata/updatetransaction/" + id;
            string jsonpayload = jss.Serialize(transaction);
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

        // GET: Transaction/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "transactiondata/findtransaction/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TransactionDto selectedTransaction = response.Content.ReadAsAsync<TransactionDto>().Result;
            return View(selectedTransaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "transactiondata/deletetransaction/" + id;
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