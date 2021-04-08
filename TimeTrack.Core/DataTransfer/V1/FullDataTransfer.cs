using System.Collections.Generic;

namespace TimeTrack.Core.DataTransfer.V1
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