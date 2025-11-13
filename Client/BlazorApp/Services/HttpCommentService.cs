namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient httpClient;

    public HttpCommentService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

}
