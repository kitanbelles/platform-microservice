using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data; 

public class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProd)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
        }
    }

    private static void SeedData(AppDbContext context, bool isProd)
    {
        if(isProd)
        {
            Console.WriteLine("----> Attempting to apply migration");
            try
            {
                context.Database.Migrate(); 
            }
            catch(Exception ex)
            {
                Console.WriteLine($"----> Attempting to apply migration{ex.Message}");
            }
        }
        if(!context.Platforms.Any())
        {
            Console.WriteLine("------>Seeding the data");
            context.Platforms.AddRange(
                new Platform(){Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                new Platform(){Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free"},
                new Platform(){Name = "Kubernetes", Publisher = "Cloud Native Comupting Foundation", Cost = "Free"}
            );
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("------>We already have data");
        }
    }
}