namespace Todo.Domain.Validators
{
    public class UserMessage
    {
        public string Message { get; }

        public UserMessage(string message)
        {
            Message = message;
        }

        public UserMessage Format(params object[] args)
        {
            return new UserMessage(string.Format(Message, args));
        }
    }

    public static class UserMessages
    {
        public static UserMessage RequiredField => new UserMessage("Please, ensure you enter {0}.");
        public static UserMessage CommitFailed => new UserMessage("There was an error saving data.");
        public static UserMessage NotFound => new UserMessage("The informed {0} was not found.");
        public static UserMessage MustBeGreatherThan => new UserMessage("{0} must be greather than {1}.");
        public static UserMessage AlreadyInUse => new UserMessage("The informed {0} is already in use.");
        public static UserMessage MakeSureAtLeast => new UserMessage("Make sure {0} it's at least {1} characters.");
        public static UserMessage InvalidFormat => new UserMessage("The informed {0} is invalid.");
        public static UserMessage IncorrectSigninCredentials => new UserMessage("Incorrect email or password.");
        public static UserMessage DoesntExists => new UserMessage("{0} doesn't exists.");
    }
}
