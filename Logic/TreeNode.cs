using System;
using System.Collections.Generic;

namespace Logic
{
    public class TreeNode
    {
        public char data;
        public int frequency;
        public List<bool> binaryValue;
        

        public TreeNode left, right;

        public TreeNode(char data, int frequency)
        {
            left = null;
            right = null;
            this.data = data;
            this.frequency = frequency;
        }
        public TreeNode(TreeNode left, TreeNode right,int frequency)
        {
            this.left = left;
            this.right = right;
            this.frequency = frequency;
        }


    }
}
