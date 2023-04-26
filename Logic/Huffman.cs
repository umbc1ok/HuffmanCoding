using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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





    }
}
