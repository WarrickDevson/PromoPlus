using System;
using System.Collections.Generic;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Promotions;

namespace promoterplus.serverless.Models.Lookups
{
    public partial class Mediatype
    {
        public Mediatype()
        {
            Media = new HashSet<Media>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public User ModifiedUser { get; set; }
        public ICollection<Media> Media { get; set; }
    }
}
