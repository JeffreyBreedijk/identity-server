using System;
using IdentityServer4.Models;
using Xunit;

namespace Utilize.Identity.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var p  = new UtilizeClient
            {
                PermissionScheme = "bla"
            };
            
            var c = _mapper.Map<UtilizeClient, Client>(p);

        }
    }
}