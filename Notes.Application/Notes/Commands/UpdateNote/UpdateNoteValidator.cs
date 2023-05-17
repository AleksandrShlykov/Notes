using FluentValidation;
using System;

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
