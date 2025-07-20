using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Controllers;
using LibraryAPI.Models;
using LibraryAPI.Data;



namespace LibraryApi.Tests.Controllers
{
    public class BooksControllerTests
    {
        private LibraryContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: "TestLibraryDB")
                .Options;

            var context = new LibraryContext(options);
            context.Books.AddRange(new List<Book>
            {
                new Book { BookId = 1, Title = "Book A", Author = "Author A",  PublishedDate = DateTime.Now, CategoryId = 1 },
                new Book { BookId = 2, Title = "Book B", Author = "Author B", PublishedDate = DateTime.Now, CategoryId = 1 }
            });
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetBooks_ReturnsListOfBooks()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new BooksController(context);

            // Act
            var result = await controller.GetBooks();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var books = Assert.IsType<List<Book>>(okResult.Value);

            // Assert
            Assert.Equal(2, books.Count);
        }
    }
}
