using PaySystemsApi.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaySystemsAPI.Configuration
{
    public class TokenParameters:ITokenParameters
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Id { get; set; }
        public IList<string> Role { get; set; }
    }
}
