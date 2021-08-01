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
                        PatronName = "Vasan",
                        PaymentPrice = 50,
                        PaymentDate = DateTime.Parse("2018-2-12")
                        
                    },
                    new Payment
                    {
                        PatronName = "Deva",
                        PaymentPrice = 55,
                        PaymentDate = DateTime.Parse("2019-2-12")
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
