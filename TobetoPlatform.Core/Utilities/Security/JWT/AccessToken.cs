﻿// --- Dosya: AccessToken.cs ---
namespace TobetoPlatform.Core.Utilities.Security.JWT
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}