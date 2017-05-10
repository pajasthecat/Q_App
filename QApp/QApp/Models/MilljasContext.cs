using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QApp.Models
{
    public partial class MilljasContext : DbContext
    {
        public MilljasContext(DbContextOptions<MilljasContext> options) : base(options)
        {

        }

    }


}
