using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDBContext _context;
    
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment comment)
    {
        var existingComment = await _context.Comments.FindAsync(id);

        if (existingComment == null)
            return null;

        existingComment.Title = comment.Title;
        existingComment.Content = comment.Content;

        await _context.SaveChangesAsync();

        return existingComment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var existingComment = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);
        
        if (existingComment == null)
            return null;
        
        _context.Comments.Remove(existingComment);
        await _context.SaveChangesAsync();
        
        return existingComment;
    }
    
}