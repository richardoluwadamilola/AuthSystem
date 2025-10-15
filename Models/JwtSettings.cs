﻿namespace AuthSystem.Models
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
    }
}
