namespace Application.Interfaces
{
    // Interface for the user accessor, defines the methods that the user accessor must implement.
    // Interface is located here so that it is injectable into application layer without the application layer needing access to the persistence layer
    public interface IUserAccessor
    {
        string GetUsername();
    }
}