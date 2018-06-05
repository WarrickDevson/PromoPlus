using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace promoterplus.serverless.Models
{
    [NotMapped]
    public class Custom
    {
        public class ClientProductsReturn
        {
            public string Client { get; set; }
            public List<Product> Products { get; set; }
        }

        public class Product
        {
            public int Id { get; set; }
            public string Label { get; set; }
        }
    }
}
