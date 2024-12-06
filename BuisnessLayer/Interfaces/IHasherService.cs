using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Interfaces
{
    public interface IHasherService
    {
        public Tuple<string, string> HashPassword(string password);

        public bool VerifyPassword(string password, string storedHash, string salt);

        public string ComputeIterativeHash(string password, string salt);

    }
}
