using MinimalApiBooks.Interfaces.Comment;

namespace MinimalApiBooks.Services.Comment
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository commentRepository;

		public CommentService(ICommentRepository commentRepository)
        {
			this.commentRepository = commentRepository;
		}
        public async Task<int> Create(Entities.Comment comment)
		{
			return await commentRepository.Create(comment);
		}

		public async Task Delete(int id)
		{
			await commentRepository.Delete(id);
		}

		public async Task<bool> Exist(int id)
		{
			return await commentRepository.Exist(id);
		}

		public async Task<List<Entities.Comment>> GetAll(int bookId)
		{
			return await commentRepository.GetAll(bookId);
		}

		public async Task<Entities.Comment?> GetById(int id)
		{
			return await commentRepository.GetById(id);
		}

		public async Task Update(Entities.Comment comment)
		{
			await commentRepository.Update(comment);
		}
	}
}
