using System;

namespace DTOs;

public class UserDto
{
 public int Id { get; set; }
 public string UserName{ get; set; }

    public static implicit operator int(UserDto? v)
    {
        throw new NotImplementedException();
    }
}
