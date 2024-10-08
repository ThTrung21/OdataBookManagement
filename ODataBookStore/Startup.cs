﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Deserialization;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ODataBookStore.Models;
using static ODataBookStore.Models.EDM;

namespace ODataBookStore
{
	public class Startup
	{
		public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseODataBatching();
			app.UseRouting();
			app.Use(next => context =>
			{
				var endpoint = context.GetEndpoint();
				if (endpoint == null)
					return next(context);

				IEnumerable<string> templates;
				IODataRoutingMetadata metadata =
				endpoint.Metadata.GetMetadata<IODataRoutingMetadata>();
				if (metadata != null)
				{
					templates = metadata.Template.GetTemplates();
				}
				return next(context);
			});
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				//endpoints.("odata", "odata", GetEdmModel());
			});

			


		}
		public void ConfigureServices(IServiceCollection services) {
			services.AddDbContext<BookStoreContext>(opt => opt.UseInMemoryDatabase("BookLists"));
			services.AddControllers();

			services.AddControllers().AddOData(option => option.Select().Filter()
			.Count().OrderBy().Expand().SetMaxTop(100).AddRouteComponents("odata", GetEdmModel()) );
		}
		
		private static IEdmModel GetEdmModel()
		{
			ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
			builder.EntitySet<Book>("Books");
			builder.EntitySet<Press>("Presses");
			return builder.GetEdmModel();

		}
	}
}