using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;


namespace Asgn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnFreq_Click(object sender, RoutedEventArgs e)
        {
            List<SymbolFreq> freqTbl = new List<SymbolFreq>(); //Contains the symbol frequency table.

            if (String.IsNullOrEmpty(txtPlain.Text)) { //Check if text exists to build table with.
                MessageBox.Show("No text entered");
            } else {
                if (String.IsNullOrEmpty(txtFreqTbl.Text)) { //Test if a frequency table exists.
                    txtCompressed.Text = "";
                    txtDecompressed.Text = "";
                    createFreqTbl(txtPlain.Text, freqTbl); //Create freq table.
                    outFreqTbl(freqTbl); //Output table to form.
                } else {
                    if (MessageBox.Show("Overwrite current Frequency Table?", "Frequency Table", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) { //Ask if user wants to overwrite current frequency table.
                        txtCompressed.Text = "";
                        txtDecompressed.Text = "";
                        createFreqTbl(txtPlain.Text, freqTbl); //Create freq table.
                        outFreqTbl(freqTbl); //Output table to form.
                    }
                }
            }
        }

        private void btnCompress_Click(object sender, RoutedEventArgs e)
        {
            List<SymbolFreq> freqTbl = new List<SymbolFreq>(); //Contains the symbol frequency table.
            List<HuffLeafNode> leafNodePtrs = new List<HuffLeafNode>(); //Contains references to the leaf objects.
            DAABitArray binaryStream = new DAABitArray();
            bool noSymbol;

            if (String.IsNullOrEmpty(txtPlain.Text)) { //Check if text to compress.
                MessageBox.Show("No text to compress.");
            } else {
                if (String.IsNullOrEmpty(txtFreqTbl.Text)) { //Check if a frequency table loaded.
                    if (MessageBox.Show("No Frequency table loaded, Generate?", "Frequency Table", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK) { //Ask user if they want a frequency table generated.
                        txtCompressed.Text = "";
                        txtDecompressed.Text = "";
                        createFreqTbl(txtPlain.Text, freqTbl); //Create freq table.
                        outFreqTbl(freqTbl); //Output table to form.
                        buildHuff(freqTbl, leafNodePtrs); //Create Huffman tree.
                        noSymbol = buildStream(binaryStream, txtPlain.Text, leafNodePtrs); //Build the full binary stream.
                        if (noSymbol == false){ //If a symbol(s) are found that don't exist, don't continue.
                            txtCompressed.Text = compress(binaryStream); //Compress.
                        }
                    }
                } else {
                    txtCompressed.Text = "";
                    txtDecompressed.Text = "";
                    loadFreqTbl(txtFreqTbl.Text, freqTbl); //Create freq table. Updates it if it has been modified.
                    outFreqTbl(freqTbl); //Output table to form.
                    buildHuff(freqTbl, leafNodePtrs); //Create Huffman tree.
                    noSymbol = buildStream(binaryStream, txtPlain.Text, leafNodePtrs); //Build the full binary stream.
                    if (noSymbol == false) { //If a symbol(s) are found that doesn't exist, don't continue.
                        txtCompressed.Text = compress(binaryStream); //Compress.
                    }
                }
            }
        }

        private void btnDecompress_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtCompressed.Text)) { //Check if there is text to decompress.
                MessageBox.Show("No text to decompress.");
            } else {
                if (String.IsNullOrEmpty(txtFreqTbl.Text)) { //Check if there is a frequency table.
                    MessageBox.Show("No frequency table loaded.");
                } else {
                    List<SymbolFreq> freqTbl = new List<SymbolFreq>(); //Contains the symbol frequency table.
                    List<HuffLeafNode> leafNodePtrs = new List<HuffLeafNode>(); //Contains references to the leaf objects.
                    loadFreqTbl(txtFreqTbl.Text, freqTbl); //Create freq table.
                    outFreqTbl(freqTbl); //Output table to form.
                    buildHuff(freqTbl, leafNodePtrs); //Create Huffman tree.
                    txtDecompressed.Text = decompress(txtCompressed.Text, leafNodePtrs); //Decompress text and diplay results.
                }
            }
        }

        //Build the Huffman tree.
        private void buildHuff(List<SymbolFreq> freqTbl, List<HuffLeafNode> leafNodePtrs)
        {
            List<HuffTreeNode> huffTree = new List<HuffTreeNode>(); //Contains the nodes that will be combined into the Hoffman tree.
            HuffTreeNode min1, min2, newNode;
            HuffLeafNode tempNode;

            foreach (SymbolFreq SF in freqTbl) //Load the two Lists with leaf nodes.
            {
                tempNode = new HuffLeafNode(SF.getFreq(), SF.getSymbol());
                huffTree.Add(tempNode);
                leafNodePtrs.Add(tempNode);
            }
            while (huffTree.Count > 1) //Combine nodes until only root node left.
            {
                min1 = huffTree[0];
                min2 = huffTree[1];
                if (huffTree.Count != 2) { //if only 2 nodes left, proceed to combine stage.
                    for (int i = 2; i < huffTree.Count; i++) { //Determine smallest in two elements array.
                        if ((huffTree[i].getFreq() < min1.getFreq()) && (huffTree[i].getFreq() < min2.getFreq())) { //Current node frequency smaller than both min1 & min2.
                            if (min1.getFreq() <= min2.getFreq()) { //Find smallest of min1 & min2. If they are equal, replace min1.
                                min1 = huffTree[i];
                            } else {
                                min2 = huffTree[i];
                            }
                        } else if (huffTree[i].getFreq() < min1.getFreq()) { //if current node frequency smaller than min1 then set current node as min1.
                            min1 = huffTree[i];
                        }
                        else if (huffTree[i].getFreq() < min2.getFreq()) { //if current node frequency smaller than min2 then set current node as min2.
                            min2 = huffTree[i];
                        }
                    }
                }
                newNode = new HuffTreeNode(min1.getFreq() + min2.getFreq(), min1, min2); //create new non-symbol node and update the Huffman tree.
                min1.setParent(newNode); //Update parent references.
                min2.setParent(newNode);
                huffTree.Add(newNode); //Add new node to tree.
                huffTree.Remove(min1); //Remove combines nodes from tree.
                huffTree.Remove(min2);
            }
            calcHuffCode(leafNodePtrs); //Calculate the huffman code for each leaf node.
        }

        //Runs from each leaf node to root to determine the Huffman code.
        private void calcHuffCode(List<HuffLeafNode> leafNodePtrs)
        {
            HuffTreeNode currNode;
            HuffLeafNode leafNode;
            if (leafNodePtrs.Count == 1) { //Only one entry in huffman tree.
                leafNodePtrs[0].huffCode.Append(0);
            } else {
                foreach (HuffLeafNode HN in leafNodePtrs) {
                    currNode = leafNode = HN;
                    while (currNode.getParent() != null) { //Move up through huffman tree.
                        if (currNode.Equals(currNode.getParent().getLeft())) { //Determine wether to append a 0 or 1.
                            leafNode.huffCode.Append(0);
                        } else {
                            leafNode.huffCode.Append(1);
                        }
                        currNode = currNode.getParent();
                    }
                    leafNode.huffCode.Reverse(); //Huffman code developed backwards, so reverse it.
                }
            }
        }

        //Builds a DAABitArray with the Huffman tree from the entered text.
        private bool buildStream(DAABitArray binaryStream, string input, List<HuffLeafNode> leafNodePtrs)
        {
            bool found, noSymbol, outputOnce;
            found = noSymbol = outputOnce = false;
            foreach (char ch in input) //Iterate through each symbol in entered text.
            {
                for (int i = 0; i < leafNodePtrs.Count; i++)
                {
                    if (leafNodePtrs[i].getSymbol() == ch) { //Find the symbol that matches the char.
                        for (int ii = 0; ii < leafNodePtrs[i].huffCode.NumBits; ii++) //Add the huffman code to the binary stream one bit at a time.
                        {
                            binaryStream.Append(leafNodePtrs[i].huffCode.GetBitAsBool(ii));
                        }
                        found = true;
                    }
                }
                if (found == false && outputOnce == false && ch != '\r') //Test if symbol rxidtd in the frequency table.
                {
                    MessageBox.Show("Symbol '" + ch + "' does not exist. Please check data input.");
                    noSymbol = true;
                    outputOnce = true; //Used to only output the above error message once.
                }
                found = false;
            }
            return noSymbol;
        }

        //Converts binary stream to alphanumeric cipher.
        private string compress(DAABitArray binaryStream)
        {
            int mod;
            CompressDepress compressor = new CompressDepress(); //Object to manage depression/compression.
            mod = binaryStream.NumBits % 6; //Test if divisible by 6.
            if (mod != 0) { //Pad bit stream with 0's until a multiple of 6.
                for (int i = 0; i < 6-mod; i++)
                {
                    binaryStream.Append(0);
                }
            }
            return compressor.compress(binaryStream);
        }

        //Converts alphanumeric cipher to binary to original text.
        private string decompress(String compressedText, List<HuffLeafNode> leafNodePtrs)
        {
            CompressDepress deCompressor = new CompressDepress(); //Object to manage depression/compression.
            DAABitArray deCompStream = deCompressor.decompressCipher(compressedText); //Convert alphanumeric compressed text to binary stream.
            string deCompString = deCompressor.decompressStream(deCompStream, leafNodePtrs); //Convert binary stream to text through Huffman tree.
            return deCompString;
        }

        //Creates an optimal frequency table from the text entered.
        private void createFreqTbl(string contents, List<SymbolFreq> freqTbl)
        {
            bool exists = false; //If exists = false, add the symbol to the list.
            foreach (char ch in contents) //Iterate through each symbol in entered text.
            {
                foreach (SymbolFreq SF in freqTbl) //Test if the char exists in list.
                {
                    if (ch == SF.symbol)
                    {
                        SF.addFreq(); //Plus 1.
                        exists = true;
                    }
                }
                if ((exists == false) && (ch != '\r')) //Newline character is \n\r (2 chars), so skip \r when counting.
                {
                    freqTbl.Add(new SymbolFreq(ch, 1)); //Add symbol to list.
                }
                else
                {
                    exists = false;
                }
            }
        }

        //Load the freq table into a List from form.
        private void loadFreqTbl(string contents, List<SymbolFreq> freqTbl)
        {
            string[] line = contents.Split('\n'); //Create array of lines.
            string[] words;
            foreach (string ln in line){
                words = ln.Split(':');
                if (words.Length == 2)
                {
                    if ((isDouble(words[1]) == true) && (words[0] != "")) //If in valid format, add to list.
                    {
                        freqTbl.Add(new SymbolFreq(words[0][0],Convert.ToDouble(words[1])));
                    }
                    else if ((isDouble(words[1]) == true) && (words[0] == "")) //newline character is handled seperately as its value is made up of 2 chars.
                    {
                        freqTbl.Add(new SymbolFreq('\n', Convert.ToDouble(words[1])));
                    }
                }
            }
         }

        //Outputs the optimum frequency table to form.
        private void outFreqTbl(List<SymbolFreq> freqTbl)
        {
            string tempString = "";
            foreach (SymbolFreq SF in freqTbl){ //Create string containing table data.
                tempString = (tempString + SF.symbol + ":" + SF.freq + "\n");
            }
            tempString = tempString.Remove(tempString.Length - 1); //Remove last end line character.
            txtFreqTbl.Text = tempString;
        }

        //Test that the frequency is a valid int/double.
        private bool isDouble(string inString)
        {
            double result;
            bool isDbl = false;
            try
            {
                result = Convert.ToDouble(inString); //If conversion works, return true.
                isDbl = true;
            }
            catch (FormatException){}
            catch (OverflowException){}
            return isDbl;
        }

        //Button to clear text fields.
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtPlain.Text = "";
            txtFreqTbl.Text = "";
            txtCompressed.Text = "";
            txtDecompressed.Text = "";
        }
    }
}