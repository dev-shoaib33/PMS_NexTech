using Microsoft.EntityFrameworkCore;
using PMS.DB.Model.Data;
using PMS.DB.Model.EF.Models;
using PMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PMS.Repositories.Repositories;

public class ProductRepository : Repository<PmsProduct>, IProductRepository
{
    private AppDbContext _context;

    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public List<PmsProduct> GetProducts(int pageSize, int pageNumber, string searchText, out int totalCount)
    {
        IQueryable<PmsProduct> query = _context.PmsProducts
            .Where(x => x.ActiveFlag);

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(x => x.ProductName.Contains(searchText));
        }

        totalCount = query.Count();

        if (pageSize > 0)
        {
            query = query.OrderByDescending(x => x.LastUpdatedDate).Skip((pageNumber) * pageSize)
                         .Take(pageSize);
        }

        List<PmsProduct> products = query.Select(x => new PmsProduct()
        {
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            Price = x.Price,
            CategoryLkpId = x.CategoryLkpId,
            CategoryLkp = x.CategoryLkp,
            CreatedBy = x.CreatedBy,
            Uom = x.Uom,
            Description = x.Description,
            CreatedDate = x.CreatedDate
        }
            ).ToList();

        return products;
    }

    //public List<PmsProduct> GetAllPoducts(int pageSize, int pageNumber, string searchText)
    //{
    //    int recordsToSkip = 0; 
    //    if (pageSize > 0)
    //    {
    //        recordsToSkip = (pageNumber) * pageSize;
    //    }
    //    List<PmsProduct> data = new List<PmsProduct>();

    //    if (!string.IsNullOrEmpty(searchText))
    //    {
    //        data = _context.PmsProducts.Where(x => searchText.Contains(x.ProductName))
    //           .Skip(recordsToSkip)
    //           .Take(pageSize)
    //           .ToList(); 
    //    }
    //    else
    //    {
    //        data = _context.PmsProducts.Skip(recordsToSkip)
    //        .Take(pageSize)
    //        .ToList();
    //    }

    //    return data;
    //}
    //public void Update(PmsProduct product)
    //{
    //    _context.Update(product);
    //    //var produtcDB = _context.PmsProducts.FirstOrDefault(x => x.ProductId == product.ProductId);
    //    //if (produtcDB != null)
    //    //{
    //    //    produtcDB.ProductName = product.ProductName;
    //    //    produtcDB.Description = product.Description;
    //    //    produtcDB.Price = product.Price;
    //    //    produtcDB.CategoryLkpId = product.CategoryLkpId;
    //    //    if (produtcDB != null)
    //    //    {
    //    //        produtcDB.ImageName = product.ImageName;
    //    //    }
    //    //}
    //}
}
