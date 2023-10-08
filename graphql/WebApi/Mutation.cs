namespace WebApi
{
    public class Mutation
    {
        public async Task<Book> AddBook(Book input)
        {

            var book = new Book
            {
                Title = input.Title,
                Author = input.Author,
            };

            return book;
        }
    }
}
