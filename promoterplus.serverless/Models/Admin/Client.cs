using System;
using System.Collections.Generic;
using promoterplus.serverless.Models.Promotions;

namespace promoterplus.serverless.Models.Admin
{
    public partial class Client
    {
        public Client()
        {
            Userclient = new HashSet<UserClient>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public User ModifiedUser { get; set; }
        public virtual ICollection<UserClient> Userclient { get; set; }
    }
}
