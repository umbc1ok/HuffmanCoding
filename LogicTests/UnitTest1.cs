
using Logic;

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
            f.countOccurences("CHA£A");
            f.buildATree();
            f.AssignCodesToChars(f.tree.ElementAt(0), new List<bool>());
            List<bool> result = f.EncodeAString("CHA£A");
            foreach (bool b in result)
            {
                Console.Write(b.ToString());
            }
        }


    }
}