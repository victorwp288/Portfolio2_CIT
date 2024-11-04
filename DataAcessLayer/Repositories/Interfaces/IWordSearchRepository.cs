using DataAcessLayer.Entities.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Repositories.Interfaces
{
    public interface IWordSearchRepository
    {
        Task<IEnumerable<WordAndFrequency>> WordToWordsQueryAsync(int resultLimit, params string[] keywords);
    }
}
