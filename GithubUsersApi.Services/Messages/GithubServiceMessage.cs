using System;

namespace GithubUsersApi.Messages
{
    public class GithubServiceMessage<T>
    {
        public T Message { get; }
        public Exception Exception { get; }

        public GithubServiceMessage(T message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }

        public Boolean HasException
        {
            get {  return Exception != null; }
        }
    }
}