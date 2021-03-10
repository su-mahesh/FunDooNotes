using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.EmailMessageModel
{
    public class ResetLinkEmailModel
    {
        public string Email { get; set; }
        public string JwtToken { get; set; }
    }
}
