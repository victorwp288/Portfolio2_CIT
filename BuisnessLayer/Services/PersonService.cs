using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAcessLayer;
using DataAcessLayer.Movies;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class PersonService : IPersonService
    {
        private readonly ImdbContext _context;

        public PersonService(ImdbContext context)
        {
            _context = context;
        }

        public async Task<PersonDTO> GetPersonByIdAsync(string nconst)
        {
            var person = await _context.NameBasics
                .Include(p => p.PersonProfessions)
                .Include(p => p.PersonKnownTitles)
                    .ThenInclude(pkt => pkt.TitleBasic)
                .SingleOrDefaultAsync(p => p.Nconst == nconst);

            if (person == null)
                throw new KeyNotFoundException("Person not found.");

            var personDto = MapPersonToDTO(person);
            return personDto;
        }

        public async Task<IEnumerable<PersonDTO>> SearchPersonsAsync(string query)
        {
            var persons = await _context.NameBasics
                .Include(p => p.PersonProfessions)
                .Include(p => p.PersonKnownTitles)
                    .ThenInclude(pkt => pkt.TitleBasic)
                .Where(p => p.PrimaryName.Contains(query))
                .ToListAsync();

            var personDtos = persons.Select(MapPersonToDTO).ToList();
            return personDtos;
        }

        private PersonDTO MapPersonToDTO(NameBasic person)
        {
            return new PersonDTO
            {
                NConst = person.Nconst,
                PrimaryName = person.PrimaryName,
                BirthYear = person.BirthYear,
                DeathYear = person.DeathYear,
                Professions = person.PersonProfessions?.Select(pp => pp.Profession).ToList() ?? new List<string>(),
                KnownForTitles = person.PersonKnownTitles?.Select(pkt => new TitleDTO
                {
                    TConst = pkt.Tconst,
                    PrimaryTitle = pkt.TitleBasic?.PrimaryTitle
                }).ToList() ?? new List<TitleDTO>()
            };
        }
    }
}
