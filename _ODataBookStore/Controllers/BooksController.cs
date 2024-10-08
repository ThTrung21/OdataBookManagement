using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using static _ODataBookStore.Models.EDM;

namespace _ODataBookStore.Controllers
{
	public class BooksController : ODataController
	{
		private BookStoreContext db;
		public BooksController(BookStoreContext context) 
		{
			db = context;
			db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			if(context.Books.Count()==0)
			{
				foreach(var b in DataSource.GetAllBooks())
				{
					context.Books.Add(b);
					context.Presses.Add(b.Press);
				}
				context.SaveChanges();
			}
		}
		
		[EnableQuery(PageSize =1)]
		public IActionResult Get()
		{
			return Ok(db.Books);
		}

		[EnableQuery]
		public IActionResult Get(int key, string version) 
		{ 
			return Ok(db.Books.FirstOrDefault(c=>c.Id==key));

		}
		[EnableQuery]
		public IActionResult Post([FromBody]Book Book)
		{
			db.Books.Add(Book);
			db.SaveChanges();
			return Created(Book);
		}
		[EnableQuery]
		public IActionResult Delete([FromBody]int key)
		{
			Book b = db.Books.FirstOrDefault(c => c.Id == key);
			if (b == null)
			{
				return NotFound();
			}
			db.Books.Remove(b);
			db.SaveChanges();
			return Ok();
				
		}
	}
}
