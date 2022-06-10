using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Book> objList = _db.Books.ToList();
            return View(objList);
        }

        //public IActionResult Upsert(int? id)
        //{
        //   Author obj = new Author();
        //    if(id == null)
        //    {
        //        return View(obj);
        //    }
        //    //this is for edit (logicno :D)
        //    obj = _db.Authors.FirstOrDefault(u => u.Author_Id == id);
        //    if(obj == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(obj);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(Author obj)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        if(obj.Author_Id == 0)
        //        {
        //            _db.Authors.Add(obj);
        //        }
        //        else
        //        {
        //            _db.Authors.Update(obj);
        //        }
        //        _db.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(obj);
        //}

        //public IActionResult Delete(int id)
        //{
        //    var objFromDb = _db.Authors.FirstOrDefault(x => x.Author_Id == id);
        //    _db.Authors.Remove(objFromDb);

        //    _db.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //    return View();
        //}

        //public IActionResult CreateMultiple2()
        //{
        //    List<Author> catList = new List<Author>();

        //    for(int i = 1; i <= 2; i++)
        //    {
        //        catList.Add(new Author { FirstName = Guid.NewGuid().ToString(), LastName = Guid.NewGuid().ToString() });
        //        //_db.Categories.Add(new Category { Name = Guid.NewGuid().ToString() });
        //    }
        //    _db.Authors.AddRange(catList);
        //    _db.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //    return View();
        //}

        //public IActionResult CreateMultiple5()
        //{
        //    List<Author> catList = new List<Author>();

        //    for (int i = 1; i <= 5; i++)
        //    {
        //        catList.Add(new Author { FirstName = Guid.NewGuid().ToString(), LastName = Guid.NewGuid().ToString()});
        //        //_db.Categories.Add(new Category { Name = Guid.NewGuid().ToString() });
        //    }
        //    _db.Authors.AddRange(catList);
        //    _db.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //    return View();
        //}

        //public IActionResult RemoveMultiple2()
        //{
        //    IEnumerable<Author> catList = _db.Authors.OrderByDescending(x => x.Author_Id).Take(2).ToList();

        //    _db.Authors.RemoveRange(catList);
        //    _db.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //    return View();
        //}

        //public IActionResult RemoveMultiple5()
        //{
        //    IEnumerable<Author> catList = _db.Authors.OrderByDescending(x => x.Author_Id).Take(5).ToList();

        //    _db.Authors.RemoveRange(catList);
        //    _db.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //    return View();
        //}
    }
}
