using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace MonitorApi.Models.DataBase
{
    public class User : IdentityUser
    {
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
