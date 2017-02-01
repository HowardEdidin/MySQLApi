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
        ///     Gets the records.
        /// </summary>
        /// <param name="query">The SQL query.</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        /// <exception cref="HttpResponseMessage"></exception>
        [HttpGet]
        [SwaggerOperation("GetRecords")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<dynamic> GetRecords(string query)
        {
            using (var con = Connect())
            {
                var list = new List<dynamic>();

                try
                {
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.Default, new CancellationToken());
                        if (!rdr.HasRows)
                            return new HttpResponseMessage(HttpStatusCode.NotFound)
                            {
                                Content = new StringContent("No Records Found")
                            };
                        while (rdr.Read())
                            list.Add(rdr);

                        return list;
                    }
                }
                catch (Exception e)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest) {Content = new StringContent(e.Message)};
                }
            }
        }

        private static MySqlConnection Connect()
        {
            const string ConnStr =
                "server=azu-lnx-hydratest;user=hellboy;database=vitaldev;port=3306;password=qu4gm1r3;";

            using (var conn = new MySqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    return conn;
                }
                catch (Exception e)
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError) {Source = e.Message};
                }
            }
        }
    }
}