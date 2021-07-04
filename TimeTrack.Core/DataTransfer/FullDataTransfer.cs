using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using TimeTrack.Core.DataTransfer.V1;

namespace TimeTrack.Core.DataTransfer
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot(nameof(FullDataTransfer))]
    public class FullDataTransfer
    {
     
        [XmlIgnore]
        public IEnumerable<ActivityDataTransfer> Activities { get; set; }
        
        [JsonIgnore]
        [XmlArray(nameof(Activities))]
        public List<ActivityDataTransfer> ActivitiesSurrogate
        {
            get { return Activities.ToList(); }
            set { Activities = value; }
        }
        

        [XmlIgnore]
        public IEnumerable<CustomerDataTransfer> Customers { get; set; }

        [JsonIgnore]
        [XmlArray(nameof(Customers))]
        public List<CustomerDataTransfer> CustomersSurrogate
        {
            get { return Customers.ToList(); }
            set { Customers = value; }
        }
        
        [XmlIgnore]
        public IEnumerable<ProjectDataTransfer> Projects { get; set; }
 
        [JsonIgnore]
        [XmlArray(nameof(Projects))]
        public List<ProjectDataTransfer> ProjectsSurrogate
        {
            get { return Projects.ToList(); }
            set { Projects = value; }
        }
        
        [XmlIgnore]
        public IEnumerable<ActivityTypeDataTransfer> ActivityTypes { get; set; }
        
        [JsonIgnore]
        [XmlArray(nameof(ActivityTypes))]
        public List<ActivityTypeDataTransfer> ActivityTypesSurrogate
        {
            get { return ActivityTypes.ToList(); }
            set { ActivityTypes = value; }
        }
    }
}