using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CMM.Data; // So that can find the context class for the table connection.


namespace CMM.Models
{
    public class SeedData //to help hardcode your data direct to your db when first load the program
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CMMNewContext(
               serviceProvider.GetRequiredService<DbContextOptions<CMMNewContext>>()))
            {
                // Look for any flower.
                if (context.Payment.Any()) //if table is empty
                {
                    return;
                }
                context.Payment.AddRange( //hard code data to your table
                    new Payment
                    {
                        User_id = "f3ab64cb-6c9d-4560-8b2f-a50ff48141b8",
                        PaymentPrice = 50,
                        PaymentDate = DateTime.Parse("2021-7-31"),
                        ConcertID = 1
                    },
                    new Payment
                    {
                        User_id = "7cd07b43-e6a0-4937-8882-34be21e80839",
                        PaymentPrice = 55,
                        PaymentDate = DateTime.Parse("2021-7-31"),
                        ConcertID = 1
                    }
                ); ; ; ; ;
                context.SaveChanges();
            }
        }
    }
}
