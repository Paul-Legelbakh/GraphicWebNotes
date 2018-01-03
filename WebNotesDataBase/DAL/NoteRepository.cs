using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNotesDataBase.Models;

namespace WebNotesDataBase.DAL
{
    public class NoteRepository
    {
        public WebNotesContext context = new WebNotesContext();
        public UnitOfWork<Note> unitOfWork;
        public NoteRepository()
        {
            unitOfWork = new UnitOfWork<Note>(context);
        }
    }
}