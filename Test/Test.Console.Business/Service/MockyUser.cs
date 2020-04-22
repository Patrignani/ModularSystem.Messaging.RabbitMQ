using System;
using System.Collections.Generic;
using System.Text;
using Test.Signature.DTOs;

namespace Test.Console.Business.Service
{
    public class MockyUser
    {
        public ICollection<User> GetAll()
        {
            var users = new List<User>
           {
             new User{
                 Active = true,
                 Address ="Av. ",
                 City="Jundiaí",
                 Country="Brasil",
                 State="São Paulo",
                 Id= 1,
                 Identification= "aaaa"
                 },
             new User{
                 Active = true,
                 Address ="Av. aaa",
                 City="Jundiaí aaa",
                 Country="Brasil aaa",
                 State="São Paulo aaa",
                 Id= 2,
                 Identification= "aaaa"
                 },
              new User{
                 Active = true,
                 Address ="Av. bb",
                 City="Jundiaí bb",
                 Country="Brasil bb",
                 State="São Paulo bb",
                 Id= 3,
                 Identification= "bbb"
                 }
            };

            return users;
        }
    }
}
