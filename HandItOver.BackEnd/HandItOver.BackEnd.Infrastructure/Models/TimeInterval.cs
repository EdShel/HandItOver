using System;

namespace HandItOver.BackEnd.Infrastructure.Models
{
    public class TimeInterval
    {
        public TimeInterval(DateTime? begin, DateTime? end)
        {
            this.Begin = begin;
            this.End = end;
        }

        public DateTime? Begin { get; }

        public DateTime? End { get; }

        public TimeSpan? GetDuration()
        {
            return this.Begin == null || this.End == null
                ? null
                : this.End - this.Begin;
        }

        public bool DoesFullyContain(TimeInterval other)
        {
            bool otherBeginIsInside = this.Begin == null
                || (other.Begin != null && this.Begin <= other.Begin);
            bool otherEndIsInside = this.End == null
                || (other.End != null && other.End <= this.End);
            return otherBeginIsInside && otherEndIsInside;
        }

        public bool DoesContain(DateTime period)
        {
            return (this.Begin == null || this.Begin <= period)
                && (this.End == null || period <= this.End);
        }
    }
}
