using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Domain.Models;
using TopSpeed.Infrastructure.Common;

namespace TopSpeed.Infrastructure.Repositories
{
    public class PostRepository : GenericRepository<PostModel>, IPostRepository
    {
        public PostRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task Update(PostModel Post)
        {
           var objfrmdb = await _dbContext.Post.FirstOrDefaultAsync(x=>x.Id == Post.Id);

            if (objfrmdb != null)
            {
                objfrmdb.Id = Post.Id;
                objfrmdb.VehicleTypeId = Post.VehicleTypeId;
                objfrmdb.Name = Post.Name;
                objfrmdb.EngineAndFuelType = Post.EngineAndFuelType;
                objfrmdb.Transmission= Post.Transmission;
                objfrmdb.Engine= Post.Engine;
                objfrmdb.Range= Post.Range;
                objfrmdb.Rating= Post.Rating;
                objfrmdb.SeatingCapacity= Post.SeatingCapacity;
                objfrmdb.Milage= Post.Milage;
                objfrmdb.PriceFrom = Post.PriceFrom;
                objfrmdb.PriceTo = Post.PriceTo;
                objfrmdb.TopSpeed= Post.TopSpeed;

                if (Post.VehicleImage != null) 
                {
                objfrmdb.VehicleImage= Post.VehicleImage;
                }
                _dbContext.Update(objfrmdb);

                await _dbContext.SaveChangesAsync(); /*thisonefrmchatgpt*/
            }
        }    

        public async Task<PostModel> GetPostById(Guid id)
        {
            return await _dbContext.Post.Include(x => x.Brand).Include(x => x.VehicleType).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<PostModel>> GetAllPost()
        {
            return await _dbContext.Post.Include(x => x.Brand).Include(x => x.VehicleType).ToListAsync();
        }

        public async Task<List<PostModel>> GetAllPost(Guid? skipRecord, Guid? brandId)
        {
            var query = _dbContext.Post.Include(x => x.Brand).Include(x => x.VehicleType).OrderByDescending(x => x.ModifiedOn);

            if (brandId == Guid.Empty) 
            {
            return await query.ToListAsync();
            } 
            
            if (brandId != Guid.Empty) 
            {
                query = (IOrderedQueryable<PostModel>)query.Where(x=> x.BrandId == brandId);

            }

            var posts = await query.ToListAsync();

            if (skipRecord.HasValue)
            {
                var recordToRemove = posts.FirstOrDefault(x=> x.Id == skipRecord.Value);
                if (recordToRemove != null) 
                {
                    posts.Remove(recordToRemove);
                }
            }
            return posts;
        }

        public async Task<List<PostModel>> GetAllPost(string searchName, Guid? brandId, Guid? vehicleTypeId)
        {
            var query = _dbContext.Post.Include(x => x.Brand).Include(x => x.VehicleType).OrderByDescending(x => x.ModifiedOn);

            if (searchName == string.Empty && brandId == Guid.Empty && vehicleTypeId == Guid.Empty)
            {
              return await query.ToListAsync();
            }

            if (brandId != Guid.Empty) 
            {
            query = (IOrderedQueryable<PostModel>)query.Where(x=> x.BrandId == brandId);
            }

            if (vehicleTypeId != Guid.Empty) 
            {
            query = (IOrderedQueryable<PostModel>)query.Where(x=> x.VehicleTypeId == vehicleTypeId);
            }
            
            if (!string.IsNullOrEmpty(searchName)) 
            {
            query = (IOrderedQueryable<PostModel>)query.Where(x=> x.Name.Contains(searchName));
            }
            return await query.ToListAsync();

        }
    }
}
