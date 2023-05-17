using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Tests.Common;

namespace Notes.Tests.Notes.Commands
{
    public class DeleteNoteHandlerCommandTest : TestCommandBase
    {
        [Fact]
        public async Task DeleteNotecommandHandler_Success()
        {
            //Arrange
            var handler = new DeleteNoteCommandHandler(_context);
            var userId = NotesContextFactory.UserAId;

            //Act
            await handler.Handle(
                new DeleteNoteCommand
                {
                    UserId = userId,
                    Id = NotesContextFactory.NoteIdForDelete
                }, CancellationToken.None
                );

            //Assert
            Assert.Null(_context.Notes.SingleOrDefault(note => note.Id == NotesContextFactory.NoteIdForDelete));
        }

        [Fact]
        public async void DeleteNoteCommandHandler_FailOnWrongId()
        {
            //Arrange
            var handler = new DeleteNoteCommandHandler(_context);

            //Act

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteNoteCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = NotesContextFactory.UserAId
                    }, CancellationToken.None
                    ));
        }
        [Fact]
        public async void DeleteNoteCommandHanler_FailsOnWrongUserId()
        {
            //Arrange
            var deleteHandler = new DeleteNoteCommandHandler(_context);
            var createHandler = new CreateNoteCommandHandler(_context);
            var noteId = await createHandler.Handle(
                new CreateNoteCommand
                {
                    Title = "create title",
                    Details = "create detail",
                    UserId = NotesContextFactory.UserAId
                }, CancellationToken.None);
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
              await deleteHandler.Handle(
                  new DeleteNoteCommand
                  {
                      Id = Guid.NewGuid(),
                      UserId = NotesContextFactory.UserBId
                  }, CancellationToken.None));

        }
    }
}
