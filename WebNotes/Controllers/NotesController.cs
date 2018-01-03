using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebNotesDataBase.DAL;
using WebNotesDataBase.Models;

namespace WebNotes.Controllers
{
    public class NotesController : Controller
    {
        //create new connections of database

        // GET: Notes
        private UserRepository uowUser;
        private NoteRepository uowNote;
        public GenericRepository<Note> noteRepository;
        public GenericRepository<User> userRepository;
        public NotesController()
        {
            uowUser = new UserRepository();
            uowNote = new NoteRepository();
            noteRepository = uowNote.unitOfWork.EntityRepository;
            userRepository = uowUser.unitOfWork.EntityRepository;
        }
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            List<Note> notes = new List<Note>(noteRepository.Get());
            if (Request.Cookies["login"] != null)
                return View(notes.ToPagedList(pageNumber, pageSize));
            else return RedirectToAction("../Users/Login");
        }

        // GET: Notes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteRepository.GetByID(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NoteId,CreatedDate,EditedDate,Label,Body")] Note note)
        {
            if (ModelState.IsValid)
            {
                User usr = userRepository.GetByID(Convert.ToInt64(Request.Cookies["login"].Value));
                note.CreatedDate = DateTime.Now;
                note.EditedDate = DateTime.Now;
                note.User = usr;
                noteRepository.Insert(note);
                noteRepository.Save();
                return RedirectToAction("Index");
            }
            return View(note);
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteRepository.GetByID(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NoteId,CreatedDate,EditedDate,Label,Body")] Note note)
        {
            if (ModelState.IsValid)
            {
                Note nt = noteRepository.GetByID(note.NoteId);
                nt.EditedDate = DateTime.Now;
                nt.Label = note.Label;
                nt.Body = note.Body;
                noteRepository.Update(nt);
                noteRepository.Save();
                return RedirectToAction("Index");
            }
            return View(note);
        }

        // GET: Notes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteRepository.GetByID(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            noteRepository.Delete(id);
            noteRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
