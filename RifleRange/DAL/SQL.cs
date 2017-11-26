using System.Configuration;

namespace RifleRange.DAL
{
    internal abstract class SQL
    {
        public static string RifleRange { get { return ConfigurationManager.ConnectionStrings["RifleRange"].ConnectionString; } }

        public SQL()
        {
        }
    }
}
