using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Dapper;
using MySql.Data.MySqlClient;
using Swashbuckle.Swagger.Annotations;

namespace MySQLApi.Controllers
{
    public class DatabaseController : ApiController
    {
        /// <summary>
        ///     Connects this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        private static IDbConnection Connect
        {
            get
            {
                try
                {
                    var cn = new MySqlConnection
                    {
                        ConnectionString = ConfigurationManager.AppSettings["ConnectionString"]
                    };
                    cn.Open();
                    return cn;
                }
                catch (Exception e)
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError) {Source = e.Message};
                }
            }
        }

        /// <summary>
        ///     Gets the records.
        /// </summary>
        /// <param name="query">The SQL query.</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        /// <exception cref="HttpResponseMessage"></exception>
        [HttpGet]
        [SwaggerOperation("GetRecords")]
        [SwaggerResponse(HttpStatusCode.OK, "Success")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<dynamic>> GetRecords(string query)
        {
            using (var con = Connect)
            {
                try
                {
                    var list = await con.QueryAsync(query).ConfigureAwait(false);

                    var enumerable = list as IList<dynamic> ?? list.ToList();

                    return enumerable;
                }
                catch (Exception e)
                {
                    var ex = new HttpResponseException(HttpStatusCode.InternalServerError);
                    ex.Response.Content = new StringContent(e.Message);
                    throw ex;
                }
            }
        }
    }
}