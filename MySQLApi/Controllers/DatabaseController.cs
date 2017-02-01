using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MySql.Data.MySqlClient;
using Swashbuckle.Swagger.Annotations;

namespace MySQLApi.Controllers
{
    public class DatabaseController : ApiController
    {

        /// <summary>
        /// Gets the records.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        /// <exception cref="HttpResponseMessage"></exception>
        [SwaggerOperation("GetRecords")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<dynamic> GetRecords(string query)
        {
            var con = Connect();

            var list = new List<dynamic>();


            try
            {
                var cmd = new MySqlCommand(query, con);
                var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.Default, new CancellationToken());


                if (!rdr.HasRows) throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
                while (rdr.Read())
                    list.Add(rdr);

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static MySqlConnection Connect()
        {
            const string ConnStr =
                "server=138.91.241.48;user=hellboy;database=vitaldev;port=3306;password=qu4gm1r3;";

            using (var conn = new MySqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    return conn;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}