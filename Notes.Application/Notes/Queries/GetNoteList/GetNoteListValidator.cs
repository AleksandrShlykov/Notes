using FluentValidation;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Queries.GetNoteList
{
    public class GetNoteListValidator:AbstractValidator<GetNoteListQuery>
    {
        public GetNoteListValidator()
        {
            RuleFor(note => note.UserId).NotEqual(Guid.Empty);
        }
    }
}
