using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Utilities;
using Data.Core;
using Mapster;

namespace Business.Core
{
    /// <summary>
    /// Clase base abstracta para servicios de negocio que encapsula operaciones CRUD genéricas
    /// sobre cualquier entidad, utilizando un patrón de repositorio y mapeo automático con Mapster.
    /// </summary>
    /// <typeparam name="TDto">Tipo de objeto de transferencia de datos (DTO).</typeparam>
    /// <typeparam name="TEntity">Tipo de entidad del dominio correspondiente en la base de datos.</typeparam>
    public abstract class ServiceBase<TDto, TEntity> : IServiceBase<TDto, TEntity>
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly ILogger _logger;

        protected ServiceBase(IRepository<TEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return entities.Adapt<IEnumerable<TDto>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de {Entity}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException(nameof(id), "El identificador debe ser mayor que cero.");

            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    throw new EntityNotFoundException(typeof(TEntity).Name, id);

                return entity.Adapt<TDto>();
            }
            catch (BusinessException) { throw; } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro con ID {Id} de {Entity}", id, typeof(TEntity).Name);
                throw new BusinessException("Ocurrió un error inesperado al consultar los datos.", ex);
            }
        }




        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            if (dto == null)
                throw new ValidationException(nameof(dto), "Los datos enviados son nulos o inválidos.");

            try
            {
                var entity = dto.Adapt<TEntity>();
                var created = await _repository.AddAsync(entity);
                return created.Adapt<TDto>();
            }
            catch (BusinessException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear entidad {Entity}", typeof(TEntity).Name);
                throw new BusinessException("Error al intentar crear el registro.", ex);
            }
        }


        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            if (dto == null)
                throw new ValidationException(nameof(dto), "Los datos enviados para actualización son inválidos.");

            try
            {
                var entity = dto.Adapt<TEntity>();
                var updated = await _repository.UpdateAsync(entity);
                if (!updated)
                    throw new BusinessException("No se pudo actualizar el registro.");

                return dto;
            }
            catch (BusinessException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar entidad {Entity}", typeof(TEntity).Name);
                throw new BusinessException("Error al intentar actualizar el registro.", ex);
            }
        }

        public virtual async Task<bool> DeletePermanentAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException(nameof(id), "El identificador debe ser mayor que cero.");

            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (BusinessException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente el registro con ID {Id} de {Entity}", id, typeof(TEntity).Name);
                throw new BusinessException("Error al eliminar el registro de forma permanente.", ex);
            }
        }

        public virtual async Task<bool> DeleteLogicalAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException(nameof(id), "El identificador debe ser mayor que cero.");

            try
            {
                return await _repository.DeleteLogicalAsync(id);
            }
            catch (BusinessException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente el registro con ID {Id} de {Entity}", id, typeof(TEntity).Name);
                throw new BusinessException("Error al eliminar el registro de forma lógica.", ex);
            }
        }



        public async Task<bool> UpdateIsDeleted(int id)
        {
            if (id <= 0)
                throw new ValidationException(nameof(id), "El identificador debe ser mayor que cero.");

            try
            {
                return await _repository.UpdateIsDeleted(id);
            }
            catch (BusinessException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar lógicamente el registro con ID {Id} de {Entity}", id, typeof(TEntity).Name);
                throw new BusinessException("Error al actualizar el registro de forma lógica.", ex);
            }
        }

    }
}
