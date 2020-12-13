using ErkinStudy.Infrastructure.Context;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Infrastructure.Services
{
    public class SpecialtyService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SpecialtyService> _logger;

        public SpecialtyService(AppDbContext context, ILogger<SpecialtyService> logger)
        {
            _context = context;
            _logger = logger;
        }

        
    }
}
