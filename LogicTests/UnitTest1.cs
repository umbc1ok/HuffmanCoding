
using Logic;
using NuGet.Frameworks;
using System.Net;

namespace LogicTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CountingOccurencesTest()
        {
            Huffman f = new Huffman();
            f.countOccurences("ccha");
            Assert.IsTrue(f.occurencesInString['c'] == 2);
            Assert.IsTrue(f.occurencesInString['h'] == 1);
        }

        [TestMethod]
        public void TreeCreationTest()
        {
            Huffman f = new Huffman();
            f.countOccurences("CSHACHA");
            f.buildATree();
            Assert.IsTrue(f.tree.Count == 1);
            Assert.AreEqual(1, f.tree.ElementAt(0).frequency);
        }
        [TestMethod]
        public void CodingTest()
        {
            Huffman f = new Huffman();
            /*f.countOccurences("HA£ASTRASQUAD LALA");
            f.buildATree();
            f.SetBinaryValues(f.tree.ElementAt(0), new List<bool>());*/
            List<bool> result = f.EncodeAString("HA£ASTRASQUAD LALA");
            string decoded = f.Decode(result);
            Assert.AreEqual(decoded, "HA£ASTRASQUAD LALA");
        }

        [TestMethod]
        public void CommsTest()
        {
            byte[] bytes = { 255, 232 };
            Task.Run(() => { Client.SendData("localhost", bytes); });
            byte[] buffer = Server.ReceiveData();
            Assert.AreEqual(bytes.Length, buffer.Length);
        }
        [TestMethod]
        public void SerializationAndDeSerialization()
        {
            Huffman f = new Huffman();
            Huffman f2 = new Huffman();
            f.EncodeAString("HA£ASTRASQUAD LALA");
            Dictionary<char, int> test = f.occurencesInString;
            f2.DeserializeOccurences(f.SerializeOccurences());
            Assert.AreEqual(test.Count, f2.occurencesInString.Count);
        }


        [TestMethod]
        public void ListToByteConversion()
        {
            Huffman f = new Huffman();
            List<bool> temp = f.EncodeAString("HA£ASTRASQUAD LALA");
            byte[] byteArray = f.ConvertBoolsToBytes(temp);
            List<bool> temp2 = f.ConvertBytesToBools(byteArray);
            Assert.AreEqual(temp.Count, temp2.Count);
        }

        [TestMethod]
        public void DecodingAfterTransmission()
        {
            Huffman f = new Huffman();
            string entry = "Polskaa";
            List<bool> temp = f.EncodeAString(entry);
            byte[] encodedMessage = f.ConvertBoolsToBytes(temp);
            Task.Run(() => { Client.SendData("localhost", encodedMessage); });
            byte[] encodedMessageReceived = Server.ReceiveData();

            byte[] SerializedTree = f.SerializeOccurences();
            Task.Run(() => { Client.SendData("localhost",SerializedTree); });
            byte[] SerializedTreeReceived = Server.ReceiveData();


            Huffman f2 = new Huffman();
            f2.DeserializeOccurences(SerializedTreeReceived);
            f2.buildATree();
            List<bool> encodedMessageInBools = f2.ConvertBytesToBools(encodedMessageReceived);
            string result = f2.Decode(encodedMessageInBools);
            Assert.AreEqual(result, entry);
        }


    }
}