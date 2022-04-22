using System;

namespace _WebAppHang.Services
{
    public class GuidService : IGuidService
    {
        public string GetRandomIdentifier()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
