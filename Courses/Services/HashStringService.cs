using Interfaces.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class HashStringService : IHashString
    {
        private IDataProtectionProvider _provider;
        public HashStringService(IDataProtectionProvider provider)
        {
            _provider = provider;
        }

        public string Decrypt(string Hash_text)
        {
            var Purpose = "123456789@dgasdasdskfsdjf#$100";
            var protector = _provider.CreateProtector(Purpose);
            return protector.Unprotect(Hash_text);
        }

        public string Encrypt(string plain_text)
        {
            var Purpose = "123456789@dgasdasdskfsdjf#$100";
            var protector = _provider.CreateProtector(Purpose);
            return protector.Protect(plain_text);
        }
    }
}
