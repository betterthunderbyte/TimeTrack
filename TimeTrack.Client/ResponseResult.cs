
namespace TimeTrack.Client
{
    public class ResponseResult<A, B, C> where A : class, new() where B : class?, new() where C : class?, new()
    {
        public A Result { get; set; }
        public B Response { get; set; }
        public C Request { get; set; }

        public ResponseResult(A result)
        {
            Result = result;
            Response = null;
            Request = null;
        }
        
        public ResponseResult(A result, B response)
        {
            Result = result;
            Response = response;
        }
        
        public ResponseResult(A result, C request)
        {
            Result = result;
            Response = null;
            Request = request;
        }
        
        public ResponseResult(A result, B response, C request)
        {
            Result = result;
            Response = response;
            Request = request;
        }
    }
}