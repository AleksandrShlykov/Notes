using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteValidator : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteValidator()
        {
            RuleFor(updateNoteCommand =>
            updateNoteCommand.UserId).NotEqual(Guid.Empty);
            RuleFor(updateNoteCommand =>
            updateNoteCommand.Id).NotEqual(Guid.Empty);
            RuleFor(updateNoteCommand =>
            updateNoteCommand.Title).NotEmpty().MaximumLength(255);
        }
    }
}
