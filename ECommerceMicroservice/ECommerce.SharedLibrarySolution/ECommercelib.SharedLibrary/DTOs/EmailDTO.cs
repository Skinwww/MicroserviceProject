namespace ECommercelib.SharedLibrary.DTOs
{
    public class EmailDTO
    {
        public string Title { get; }
        public string Content { get; }

        public EmailDTO(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
