﻿namespace AuthSystem.Models.DTOS
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public UserDTO? User { get; set; }
    }
}
