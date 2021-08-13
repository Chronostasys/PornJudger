using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PornJudger.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public long ManagedId { get; set; }
        public List<string> Roles { get; set; }


        public string Name { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}
