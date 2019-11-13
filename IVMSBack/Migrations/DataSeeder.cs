using IVMSBack.Areas.Identity.Data;
using IVMSBack.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IVMSBack.Migrations
{
    public class DataSeeder
    {
        public static async Task SeedCountriesAsync(IVMSBackContext context, 
            RoleManager<IVMSBackRole> roleManager,
            UserManager<IVMSBackUser> userManager)
        {
            var roleAdmin = new IVMSBackRole();
            roleAdmin.Name = "Super Administrador";
            await roleManager.CreateAsync(roleAdmin);

            var role = new IVMSBackRole();
            role.Name = "Administrador";
            await roleManager.CreateAsync(role);

            role = new IVMSBackRole();
            role.Name = "Monitorista";
            await roleManager.CreateAsync(role);

            role = new IVMSBackRole();
            role.Name = "Conductor";
            await roleManager.CreateAsync(role);

            var user = new IVMSBackUser();
            user.Name = "Gad Arenas";
            user.UserName = "garenas@sysware.com.mx";
            user.Email = "garenas@sysware.com.mx";
            await userManager.CreateAsync(user, "Sysware2016");
        }
    }
}
