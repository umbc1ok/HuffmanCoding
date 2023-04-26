using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logic
{
    public class FileCreator
    {

        byte[] serialziedString;
        byte[] serialziedHuffmanTree;

        public FileCreator() { }

        public void Serialize(Huffman h, string message)
        {
            serialziedHuffmanTree = JsonSerializer.SerializeToUtf8Bytes(TreeToString(h.tree.ElementAt(0)));
            serialziedString = JsonSerializer.SerializeToUtf8Bytes(message.ToString());
        }


        public string TreeToString(TreeNode root)
        {
            if (root == null)
            {
                return "";
            }
            string result = root.data.ToString() + root.frequency.ToString();
            if (root.left != null)
            {
                result += "L" + TreeToString(root.left);
            }
            if (root.right != null)
            {
                result += "R" + TreeToString(root.right);
            }
            return result;
        }
        public TreeNode StringToTree(ref string s)
        {
            if (s.Length == 0)
            {
                return null;
            }
            char c = s[0];
            int freq = 0;
            int i = 1;
            while (i < s.Length && char.IsDigit(s[i]))
            {
                freq = freq * 10 + (s[i] - '0');
                i++;
            }
            TreeNode node = new TreeNode(c, freq);
            if (i < s.Length && s[i] == 'L')
            {
                s = s.Substring(i + 1);
                node.left = StringToTree(ref s);
            }
            if (i < s.Length && s[i] == 'R')
            {
                s = s.Substring(i + 1);
                node.right = StringToTree(ref s);
            }
            return node;
        }


    }
}
