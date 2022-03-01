using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteSoftTest
{
    public class Replacement
    {
        public string replacement { get; set; }
        public string source { get; set; }

        public override string ToString()
        {
            return string.Format("\nreplacement: {0}, \nsource: {1}", replacement, source);
        }
    }
}
