using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XsollaService.Controllers
{
    /// <summary>
    /// APIMethod that accepts the amount and purpose of payment 
    /// and returns sessionId - payment session identifier
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InitPaymentController : ControllerBase
    {       

        private readonly ILogger<InitPaymentController> _logger;

        /// <summary>
        /// InitPaymentController Constructor
        /// </summary>
        /// <param name="logger">Type used to perform logging</param>
        public InitPaymentController(ILogger<InitPaymentController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method, accepts amount and purpose of payment 
        /// and returns sessionId - payment session identifier
        /// </summary>
        /// <param name="sum">Payment amount</param>
        /// <param name="obj">Purpose of payment</param>
        /// <returns>Returns sessionId - payment session identifier</returns>
        [HttpGet]
        public string Get(int sum, string obj)
        {
            Dictionary<string, Payment> payments;
            Payment next;

            using (FileStream stream = new FileStream("Lib.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                stream.Position = 0;
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                payments = DeserializationDict(binaryFormatter, stream);

                next = new Payment(sum, obj, Guid.NewGuid().ToString());
                payments.Add(next.sessionID, next);

                stream.Position = 0;
                binaryFormatter.Serialize(stream, payments);
            }

            return next.sessionID;
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

    }
}
