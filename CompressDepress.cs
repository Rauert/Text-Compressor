using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

//Class to perform compression and decompression.
namespace Asgn
{
    class CompressDepress
    {
        string[] alphanumerics = new string[64] {" ",
                                                 "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
                                                 "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                                 "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                                                 "\n"}; //Array containing the cipher values.

        //Converts binary stream to alphanumeric cipher.
        public string compress(DAABitArray binaryStream)
        {
            string compressedStream = "";
            for (int i = 5; i < binaryStream.NumBits; i = i + 6) { //Iterate through every six bits.
                compressedStream = compressedStream + alphanumerics[binaryStream.GetBitRange(i - 5, i)]; //Add alphanumeric to output stream.
            }
            return compressedStream;
        }

        //Converts alphanumeric cipher to binary stream.
        public DAABitArray decompressCipher(String compressedText)
        {
            DAABitArray decompStream = new DAABitArray();
            foreach (char ch in compressedText) { //Iterate through each character of the alphanumeric code.
                for (int i = 0; i < alphanumerics.Length; i++) { //Find the character in the alphanumeric array.
                    if (alphanumerics[i] == ch.ToString()) {
                        decompStream.Append(i, 6); //Add equivalent six bit code to bit stream.
                    }
                }
            }
            return decompStream;
        }

        //Converts binary stream to uncompressed text.
        public string decompressStream(DAABitArray decompStream, List<HuffLeafNode> leafNodePtrs)
        {
            DAABitArray tempBitArray = new DAABitArray();
            string decompString = "";

            for (int i = 0; i < decompStream.NumBits; i++) { //Iterate through each bit of bit stream.
                tempBitArray.Append(decompStream.GetBitAsBool(i)); //Add one bit to temp bit stream.
                for (int j = 0; j < leafNodePtrs.Count; j++) { //Compare temp bit stream to each leaf node of huffman tree.
                    if (bitArraysEqual(tempBitArray, leafNodePtrs[j].getHashCode()) == true) { //If symbol found, add character to output string.
                        tempBitArray = new DAABitArray();
                        decompString = decompString + leafNodePtrs[j].getSymbol();
                    }
                }
            }
            return decompString;
        }

        //Determine if two DAABitArray objects are equal.
        private bool bitArraysEqual(DAABitArray input1, DAABitArray input2)
        {
            string tempBitString1 = "";
            string tempBitString2 = "";
            for (int i = 0; i < input1.NumBits; i++) //Convert both DAABitArray objects to strings.
            {
                tempBitString1 = tempBitString1 + input1.GetBitAsLong(i);
            }
            for (int j = 0; j < input2.NumBits; j++)
            {
                tempBitString2 = tempBitString2 + input2.GetBitAsLong(j);
            }
            return (tempBitString1 == tempBitString2); //Test if strings are equal.
        }

        //Converts DAABitArray object to string. Used for debugging.
        private String outputBitArray(DAABitArray input)
        {
            string tempBitString = "";
            for (int i = 0; i < input.NumBits; i++)
            {
                tempBitString = tempBitString + input.GetBitAsLong(i);
            }
            return tempBitString;
        }
    }
}