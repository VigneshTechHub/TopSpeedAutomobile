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
    public class VehicleTypeRepository : GenericRepository<VehicleTypeModel>, IVehicleTypeRepository
    {
        public VehicleTypeRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            
        }
        public async Task Update(VehicleTypeModel vehicleType)
        {
            var objfrmdb = await _dbContext.VehicleType.FirstOrDefaultAsync(x=>x.Id == vehicleType.Id);

            if (objfrmdb != null)
            {

                objfrmdb.Name = vehicleType.Name;
            }
            _dbContext.Update(objfrmdb);
        }
    }
}
