using System;

namespace Todo.Infra.Data.Accessor
{
    public interface IUserAccessor
    {
        Guid GetUserId();
        bool IsAuthenticated();
    }
}
