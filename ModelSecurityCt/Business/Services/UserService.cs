using Business.Core;
using Data.Core;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Mapster;
using Microsoft.Extensions.Logging;
using static Dapper.SqlMapper;

namespace Business.Services
{
    public class UserService : ServiceBase<UserDTO, User>
    {
        public UserService(IUserRepository repository, ILogger<UserService> logger)
            : base(repository, logger) { }

      ///*  public async Task<IEnumerable<UserDTO>> Login()
      //  {
      //      try
      //      {
      //          var entities = await _repository.
      //          return entities.Adapt<IEnumerable<UserDTO>>();
      //      }
      //      catch (Exception ex)
      //      {
      //          _logger.LogError(ex, "Error al obtener todos los registros de {Entity}", typeof(User).Name);
      //          throw;
      //      }
      //  }*/

    }

      

}
