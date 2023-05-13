using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Persistion;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace Notes.Tests.Notes.Queries
{
    [Collection("QueryCollection")]
    public class GetNotesDetailsQueryHabdlerTests
    {
        private readonly NotesDbContext _context;
        private readonly IMapper _mapper;
        public GetNotesDetailsQueryHabdlerTests(QueryTestFixture fixture) => (_context, _mapper) = (fixture.Context, fixture.Mapper);

        [Fact]
        public async Task GetNotesDetaisQueryHandler_Success()
        {
            //Arrenge
            var handler = new GetNoteDetailsQueryHandler(_context, _mapper);
            //Act
            var result = await handler.Handle(
                new GetNoteDetailsQuery
                {
                    UserId = NotesContextFactory.UserAId,
                    Id = Guid.Parse("9A7FB52F-53F9-4DE7-8898-D6512E1B24E2")
                }, CancellationToken.None);

            //Assert
            result.ShouldBeOfType<NoteDetailsVm>();
            result.Title.ShouldBe("TestTitle1");
            result.CreationDate.ShouldBe(DateTime.Today);
        }

    }
}
