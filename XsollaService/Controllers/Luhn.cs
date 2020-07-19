using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XsollaService.Controllers
{
    /// <summary>
    /// APIMethod that accepts card data and sessionId.
    /// The card number must be checked against the algorithm of the Luhn.
    /// Valid numbers should simulate successful payment,
    /// invalid numbers should return an error.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class LuhnController : ControllerBase
    {
        private readonly ILogger<LuhnController> _logger;

        /// <summary>
        /// LuhnController Constructor
        /// </summary>
        /// <param name="logger">Type used to perform logging</param>
        public LuhnController(ILogger<LuhnController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method that accepts card data and checks numbers for validity.
        /// </summary>
        /// <param name="number">The number of the card.</param>
        /// <param name="CVV_CVC">CVV or CVC of the card.</param>
        /// <param name="date">Date of registration of the card.</param>
        /// <param name="sessionID">Session ID</param>
        /// <returns>Returns the payment status </returns>
        [HttpGet]
        public string Get(string number,int CVV_CVC, DateTime date, string sessionID)
        {
            Dictionary<string, Payment> payments;

            bool check = false;

            DateTime paymentDate = DateTime.Now;
            DateTime nowDate = DateTime.Now;

            using (FileStream stream = new FileStream("Lib.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                stream.Position = 0;
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                payments = DeserializationDict(binaryFormatter, stream);

                check = CheckSessionID(payments, sessionID, ref paymentDate);

                stream.Position = 0;
                binaryFormatter.Serialize(stream, payments);
            }

            if (!check)
            {
                return "sessionID not found";
            }

            if(!CheckDate(nowDate,paymentDate))
            {
                return "Session timeframe expired";
            }

            return CheckLuhn(number)? "true - Valid card number" : "false - Invalid card number";                            
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
        /// Method checks the validity of the session identifier.
        /// </summary>
        /// <param name="payments">Dictionary with all session ID</param>
        /// <param name="sessionID">session ID</param>
        /// <param name="paymentDate">Reference to date of found payment </param>
        /// <returns>Returns true if the session ID exists, otherwise false</returns>
        static public bool CheckSessionID(Dictionary<string, Payment> payments, string sessionID, ref DateTime paymentDate)
        {
            foreach (var payment in payments)
            {
                if (payment.Key == sessionID)
                {
                    paymentDate = payment.Value.date;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method limits the life time of the payment session.
        /// </summary>
        /// <param name="nowDate">Current time</param>
        /// <param name="paymentDate">session ID generation time</param>
        /// <returns>Returns true if current time is 5 minutes longer 
        /// than session ID creation time, otherwise false.</returns>
        static public bool CheckDate(DateTime nowDate, DateTime paymentDate)
        {
            if (paymentDate > nowDate)
            {
                return false;
            }

            return nowDate.Subtract(paymentDate).TotalMinutes <= 5;
        }

        /// <summary>
        /// The method checks the validity of the map using the Luhn method.
        /// </summary>
        /// <param name="number">Card number</param>
        /// <returns>Returns the boolean variable</returns>
        static public bool CheckLuhn(string number)
        {
            int sum = 0;
            int len = number.Length;

            for (int i = 0; i < len; i++)
            {
                int add = (number[i] - '0') * (2 - (i + len) % 2);
                add -= add > 9 ? 9 : 0;
                sum += add;
            }

            return sum % 10 == 0;
        }
    }
}
