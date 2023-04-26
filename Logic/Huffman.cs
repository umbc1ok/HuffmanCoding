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
            while (tree.Count > 2)
            {
                SortRootNodes();
                TreeNode left = tree[0];
                TreeNode right = tree[1];
                tree.Remove(right);
                tree.Remove(left);
                TreeNode newNode = new TreeNode(left,right, right.frequency + left.frequency);
                tree.Add(newNode);
            }
            /* Top node is "virtual", that's why we need to make it separately (because it needs to have frequency == 1 on top)*/

            TreeNode lastLeft = tree[0];
            TreeNode lastRight = tree[1];
            tree.Remove(lastRight);
            tree.Remove(lastLeft);
            TreeNode newNode2 = new TreeNode(lastLeft, lastRight, 1);
            tree.Add(newNode2);

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
