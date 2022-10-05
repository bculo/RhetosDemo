void Main()
{
    ConsoleLogger.MinLevel = EventType.Info; // Use EventType.Trace for more detailed log.
    string rhetosHostAssemblyPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), @"..\Bookstore.Service.dll");
    using (var scope = LinqPadRhetosHost.CreateScope(rhetosHostAssemblyPath))
    {
        var context = scope.Resolve<Common.ExecutionContext>();
        var repository = context.Repository;
		
		
		//Assignment 3 with Load
		Console.WriteLine("Assignment 3 START");
		var books = repository.Bookstore.Book.Load(); //select books -> DB
		var authorsGuidsArray = books.Select(i => i.AuthorID ?? Guid.Empty).Where(i => i != Guid.Empty).Distinct().ToArray(); //select author ids -> IN MEMORY
		var authorsData = repository.Bookstore.Person.Load(authorsGuidsArray); //select authors -> DB
		
		var assignment3Result = new List<(string authorName, string title)>();
		foreach(var book in books) { //rostiljanje in memory
			if(!book.AuthorID.HasValue){
				assignment3Result.Add(new (null, book.Title));
				continue;
			}
			
			var author = authorsData.FirstOrDefault(i => i.ID == book.AuthorID);
			assignment3Result.Add(new (author?.Name ?? null, book.Title));
		}
		assignment3Result.Dump(); //Print result
		Console.WriteLine("Assignment 3 END\n\n");

		//Assignment 4<
		Console.WriteLine("Assignment 4 START");
		var query = repository.Bookstore.Book.Query().Select(i => new { i.Author.Name, i.Title });
		query.Dump();
		Console.WriteLine("Assignment 4 END\n\n");

		//Assignment 5
		Console.WriteLine("Assignment 5 START\n");
		query.ToString().Dump();
		Console.WriteLine("Assignment 5 END\n\n");

		//Assignment 6
		Console.WriteLine("Assignment 6 START\n");
		var param = new Bookstore.InsertManyBooks
		{
			NumberOfBooks = 3,
			Title = "Random Title"
		};
		repository.Bookstore.InsertManyBooks.Execute(param);
		//COMMIT NEDDED
		Console.WriteLine("Assignment 6 START\n");
	}
}
