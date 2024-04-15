using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

PubContext _context = new(); //existing database

//  ConnectExistingArtestAndCoverObjects();
// ReassignACoverToArtist();

void ReassignACoverToArtist()
{
    var coverWithArtist = _context.Covers
                         .Include(e => e.Artists.Where(a => a.ArtistId == 6))
                         .FirstOrDefault(q => q.CoverId == 7);
    // coverWithArtist?.Artists.RemoveAt(0);

    var artist6 = _context.Artists.Find(8);
    coverWithArtist.Artists.Add(artist6);
    _context.ChangeTracker.DetectChanges();
    var state1 = _context.ChangeTracker.DebugView.LongView;
    _context.SaveChanges();
    var state2 = _context.ChangeTracker.DebugView.LongView;
}

void ConnectExistingArtestAndCoverObjects()
{
    //var artistA = _context.Artists.Find(1);
    //  var artistB = _context.Artists.Find(2);
    //var coverA = _context.Covers.Find(1);
    //coverA.Artists.Add(artistA);
    var artist = new Artist { FirstName = "Mostafa", LastName = "Mahmoud" };
    var cover = new Cover { DesignIdeas = "mmr", DigitalOnly = false };
    _context.Artists.Add(artist);

    artist.Covers.Add(cover);

    var state = _context.ChangeTracker.DebugView.LongView;

    _context.SaveChanges();
    var states = _context.ChangeTracker.DebugView.LongView;

}