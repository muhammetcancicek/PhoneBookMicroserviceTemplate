using MongoDB.Driver;
using ReportService.Domain.Entities;
using ReportService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IMongoCollection<Report> _reportCollection;

        public ReportRepository(IMongoDatabase database)
        {
            _reportCollection = database.GetCollection<Report>("Reports");
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _reportCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Report> GetByIdAsync(Guid id)
        {
            return await _reportCollection.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Report report)
        {
            await _reportCollection.InsertOneAsync(report);
        }

        public async Task UpdateAsync(Report report)
        {
            await _reportCollection.ReplaceOneAsync(r => r.Id == report.Id, report);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _reportCollection.DeleteOneAsync(r => r.Id == id);
        }

    }
}
