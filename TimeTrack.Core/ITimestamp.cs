using System;

namespace TimeTrack.Core
{
    interface ITimestamp
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}
