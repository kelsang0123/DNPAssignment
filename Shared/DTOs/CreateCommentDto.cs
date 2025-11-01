using System;

namespace DTOs;

public class CreateCommentDto
{
public required string Body { get; set; }
public required int UserId { get; set; }
public required int postId{ get; set; }
}
