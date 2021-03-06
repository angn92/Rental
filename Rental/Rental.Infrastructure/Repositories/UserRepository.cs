﻿using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Repository;
using Rental.Infrastructure.EF;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RentalContext _context;

        public UserRepository(RentalContext rentalContext)
        {
            _context = rentalContext;
        }

        public async Task<User> GetAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}

