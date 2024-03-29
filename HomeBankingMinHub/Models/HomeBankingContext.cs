﻿using Microsoft.EntityFrameworkCore;

namespace HomeBankingMinHub.Models
{
    public class HomeBankingContext:DbContext
    {
        public HomeBankingContext(DbContextOptions<HomeBankingContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
    }
}
