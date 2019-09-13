using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.Security
{
    public class PersonPolicyRequirment : IAuthorizationRequirement
    {
        public bool CheckUserType { get; private set; }

        public PersonPolicyRequirment(bool chUtype)
        {
            this.CheckUserType = chUtype;
        }
    }
}
