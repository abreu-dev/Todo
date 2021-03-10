using Todo.Domain.Commands;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace Todo.Domain.Mediator
{
    public interface IMediatorHandler
    {
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
    }
}