using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClass.Model;
namespace MyClass.DAO
{
    public class ProductsDAO
    {
        private MyDBContext db = new MyDBContext();

        //Select *from....
        public List<Products> getList()
        {
            return db.Products.ToList();
        }

        //select *from cho Index chỉ với status 1 và 2
        public List<Products> getList(string status = "ALL")//status = 0 ,1 , 2
        {
            List<Products> list = null;
            switch (status)
            {
                case "Index"://1,2
                    {
                        list = db.Products.Where(m => m.Status != 0).ToList();
                        break;
                    }

                case "Trash"://0
                    {
                        list = db.Products.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Products.ToList();
                        break;
                    }

            }

            return list;
        }

        public Products getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Products.Find(id);
            }
        }

        //tao moi
        public int Insert(Products row)
        {
            db.Products.Add(row);
            return db.SaveChanges();
        }


        //cap nhat
        public int Update(Products row)
        {

            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(Products row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();//thanh cong => return ve 1
        }
    }
}
