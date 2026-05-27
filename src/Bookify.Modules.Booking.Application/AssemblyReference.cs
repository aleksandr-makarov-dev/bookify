using System.Reflection;

namespace Bookify.Modules.Booking.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}