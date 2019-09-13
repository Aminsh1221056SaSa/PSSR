using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security
{
    public class PersonPolicyRequirment : IAuthorizationRequirement
    {
        public string[] UserType { get; private set; }
        public bool CheckUserType { get; private set; }

        public PersonPolicyRequirment(bool chUtype,string[] userType)
        {
            this.UserType = userType;
            this.CheckUserType = chUtype;
        }
    }
}
