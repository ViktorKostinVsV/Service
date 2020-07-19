using System;

namespace XsollaService
{
    /// <summary>
    /// Initializing purchasing session class
    /// </summary>
    [Serializable]
    public class Payment
    {
        /// <summary>
        /// sum
        /// </summary>
        public int sum { get; set; }
        /// <summary>
        /// purpose of payment
        /// </summary>
        public string obj { get; set; }
        /// <summary>
        /// Session ID
        /// </summary>
        public string sessionID { get; set; }
        /// <summary>
        /// Date of payment creation 
        /// </summary>
        public DateTime date { get; set; }

        /// <summary>
        /// Payment class constructor
        /// </summary>
        /// <param name="s">sum</param>
        /// <param name="o">purpose of payment</param>
        /// <param name="ID">Session ID</param>
        public Payment(int s, string o, string ID)
        {
            sum = s;
            obj = o;
            sessionID = ID;
            date = DateTime.Now;
        }

        /// <summary>
        /// Payment class constructor
        /// </summary>
        /// <param name="s">sum</param>
        /// <param name="o">purpose of payment</param>
        /// <param name="ID">Session ID</param>
        /// <param name="d">DateTime</param>
        public Payment(int s, string o, string ID, DateTime d)
        {
            sum = s;
            obj = o;
            sessionID = ID;
            date = d;
        }
    }
}
