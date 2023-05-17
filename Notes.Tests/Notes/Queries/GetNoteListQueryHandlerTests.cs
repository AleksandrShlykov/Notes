using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Persistion;
using Notes.Tests.Common;
using Shouldly;

namespace Notes.Tests.Notes.Queries
{
    [Collection("QueryCollection")]
    public class GetNoteListQueryHandlerTests
    {
        private readonly NotesDbContext _context;
        private readonly IMapper _mapper;
        public GetNoteListQueryHandlerTests(QueryTestFixture fixture) => (_context, _mapper) = (fixture.Context, fixture.Mapper);
        [Fact]
        public async Task GetNoteListQueryHandler_Success()
        {
            //Arrange
            var handler = new GetNoteListQueryHandler(_context, _mapper);

            //Act
            var result = await handler.Handle(
                new GetNoteListQuery
                {
                    UserId = NotesContextFactory.UserBId
                }, CancellationToken.None);

            //Assert
            result.ShouldBeOfType<NoteListVm>();
            result.Notes.Count.ShouldBe(2);

        }
    }
}
