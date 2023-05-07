using AdministartionConsole.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Rental.Infrastructure.EF;

namespace AdministartionConsole.Helpers
{
    public interface IDictionaryDtoHelper
    {
        Task<List<DictionaryDto>> GetAll();
    }

    public class DictionaryDtoHelper : IDictionaryDtoHelper
    {
        private readonly ApplicationDbContext _context;

        public DictionaryDtoHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DictionaryDto>> GetAll()
        {
            var dictionary = await _context.Dictionaries.ToListAsync();

            var dictionaryDto = new List<DictionaryDto>();

            foreach (var item in dictionary)
            {
                dictionaryDto.Add(new DictionaryDto
                {
                    Name = item.Name,
                    Value = item.Value
                });
            }
            return dictionaryDto;
        }
    }
}
