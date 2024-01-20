using AutoMapper;
using MinimalApiBooks.DTOs.Authors;
using MinimalApiBooks.DTOs.Books;
using MinimalApiBooks.DTOs.Comment;
using MinimalApiBooks.DTOs.Genres;
using MinimalApiBooks.Entities;

namespace MinimalApiBooks.Utilities.AutoMapper
{
	public class AutoMapperProfiles: Profile
	{
        public AutoMapperProfiles()
        {
            CreateMap<GenreRequestDTO, Genre>();
            CreateMap<Genre, GenreResponseDTO>();

            CreateMap<AuthorRequestDTO, Author>()
                .ForMember(x => x.Photo, options => options.Ignore());
            CreateMap<Author, AuthorResponseDTO>();

            CreateMap<BookRequestDTO, Book>()
                .ForMember(x => x.CoverImage, options => options.Ignore());
            CreateMap<Book, BookResponseDTO>();

            CreateMap<CommentRequestDTO, Comment>();
            CreateMap<Comment, CommentResponseDTO>();
        }
    }
}
