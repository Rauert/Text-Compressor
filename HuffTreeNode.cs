using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Class to store non symbol nodes in the huffman tree.
namespace Asgn
{
    class HuffTreeNode
    {
        public double freq;
        public HuffTreeNode leftChild;
        public HuffTreeNode rightChild;
        public HuffTreeNode parent;

        public HuffTreeNode(double inFreq, HuffTreeNode inLeftChild, HuffTreeNode inRightChild) //Constructor used when combining nodes.
        {
            freq = inFreq;
            leftChild = inLeftChild;
            rightChild = inRightChild;
            parent = null;
        }

        public HuffTreeNode(double inFreq) //Used when constructing from HuffLeafNode.
        {
            freq = inFreq;
            leftChild = null;
            rightChild = null;
            parent = null;
        }

        public HuffTreeNode getLeft()
        {
            return leftChild;
        }

        public HuffTreeNode getRight()
        {
            return rightChild;
        }

        public HuffTreeNode getParent()
        {
            return parent;
        }

        public void setParent(HuffTreeNode inParent)
        {
            parent = inParent;
        }

        public double getFreq()
        {
            return freq;
        }

        public bool isLeaf() //Returns true if leaf node.
        {
            bool isLf = false;
            if ((leftChild == null) && (rightChild == null)){
                isLf = true;
            }
            return isLf;
        }

        public virtual string toString() //toString used for debugging.
        {
            return ("Frequency: " + freq);
        }
    }
}