using ApiApplication.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ApiApplication.Models;
using Microsoft.Extensions.Options;
using IMDbApiLib;
using System.Threading.Tasks;
using IMDbApiLib.Models;
using System.Linq.Expressions;
using ApiApplication.Wrappers;

namespace ApiApplication.Database
{
    public class ShowtimesRepository : IShowtimesRepository
    {
        private readonly CinemaContext _context;
        private readonly ImdbSetting _imdbSetting;
        public ShowtimesRepository(CinemaContext context, IOptions<ImdbSetting> imdbSetting)
        {
            _context = context;
            _imdbSetting = imdbSetting.Value;
        }

        public async Task<Response<ShowtimeEntity>> Add(ShowtimeEntity showtimeEntity)
        {
            var movieInfo = await this.GetMovieById(showtimeEntity.Movie.ImdbId);
            if (movieInfo !=null) {
                showtimeEntity.Movie.Title = movieInfo.Title;
                showtimeEntity.Movie.Stars = movieInfo.Stars;
                showtimeEntity.Movie.ReleaseDate = DateTime.Parse(movieInfo.ReleaseDate);
            }
            await _context.Set<ShowtimeEntity>().AddAsync(showtimeEntity);
            await _context.SaveChangesAsync();

            return new Response<ShowtimeEntity>() {Data= showtimeEntity,Message= "Added ShowTime Successfully",Succeeded=true} ;
        }

        public async Task<Response<ShowtimeEntity>> Delete(int id)
        {
            var showTime = await _context.Showtimes.Where(x=>x.Id==id).FirstOrDefaultAsync();
            if (showTime != null) {
                _context.Showtimes.Remove(showTime);
                await _context.SaveChangesAsync();
            }
            return new Response<ShowtimeEntity>() { Data = showTime, Message = "Deleted ShowTime Successfully", Succeeded = true };
        }

        public Response<IEnumerable<ShowtimeEntity>> GetCollection()
        {
            return GetCollection(null);
        }

        public Response<IEnumerable<ShowtimeEntity>> GetCollection(Expression<Func<ShowtimeEntity, bool>> filter)
        {
            IQueryable<ShowtimeEntity> query = _context.Showtimes.Include(x => x.Movie).AsQueryable();
            if (filter!=null)
            {
                query = query.Where(filter);
            }            
            return new Response<IEnumerable<ShowtimeEntity>>() { Data = query.ToList(), Message = "Get ShowTime Successfully", Succeeded = true };
        }

        private async Task<TitleData> GetMovieById(string Id) {
            var apiLib = new ApiLib(_imdbSetting.key);
            return  await apiLib.TitleAsync(Id);

        }

        public async Task<Response<ShowtimeEntity>> Update(ShowtimeEntity showtimeEntity)
        {
            var movieInfo = new TitleData();
            var existingMovieInfo = await _context.Movies.Where(x => x.ShowtimeId == showtimeEntity.Id).AsNoTracking().FirstOrDefaultAsync();

            if (showtimeEntity.Movie.ImdbId != null)
            {
                movieInfo = await this.GetMovieById(showtimeEntity.Movie.ImdbId);

                if (movieInfo != null)
                {
                    if (showtimeEntity.Movie.Id == 0) {
                        showtimeEntity.Movie.Id = existingMovieInfo.Id;
                    }
                    showtimeEntity.Movie.Title = movieInfo.Title;
                    showtimeEntity.Movie.Stars = movieInfo.Stars;
                    showtimeEntity.Movie.ReleaseDate = DateTime.Parse(movieInfo.ReleaseDate);
                }
            }
            else {
  
                showtimeEntity.Movie = existingMovieInfo;
            }
            
            _context.Showtimes.Update(showtimeEntity);
            await _context.SaveChangesAsync();
            
            return new Response<ShowtimeEntity>() { Data = showtimeEntity, Message = "Updated ShowTime Successfully", Succeeded = true };
        }
    }
}
