using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Core
{
    public enum UseCaseResultType
    {
        Ok,
        NoContent,
        BadRequest,
        Conflict,
        NotFound,
        Accepted
    }

    public class UseCaseResult<T> where T : class
    {
        public bool Successful { get; set; }

        public T Item => Items.First();

        public IEnumerable<T> Items { get; set; }

        public UseCaseResultType ResultType { get; set; }

        /// <summary>
        /// Um Fehler oder ähnliches auszugeben
        /// </summary>
        public object MessageOutput { get; set; }

        public UseCaseResult<B> To<B>() where B : class, IUseCaseConverter<T>, new()
        {
            IEnumerable<B> r = null;

            if(Items != null)
            {
                r = Items.ToList().ConvertAll(x =>
                {
                    var v = new B();
                    v.From(x);
                    return v;
                });
            }

            return new UseCaseResult<B>()
            {
                Items = r,
                MessageOutput = MessageOutput,
                Successful = Successful,
                ResultType = ResultType
            };
        }

        public static UseCaseResult<T> Success(T result)
        {
            return new UseCaseResult<T>() {
                Items = new []{result},
                Successful = true,
                ResultType = UseCaseResultType.Ok
            };
        }

        public static UseCaseResult<T> Success(IEnumerable<T> result)
        {
            return new UseCaseResult<T>()
            {
                Items = result,
                Successful = true,
                ResultType = UseCaseResultType.Ok
            };
        }

        public static UseCaseResult<T> Failure(UseCaseResultType resultType, object message)
        {
            return new UseCaseResult<T>() {
                Items = null,
                ResultType = resultType,
                MessageOutput = message
            };
        }
    }
}
