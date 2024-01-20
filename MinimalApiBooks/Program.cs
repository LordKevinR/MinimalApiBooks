using Microsoft.EntityFrameworkCore;
using MinimalApiBooks;
using MinimalApiBooks.EndPoints.Authors;
using MinimalApiBooks.EndPoints.Books;
using MinimalApiBooks.EndPoints.Comments;
using MinimalApiBooks.EndPoints.Genres;
using MinimalApiBooks.Interfaces.Author;
using MinimalApiBooks.Interfaces.Book;
using MinimalApiBooks.Interfaces.Comment;
using MinimalApiBooks.Interfaces.FileStorage;
using MinimalApiBooks.Interfaces.Genre;
using MinimalApiBooks.Repositories.Author;
using MinimalApiBooks.Repositories.Book;
using MinimalApiBooks.Repositories.Comment;
using MinimalApiBooks.Repositories.Genre;
using MinimalApiBooks.Services.Author;
using MinimalApiBooks.Services.Azure;
using MinimalApiBooks.Services.Book;
using MinimalApiBooks.Services.Comment;
using MinimalApiBooks.Services.Genre;

var builder = WebApplication.CreateBuilder(args);

var AllowedOrigins = builder.Configuration.GetValue<string>("allowedorigins")!;
//Services Area

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer("name=DefaultConnection");
});

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(configuration =>
	{
		configuration
		.WithOrigins(AllowedOrigins)
		.AllowAnyMethod()
		.AllowAnyHeader();
	});

	options.AddPolicy("free", configuration =>
	{
		configuration
		.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod();
	});
});

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IFileStorage, AzureFileStorage>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));


//Services Area 


var app = builder.Build();


//Middleware Area

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseCors();

app.UseOutputCache();

app.MapGroup("/genres").MapGenres();
app.MapGroup("/authors").MapAuthors();
app.MapGroup("/books").MapBooks();
app.MapGroup("/book/{bookId:int}/comments").MapComments();

//Middleware Area



app.Run();
