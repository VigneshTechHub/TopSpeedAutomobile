using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.Models;

namespace TopSpeed.Application.Contracts.Presistence
{
    public interface IPostRepository : IGenericRepository<PostModel>
    {
        Task Update(PostModel Post);

        Task<PostModel> GetPostById(Guid id);

        Task<List<PostModel>> GetAllPost();

        Task<List<PostModel>> GetAllPost(Guid? skipRecord, Guid? brandId);  

        Task<List<PostModel>> GetAllPost(string? searchNamr, Guid? brandId, Guid? vehicleTypeId);


    }
}
