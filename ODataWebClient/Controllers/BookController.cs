using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.JavaScript;
using static ODataAPI.Models.EDM;

namespace ODataBookStoreWebClient.Controllers
{
	public class BookController : Controller
	{
		private readonly HttpClient client = null;
		private readonly string ProductApiUrl;

		public BookController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			ProductApiUrl= "http://localhost:5297/odata/Books";
		}

		public async Task<IActionResult> Index()
		{
			HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
			string strData = await response.Content.ReadAsStringAsync();
			dynamic temp = JObject.Parse(strData);
			var lst = temp.value;
			List<Book> items = ((JArray)temp.value).Select(x => new Book
			{
				Id = (int)x["Id"],
				Author = (string)x["Author"],
				ISBN = (string)x["ISBN"],
				Title = (string)x["Title"],
				Price = (decimal)x["Price"],
			}).ToList();
			return View(items);
		}

		// GET: Book/Details/5
		public async Task<IActionResult> Details(int id)
		{
			HttpResponseMessage response = await client.GetAsync($"{ProductApiUrl}({id})");
			if (response.IsSuccessStatusCode)
			{
				string strData = await response.Content.ReadAsStringAsync();
				var book = JObject.Parse(strData);
				var item = new Book
				{
					Id = (int)book["Id"],
					Author = (string)book["Author"],
					ISBN = (string)book["ISBN"],
					Title = (string)book["Title"],
					Price = (decimal)book["Price"],
				};
				return View(item);
			}
			return NotFound();
		}

		// GET: Book/Create
		public IActionResult Create()
		{
			return View(new Book());
		}

		// POST: Book/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                var jsonBook = JObject.FromObject(book);
                var content = new StringContent(jsonBook.ToString(), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(ProductApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index)); // Redirect to Index after creating
                }
                ModelState.AddModelError("", "Unable to create record.");
            }
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{ProductApiUrl}({id})");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var book = JObject.Parse(strData);
                var item = new Book
                {
                    Id = (int)book["Id"],
                    Author = (string)book["Author"],
                    ISBN = (string)book["ISBN"],
                    Title = (string)book["Title"],
                    Price = (decimal)book["Price"],
                };
                return View(item);
            }
            return NotFound();
        }

        // POST: Book/Edit/5
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Book book)
		{
			if (ModelState.IsValid)
			{
				var jsonBook = JObject.FromObject(book);
				var content = new StringContent(jsonBook.ToString(), System.Text.Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.PutAsync($"{ProductApiUrl}({id})", content);

				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction(nameof(Index));
				}
				ModelState.AddModelError("", "Unable to edit record.");
			}
			return View(book);
		}

		// GET: Book/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			HttpResponseMessage response = await client.GetAsync($"{ProductApiUrl}({id})");
			if (response.IsSuccessStatusCode)
			{
				string strData = await response.Content.ReadAsStringAsync();
				var book = JObject.Parse(strData);
				var item = new Book
				{
					Id = (int)book["Id"],
					Author = (string)book["Author"],
					ISBN = (string)book["ISBN"],
					Title = (string)book["Title"],
					Price = (decimal)book["Price"],
				};
				return View(item);
			}
			return NotFound();
		}

		// POST: Book/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			HttpResponseMessage response = await client.DeleteAsync($"{ProductApiUrl}({id})");

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction(nameof(Index));
			}
			return NotFound();
		}
	}
}
