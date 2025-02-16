using System.Collections.Concurrent;

namespace ChatAPI.Data
{
    public class SharedDBDictionary
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _userPublicKeys = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

        public ConcurrentDictionary<string, ConcurrentDictionary<string, string>> userPublickKeys => _userPublicKeys;
    }
}
