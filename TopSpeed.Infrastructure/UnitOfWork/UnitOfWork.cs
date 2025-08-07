using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Infrastructure.Common;
using TopSpeed.Infrastructure.Repositories;

namespace TopSpeed.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UnitOfWork(ApplicationDbContext dbcontext, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
           _dbcontext = dbcontext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;

            Brand = new BrandRepository(_dbcontext);

            VehicleType = new VehicleTypeRepository(_dbcontext);

            Post = new PostRepository(_dbcontext);
        }
        public IBrandRepository Brand { get;private set; }

        public IVehicleTypeRepository VehicleType { get; private set; }

        public IPostRepository Post { get; private set; }

        public void Dispose()
        {
           _dbcontext.Dispose();
        }

        public async Task SaveAsync()
        {
            _dbcontext.SaveCommonFields(_userManager,_httpContextAccessor);
           await _dbcontext.SaveChangesAsync();
        }
    }
}
