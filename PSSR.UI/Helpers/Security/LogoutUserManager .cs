using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security
{
    public class LogoutUserManager
    {
        List<User> _user = new List<User>();
        public void Add(string sub,string sid)
        {
            _user.Add(new User { Sub = sub, Sid = sid });
        }
        public bool IsLoggedOut(string sub,string sid)
        {
            var matches = _user.Any(s => s.IsMatch(sub, sid));
            if (matches)
            {
                var u = _user.First(s => s.Sid == sid && s.Sub == sub);

                _user.Remove(u);
            }
            return matches;
        }

        private class User
        {
            public string Sub { get; set; }
            public string Sid { get; set; }

            public bool IsMatch(string sub,string sid)
            {
                return (this.Sid == sid && this.Sub == sub)
                    || (this.Sid == sid && this.Sub == null)
                    || (this.Sid == null && this.Sub == sub);
            }
        }
    }
}
