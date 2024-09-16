using AutoMapper;
using Ecommerce.DataService.Data;
using Ecommerce.DataService.Repositories.Interfaces;
using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataService.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly UserManager<User> _userManager;
        protected readonly IMapper _mapper;

        public GenericRepository(AppDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }
        public virtual async Task<Result<T>> AddAsync(T entity)
        {
            var result = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Result<T>.Success(result.Entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);
            if (entity == null) return false;
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exists(Guid id)
        {
            return await GetAsync(id) != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetAsync(Guid? id)
        {
            if (id == null)
            {
                return null;
            }

            return await _context.FindAsync<T>(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
