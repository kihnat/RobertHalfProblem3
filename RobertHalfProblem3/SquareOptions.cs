using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertHalfProblem3
{
    class SquareOptions
    {
        private static List<string> InputOptions = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public string Value { get; set; }

        public List<string> Options { get; set; }

        public int XCoordinate { get; }

        public int YCoordinate { get; }

        public int BoxNumber { get; }

        public SquareOptions(int x, int y, int z)
        {
            this.Options = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            this.Value = ".";
            this.XCoordinate = x;
            this.YCoordinate = y;
            this.BoxNumber = z;
        }

        public SquareOptions(string value, int x, int y, int z)
        {
            if (InputOptions.Contains(value))
            {
                this.Options = new List<string> { value };
                this.Value = value;
                this.XCoordinate = x;
                this.YCoordinate = y;
                this.BoxNumber = z;
            }
            else
            {
                this.Options = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                this.Value = ".";
                this.XCoordinate = x;
                this.YCoordinate = y;
                this.BoxNumber = z;
            }
        }
    }
}
