using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(IVMSBack.Areas.Identity.IdentityHostingStartup))]
namespace IVMSBack.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
        }
    }
}