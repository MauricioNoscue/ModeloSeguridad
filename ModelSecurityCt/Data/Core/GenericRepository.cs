﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Entity.context;
using Entity.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Data.Core
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public GenericRepository(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            //_httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Obtiene todos los registros de la entidad T desde la base de datos.
        /// </summary>
        /// <returns>Una lista de todos los registros encontrados.</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                
                .ToListAsync();
        }

        /// <summary>
        /// Busca un registro por su identificador único.
        /// </summary>
        /// <param name="id">El identificador del registro a buscar.</param>
        /// <returns>El registro encontrado o null si no existe.</returns>
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// Agrega una nueva entidad a la base de datos.
        /// </summary>
        /// <param name="entity">La entidad que se desea agregar.</param>
        /// <returns>La entidad agregada con sus valores actualizados, si aplica.</returns>
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Actualiza los datos de una entidad existente.
        /// </summary>
        /// <param name="entity">La entidad con los datos actualizados.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return true; 
        }

        // <sumse si nomary>
        /// Elimina físicamente un registro de la base de datos según su identificador.
        /// </summary>
        /// <param name="id">El identificador de la entidad a eliminar.</param>
        /// <returns>True si se eliminó correctamente; Fal se encontró.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return false;
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Realiza una eliminación lógica de la entidad, marcando la propiedad IsDeleted como true.
        /// </summary>
        /// <param name="id">El identificador de la entidad a eliminar lógicamente.</param>
        /// <returns>True si se actualizó correctamente; False si no se encontró o no tiene la propiedad IsDeleted.</returns>
        public async Task<bool> DeleteLogicalAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return false;

            var prop = entity.GetType().GetProperty("IsDeleted");
            if (prop != null)
            {
                prop.SetValue(entity, true);
                await _context.SaveChangesAsync();
                return true;
            }   
            _logger.LogWarning($"La entidad {typeof(T).Name} no tiene propiedad IsDeleted");
            return false;
        }


        public async Task<bool> UpdateIsDeleted(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return false;

            var prop = entity.GetType().GetProperty("IsDeleted");
            if (prop != null && prop.PropertyType == typeof(bool))
            {
                var currentValue = (bool)prop.GetValue(entity)!;
                prop.SetValue(entity, !currentValue);
                await _context.SaveChangesAsync();
                return true;
            }

            _logger.LogWarning($"La entidad {typeof(T).Name} no tiene propiedad IsDeleted o no es de tipo bool.");
            return false;
        }

        //public async Task<IEnumerable<T>> GetAllAsyncJWT()
        //{
        //    var role = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

        //    var query = _context.Set<T>().AsQueryable();

        //    if (role != "Admin")
        //    {
        //        // Filtrado por reflexión si la propiedad IsDeleted existe
        //        var prop = typeof(T).GetProperty("IsDeleted");
        //        if (prop != null && prop.PropertyType == typeof(bool))
        //        {
        //            query = query.Where(e =>
        //                (bool)prop.GetValue(e)! == false
        //            ).AsQueryable();
        //        }
        //    }

        //    return query.ToList(); // o await query.ToListAsync() si haces que todo sea async
        //}

    }
}
