using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

PubContext _context = new(); //existing database

SimpleRawSQL();

void SimpleRawSQL()
{
    var authors = _context.Authors.FromSqlRaw("select * from authors").ToList();
}

//GetAllBooksWithCovers();
//GetAllBooksWithExistingCovers();
//MultiLevelInInclude();
//NewBookAndCover();
//AddCoverToExistingBook();
//AddCoverToExistingBookWithTrackedCover();
//ProtechtingFromUniqueFKSideEffect();
//AddCoverToExistingBookHasUnTrackedCover();


void ProtechtingFromUniqueFKSideEffect()
{
    var TheneverDesignIdea = "A Spirally Spiral";
    var book = _context.Books.Include(e => e.Cover).FirstOrDefault(e => e.BookId == 25);
    if (book.Cover != null)
    {
        book.Cover.DesignIdeas = TheneverDesignIdea;
    }
    else
    {
        book.Cover = new Cover { DesignIdeas = TheneverDesignIdea };
    }
    _context.SaveChanges();
}

void AddCoverToExistingBookHasUnTrackedCover()
{
    // error foriegn key duplication
    var book = _context.Books.Find(25);
    book.Cover = new Cover { DesignIdeas = "***123**********" };
    _context.SaveChanges();
    var debugView = _context.ChangeTracker.DebugView.ShortView;
}

void AddCoverToExistingBookWithTrackedCover()
{
    var book = _context.Books.Include(e => e.Cover).FirstOrDefault(e => e.BookId == 25);
    book.Cover = new Cover { DesignIdeas = "---------" };
    _context.ChangeTracker.DetectChanges();
    var debugView = _context.ChangeTracker.DebugView.ShortView;
    _context.SaveChanges();
    /* 
     if we add cover to book which has existing cover ef core will delete the old one then insert the new one
     Book {BookId: 25} Unchanged FK {AuthorId: 12}
     Cover {CoverId: -2147482647} Added FK {BookId: 25}
     Cover {CoverId: 18} Deleted FK {BookId: 25} 
     */
}

void AddCoverToExistingBook()
{
    var book = _context.Books.Find(25);
    if (book is not null)
    {
        book.Cover = new Cover { DesignIdeas = "***************" };
        _context.SaveChanges();
        var results = _context.ChangeTracker.DebugView.LongView;

    }
}

void NewBookAndCover()
{
    var author = new Author { FirstName = "Alaa", LastName = "Rady" };
    var book = new Book
    {
        Title = "Call Me Ishtar",
        PublishDate = new DateOnly(1973, 3, 1)
    };
    author.Books.Add(book);
    book.Cover = new Cover { DesignIdeas = "Image of ishtar" };

    var artists = new List<Artist> {
        new Artist {FirstName="Mostafa",LastName="Mahmoud" },
        new Artist {FirstName="Ali",LastName="Lady" }
    };
    book.Cover.Artists.AddRange(artists);
    _context.Authors.Add(author);
    var result = _context.ChangeTracker.DebugView.ShortView;
    _context.SaveChanges();


}

void MultiLevelInInclude()
{
    var authorGraph = _context.Authors.AsNoTracking()
         .Include(e => e.Books)
         .ThenInclude(e => e.Cover)
         .ThenInclude(e => e.Artists)
         .FirstOrDefault(e => e.AuthorId == 2);
    Console.WriteLine();
    Console.WriteLine("*************----------***************");
    Console.WriteLine();
    Console.WriteLine("author:  " + authorGraph?.FirstName + "  " + authorGraph?.LastName);

    foreach (var book in authorGraph.Books)
    {
        Console.WriteLine($"Book: {book.Title}");
        if (book.Cover != null)
        {
            Console.WriteLine($"Design ideas: {book.Cover.DesignIdeas}");
            Console.WriteLine($"Artists : ");
            book.Cover.Artists.ForEach(e => Console.Write(e.LastName));
        }
    }
}

void GetAllBooksWithCovers()
{
    var booksAndCovers = _context.Books.Include(e => e.Cover).ToList();
    foreach (var book in booksAndCovers)
    {
        Console.WriteLine(book.Title + (book.Cover == null ? "---No Cover yet" : $"--- the cover design idea is {book.Cover.DesignIdeas}"));
    }
}

void GetAllBooksWithExistingCovers()
{
    var booksAndCovers = _context.Books.Include(e => e.Cover).Where(e => e.Cover != null).ToList();
    foreach (var book in booksAndCovers)
    {
        Console.WriteLine("*****************" + book.Title + $"\n the cover design idea is {book.Cover.DesignIdeas}");
    }
}