using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public class UseCaseResult<T>
    {
        public bool Successful { get; set; }

        public T Value => Values.First();

        public IEnumerable<T> Values { get; set; }

        public UseCaseResultType ResultType { get; set; }

        /// <summary>
        /// Um Fehler oder ähnliches auszugeben
        /// </summary>
        public object MessageOutput { get; set; }

        public UseCaseResult<B> To<B>() where B : class, IUseCaseConverter<T>, new()
        {
            IEnumerable<B> r = null;

            if(Values != null)
            {
                r = Values.ToList().ConvertAll(x =>
                {
                    var v = new B();
                    v.From(x);
                    return v;
                });
            }

            return new UseCaseResult<B>()
            {
                Values = r,
                MessageOutput = MessageOutput,
                Successful = Successful,
                ResultType = ResultType
            };
        }

        public static UseCaseResult<T> Success(T result)
        {
            return new UseCaseResult<T>() {
                Values = new []{result},
                Successful = true,
                ResultType = UseCaseResultType.Ok
            };
        }

        public static UseCaseResult<T> Success(IEnumerable<T> result)
        {
            return new UseCaseResult<T>()
            {
                Values = result,
                Successful = true,
                ResultType = UseCaseResultType.Ok
            };
        }

        public static UseCaseResult<T> Failure(UseCaseResultType resultType = UseCaseResultType.BadRequest, object message = null)
        {
            return new UseCaseResult<T>() {
                Values = null,
                ResultType = resultType,
                MessageOutput = message
            };
        }
    }
}
