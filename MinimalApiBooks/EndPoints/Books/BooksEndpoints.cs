using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalApiBooks.DTOs.Books;
using MinimalApiBooks.DTOs.Pagination;
using MinimalApiBooks.Entities;
using MinimalApiBooks.Interfaces.Book;
using MinimalApiBooks.Interfaces.FileStorage;
using MinimalApiBooks.Interfaces.Genre;

namespace MinimalApiBooks.EndPoints.Books
{
	public static class BooksEndpoints
	{
		private static readonly string container = "books";

		public static RouteGroupBuilder MapBooks(this RouteGroupBuilder group)
		{
			group.MapGet("/", GetAllBooks)
				.CacheOutput(c => c.Expire(TimeSpan.FromHours(1))
				.Tag("get-books"))
				.RequireCors("free");
			
			group.MapGet("/{id:int}", GetBookById)
				.CacheOutput(c => c.Expire(TimeSpan.FromHours(1))
				.Tag("get-books"))
				.RequireCors("free");

			group.MapPost("/", PostBook)
				.DisableAntiforgery()
				.RequireCors("free");

			group.MapPut("/{id:int}", UpdateBook)
				.DisableAntiforgery()
				.RequireCors("free");		
			
			group.MapDelete("/{id:int}", DeleteBook)
				.RequireCors("free");
				
			group.MapPost("/{id:int}/assigngenres", AssignGenres)
				.RequireCors("free");

			return group;
		}

		private static async Task<Ok<List<BookResponseDTO>>> GetAllBooks(IBookService bookService, IMapper mapper, int page = 1, int recordsPerPage = 10)
		{
			var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
			var books = await bookService.GetAll(pagination);
			var booksResponse = mapper.Map<List<BookResponseDTO>>(books);
			return TypedResults.Ok(booksResponse);
		}

		private static async Task<Results<Ok<BookResponseDTO>, NotFound>> GetBookById(int id, IBookService bookService, IMapper mapper)
		{
			var book = await bookService.GetById(id);
			
			if(book is null) { return TypedResults.NotFound(); }

			var bookResponse = mapper.Map<BookResponseDTO>(book);
			return TypedResults.Ok(bookResponse);
		}

		private static async Task<Created<BookResponseDTO>> PostBook([FromForm] BookRequestDTO bookRequestDTO, IMapper mapper, IBookService bookService, IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
		{
			var book = mapper.Map<Book>(bookRequestDTO);

			if (bookRequestDTO.CoverImage is not null)
			{
				var url = await fileStorage.Store(container, bookRequestDTO.CoverImage);
				book.CoverImage = url;
			}

			var id = await bookService.Create(book);
			await outputCacheStore.EvictByTagAsync("get-books", default);
			var bookResponse = mapper.Map<BookResponseDTO>(book);
			return TypedResults.Created($"/books/{id}", bookResponse);
		}

		private static async Task<Results<NoContent, NotFound>> UpdateBook(int id, [FromForm] BookRequestDTO bookRequestDTO, IBookService bookService, IMapper mapper, IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
		{
			var bookDb = await bookService.GetById(id);

			if (bookDb is null) { return TypedResults.NotFound(); }

			var bookForUpdate = mapper.Map<Book>(bookRequestDTO);
			bookForUpdate.Id = id;
			bookForUpdate.CoverImage = bookDb.CoverImage;

			if(bookRequestDTO.CoverImage is not null)
			{
				var url = await fileStorage.Update(bookForUpdate.CoverImage, container, bookRequestDTO.CoverImage);
				bookForUpdate.CoverImage = url;
			}

			await outputCacheStore.EvictByTagAsync("get-books", default);
			await bookService.Update(bookForUpdate);
			return TypedResults.NoContent();
		}

		private static async Task<Results<NoContent, NotFound>> DeleteBook(int id, IBookService bookService, IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
		{
			var bookDb = await bookService.GetById(id);

			if (bookDb is null) { return TypedResults.NotFound(); }

			if(bookDb.CoverImage is not null)
			{
				await fileStorage.Delete(bookDb.CoverImage, container);
			}

			await bookService.Delete(id);
			await outputCacheStore.EvictByTagAsync("get-books", default);
			return TypedResults.NoContent();
		}

		private static async Task<Results<NoContent, NotFound, BadRequest<string>>> AssignGenres(int id, List<int> genresIds, IBookService bookService, IGenreService genreService, IOutputCacheStore outputCacheStore)
		{
			if(!await bookService.Exist(id))
			{
				return TypedResults.NotFound();
			}

			var existingGenres = new List<int>();

			if(genresIds.Count != 0)
			{
				existingGenres = await genreService.ExistList(genresIds);
			}

			if(existingGenres.Count != genresIds.Count)
			{
				var NotExistingGenres = genresIds.Except(existingGenres);
				return TypedResults.BadRequest($"the genres with the id: {string.Join(", ", NotExistingGenres )} don't exist");
			}

			await bookService.AssignGenres(id, genresIds);
			await outputCacheStore.EvictByTagAsync("get-books", default);
			return TypedResults.NoContent();
		}
	}
}
