//using PMS.DB.Model.Data;
//using PMS.Repositories.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PMS.Repositories.Repositories
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        private AppDbContext _context;

//        //public ICategoryRepository Category { get; private set; }
//        public IProductRepository Product { get; private set; }


//        public UnitOfWork(AppDbContext context)
//        {
//            _context = context;
//            //Category = new CategoryRepository(context);
//            Product = new ProductRepository(context);

//        }

//        public void Save()
//        {
//            _context.SaveChanges();
//        }

//        public void SaveChanges()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
