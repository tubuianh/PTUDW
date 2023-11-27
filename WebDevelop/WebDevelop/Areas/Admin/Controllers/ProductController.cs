using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using UDW.Library;
using System.IO;

namespace WebDevelop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductsDAO productsDAO = new ProductsDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();
        /// /////////////////////////////////////////
        /// <returns></returns>
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View(productsDAO.getList("Index"));
        }

        /// /////////////////////////////////////////
        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(products);
        }


        /// /////////////////////////////////////////
        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name"); //CatID - truy van tu bang Categories
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "Id", "Name"); // SupID - truy van tu bang Suppliers
            //dung de lua chon tu danh sach droplist nhu bang Categories : ParentID va Suppliers : ParentID
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products)
        {
            if (ModelState.IsValid)
            {
                //xu ly thong tin tu dong cho mot so truong
                //Xư lý tự động: Create at
                products.CreateAt = DateTime.Now;
                //Xư lý tự động: Update at
                products.UpdateAt = DateTime.Now;

                //Xư lý tự động: Create by
                products.CreateBy = Convert.ToInt32(Session["UserID"]);
                //Xư lý tự động: Update by
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);

                products.Slug = XString.Str_Slug(products.Name);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Img = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/product";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //luu vao DB
                productsDAO.Insert(products);
                TempData["message"] = new XMessage("success", "Tạo mới sản phẩm thành công");
                return RedirectToAction("Index");
            }

            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name"); //CatID - truy van tu bang Categories
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "Id", "Name"); // SupID - truy van tu bang Suppliers
            return View(products);
        }


        /// /////////////////////////////////////////
        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name"); //CatID - truy van tu bang Categories
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "Id", "Name"); // SupID - truy van tu bang Suppliers
            return View(products);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong: slug, create, createAt/By,  updaeAt/By, order

                //Xư lý tự động: Update at
                products.UpdateAt = DateTime.Now;

                products.Slug = XString.Str_Slug(products.Name);

                var img = Request.Files["img"];//lay thong tin file
                string PathDir = "~/Public/img/product";

                if (img.ContentLength != 0 && products.Img != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), products.Img);
                    System.IO.File.Delete(DelPath);
                }

                if (img.ContentLength != 0)
                {

                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Img = imgName;
                        //upload hinh

                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }

                }

                //Xư lý tự động: Slug

                //cap nhat mau tin vao db
                productsDAO.Update(products);
                TempData["message"] = new XMessage("success", "Cập nhật sản phẩm thành công");
                return RedirectToAction("Index");
            }
            return View(products);
        }


        /// /////////////////////////////////////////
        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = productsDAO.getRow(id);
            productsDAO.Delete(products);
            //thong bao xoa san pham thanh cong
            TempData["message"] = new XMessage("success", "Xóa sản phẩm thành công");
            return RedirectToAction("Trash");
        }

        /// /////////////////////////////////////////
        //phat sinh them 1 so action: status, trash, deltrash, undo
        //status
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //truy vấn dòng có id = id yêu cầu
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {

                //chuyển đổi trạng thái của status từ 1 ->2, nếu 2 -> 1
                products.Status = (products.Status == 1) ? 2 : 1;

                //cạp nhật gtri updateAt
                products.UpdateAt = DateTime.Now;

                //cập nhật lại DB
                productsDAO.Update(products);
                //Thông báo cập nhật thành công
                TempData["message"] = new XMessage("success", "Cập nhật  trạng thái thành công");
                return RedirectToAction("Index");
            }
        }


        /// /////////////////////////////////////////
        //DelTrash
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            //truy vấn dòng có id = id yêu cầu
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            else
            {

                //chuyển đổi trạng thái của status từ 1,2 -> 0, không hiển thị ở index
                products.Status = 0;

                //cạp nhật gtri updateAt
                products.UpdateAt = DateTime.Now;

                //cập nhật lại DB
                productsDAO.Update(products);
                //Thông báo cập nhật thành công
                TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
                return RedirectToAction("Index");
            }
        }


        /// /////////////////////////////////////////
        //Trash

        // GET: Admin/product/trash
        public ActionResult Trash()
        {
            return View(productsDAO.getList("Trash"));
        }

        //Recover
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại!");
                return RedirectToAction("Index");
            }
            //truy vấn dòng có id = id yêu cầu
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại!");
                return RedirectToAction("Index");
            }
            else
            {

                //chuyển đổi trạng thái của status từ 0->2: ko xuat ban
                products.Status = 2;

                //cạp nhật gtri updateAt
                products.UpdateAt = DateTime.Now;

                //cập nhật lại DB
                productsDAO.Update(products);
                //Thông báo phục hồi thành công
                TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công!");
                return RedirectToAction("Trash");
            }
        }

    }
}
