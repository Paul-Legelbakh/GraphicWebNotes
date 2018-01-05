namespace WebNotesDataBase.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class WebNotesContext : DbContext
    {
        public WebNotesContext()
            : base("WebNotesDataBase")
        {
        }

        //Fluent API realization
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>()
                .HasRequired(note => note.User)
                .WithMany(user => user.Notes)
                .HasForeignKey(note => note.UserId)
                .WillCascadeOnDelete(false);
            //modelBuilder.Entity<User>()
            //    .HasIndex(user => user.Email)
            //    .IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}