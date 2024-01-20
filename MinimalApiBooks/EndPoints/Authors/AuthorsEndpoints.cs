using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalApiBooks.DTOs.Authors;
using MinimalApiBooks.DTOs.Pagination;
using MinimalApiBooks.Entities;
using MinimalApiBooks.Interfaces.Author;
using MinimalApiBooks.Interfaces.FileStorage;

namespace MinimalApiBooks.EndPoints.Authors
{
	public static class AuthorsEndpoints
	{
		private static readonly string container = "authors";

		public static RouteGroupBuilder MapAuthors(this RouteGroupBuilder group)
		{
			group.MapGet("/", GetAllAuthors)
				.CacheOutput(c => c.Expire(TimeSpan.FromHours(1))
				.Tag("get-authors"))
				.RequireCors("free");

			group.MapGet("/{id:int}", GetAuthorById)
				.CacheOutput(c => c.Expire(TimeSpan.FromHours(1))
				.Tag("get-authors"))
				.RequireCors("free");	
			
			group.MapGet("getByName/{name}", GetAuthorByName)
				.CacheOutput(c => c.Expire(TimeSpan.FromHours(1))
				.Tag("get-authors"))
				.RequireCors("free");

			group.MapPost("/", PostAuthor)
				.DisableAntiforgery()
				.RequireCors("free");
					
			group.MapPut("/{id:int}", UpdateAuthor)
				.DisableAntiforgery()
				.RequireCors("free");					
			
			group.MapDelete("/{id:int}", DeleteAuthor)
				.RequireCors("free");

			return group;
		}

		private static async Task<Ok<List<AuthorResponseDTO>>> GetAllAuthors(IAuthorService authorService, IMapper mapper, int page = 1, int recordsPerPage = 10)
		{
			var paginationDto = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };

			var authors = await authorService.GetAll(paginationDto);
			var authorsRespose = mapper.Map<List<AuthorResponseDTO>>(authors);
			return TypedResults.Ok(authorsRespose);
		}

		private static async Task<Results<Ok<AuthorResponseDTO>, NotFound>> GetAuthorById(int id, IAuthorService authorService, IMapper mapper)
		{
			var author = await authorService.GetById(id);

			if (author is null)
			{
				return TypedResults.NotFound();
			}

			var authorRespose = mapper.Map<AuthorResponseDTO>(author);
			return TypedResults.Ok(authorRespose);
		}

		private static async Task<Ok<List<AuthorResponseDTO>>> GetAuthorByName(string name, IAuthorService authorService, IMapper mapper)
		{
			var authors = await authorService.GetByName(name);
			var authorResponse = mapper.Map<List<AuthorResponseDTO>>(authors);
			return TypedResults.Ok(authorResponse);
		}

		private static async Task<Created<AuthorResponseDTO>> PostAuthor([FromForm] AuthorRequestDTO authorRequestDTO, IMapper mapper, IAuthorService authorService, IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
		{
			var author = mapper.Map<Author>(authorRequestDTO);

			if (authorRequestDTO.Photo is not null)
			{
				var url = await fileStorage.Store(container, authorRequestDTO.Photo);
				author.Photo = url;
			}

			var id = await authorService.Create(author);
			await outputCacheStore.EvictByTagAsync("get-authors", default);
			var authorResponse = mapper.Map<AuthorResponseDTO>(author);
			return TypedResults.Created($"/authors/{id}", authorResponse);
		}

		private static async Task<Results<NoContent, NotFound>> UpdateAuthor(int id, [FromForm] AuthorRequestDTO authorRequest, IAuthorService authorService, IFileStorage fileStorage, IOutputCacheStore outputCacheStore, IMapper mapper)
		{
			var authorDb = await authorService.GetById(id);

			if(authorDb is null) { return TypedResults.NotFound(); }

			var authorForUpdate = mapper.Map<Author>(authorRequest);
			authorForUpdate.Id = id;
			authorForUpdate.Photo = authorDb.Photo;

			if(authorRequest.Photo is not null)
			{
				var url = await fileStorage.Update(authorForUpdate.Photo, container, authorRequest.Photo);

				authorForUpdate.Photo = url;
			}

			await authorService.Update(authorForUpdate);
			await outputCacheStore.EvictByTagAsync("get-authors", default);
			return TypedResults.NoContent();
		}

		private static async Task<Results<NoContent, NotFound>> DeleteAuthor(int id,IAuthorService authorService, IFileStorage fileStorage, IOutputCacheStore outputCacheStore)
		{
			var author = await authorService.GetById(id);

			if(author is null)
			{
				return TypedResults.NotFound();
			}

			await fileStorage.Delete(author.Photo, container);
			await authorService.Delete(id);
			await outputCacheStore.EvictByTagAsync("get-authors", default);
			return TypedResults.NoContent();
		}
	}
}
