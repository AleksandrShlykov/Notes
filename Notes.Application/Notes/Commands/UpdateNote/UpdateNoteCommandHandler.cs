﻿using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand>
    {
        public readonly INotesDbContext _dbContext;
        public UpdateNoteCommandHandler(INotesDbContext dbContext) { _dbContext = dbContext; }


        public async Task Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var entity =
                await _dbContext.Notes.FirstOrDefaultAsync(note => note.Id == request.Id, cancellationToken);
            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }
            entity.Details = request.Details;
            entity.Title= request.Title;
            entity.EditDate = DateTime.Now;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ;
        }


    }
}