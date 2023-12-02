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
    public class SupplierController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();


        /// /////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("Index"));
        }



        /// /////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        /// /////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.ListOrder = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong: slug, create, createAt/By,  updaeAt/By, order
                //Xư lý tự động: Create at
                suppliers.CreateAt = DateTime.Now;
                //Xư lý tự động: Update at
                suppliers.UpdateAt = DateTime.Now;

                //Xư lý tự động: Create by
                suppliers.CreateBy = Convert.ToInt32(Session["UserID"]);
                //Xư lý tự động: Update by
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //Xư lý tự động: Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }

                //Xư lý tự động: Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Img = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier";                       
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                //tao du lieu thanh cong
                suppliersDAO.Insert(suppliers);
                TempData["message"] = new XMessage("success", "Tạo mới nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        /// /////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong: slug, create, createAt/By,  updaeAt/By, order
               
                //Xư lý tự động: Update at
                suppliers.UpdateAt = DateTime.Now;

                //Xư lý tự động: Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }

                suppliers.Slug = XString.Str_Slug(suppliers.Name);

                var img = Request.Files["img"];//lay thong tin file
                string PathDir = "~/Public/img/supplier";

                if (img.ContentLength != 0 && suppliers.Img != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Img);
                    System.IO.File.Delete(DelPath);
                }

                if (img.ContentLength != 0)
                {
                    
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Img = imgName;
                        //upload hinh
                        
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }

                }
               
                    //Xư lý tự động: Slug
                   
                //cap nhat mau tin vao db
                suppliersDAO.Update(suppliers);
                TempData["message"] = new XMessage("success", "Cập nhật nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }


        /// /////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = suppliersDAO.getRow(id);
            var img = Request.Files["img"];//lay thong tin file
            string PathDir = "~/Public/img/supplier";
            //xoa mau tin ra khoi db
            if (suppliersDAO.Delete(suppliers)==1)
            {
                if(suppliers.Img != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Img);
                    System.IO.File.Delete(DelPath);
                }
            }    
            TempData["message"] = new XMessage("success", "Xóa nhà cung cấp thành công");
            return RedirectToAction("Trash");
        }

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
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {

                //chuyển đổi trạng thái của status từ 1 ->2, nếu 2 -> 1
                suppliers.Status = (suppliers.Status == 1) ? 2 : 1;

                //cạp nhật gtri updateAt
                suppliers.UpdateAt = DateTime.Now;

                //cập nhật lại DB
                suppliersDAO.Update(suppliers);
                //Thông báo cập nhật thành công
                TempData["message"] = new XMessage("success", "Cập nhật  trạng thái thành công");
                return RedirectToAction("Index");
            }
        }

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
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            else
            {

                //chuyển đổi trạng thái của status từ 1,2 -> 0, không hiển thị ở index
                suppliers.Status = 0;

                //cạp nhật gtri updateAt
                suppliers.UpdateAt = DateTime.Now;

                //cập nhật lại DB
                suppliersDAO.Update(suppliers);
                //Thông báo cập nhật thành công
                TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
                return RedirectToAction("Index");
            }
        }

        //Trash

        // GET: Admin/supplier/trash
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));
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
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại!");
                return RedirectToAction("Index");
            }
            else
            {

                //chuyển đổi trạng thái của status từ 0->2: ko xuat ban
                suppliers.Status = 2;

                //cạp nhật gtri updateAt
                suppliers.UpdateAt = DateTime.Now;

                //cập nhật lại DB
                suppliersDAO.Update(suppliers);
                //Thông báo phục hồi thành công
                TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công!");
                return RedirectToAction("Index");
            }
        }


    }
}
