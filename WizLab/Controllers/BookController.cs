using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;
using WizLib_Model.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            //List<Book> objList = _db.Books.ToList();
            //foreach (var obj in objList)
            //{
            //    obj.Publisher = _db.Publishers.FirstOrDefault(x => x.Publicher_Id == obj.Publisher_Id); // veliki broj poziva prema bazi podataka
            //    _db.Entry(obj).Reference(x => x.Publisher).Load(); // ('Explicit Loading' princip) koristimo Reference jer za svaku knjigu trenutno imamo jednog Objavljivaca (Publisher) (1 : *), da smo ih imali vise (* : *) koristili bi Collection
            //    _db.Entry(obj).Collection(x => x.BookAuthors).Load();

            //    foreach(var bookAuth in obj.BookAuthors)
            //    {
            //        _db.Entry(bookAuth).Reference(x => x.Author).Load();
            //    }
                //Explicit loading je odlicna stvar, imamo manji broj poziva prema bazi, sto direktno utice na performanse aplikacije, ali idalje je sporo :D
            //}


            // Eager Loading (Include) - je proces gdje kveri za jedan tip entiteta takodje ucita realted entitet (sve u sklopu jednog kverija) :
            List<Book> objList = _db.Books.Include(x => x.Publisher).Include(x => x.BookAuthors).ThenInclude(x => x.Author).ToList(); // jedan poziv prema bazi!
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            BookVM obj = new BookVM();
            obj.PublisherList = _db.Publishers.Select(i => new SelectListItem // projekcija, dakle uzmemo odredjene aitrubte iz klase Publisher i slikamo u objakatPublisher list koji je tipa SlectListItem
            {
                Text = i.Name,
                Value = i.Publicher_Id.ToString()
            });

            if (id == null)
            {
                return View(obj);
            }
            //this is for edit (logicno :D)
            obj.Book = _db.Books.FirstOrDefault(u => u.Book_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM obj)
        {
                if (obj.Book.Book_Id == 0)
                {
                    _db.Books.Add(obj.Book);
                }
                else
                {
                    _db.Books.Update(obj.Book);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));

            return View(obj);
        }

        public IActionResult Delete(int id)
        {
            var objFromDb = _db.Books.FirstOrDefault(x => x.Book_Id == id);
            _db.Books.Remove(objFromDb);

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
            return View();
        }

        public IActionResult Details(int? id)
        {
            BookVM obj = new BookVM();

            if (id == null)
            {
                return View(obj);
            }
            //this is for edit (logicno :D)
            //obj.Book = _db.Books.FirstOrDefault(u => u.Book_Id == id);
            //obj.Book.BookDetail = _db.BookDetails.FirstOrDefault(x => x.BookDetail_Id == obj.Book.BookDetail_Id);
            // Eager Loading (Include) - je proces gdje kveri za jedan tip entiteta takodje ucita realted entitet (sve u sklopu jednog kverija) :
            obj.Book = _db.Books.Include(x => x.BookDetail).FirstOrDefault(x => x.Book_Id == id); // LEFT JOIN tabele Book i tabele BookDetail preko FK(PK) BookDetail_Id (klasika)

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(BookVM obj)
        {
            if (obj.Book.BookDetail.BookDetail_Id == 0)
            {
                _db.BookDetails.Add(obj.Book.BookDetail);
                _db.SaveChanges(); // sacuvamo BookDetail

                var BookFromDb = _db.Books.FirstOrDefault(x => x.Book_Id == obj.Book.Book_Id);
                BookFromDb.BookDetail_Id = obj.Book.BookDetail.BookDetail_Id;
                _db.SaveChanges(); // sacuvamo Book
            }
            else
            {
                _db.BookDetails.Update(obj.Book.BookDetail);
                _db.SaveChanges(); // sacuvamo BookDetail
            }

            return RedirectToAction(nameof(Index));

            return View(obj);
        }


        public IActionResult ManageAuthors(int id) //dropdown menu
        {
            BookAuthorVM obj = new BookAuthorVM
            {
                BookAuthorList = _db.BookAuthors.Include(x => x.Author).Include(x => x.Book).ToList()
                                                .Where(x => x.Book_Id == id).ToList(),
                BookAuthor = new BookAuthor()
                {
                    Book_Id = id
                },
                Book = _db.Books.FirstOrDefault(x => x.Book_Id == id)
            };

            List<int> tempListOfAssignedAuthors = obj.BookAuthorList.Select(x => x.Author_Id).ToList();

            var tempList = _db.Authors.Where(x => !tempListOfAssignedAuthors.Contains(x.Author_Id)).ToList();

            obj.AuthorList = tempList.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Author_Id.ToString()
            });

            return View(obj);
        }

        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if(bookAuthorVM.BookAuthor.Book_Id != 0 && bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _db.BookAuthors.Add(bookAuthorVM.BookAuthor);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id } );
        }

        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM) // to je mode sa view-a ManageAuthors :D ( @model WizLib_Model.ViewModels.BookAuthorVM )
        {
            int bookId = bookAuthorVM.Book.Book_Id;
            BookAuthor bookAuthor = _db.BookAuthors.FirstOrDefault(x => x.Author_Id == authorId && x.Book_Id == bookId);

            _db.BookAuthors.Remove(bookAuthor);
            _db.SaveChanges();

            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
        }

        public IActionResult PlayGround()
        {
            //var bookTemp = _db.Books.FirstOrDefault();
            //bookTemp.Price = 100;

            //var bookCollection = _db.Books;
            //double totalPrice = 0;

            //foreach (var book in bookCollection)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookList = _db.Books.ToList();
            //foreach (var book in bookList)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookCollection2 = _db.Books;
            //var bookCount1 = bookCollection2.Count();

            //var bookCount2 = _db.Books.Count();

            //IEnumerable<Book> BookList1 = _db.Books;
            //var FilteredBook1 = BookList1.Where(x => x.Price > 500).ToList();

            //IQueryable<Book> BookList2 = _db.Books;
            //var FilteredBook2 = BookList2.Where(x => x.Price > 500).ToList();

            //var BookList3 = _db.Books;
            //var FilteredBook3 = BookList1.Where(x => x.Price > 500).ToList();

            //var category = _db.Categories.FirstOrDefault();
            //_db.Entry(category).State = EntityState.Modified; // natjerali smo da izvrsi update query iako nista nismo promjenili

            //_db.SaveChanges();

            //var bookTemp1 = _db.Books.Include(x => x.BookDetail).FirstOrDefault(y => y.Book_Id == 4);
            //bookTemp1.BookDetail.NumberOfChapters = 2222;

            //_db.Books.Update(bookTemp1); // updateuje sve atribute
            //_db.SaveChanges();

            //var bookTemp2 = _db.Books.Include(x => x.BookDetail).FirstOrDefault(y => y.Book_Id == 4);
            //bookTemp2.BookDetail.Weight = 3333;

            //_db.Books.Attach(bookTemp2); // updateujemo samo propertije koji su promjenjeni (za one zapise koji sigurno postoje u bazi)
            //_db.SaveChanges();

            //VIEWS

            var viewList = _db.BookDetailsFromViews.ToList();
            var viewList1 = _db.BookDetailsFromViews.FirstOrDefault();
            var viewList2 = _db.BookDetailsFromViews.Where(x => x.Price > 500);

            var bookRaw = _db.Books.FromSqlRaw("SELECT * FROM dbo.Books").ToList(); // nikad nemoj proslijedjivati direkt parametere (npr u where uslov) jer tada je aplikacija ranjiva na
                                                                                    // SQL napade (vec preko varijable kao 'string interpolation' i znak $ (i sad smo zasticeni ok SQL napada))!!!
            // var bookRaw = _db.Books.FromSqlRaw("SELECT * FROM dbo.Books where book_id=1").ToList(); // ranjivo !!
            int id = 1;
            var bookTemp1= _db.Books.FromSqlRaw($"SELECT * FROM dbo.Books WHERE Book_Id={id}").ToList();

            var bookSproc = _db.Books.FromSqlInterpolated($"EXEC dbo.getAllBookDetails {id}").ToList(); // call stored procedure


            //  .NET 5 and above

            var BookFilter1 = _db.Books.Include(e => e.BookAuthors.Where(x => x.Author_Id == 1)).ToList(); // filtriranje je moguce po kolekciji a ne i po entitetu (BookAuthors je kolekcija)

            var BookFilter2 = _db.Books.Include(e => e.BookAuthors.OrderByDescending(x => x.Author_Id).Take(2)).ToList();

            return RedirectToAction(nameof(Index));
        }

    }
}
