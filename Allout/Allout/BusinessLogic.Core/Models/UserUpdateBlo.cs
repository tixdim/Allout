﻿namespace BusinessLogic.Core.Models
{
	public class UserUpdateBlo
	{
        public string? Email { get; set; }
        public string? Nickname { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? AvatarUrl { get; set; }
    }
}

