namespace BusinessLayer.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessLayer.DTOs;

    public interface IPersonService
    {
        Task<PersonDTO> GetPersonByIdAsync(string nconst);
        Task<IEnumerable<PersonDTO>> SearchPersonsAsync(string query);
    }
}
