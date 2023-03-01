using RequestsKeeper.DB;

namespace RequestsKeeper.Tools
{
    public class DBInstance
    {
        private static User502Context bd;
        public static User502Context GetInstance()
        {
            if(bd == null)
            {
                bd = new User502Context();
            }
            return bd;
        }
    }
}
