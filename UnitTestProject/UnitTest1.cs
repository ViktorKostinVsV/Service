using Microsoft.VisualStudio.TestTools.UnitTesting;
using XsollaService;
using XsollaService.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void DeserializationDict_and_GetResult_Tests_1()
        {
            // Arrange
            string obj = "";
            Dictionary<string, Payment> payments = new Dictionary<string, Payment>();
            Dictionary<string, Payment> payments2 = new Dictionary<string, Payment>();
            Payment next;
            for (int i = 0; i < 10; i++)
            {
                next = new Payment(i, obj, Guid.NewGuid().ToString());

                payments.Add(next.sessionID, next);
                payments2.Add(next.sessionID, next);
            }

            // Act
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                bf.Serialize(stream, payments);
                payments.Clear();
            }

            using (FileStream stream = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                stream.Position = 0;
                payments = InitPaymentController.DeserializationDict(bf, stream);
            }

            // Assert
            string one = AdminController.GetResult(payments);
            string two = AdminController.GetResult(payments2);

            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void DeserializationDict_and_GetResult_Tests_2()
        {
            // Arrange

            string obj = "";
            Dictionary<string, Payment> payments = new Dictionary<string, Payment>();
            Dictionary<string, Payment> payments2 = new Dictionary<string, Payment>();
            Payment next;
            for (int i = 0; i < 10; i++)
            {
                next = new Payment(i, obj, Guid.NewGuid().ToString());

                payments.Add(next.sessionID, next);
                payments2.Add(next.sessionID, next);
            }

            // Act
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                bf.Serialize(stream, payments);
                payments.Clear();
            }

            using (FileStream stream = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                stream.Position = 0;
                payments = LuhnController.DeserializationDict(bf, stream);
            }

            // Assert
            string one = AdminController.GetResult(payments);
            string two = AdminController.GetResult(payments2);

            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void DeserializationDict_and_GetResult_Tests()
        {
            // Arrange
            string obj = "";
            Dictionary<string, Payment> payments = new Dictionary<string, Payment>();
            Dictionary<string, Payment> payments2 = new Dictionary<string, Payment>();
            Payment next;
            for (int i = 0; i < 10; i++)
            {
                next = new Payment(i, obj, Guid.NewGuid().ToString());

                payments.Add(next.sessionID, next);
                payments2.Add(next.sessionID, next);
            }

            // Act
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                bf.Serialize(stream, payments);
                payments.Clear();
            }

            using (FileStream stream = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                stream.Position = 0;
                payments = AdminController.DeserializationDict(bf, stream);
            }

            // Assert
            string one = AdminController.GetResult(payments);
            string two = AdminController.GetResult(payments2);

            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void CheckSessionID_Test_1()
        {
            // Arrange
            string obj = "";
            Dictionary<string, Payment> payments = new Dictionary<string, Payment>();
            Payment next;
            string guid = "";


            // Act
            for (int i = 0; i < 10; i++)
            {
                next = new Payment(i, obj, Guid.NewGuid().ToString());
                payments.Add(next.sessionID, next);

                if (i == 5)
                {
                    guid = next.sessionID;
                }
            }
            DateTime time = new DateTime();
            bool cheack = LuhnController.CheckSessionID(payments, guid, ref time);

            // Assert
            Assert.AreEqual(true, cheack);
        }

        [TestMethod]
        public void CheckSessionID_Test_2()
        {
            // Arrange
            string obj = "";
            Dictionary<string, Payment> payments = new Dictionary<string, Payment>();
            Payment next;

            // Act
            for (int i = 0; i < 10; i++)
            {
                next = new Payment(i, obj, Guid.NewGuid().ToString());
                payments.Add(next.sessionID, next);
            }
            DateTime time = new DateTime();
            bool cheack2 = LuhnController.CheckSessionID(payments, "kjlkfdjglks-dfh-fdsg", ref time);

            // Assert
            Assert.AreEqual(false, cheack2);
        }

        [TestMethod]
        public void CheckDate_Test_1()
        {
            // Arrange
            DateTime date = new DateTime(2020, 07, 19, 3, 09, 30);
            DateTime date2 = new DateTime(2020, 07, 19, 3, 05, 30);

            // Act
            bool check = LuhnController.CheckDate(date, date2);

            // Assert
            Assert.AreEqual(true, check);
        }

        [TestMethod]
        public void CheckDate_Test_2()
        {
            // Arrange
            DateTime date = DateTime.Now;
            DateTime date2 = new DateTime(2019, 07, 19, 2, 59, 30);

            // Act
            bool check = LuhnController.CheckDate(date, date2);

            // Assert
            Assert.AreEqual(false, check);
        }

        [TestMethod]
        public void CheckDate_Test_3()
        {
            // Arrange
            DateTime date = DateTime.Now;
            DateTime date2 = new DateTime(2021, 07, 19, 2, 59, 30);

            // Act
            bool check = LuhnController.CheckDate(date, date2);

            // Assert
            Assert.AreEqual(false, check);
        }

        [TestMethod]
        public void CheckLuhn_Test_1()
        {
            // Arrange
            string num = "4461261212345468";

            // Act
            bool check = LuhnController.CheckLuhn(num);

            // Assert
            Assert.AreEqual(true, check);
        }

        [TestMethod]
        public void CheckLuhn_Test_2()
        {
            // Arrange
            string num = "1461261212345458";

            // Act
            bool check = LuhnController.CheckLuhn(num);

            // Assert
            Assert.AreEqual(false, check);
        }

        [TestMethod]
        public void ReturnList_Test_1()
        {
            // Arrange
            string obj = "";
            Dictionary<string, Payment> payments = new Dictionary<string, Payment>();
            Dictionary<string, Payment> payments2 = new Dictionary<string, Payment>();
            Payment next;
            for (int i = 0; i < 10; i++)
            {
                next = new Payment(i, obj, Guid.NewGuid().ToString(), new DateTime(2020, 07, 7 + i));
                if (7 + i > 10)
                {
                    payments2.Add(next.sessionID, next);
                }
                payments.Add(next.sessionID, next);
            }

            // Act
            var list = AdminController.ReturnList(payments, new DateTime(2020, 07, 7 + 4), new DateTime());

            // Assert
            string one = AdminController.GetResult(list);
            string two = AdminController.GetResult(payments2);

            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void ReturnList_Test_2()
        {
            // Arrange
            string obj = "";
            Dictionary<string, Payment> payments = new Dictionary<string, Payment>();

            // Act
            var list = AdminController.ReturnList(payments, new DateTime(), new DateTime());

            // Assert
            string one = AdminController.GetResult(list);

            Assert.AreEqual(list, payments);
        }

        [TestMethod]
        public void ReturnList_Test_3()
        {
            // Arrange
            string obj = "";
            Dictionary<string, Payment> payments = new Dictionary<string, Payment>();
            Dictionary<string, Payment> payments2 = new Dictionary<string, Payment>();
            Payment next;
            for (int i = 0; i < 10; i++)
            {
                next = new Payment(i, obj, Guid.NewGuid().ToString(), new DateTime(2020, 07, 7 + i));
                if (7 + i > 10 && 7 + i <= 16)
                {
                    payments2.Add(next.sessionID, next);
                }
                payments.Add(next.sessionID, next);
            }

            // Act
            var list = AdminController.ReturnList(payments, new DateTime(2020, 07, 7 + 4), new DateTime(2020, 07, 7 + 9));

            // Assert
            string one = AdminController.GetResult(list);
            string two = AdminController.GetResult(payments2);

            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void ReturnList_Test_4()
        {
            // Arrange
            string obj = "";
            Dictionary<string, Payment> payments = new Dictionary<string, Payment>();
            Dictionary<string, Payment> payments2 = new Dictionary<string, Payment>();
            Payment next;
            for (int i = 0; i < 10; i++)
            {
                next = new Payment(i, obj, Guid.NewGuid().ToString(), new DateTime(2020, 07, 7 + i));
                if (7 + i <= 16)
                {
                    payments2.Add(next.sessionID, next);
                }
                payments.Add(next.sessionID, next);
            }

            // Act
            var list = AdminController.ReturnList(payments, new DateTime(), new DateTime(2020, 07, 7 + 9));

            // Assert
            string one = AdminController.GetResult(list);
            string two = AdminController.GetResult(payments2);

            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void ReturnList_Test_5()
        {
            // Arrange

            // Act
            var list = AdminController.ReturnList(null, new DateTime(), new DateTime());

            // Assert

            Assert.AreEqual(list, null);
        }
    }
}
