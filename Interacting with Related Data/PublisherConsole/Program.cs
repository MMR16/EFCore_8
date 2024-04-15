
using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

PubContext _context = new(); //existing database

//InsertBookWithExisitingAuthor();
//InsertBookToExistAuthor();
//InsertNewAuthorWithNewBook();

//ExplicitLoadingCollection();

ModifyRelatedDatatWhenNotTracked();

void ModifyRelatedDatatWhenNotTracked()
{
    var author = _context.Authors.Include(e => e.Books).FirstOrDefault(e => e.FirstName== "Ruth"); // if we use include it will delete all related data
    var author2 = _context.Authors.Find(2); // find get all related data == include & it will delete all related data
    _context.Remove(author2);
    var state = _context.ChangeTracker.DebugView.ShortView;

    author.Books[0].BasePrice = (decimal)10.00;
    var newContext = new PubContext();
    newContext.Entry(author.Books[0]).State=EntityState.Modified; //EntityState if we use notracking or diffrent instance of  dbcontext
    var state2 = newContext.ChangeTracker.DebugView.ShortView;
    newContext.SaveChanges();
}

void ExplicitLoadingCollection()
{
    var authors = _context.Authors.Count();
    Console.WriteLine("***************************************");
    var author = _context.Authors.FirstOrDefault(e => e.FirstName == "Lynda");
    if (author is not null)
    {
        _context.Entry(author).Collection(e => e.Books).Load(); // load can only load from single object in memory
        _context.Entry(author).Collection(e => e.Books).Query().Where(e => e.Title.Contains("new")).ToList(); // filter on load can using query()
    }
}


#region Insert_One_Many
void InsertBookWithExisitingAuthor()
{
    var book = new Book
    {
        Title = "111",
        PublishDate = new DateOnly(2050, 2, 1)
    };
    book.Author = _context.Authors.Find(3);
    _context.Books.Add(book);
    _context.SaveChanges();
}
void InsertBookToExistAuthor()
{
    var author = _context.Authors.FirstOrDefault(e => e.FirstName == "Ruth");
    if (author is not null)
    {
        // var books = new Book[]
        // {
        //new Book{ Title = "111",
        // PublishDate = new DateOnly(2050, 2, 1),AuthorId=author.AuthorId},
        //  new Book{ Title = "222",
        // PublishDate = new DateOnly(2050, 2, 1),AuthorId=author.AuthorId},
        // };
        // _context.Books.AddRange(books);

        var books = new Book[]
      {
       new Book{ Title = "111",
        PublishDate = new DateOnly(2050, 2, 1)},
         new Book{ Title = "222",
        PublishDate = new DateOnly(2050, 2, 1)},
      };

        author.Books.AddRange(books);
        _context.SaveChanges();

    }

}
void InsertNewAuthorWithNewBook()
{
    var author = new Author { FirstName = "Lynda", LastName = "Rutledge" };
    var book = new Book[]
    {
       new Book{ Title = "111",
        PublishDate = new DateOnly(2050, 2, 1)},
         new Book{ Title = "222",
        PublishDate = new DateOnly(2050, 2, 1)},
    };
    author.Books.AddRange(book);
    //author.Books.Add(new Book
    //{
    //    Title = "West With Giraffes",
    //    PublishDate = new DateOnly(2021, 2, 1)
    //});
    _context.Authors.Add(author);
    //  _context.Books.Add(book);
    _context.SaveChanges();
}
#endregion
