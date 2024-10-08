	using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.JavaScript;

namespace ODataBookStoreWebClient.Controllers
{
	public class BookController : Controller
	{
		private readonly HttpClient client = null;
		private string ProductApiUrl = " ";

		public BookController()
		{
			client=new HttpClient();
			var contentType	=new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			ProductApiUrl = "http://localhost:5237/odata/Books";
		}

		public async IActionResult Index()
		{
			HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
			string strData = await response.Content.ReadAsStringAsync();
			dynamic temp= 

			return View();
		}
	}
}
