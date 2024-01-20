using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalApiBooks.DTOs.Genres;
using MinimalApiBooks.Entities;
using MinimalApiBooks.Interfaces.Genre;

namespace MinimalApiBooks.EndPoints.Genres
{
	public static class GenresEndpoints
	{
		public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
		{
			group.MapGet("/", GetAllGenres)
				.CacheOutput(c => c.Expire(TimeSpan.FromHours(1))
				.Tag("get-genres"))
				.RequireCors("free");

			group.MapGet("/{id:int}", GetGenreById)
				.CacheOutput(c => c.Expire(TimeSpan.FromHours(1))
				.Tag("get-genres"))
				.RequireCors("free");

			group.MapPost("/", PostGenre)
				.RequireCors("free");

			group.MapPut("/{id:int}", UpdateGenre)
				.RequireCors("free");

			group.MapDelete("/{id:int}", DeleteGenre)
				.RequireCors("free");

			return group;
		}

		static async Task<Ok<List<GenreResponseDTO>>> GetAllGenres(IGenreService genreService, IMapper mapper)
		{
			var genre = await genreService.GetAll();
			var GenreResponseDTO = mapper.Map<List<GenreResponseDTO>>(genre);
			return TypedResults.Ok(GenreResponseDTO);
		}

		static async Task<Results<Ok<GenreResponseDTO>, NotFound>> GetGenreById(int id, IGenreService genreService, IMapper mapper)
		{
			var genre = await genreService.GetById(id);
			if (genre is null)
			{
				return TypedResults.NotFound();
			}
			var GenreResponseDTO = mapper.Map<GenreResponseDTO>(genre);
			return TypedResults.Ok(GenreResponseDTO);
		}

		static async Task<Created<GenreResponseDTO>> PostGenre(GenreRequestDTO genreRequestDTO, IGenreService genreService, IOutputCacheStore outputCacheStore, IMapper mapper)
		{
			var genre = mapper.Map<Genre>(genreRequestDTO);

			var id = await genreService.Create(genre);
			await outputCacheStore.EvictByTagAsync("get-genres", default);

			var genreResponseDTO = mapper.Map<GenreResponseDTO>(genre);

			return TypedResults.Created($"/genres/{id}", genreResponseDTO);
		}

		static async Task<Results<NotFound, NoContent>> UpdateGenre(int id, GenreRequestDTO genreRequestDTO, IGenreService genreService, IOutputCacheStore outputCacheStore, IMapper mapper)
		{
			var exist = await genreService.Exist(id);

			if (!exist)
			{
				return TypedResults.NotFound();
			}

			var genre = mapper.Map<Genre>(genreRequestDTO);
			genre.Id = id;

			await genreService.Update(genre);
			await outputCacheStore.EvictByTagAsync("get-genres", default);
			return TypedResults.NoContent();
		}

		static async Task<Results<NotFound, NoContent>> DeleteGenre(int id, IGenreService genreService, IOutputCacheStore outputCacheStore)
		{
			var exist = await genreService.Exist(id);

			if (!exist)
			{
				return TypedResults.NotFound();
			}

			await genreService.Delete(id);
			await outputCacheStore.EvictByTagAsync("get-genres", default);
			return TypedResults.NoContent();
		}
	}
}
