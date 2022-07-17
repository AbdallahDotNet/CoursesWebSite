using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IHashString
    {
        public string Encrypt(string plain_text);
        public string Decrypt(string Hash_text);
    }
}
