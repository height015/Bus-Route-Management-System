using System;
using BRMSAPI.Domain;
using Domain;

namespace Service.Contacts;

public interface ITokenService
{
    string GenerateToken(Passengers appUserObj, string[] roles);
    void InvalidateJwtToken(string token); 
}

