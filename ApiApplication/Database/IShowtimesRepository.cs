using ApiApplication.Database.Entities;
using ApiApplication.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiApplication.Database
{
    public interface IShowtimesRepository
    {
        Response<IEnumerable<ShowtimeEntity>> GetCollection();
        Response<IEnumerable<ShowtimeEntity>> GetCollection(Expression<Func<ShowtimeEntity, bool>> filter);
        Task<Response<ShowtimeEntity>> Add(ShowtimeEntity showtimeEntity);
        Task<Response<ShowtimeEntity>> Update(ShowtimeEntity showtimeEntity);
        Task<Response<ShowtimeEntity>> Delete(int id);
    }
}
