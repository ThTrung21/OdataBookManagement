using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using static ODataAPI.Models.EDM;

namespace ODataBookStoreWebClient.Controllers
{
	public class PressController : Controller
	{
		private readonly HttpClient client = null;
		private string ProductApiUrl = "";

		public PressController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			ProductApiUrl = "http://localhost:5297/odata/Presses";
		}

		public async Task<IActionResult>Index()
		{
			HttpResponseMessage response= await client.GetAsync(ProductApiUrl);
			string strData=await response.Content.ReadAsStringAsync();

			dynamic temp=JObject.Parse(strData);
			var list = temp.value;
			List<Press> items=((JArray)temp.value).Select(x=>new Press
			{
				Id= (int)x["Id"],
				Name= (string)x["Name"],
			}).ToList();
			return View(items);
		}
	}
}
