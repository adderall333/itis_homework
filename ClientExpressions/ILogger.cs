using System.Collections.Generic;

namespace ClientExpressions
{
    public interface ILogger
    {
        public void Log(object obj);
        public IEnumerable<string> GetLogs();
        public void Clear();
    }
}