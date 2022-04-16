#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using zixieWebAPI_Stocks.Models;

namespace zixieWebAPI_Stocks.Data
{
    public class zixieContext : DbContext
    {
        public zixieContext (DbContextOptions<zixieContext> options)
            : base(options)
        {
        }

        public DbSet<zixieWebAPI_Stocks.Models.Shares> Shares { get; set; }
    }
}
