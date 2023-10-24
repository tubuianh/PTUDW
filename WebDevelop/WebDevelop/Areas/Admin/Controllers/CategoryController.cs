using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using UDW.Library;

namespace WebDevelop.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {

        CategoriesDAO categoriesDAO = new CategoriesDAO();
        //INDEX

        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }


        //DETAILS
        //GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại loại sản phẩm");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại loại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        //    // GET: Admin/Category/Create

        //    //CREAT 
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"),"Id","Name");
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xư lý tự động: Create at
                categories.CreateAt = DateTime.Now;
                //Xư lý tự động: Update at
                categories.UpdateAt = DateTime.Now;
                //Xư lý tự động: ParentId
                if (categories.ParentID == null)
                {
                    categories.ParentID = 0;
                }
                //Xư lý tự động: Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }

                //Xư lý tự động: Slug
                categories.Slug = XString.Str_Slug(categories.Name);


                //Chèn thêm dòng cho DB
                categoriesDAO.Insert(categories);
                //thông báo thêm danh mục sản phẩm thành công
                //Thông báo cập nhật thành công
                TempData["message"] = new XMessage("success", "Tạo mới loại sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            //return RedirectToAction("Index");
            return View(categories);
        }
        /// //////////////////////////////////////////////////////////////////////////
        //    // GET: Admin/Category/Edit/5

        //    //EDIT
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Thông báo cập nhật thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
                Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                    //Thông báo cập nhật thất bại
                    TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                    return RedirectToAction("Index");
                }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        // POST: Admin/Category/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong: slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //xy ly tu dong : parent id
                if(categories.ParentID == null)
                {
                    categories.ParentID = 0;
                }
                //xu ly tu dong : order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //xu ly tu dong: updataAt
                categories.UpdateAt = DateTime.Now;

                //cap nhat mau tin
                categoriesDAO.Update(categories);
                //Thông báo cập nhật thành công
                TempData["message"] = new XMessage("success", "Cập nhật mẫu tin thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        //    // GET: Admin/Category/Delete/5

        //    //DELETE
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại !");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại !");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            categoriesDAO.Delete(categories);

            //thông báo thành công
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công !");
            return RedirectToAction("Trash");
        }

        //    // GET: Admin/Category/Delete/5
        //STATUS
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //truy vấn dòng có id = id yêu cầu
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {
               
                //chuyển đổi trạng thái của status từ 1 ->2, nếu 2 -> 1
                categories.Status = (categories.Status == 1) ? 2 : 1;

                //cạp nhật gtri updateAt
                categories.UpdateAt = DateTime.Now;

                //cập nhật lại DB
                categoriesDAO.Update(categories);
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
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            else
            {

                //chuyển đổi trạng thái của status từ 1,2 -> 0, không hiển thị ở index
                categories.Status = 0;

                //cạp nhật gtri updateAt
                categories.UpdateAt = DateTime.Now;

                //cập nhật lại DB
                categoriesDAO.Update(categories);
                //Thông báo cập nhật thành công
                TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
                return RedirectToAction("Index");
            }
        }

        //Trash

        // GET: Admin/Category/trash
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
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
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại!");
                return RedirectToAction("Index");
            }
            else
            {

                //chuyển đổi trạng thái của status từ 0->2: ko xuat ban
                categories.Status =2;

                //cạp nhật gtri updateAt
                categories.UpdateAt = DateTime.Now;

                //cập nhật lại DB
                categoriesDAO.Update(categories);
                //Thông báo phục hồi thành công
                TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công!");
                return RedirectToAction("Index");
            }
        }


    }
}
