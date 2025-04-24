using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Repositories;
using Entity.DTO.DTOLogin;
using Entity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business
{
    public  class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository _userRepository, IConfiguration _config)
        {
            this._userRepository = _userRepository;
            this._config = _config;
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await _userRepository.validarCredenciales(email, password);
            if (user == null) return null;
            return user;
           
        }

    }
}
