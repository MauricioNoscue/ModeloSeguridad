using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Data.Interfaces;
using Entity.context;
using Entity.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class FormModuleRepository : GenericRepository<FormModule>, IFormModuleRepository
    {
        public FormModuleRepository(ApplicationDbContext context, ILogger<FormModuleRepository> logger)
        : base(context, logger) { }

        public async Task<IEnumerable<FormModule>> GetAllAsync()
        {
            return await _context.FormModule
                .Include(fm => fm.Form)
                .Include(fm => fm.Module)
                .ToListAsync();
        }

        public async Task<FormModule?> GetByIdAsync(int id)
        {
            return await _context.Set<FormModule>()
        .Include(fm => fm.Form)
        .Include(fm => fm.Module)
        .FirstOrDefaultAsync(fm => fm.Id == id);


        }
    }
}
