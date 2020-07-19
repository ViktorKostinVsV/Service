using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XsollaService.Controllers
{
    /// <summary>
    /// APIMethod, which returns the list of all payments for the transferred period
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        /// <summary>
        /// AdminController Constructor
        /// </summary>
        /// <param name="logger">Type used to perform logging</param>
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method returns the list of all payments for the transferred period.
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="login">Login</param>
        /// <param name="Begin">From what time to make a search</param>
        /// <param name="Last">Until what time to search</param>
        /// <returns>Returns a line with selected payments </returns>
        [HttpGet]
        public string Get(string password, string login, DateTime Begin, DateTime Last)
        {
            string result = "";

            Dictionary<string, Payment> payments;


            if (password == "123456789" && login == "admin")
            {
                using (FileStream stream = new FileStream("Lib.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    stream.Position = 0;
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    payments = DeserializationDict(binaryFormatter, stream);

                    var list = ReturnList(payments, Begin, Last);
                    result = GetResult(list);

                    stream.Position = 0;
                    binaryFormatter.Serialize(stream, payments);
                }
            }
            else
            {
                result = "Wrong password or login";
            }

            return result;
        }

        /// <summary>
        /// The method deserializes the transmitted stream in Ditionary.
        /// </summary>
        /// <param name="bf">Class for serialization and deserialization of the object</param>
        /// <param name="stream">Stream for storing all sessions</param>
        /// <returns>Object Dictionary</returns>
        static public Dictionary<string, Payment> DeserializationDict(BinaryFormatter bf, FileStream stream)
        {
            Dictionary<string, Payment> payments = new Dictionary<string, Payment>();

            try
            {
                while (true)
                {
                    payments = (Dictionary<string, Payment>)bf.Deserialize(stream);
                }
            }
            catch
            {

            }

            return payments;
        }

        /// <summary>
        /// Method that selects all payments for the selected period 
        /// </summary>
        /// <param name="payments">Dictionary with all session ID</param>
        /// <param name="Begin">From what time to make a search</param>
        /// <param name="Last">Until what time to search</param>
        /// <returns>Returns Key/Value pair with suitable payments</returns>
        static public IEnumerable<KeyValuePair<string, Payment>> ReturnList(Dictionary<string, Payment> payments, DateTime Begin, DateTime Last)
        {
            if (Begin == new DateTime())
            {
                if (Last != new DateTime())
                {
                    var list = payments.Where(o => o.Value.date <= Last).Select(o => o);
                    return list;
                }
                else
                {
                    return payments;
                }
            }


            if (payments != null || payments.Count != 0)
            {
                var list = payments.Where(o => o.Value.date >= Begin).Select(o => o);

                if (Last != new DateTime())
                {
                    list = list.Where(o => o.Value.date <= Last).Select(o => o);
                }
                else
                {

                }
                return list;
            }

            return null;
        }

        /// <summary>
        /// Method returns a string with all elements in the passed dictionary
        /// </summary>
        /// <param name="list">Dictionary with string key and payment value</param>
        /// <returns>Returns a line with all elements</returns>
        static public string GetResult(IEnumerable<KeyValuePair<string, Payment>> list)
        {
            string result = "";

            if (list == null)
            {
                return result;
            }

            foreach (var l in list)
            {
                result += l.Value.sum + " | " + l.Value.obj + " | " + l.Value.date + " | " + l.Key + "\n";
            }

            return result;
        }
    }
}
