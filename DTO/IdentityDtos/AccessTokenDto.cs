using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.IdentityDtos
{
    public class AccessTokenDto
    {
        public int UserID { get; set; }
        public string AccessToken { get; set; }
    }
}
