using System;
using MySql.Data.MySqlClient;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main()
        {
            const string ConnStr = "server=138.91.241.48;user=hellboy;database=vitaldev;port=3306;password=qu4gm1r3;";
            using (var conn = new MySqlConnection(ConnStr))
            {
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();

                    const string Sql = "SELECT * FROM EDI940";
                    var cmd = new MySqlCommand(Sql, conn);
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                        Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
            }
            Console.WriteLine("Done.");
        }
    }
}