using Microsoft.AspNetCore.Identity;

namespace BikeStoreWeb.Core.Entities
{
    // IdentityUser'dan miras alıyoruz. Böylece Id, PasswordHash, Email, PhoneNumber gibi alanlar otomatik geliyor.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
