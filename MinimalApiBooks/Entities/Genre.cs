﻿using System.ComponentModel.DataAnnotations;

namespace MinimalApiBooks.Entities
{
	public class Genre
	{
        public int Id { get; set; }
        public string Name { get; set; } = null!;
		public List<GenreBook> GenresBooks { get; set; } = new List<GenreBook>();
	}
}
