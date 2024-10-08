using static _ODataBookStore.Models.EDM;

namespace _ODataBookStore
{
	public static class DataSource
	{
		private static IList<Book> listBook { get; set; }
		public static IList<Book> GetAllBooks()
		{
			if(listBook != null)
			{
				return listBook;
			}
			listBook = new List<Book>();
			Book book = new Book()
			{
				Id = 1,
				ISBN = "123-456-789",
				Title = "a very long book title",
				Price = 59.99m,
				Location = new Address
				{
					City = "HCM city",
					Street = "d2 agva",

				},
				Press = new Press()
				{
					Id = 1,
					Name = "Addison-Lee",
					Category = Category.Book
				}
			};

			listBook.Add(book);
			Book book2 = new Book()
			{
				Id = 2,
				ISBN = "123-456-782",
				Title = "a  book title",
				Price = 23m,
				Location = new Address
				{
					City = "Ha Noi",
					Street = "d2 agva",

				},
				Press = new Press()
				{
					Id = 2,
					Name = "Womp Womp",
					Category = Category.Magazine,
				}
			};

			listBook.Add(book);




			return listBook;
		}
	}
}
