using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml;

namespace Logic
{
    public class Huffman
    {
        public Dictionary<char, BitArray> dictionary = new Dictionary<char, BitArray>();
        public Dictionary<char, int> occurencesInString = new Dictionary<char, int>();
        public List<TreeNode> tree = new List<TreeNode>();

        public void countOccurences(string inputString)
        {
            foreach(char c in inputString)
            {
                if (occurencesInString.ContainsKey(c))
                {
                    occurencesInString[c]++;
                }
                else
                {
                    occurencesInString.Add(c, 1);
                }
            }
        }
        public void CreateATreeList()
        {
            foreach (KeyValuePair<char, int> c in occurencesInString)
            {
                tree.Add(new TreeNode(c.Key, c.Value));
            }
        }
        public void SortRootNodes()
        {
            Comparison<TreeNode> comparisonDelegate = (node1, node2) => node1.frequency.CompareTo(node2.frequency);
            tree.Sort(comparisonDelegate);
        }
        public void buildATree()
        {
            CreateATreeList();
            while (tree.Count > 1)
            {
                SortRootNodes();
                TreeNode left = tree[0];
                TreeNode right = tree[1];
                tree.Remove(right);
                tree.Remove(left);
                TreeNode newNode;
                if (tree.Count == 0)
                {
                    /* Top node is "virtual", that's why we need to make it separately (because it needs to have frequency == 1 on top)*/

                    newNode = new TreeNode(left, right, 1);
                }
                else
                {
                     newNode = new TreeNode(left, right, right.frequency + left.frequency);
                }
                tree.Add(newNode);
            }


        }
        public void SetBinaryValues(TreeNode currentNode, List<bool> path)
        {
            if (currentNode != null)
            {
                // If we're at the root node, start with an empty path
                if (path == null)
                {
                    path = new List<bool>();
                }


                currentNode.binaryValue = path;
                    // Traverse left, appending false to the path
                    List<bool> leftPath = new List<bool>(path);
                    leftPath.Add(false);
                    SetBinaryValues(currentNode.left, leftPath);

                    // Traverse right, appending true to the path
                    List<bool> rightPath = new List<bool>(path);
                    rightPath.Add(true);
                    SetBinaryValues(currentNode.right, rightPath);

            }
        }

        public List<bool> EncodeAString(string str)
        {
            countOccurences(str);
            buildATree();
            SetBinaryValues(tree.ElementAt(0), new List<bool>());
            List<bool> encoded = new List<bool>();
            TreeNode rootNode = tree.ElementAt(0);
            foreach(char c in str)
            {
                List<bool> temp = FindCharInTree(rootNode, c);
                foreach(bool b in temp)
                {
                    encoded.Add(b);
                }
            }
            return encoded;
        }
        public string Decode(List<bool> bits)
        {
            string result = "";
            while(bits.Count > 0)
            {
                List<bool> code = new List<bool>();
                while (bits.Count != 0)
                {
                    code.Add(bits.ElementAt(0));
                    bits.RemoveAt(0);
                    char c = FindBitSequenceInTree(tree.ElementAt(0),code);
                    if (c != '\0')
                    {
                        result += c;
                        code.Clear();
                    }
                }
            }
            return result;
        }

        public char FindBitSequenceInTree(TreeNode currentNode, List<bool> bits)
        {
            foreach (bool bit in bits)
            {
                if (bit == false)
                {
                    currentNode = currentNode.left;
                }
                else if (bit == true)
                {
                    currentNode = currentNode.right;
                }

            }
            return currentNode.data;
        }


        /* Maybe move this function into TreeNode.cs */
        public List<bool> FindCharInTree(TreeNode currentNode,char c)
        {
            if(currentNode != null)
            {
                if (currentNode.data == c)
                {
                    return currentNode.binaryValue;
                }
                else
                {
                    List<bool> result = FindCharInTree(currentNode.left, c) ?? FindCharInTree(currentNode.right, c);
                    return result;
                }
            }
            return null;
        }


        #region Serialization
        public byte[] SerializeOccurences()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            formatter.Serialize(stream, occurencesInString);

            byte[] serializedData = stream.ToArray();
            return serializedData;
        }

        public void DeserializeOccurences(byte[] occurences) {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(occurences);
            this.occurencesInString = (Dictionary<char,int>)formatter.Deserialize(stream);
        }


        public byte[] ConvertBoolsToBytes(List<bool> boolList)
        {
            int boolCount = boolList.Count;
            int byteCount = (boolCount + 7) / 8; // Round up to the nearest byte
            int extraZeroes = byteCount * 8 - boolCount; // Calculate number of zeroes to add

            byte[] byteArray = new byte[byteCount + 1]; // Add extra byte for zero count

            // Add extra zeroes to the last byte
            for (int i = 0; i < extraZeroes; i++)
            {
                byteArray[byteCount] |= (byte)(0 << (7 - i));
            }

            // Convert bools to bytes
            for (int i = 0; i < boolCount; i++)
            {
                if (boolList[i])
                {
                    byteArray[i / 8] |= (byte)(1 << (7 - (i % 8)));
                }
            }

            // Set the last byte to the number of extra zeroes added
            byteArray[byteCount] |= (byte)extraZeroes;

            return byteArray;
        }

        public List<bool> ConvertBytesToBools(byte[] byteArray)
        {
            List<bool> boolList = new List<bool>();

            int byteCount = byteArray.Length - 1; // Ignore the last byte with the zero count
            int extraZeroes = byteArray[byteCount]; // Get the number of extra zeroes added

            for (int i = 0; i < byteCount; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bool bit = (byteArray[i] & (1 << (7 - j))) != 0;
                    boolList.Add(bit);
                }
            }

            // Remove the extra zeroes from the end
            boolList.RemoveRange(boolList.Count - extraZeroes, extraZeroes);

            return boolList;
        }
        #endregion

        public string ConvertBytesToString(byte[] bytes)
        {
            string result = "";
            foreach(byte character in bytes)
            {
                result += (char)character;
            }
            return result;
        }
        public byte[] ConvertStringToBytes(string message)
        {
            byte[] bytelist = new byte[message.Length];
            for(int i = 0; i<message.Length; i++)
            {
                bytelist[i] =  (byte) message.ElementAt(i);
            }
            return bytelist;
        }

    }
}
