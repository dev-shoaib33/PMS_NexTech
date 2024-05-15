using PMS.DB.Model.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Repositories.Interfaces;

public interface IProductRepository : IRepository<PmsProduct>
{
    List<PmsProduct> GetProducts(int pageSize, int pageNumber, string searchText, out int totalCount);
    //void Update(PmsProduct product);

}
