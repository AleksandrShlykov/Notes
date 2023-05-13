using Microsoft.EntityFrameworkCore;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Tests.Common;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commands
{
    public class CreateNoteHandlercommandTest : TestCommandBase
    {
        [Fact]
        public async Task CreateNoteCommandHandler_Success()
        {
            var handler = new CreateNoteCommandHandler(_context);
            var noteDetails = "note details";
            var noteName = "note name";

            var noteId = await handler.Handle(new CreateNoteCommand
            {
                Title = noteName,
                Details = noteDetails,
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None);

            Assert.NotNull(
                await _context.Notes.SingleOrDefaultAsync(note=>
                note.Id == noteId && note.Title == noteName && note.Details == noteDetails 
                && note.UserId == NotesContextFactory.UserAId));
        }
    }
}
