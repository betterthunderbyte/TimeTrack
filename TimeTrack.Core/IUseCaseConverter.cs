namespace TimeTrack.Core
{
    public interface IUseCaseConverter<T>
    {
        public void To(out T output);
        public void From(T input);
    }
}
