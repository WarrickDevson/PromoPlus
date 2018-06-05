using System;
using System.Collections.Generic;
using promoterplus.serverless.Models.Admin;

namespace promoterplus.serverless.Models.Admin
{
    public partial class UserClient
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public Client Client { get; set; }
        public User ModifiedUser { get; set; }
        public User User { get; set; }
    }
}
