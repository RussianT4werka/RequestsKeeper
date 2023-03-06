using RequestsKeeper.Model;

namespace RequestsKeeper.Tools
{
    public static class Session
    {
        static Dictionary<string, User> _sessions = new Dictionary<string, User>();

        private static List<(string, Visitor)> _groupUserVisitors = new List<(string, Visitor)>();

        public static void CreateGroupVisitor(string guid, Visitor visitor)
        {
            _groupUserVisitors.Add(new(guid, visitor));
        }

        public static List<Visitor> ReturnListVisitor(string guid)
        {
            var visitors = new List<Visitor>();
            foreach((string, Visitor) visitor in _groupUserVisitors)
            {
                if (guid == visitor.Item1)
                {
                    visitors.Add(visitor.Item2);
                }
            }
            return visitors;
        }

        public static void RemoveVisitor(string guid)
        {
            for(int i = _groupUserVisitors.Count-1; i>=0; i--)
            {
                if(guid == _groupUserVisitors[i].Item1)
                {
                    _groupUserVisitors.Remove(_groupUserVisitors[i]);
                }
            }
        }

        public static string CreateSession(User user)
        {
            string guid = Guid.NewGuid().ToString();
            _sessions[guid] = user;
            return guid;
        }

        public static User GetUser(string session)
        {
            if (session == null || !_sessions.ContainsKey(session))
                return new User();
            return _sessions[session];
        }
    }
}
