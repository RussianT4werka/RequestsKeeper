using RequestsKeeper.Model;

namespace RequestsKeeper.Tools
{
    public static class Session
    {
        static Dictionary<string, User> _sessions = new Dictionary<string, User>();

        public static string CreateSession(User user)
        {
            string guid = Guid.NewGuid().ToString();
            _sessions[guid] = user;
            return guid;
        }

        public static User GetVisitor(string session)
        {
            if (session == null || !_sessions.ContainsKey(session))
                return new User();
            return _sessions[session];
        }
    }
}
