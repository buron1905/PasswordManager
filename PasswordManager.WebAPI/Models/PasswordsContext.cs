﻿using Microsoft.EntityFrameworkCore;
using PasswordManager.WebAPI.Models.Passwords;

namespace PasswordManager.WebAPI.Models
{
    public class PasswordsContext : DbContext
    {

        public PasswordsContext(DbContextOptions<PasswordsContext> options)
            : base(options)
        {
        }

        public DbSet<Password> Passwords { get; set; } = null!;
    }
}
