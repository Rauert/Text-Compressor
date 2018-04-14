using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Class to store symbol nodes (Leaf nodes) in the huffman tree.
namespace Asgn
{
    class HuffLeafNode : HuffTreeNode
    {
        public char symbol; //The unique char symbol.
        public DAABitArray huffCode; //The huffman code for the symbol.

        public HuffLeafNode(double inFreq, char inSymbol) : base(inFreq)
        {
            symbol = inSymbol;
            huffCode = new DAABitArray();
        }

        public char getSymbol()
        {
            return symbol;
        }

        public DAABitArray getHashCode()
        {
            return huffCode;
        }

        public override string toString() //toString used for debugging.
        {
            return ("Symbol: " + symbol + " | " + base.toString());
        }
    }
}