using MinimalApiBooks.DTOs.Pagination;

namespace MinimalApiBooks.Utilities.ExtensionsMethods
{
	public static class IQueryableExtensions
	{
		public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
		{
			return queryable
				.Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPerPage)
				.Take(paginationDTO.RecordsPerPage);
		}
	}
}
