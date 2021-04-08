using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrack.Web.Service.Tools.V1
{
    public interface IUseCaseConverter<T>
    {
        public void To(out T output);
        public void From(T input);
    }
}
