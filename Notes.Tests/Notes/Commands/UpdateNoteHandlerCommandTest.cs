using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commands
{
    public class UpdateNoteHandlerCommandTest : TestCommandBase
    {
        [Fact]
        public async Task UpdateNoteCommandTest_Success()
        {
            //Arrange
            var handler = new UpdateNoteCommandHandler(_context);
            var newNoteTitle = "new Title";
            var newNoteDetails = "new Detils";
            //Act
            await handler.Handle(
                new UpdateNoteCommand
                {
                    UserId = NotesContextFactory.UserBId,
                    Id = NotesContextFactory.NoteIdForUpdate,
                    Title = newNoteTitle,
                    Details = newNoteDetails
                }, CancellationToken.None);

            //Assert
            Assert.NotNull(await _context.Notes.SingleOrDefaultAsync(note =>
            note.Id == NotesContextFactory.NoteIdForUpdate && note.UserId == NotesContextFactory.UserBId
            && note.Title == newNoteTitle && note.Details == newNoteDetails));
        }
        [Fact]
        public async Task UpdateNoteCommandTest_FailOnWrongId()
        {
            //Arrange
            var handler = new UpdateNoteCommandHandler(_context);
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
             await handler.Handle(
                new UpdateNoteCommand
                {
                    UserId = NotesContextFactory.UserBId,
                    Id = Guid.NewGuid(),
                }, CancellationToken.None)
            );
        }
        [Fact]
        public async Task UpdateNoteCommandTest_FailOnWrongUserId()
        {
            //Arrange
            var handler = new UpdateNoteCommandHandler(_context);
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(
                new UpdateNoteCommand
                {
                    Id = NotesContextFactory.NoteIdForUpdate,
                    UserId = NotesContextFactory.UserAId
                }, CancellationToken.None));
        }
    }
}
