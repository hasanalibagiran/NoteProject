using System.Linq.Expressions;
using DataAccess.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class ArchiveDal : GenericRepository<Archive>, IArchiveDal
    {
        public ArchiveDal(AppDbContext context) : base(context)
        {
        }
    }
}