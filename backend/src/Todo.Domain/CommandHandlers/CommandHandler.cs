using Todo.Domain.Interfaces;
using Todo.Domain.Validators;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace Todo.Domain.CommandHandlers
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }

        protected async Task<ValidationResult> Commit(IUnitOfWork uow)
        {
            if (!await uow.Commit())
            {
                AddError(UserMessages.CommitFailed.Message);
            }

            return ValidationResult;
        }
    }
}
