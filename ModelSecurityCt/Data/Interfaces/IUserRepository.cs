using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Entity.DTO.DTOLogin;
using Entity.Model;

namespace Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User?> validarCredenciales(string email, string password);
        
    }
}
