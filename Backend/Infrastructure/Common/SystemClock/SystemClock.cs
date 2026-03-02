using Application.Common;

namespace Infrastructure.Common.SystemClock
{
    public sealed class SystemClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
