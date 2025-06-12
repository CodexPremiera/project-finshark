using api.Dtos.Comment;
using api.Models;

namespace api.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment comment)
    {
       return new CommentDto
       {
           Id = comment.Id,
           Title = comment.Title,
           Content = comment.Content,
           CreatedOn = comment.CreatedOn,
           CreatedBy = comment.AppUser.UserName ?? "Unknown",
           StockId = comment.StockId
       };
    }

    public static Comment ToComment(this CreateCommentDto commentDto, int stockId)
    {
        return new Comment
        {
            Title = commentDto.Title,
            Content = commentDto.Content,
            StockId = stockId
        };
    }
    
    public static Comment ToComment(this UpdateCommentDto commentDto, int stockId)
    {
        return new Comment
        {
            Title = commentDto.Title,
            Content = commentDto.Content,
            StockId = stockId
        };
    }
}