using Business.Core;
using Data.Interfaces;
using Data.Repositories;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities;

namespace Business.Services
{
    public class RolUserService : ServiceBase<RolUserDTO, RolUser>
    {
        private RolUserRepository _rolUserrepository;
        public RolUserService(IRolUserRepository repository, ILogger<RolUserService> logger, RolUserRepository rolUserrepository)
            : base(repository, logger) 
        {
            _rolUserrepository = rolUserrepository;
        }


        public async Task<IEnumerable<RolUserDTO>> GetAllWithUserEmailAsync()
        {
            try
            {
                var rolUsers = await _rolUserrepository.GetAllAsync();

                var dtos = rolUsers.Select(fm => new RolUserDTO
                {
                    Id = fm.Id,
                    RolId = fm.RolId,
                    RolName = fm.Rol?.Name,
                    Email = fm.User?.Email,
                    UserId = fm.UserId,
                    IsDeleted = fm.IsDeleted
                });

                return dtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RolUsers con Email");
                throw;
            }
        }


        public async Task<List<string>> GetRolesByUserId(int id)
        {
            if (id == null)
                throw new ValidationException("UserRol", $"{id} no puede ser nulo");

            try
            {

                if (id == null || id <= 0)
                    throw new ValidationException("Id", "El ID debe ser mayor que cero");

                var roles = await _rolUserrepository.GetRolesByUserIdAsync(id);

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la lista de roles para el usuario con id: {id}");
                throw;
            }
        }
    }
}
