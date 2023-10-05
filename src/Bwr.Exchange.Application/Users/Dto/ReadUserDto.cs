using System;
using System.Collections.Generic;
using System.Text;

namespace Bwr.Exchange.Users.Dto
{
    public class ReadUserDto
    {
        public long id { get; set; }
        public string userName { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string emailAddress { get; set; }
        public bool isActive { get; set; }
        public string fullName { get; set; }
        
    }
}
