
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
            Task.Run(() => { Client.SendData(bytes); });
            byte[] buffer = Server.ReceiveData();
            Console.WriteLine(bytes[0].ToString());
            Console.WriteLine(buffer[0].ToString());
            Assert.AreEqual(bytes, buffer);
        }
        [TestMethod]
        public void SerializationAndDeSerialization()
        {
            Huffman f = new Huffman();
            Huffman f2 = new Huffman();
            f.EncodeAString("HA£ASTRASQUAD LALA");
            Dictionary<char, int> test = f.occurencesInString;
            f2.DeserializeOccurences(f.SerializeOccurences());
            Assert.AreEqual(test, f2.occurencesInString);
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
    }
}