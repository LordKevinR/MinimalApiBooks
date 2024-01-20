using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalApiBooks.DTOs.Comment;
using MinimalApiBooks.Entities;
using MinimalApiBooks.Interfaces.Book;
using MinimalApiBooks.Interfaces.Comment;

namespace MinimalApiBooks.EndPoints.Comments
{
	public static class CommentEndpoints
	{
		public static RouteGroupBuilder MapComments(this RouteGroupBuilder group)
		{
			group.MapGet("/", GetAllByBookId)
				.CacheOutput(c => c.Expire(TimeSpan.FromHours(1))
				.Tag("get-comments"))
				.RequireCors("free");

			group.MapGet("/{id:int}", GetCommentById)
				.CacheOutput(c => c.Expire(TimeSpan.FromHours(1))
				.Tag("get-comments"))
				.RequireCors("free");

			group.MapPost("/", PostComment)
				.RequireCors("free");

			group.MapPut("/{id:int}", UpdateComment)
				.RequireCors("free");

			group.MapDelete("/{id:int}", DeleteComment)
				.RequireCors("free");

			return group;
		}

		private static async Task<Results<Ok<List<CommentResponseDTO>>, NotFound>> GetAllByBookId(int bookId, ICommentService commentService, IMapper mapper, IBookService bookService)
		{
			if (!await bookService.Exist(bookId))
			{
				return TypedResults.NotFound();
			}

			var comments = await commentService.GetAll(bookId);
			var commentResponse = mapper.Map<List<CommentResponseDTO>>(comments);
			return TypedResults.Ok(commentResponse);
		}

		private static async Task<Results<Ok<CommentResponseDTO>, NotFound>> GetCommentById(int bookId, int id, ICommentService commentService, IMapper mapper)
		{
			var comment = await commentService.GetById(id);

			if(comment is null) { return TypedResults.NotFound(); }

			var commentResponse = mapper.Map<CommentResponseDTO>(comment);
			return TypedResults.Ok(commentResponse);
		}

		private static async Task<Results<Created<CommentResponseDTO>, NotFound>> PostComment(int bookId, CommentRequestDTO commentRequestDTO, ICommentService commentService, IBookService bookService, IMapper mapper, IOutputCacheStore outputCacheStore)
		{
			if (!await bookService.Exist(bookId))
			{
				return TypedResults.NotFound();
			}

			var comment = mapper.Map<Comment>(commentRequestDTO);
			comment.BookId = bookId;

			var id = await commentService.Create(comment);
			await outputCacheStore.EvictByTagAsync("get-comments", default);
			var commentResponse = mapper.Map<CommentResponseDTO>(comment);
			return TypedResults.Created($"/comments/{id}", commentResponse);
		}

		private static async Task<Results<NoContent, NotFound>> UpdateComment(int bookId, int id, CommentRequestDTO commentRequestDTO, ICommentService commentService, IBookService bookService, IOutputCacheStore outputCacheStore)
		{

			var comment = await commentService.GetById(id);

			if (!await bookService.Exist(bookId) || comment is null || comment.BookId != bookId)
			{
				return TypedResults.NotFound();
			}

			if(commentRequestDTO.Content is not null)
			{
				comment.Content = commentRequestDTO.Content;
			}

			if(commentRequestDTO.Title is not null)
			{
				comment.Title = commentRequestDTO.Title;
			}

			await commentService.Update(comment);
			await outputCacheStore.EvictByTagAsync("get-comments", default);
			return TypedResults.NoContent();
		}

		private static async Task<Results<NoContent, NotFound>> DeleteComment(int bookId, int id, ICommentService commentService, IOutputCacheStore outputCacheStore)
		{
			var comment = await commentService.GetById(id);

			if(comment is null || comment.BookId != bookId)
			{
				return TypedResults.NotFound();
			}

			await commentService.Delete(comment.Id);
			await outputCacheStore.EvictByTagAsync("get-comments", default);
			return TypedResults.NoContent();
		}
	}
}