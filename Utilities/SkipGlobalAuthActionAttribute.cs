namespace AuthSystem.Utilities
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SkipGlobalAuthActionAttribute : Attribute
    {
    }
}
