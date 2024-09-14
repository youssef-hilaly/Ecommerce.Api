using AutoMapper;
using Ecommerce.DataService.Data;
using Ecommerce.DataService.Repositories.Interfaces;
using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Dtos.Store;
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
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {


        public StoreRepository(AppDbContext context, IMapper mapper, UserManager<User> userManager) : base(context, userManager, mapper)
        {
        }

        public override async Task<Result<Store>> AddAsync(Store entity)
        {
            var owner = await _userManager.FindByIdAsync(entity.UserId);
            if(owner == null)
            {
                return Result<Store>.Failure("User not found");
            }

            var isStoreOwner = await _userManager.IsInRoleAsync(owner, "StoreOwner");

            if (!isStoreOwner)
            {
                return Result<Store>.Failure("User is not a store owner");
            }

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Result<Store>.Success();

        }
    }
}
