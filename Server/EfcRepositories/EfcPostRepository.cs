using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext ctx;
    public EfcPostRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }
    public async Task<Post> AddAsync(Post post)
    {
        await ctx.AddAsync(post);
        await ctx.SaveChangesAsync();
        return post;
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(p=>p.Id==id);
        if(existing==null)
        {
            throw new NotFoundException($"Post with id {id} not found");
        }
        ctx.Posts.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public IQueryable<Post> GetMany()
    {
        return ctx.Posts.AsQueryable();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(p=>p.Id==id);
        if(existing==null)
        {
            throw new NotFoundException($"Post with id {id} not found");
        }
        return existing;
    }

    public async Task UpdateAsync(Post post)
    {
    if(!(await ctx.Posts.AnyAsync(p=>p.Id == post.Id)))
        {
            throw new NotFoundException("Post with id {post.Id} not found");
        }
        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
    }
}