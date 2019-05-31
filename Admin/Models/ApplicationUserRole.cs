using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Admin.Models
{
    public class ApplicationUserRole :IdentityRole<Guid>
    {
    }
}
