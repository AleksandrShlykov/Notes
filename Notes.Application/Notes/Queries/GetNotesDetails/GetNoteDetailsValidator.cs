using FluentValidation;
using Notes.Application.Notes.Queries.GetNoteDetails;
using System;

namespace Notes.Application.Notes.Queries.GetNotesDetails
{
    public class GetNoteDetailsValidator : AbstractValidator<GetNoteDetailsQuery>
    {
        public GetNoteDetailsValidator()
        {
            RuleFor(note => note.Id).NotEqual(Guid.Empty);
            RuleFor(note => note.UserId).NotEqual(Guid.Empty);
        }
    }
}
