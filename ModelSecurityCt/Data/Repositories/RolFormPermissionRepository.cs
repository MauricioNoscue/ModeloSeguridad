using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Data.Interfaces;
using Entity.context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class RolFormPermissionRepository : GenericRepository<RolFormPermission>, IRolFormPermissionRepository
    {
        public RolFormPermissionRepository(ApplicationDbContext context, ILogger<RolFormPermissionRepository> logger)
        : base(context, logger) { }


        public async Task<IEnumerable<RolFormPermission>> GetAllAsync()
        {

            return await _context.RolFormPermission
           .Include(fm => fm.Rol)
           .Include(fm => fm.Form)
           .Include(fm => fm.Permission)
         
           .ToListAsync();
        }

        public async Task<RolFormPermission?> GetByIdAsync(int id)
        {
            return await _context.Set<RolFormPermission>()
           .Include(fm => fm.Rol)
           .Include(fm => fm.Form)
           .Include(fm => fm.Permission)
        .FirstOrDefaultAsync(fm => fm.Id == id);


        }
    }
}
