using Todo.Domain.Commands;
using Todo.Domain.Mediator;
using FluentValidation.Results;
using MediatR;
using System.Threading.Tasks;

namespace Todo.Infra.CrossCutting.Bus
{
    public class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public InMemoryBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ValidationResult> SendCommand<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }
    }
}
