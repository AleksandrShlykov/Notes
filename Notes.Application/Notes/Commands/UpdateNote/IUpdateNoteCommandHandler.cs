using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Commands.UpdateNote
{
    public interface IUpdateNoteCommandHandler
    {
        Task Handle(UpdateNoteCommand request, CancellationToken cancellationToken);
    }
}