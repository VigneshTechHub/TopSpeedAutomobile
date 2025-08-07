using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Domain.Models;
using TopSpeed.Infrastructure.Common;

namespace TopSpeed.Infrastructure.Repositories
{
    public class BrandRepository : GenericRepository<BrandModel>, IBrandRepository
    {
   public BrandRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task Update(BrandModel brand)
        {
            var objfrmdb = await _dbContext.Brand.FirstOrDefaultAsync(x => x.Id == brand.Id);

            if (objfrmdb != null) 
            {
                objfrmdb.Name = brand.Name;
                objfrmdb.EstablishedYear = brand.EstablishedYear;

                if (brand.BrandLogo != null) 
                {

                    objfrmdb.BrandLogo = brand.BrandLogo;
                }
            
                _dbContext.Update(objfrmdb);
            }
        }
    }
}

