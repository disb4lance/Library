

using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Classes.Models
{
    public class User : IdentityUser
    {

        [JsonIgnore]
        public int IsnNode { get; set; }
        public string Customname { get; set; }
       
        public string CustomEmail { get; set; }
        public string Custompassword { get; set; }


    }
}
