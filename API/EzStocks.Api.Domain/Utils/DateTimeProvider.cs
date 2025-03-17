namespace EzStocks.Api.Domain.Utils
{
    public interface IDateTimeProvider 
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        private static IDateTimeProvider? _current;
        public static IDateTimeProvider Current 
        {
            get => _current ?? (_current = new DateTimeProvider());
            set => _current = value;
        }

        private DateTimeProvider()
        {
            
        }

        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
