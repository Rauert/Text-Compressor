using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    public class SymbolFreq
    {
        public char symbol;
        public double freq;

        public SymbolFreq(char inSymbol, double inFreq)
        {
            symbol = inSymbol;
            freq = inFreq;
        }
        
        public char getSymbol()
        {
            return symbol;
        }
        
        public double getFreq()
        {
            return freq;
        }
        public void addFreq()
        {
            freq++;
        }
    }
}