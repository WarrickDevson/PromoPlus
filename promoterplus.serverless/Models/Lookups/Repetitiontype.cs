using System;
using System.Collections.Generic;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Promotions;

namespace promoterplus.serverless.Models.Lookups
{
    public partial class RepetitionType
    {
        public RepetitionType()
        {
            Participant = new HashSet<Participant>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public User ModifiedUser { get; set; }
        public virtual ICollection<Participant> Participant { get; set; }
    }
}
