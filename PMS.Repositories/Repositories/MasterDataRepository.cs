using PMS.DB.Model.Data;
using PMS.DB.Model.EF.Models;
using PMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Repositories.Repositories;

public class MasterDataRepository : Repository<PmsLookup>
{
    private AppDbContext _context;

    public MasterDataRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
