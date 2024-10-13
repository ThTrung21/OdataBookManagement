using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using ODataAPI.Models;

namespace ODataAPI.Controllers
{
	[Route("odata/Presses")]
	public class PressesController : ODataController
	{
		private readonly BookDbContext _context;

		public PressesController(BookDbContext context)
		{
			_context = context;
		}

		[EnableQuery]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_context.Presses);
		}
	}
}
