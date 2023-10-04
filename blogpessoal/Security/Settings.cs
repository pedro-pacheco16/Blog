namespace blogpessoal.Security
{
    public class Settings
    {
        private static string secret = "bb92c444c29483fdd41c33f5d36c0320ae50a045bafb181de7c115639c211bdb";

        public static string Secret { get => secret; set => secret = value;}
    }
}
 