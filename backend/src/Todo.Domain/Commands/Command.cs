using FluentValidation.Results;
using MediatR;
using System;

namespace Todo.Domain.Commands
{
    public abstract class Command : IRequest<ValidationResult>
    {
        public Guid AggregateId { get; protected set; }
        public ValidationResult ValidationResult { get; protected set; }

        protected Command(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public abstract bool IsValid();
    }
}
