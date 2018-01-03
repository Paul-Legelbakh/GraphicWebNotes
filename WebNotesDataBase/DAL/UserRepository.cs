using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNotesDataBase.Models;

namespace WebNotesDataBase.DAL
{
    public class UserRepository
    {
        public WebNotesContext context = new WebNotesContext();
        public UnitOfWork<User> unitOfWork;
        public UserRepository()
        {
            unitOfWork = new UnitOfWork<User>(context);
        }
    }
}