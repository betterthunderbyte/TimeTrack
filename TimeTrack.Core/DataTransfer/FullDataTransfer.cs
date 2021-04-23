using System.Collections.Generic;
using TimeTrack.Core.DataTransfer.V1;

namespace TimeTrack.Core.DataTransfer
{
    /// <summary>
    /// 
    /// </summary>
    public class FullDataTransfer
    {
        public IEnumerable<ActivityDataTransfer> Activities { get; set; }
        public IEnumerable<CustomerDataTransfer> Customers { get; set; }
        public IEnumerable<ProjectDataTransfer> Projects { get; set; }
        public IEnumerable<ActivityTypeDataTransfer> ActivityTypes { get; set; }
    }
}