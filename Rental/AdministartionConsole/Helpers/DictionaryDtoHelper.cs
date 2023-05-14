using AdministartionConsole.Models.Dto;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;

namespace AdministartionConsole.Helpers
{
    public interface IDictionaryDtoHelper
    {
        Task<List<DictionaryDto>> GetAll();
        void CreateDictionary([NotNull] DictionaryDto dictionary);
        Task<DictionaryDto> FindByName([NotNull] string name);
        Task<Dictionary> ChangeDictionary(DictionaryDto dictionary);
    }

    public class DictionaryDtoHelper : IDictionaryDtoHelper
    {
        private readonly ApplicationDbContext _context;

        public DictionaryDtoHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary> ChangeDictionary(DictionaryDto dictionary)
        {
            var model = await _context.Dictionaries.FirstOrDefaultAsync(x => x.Name == dictionary.Name);

            if (model == null)
                throw new CoreException(ErrorCode.DictionaryNotFound, $"Dictionary {dictionary.Name} was not found.");

            model.Value = dictionary.Value;

            await _context.SaveChangesAsync();

            return model;
        }

        public void CreateDictionary([NotNull] DictionaryDto dictionaryDto)
        {
            if (dictionaryDto == null)
                throw new CoreException(ErrorCode.InvlaidInputParameters, $"Invalid input. Value {dictionaryDto} is null.");

            var dictionary = new Dictionary
            {
                Name = dictionaryDto.Name,
                Value = dictionaryDto.Value,
            };

            _context.Add(dictionary);
            _context.SaveChanges();
        }

        public async Task<DictionaryDto> FindByName([NotNull] string name)
        {
            var dictionary = await _context.Dictionaries.FirstOrDefaultAsync(x => x.Name == name);

            if (dictionary == null)
                throw new CoreException(ErrorCode.DictionaryNotFound, $"Dictionary {name} was not found.");

            var dictionaryDto = new DictionaryDto
            {
                Name = dictionary.Name,
                Value = dictionary.Value
            };

            return dictionaryDto;
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
