using Microsoft.EntityFrameworkCore;
using MinimalApiBooks.Interfaces.Comment;

namespace MinimalApiBooks.Repositories.Comment
{
	public class CommentRepository : ICommentRepository
	{
		private readonly ApplicationDbContext context;

		public CommentRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async Task<List<Entities.Comment>> GetAll(int bookId)
		{
			return await context.Comments.Where(c => c.BookId == bookId).ToListAsync();
		}

		public async Task<Entities.Comment?> GetById(int id)
		{
			return await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<int> Create(Entities.Comment comment)
		{
			context.Add(comment);
			await context.SaveChangesAsync();
			return comment.Id;
		}

		public async Task<bool> Exist(int id)
		{
			return await context.Comments.AnyAsync(c => c.Id == id);
		}

		public async Task Update(Entities.Comment comment)
		{
			context.Update(comment);
			await context.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			await context.Comments.Where(c => c.Id == id).ExecuteDeleteAsync();
		}
	}
}
