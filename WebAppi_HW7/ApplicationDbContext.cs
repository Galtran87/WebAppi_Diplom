using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi_HW7;
using WebAppi_Diplom;

namespace WebAppi_Diplom
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
            //=> Database.EnsureCreated();
     


        // Додайте DbSet для вашого додаткового контенту, якщо потрібно
    }
}