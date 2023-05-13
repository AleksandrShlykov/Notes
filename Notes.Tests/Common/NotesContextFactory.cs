using Microsoft.EntityFrameworkCore;
using Notes.Persistion;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Common
{
    public class NotesContextFactory
    {
        public static Guid UserAId = Guid.NewGuid();
        public static Guid UserBId = Guid.NewGuid();

        public static Guid NoteIdForDelete = Guid.NewGuid();
        public static Guid NoteIdForUpdate = Guid.NewGuid();

        public static NotesDbContext Create()
        {
            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new NotesDbContext(options);
            context.Database.EnsureCreated();
            context.Notes.AddRange(
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "TestDetails1",
                    EditDate = null,
                    Id = Guid.Parse("9A7FB52F-53F9-4DE7-8898-D6512E1B24E2"),
                    Title = "TestTitle1",
                    UserId = UserAId
                },
                 new Note
                 {
                     CreationDate = DateTime.Today,
                     Details = "TestDetails2",
                     EditDate = null,
                     Id = Guid.Parse("828D2F18-FEC6-4E19-B0CD-D78A2A410430"),
                     Title = "TestTitle2",
                     UserId = UserBId
                 },
                 new Note
                 {
                     CreationDate = DateTime.Today,
                     Details = "TestDetails3",
                     EditDate = null,
                     Id = NoteIdForDelete,
                     Title = "TestTitle3",
                     UserId = UserAId
                 },
                 new Note
                 {
                     CreationDate = DateTime.Today,
                     Details = "TestDetails4",
                     EditDate = null,
                     Id = NoteIdForUpdate,
                     Title = "TestTitle4",
                     UserId = UserBId
                 }
                );
            context.SaveChanges();
            return context;

        }
        public static void Destroy(NotesDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();

        }
    }
}
