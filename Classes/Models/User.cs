

using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Classes.Models
{
    public class User : IdentityUser
    {

        [JsonIgnore]
        public int IsnNode { get; set; }
       

    }
}
