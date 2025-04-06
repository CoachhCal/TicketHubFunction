using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHubFunction
{
    public class Customer
    {
 
        public int ConcertID { get; set; } = 0;

        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
 
        public string Phone { get; set; } = string.Empty;
        
        public int Quantity { get; set; } = 0;
        
        public string CreditCard { get; set; } = string.Empty;

        public string Expiration { get; set; } = string.Empty;

        public string SecurityCode { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

    }
}
