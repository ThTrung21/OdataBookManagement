using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace ODataBookStore.Controllers
{
	public class PressesController : ODataController
	{
		private BookStoreContext db;


		public PressesController(BookStoreContext context) {
			db=context;
			if (context.Books.Count() == 0)
			{
				foreach (var b in DataSource.GetAllBooks())
				{
					context.Books.Add(b);
					context.Presses.Add(b.Press);
				}
				context.SaveChanges();
			}
		}
		[EnableQuery]
		public IActionResult Get()
		{
			return Ok(db.Presses);
		}
	}
}
