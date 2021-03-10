using Todo.Domain.Commands;
using FluentValidation;

namespace Todo.Domain.Validators
{
    public class CommandValidator<T> : AbstractValidator<T> where T : Command { }
}
