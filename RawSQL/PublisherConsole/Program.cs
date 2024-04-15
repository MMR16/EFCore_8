
using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

PubContext _context = new(); //existing database

//RawSQLStoredProc();
//InterpolatedSQLStoredProc();
//RunSQLQueryScalarMethods();
//TestView();
//TestProcDeleteCover(3);

// insert using existing stored procedure in database

InsertNewAuthorUsingProc();

void InsertNewAuthorUsingProc()
{
    var author = new Author { FirstName = "Alaa", LastName = "Rady" };
    author.Books.Add(new Book { Title="the green Road"});
    _context.Authors.Add(author);
    _context.SaveChanges();
}  

void TestProcDeleteCover(int coverId)
{
    var rowCount = _context.Database
         .ExecuteSqlRaw("DeleteCover {0}",coverId);
    var debug = _context.ChangeTracker.DebugView.ShortView;

    Console.WriteLine(rowCount);
}

void TestView()
{
    var data=_context.AuthorsByArtist.ToList(); // the view configurations are in DbContext // has no key + to view 
    var data2 = _context.AuthorsByArtist.FirstOrDefault();
    var data3 = _context.AuthorsByArtist.Where(e=>e.Artist.Contains("a")).ToList();
    var debug = _context.ChangeTracker.DebugView.ShortView;



}

void RunSQLQueryScalarMethods()
{
    var ids = _context.Database
        .SqlQuery<int>($"select AuthorId from Authors").ToList();

    var titles = _context.Database
    .SqlQuery<string>($"select title from books").ToList();

    var someTitles = _context.Database
  .SqlQuery<string>($"select title as value from books")
  .Where(e => e.Contains("the")).ToList();

    var query = _context.Database
        .SqlQuery<AuthorName>($" select lastname, firstname from authors")
        .TagWith("get first & last name from author").ToList();

    var querys = _context.Database
        .SqlQuery<AuthorName>($"GetAuthorNames")
        .ToList();
}

void RawSQLStoredProc()
{
    var authors = _context.Authors
        .FromSqlRaw("authorsPublishedInyearRange {0}, {1}", 2010, 2015)
        .ToList();
}

void InterpolatedSQLStoredProc()
{
    int startDate = 2010;
    int endDate = 2015;

    var authors = _context.Authors
        .FromSql($"authorsPublishedInyearRange {startDate}, {endDate}")
        .ToList();
}


public class AuthorName
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

