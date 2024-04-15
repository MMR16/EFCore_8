using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublisherData
{
    public class CoverAssignment
    {
        public int ArtistId { get; set; }
        public int CoverId { get; set; }
        public DateTime DateCreated { get; set; }
        public string CurrentUser { get; set; }
    }
}
