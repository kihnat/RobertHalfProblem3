using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertHalfProblem3
{
    class Program
    {
        //
        private static List<List<string>> Result
        {
            get;
            set;
        }

        private List<SquareOptions> OptionsList
        {
            get; set;
        }

        static void Main(string[] args)
        {
            SolvePuzzle(Test4);
        }

        private static void SolvePuzzle(List<List<string>> input)
        {
            Result = input;
            var optionsList = ConvertToSquareOptions(input);
            bool progressBeingMade = true;

            while (progressBeingMade)
            {
                progressBeingMade = false;

                var solvedCells = optionsList.Where(x => x.Value != ".").ToList();

                //Update cell options based on what is in their row/column/box
                foreach (var cell in solvedCells)
                {
                    string solvednumber = cell.Value;
                    var matchingX = optionsList.Where(x => x.XCoordinate == cell.XCoordinate && x.YCoordinate != cell.YCoordinate);
                    var matchingY = optionsList.Where(x => x.YCoordinate == cell.YCoordinate && x.XCoordinate != cell.XCoordinate);
                    var matchingZ = optionsList.Where(x => x.BoxNumber == cell.BoxNumber && (x.YCoordinate != cell.YCoordinate || x.XCoordinate != cell.XCoordinate));
                    foreach (var xMatchedCell in matchingX)
                    {
                        xMatchedCell.Options.Remove(solvednumber);
                    }
                    foreach (var yMatchedCell in matchingY)
                    {
                        yMatchedCell.Options.Remove(solvednumber);
                    }
                    foreach (var boxMatchedCell in matchingZ)
                    {
                        boxMatchedCell.Options.Remove(solvednumber);
                    }
                }

                //Update Cell Options based on each row needing something of every value
                for (int i = 0; i < 9; i++)
                {
                    var rowCells = optionsList.Where(x => x.XCoordinate == i);
                    for (int j = 1; j <= 9; j++)
                    {
                        var searchValue = j.ToString();
                        if (rowCells.Where(x => x.Value == searchValue).Count() == 0)
                        {
                            var possibleCells = rowCells.Where(x => x.Options.Contains(searchValue));
                            if (possibleCells.Count() == 1)
                            {
                                var value = new List<string> { searchValue };
                                possibleCells.First().Options = value;
                            }
                        }
                    }
                }

                //Update Cell Options based on each column needing something of every value
                for (int i = 0; i < 9; i++)
                {
                    var columnCells = optionsList.Where(x => x.YCoordinate == i);
                    for (int j = 1; j <= 9; j++)
                    {
                        var searchValue = j.ToString();
                        if (columnCells.Where(x => x.Value == searchValue).Count() == 0)
                        {
                            var possibleCells = columnCells.Where(x => x.Options.Contains(searchValue));
                            if (possibleCells.Count() == 1)
                            {
                                var value = new List<string> { searchValue };
                                possibleCells.First().Options = value;
                            }
                        }
                    }
                }

                //Update Cell Options based on each box needing something of every value
                for (int i = 1; i <= 9; i++)
                {
                    var boxCells = optionsList.Where(x => x.BoxNumber == i);
                    for (int j = 1; j <= 9; j++)
                    {
                        var searchValue = j.ToString();
                        if (boxCells.Where(x => x.Value == searchValue).Count() == 0)
                        {
                            var possibleCells = boxCells.Where(x => x.Options.Contains(searchValue));
                            if (possibleCells.Count() == 1)
                            {
                                var value = new List<string> { searchValue };
                                possibleCells.First().Options = value;
                            }
                        }
                    }
                }

                //Fill in the solved cells
                var solvableCells = optionsList.Where(x => x.Value == "." && x.Options.Count() == 1).ToList();
                foreach (var cell in solvableCells)
                {
                    var cellValue = cell.Options.First();
                    cell.Value = cellValue;
                    Result[cell.XCoordinate][cell.YCoordinate] = cellValue;
                    progressBeingMade = true;
                }
            }

            ShowResult();
        }

        private static List<SquareOptions> ConvertToSquareOptions(List<List<string>> input)
        {
            var ret = new List<SquareOptions> { };
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    int z = GetBoxFromCoordinates(x, y);
                    var createdSquareOption = new SquareOptions(input[x][y], x, y, z);
                    ret.Add(createdSquareOption);
                }
            }
            return ret;
        }

        private static int GetBoxFromCoordinates(int x, int y)
        {
            if (x < 3 && y < 3)
            {
                return 1;
            }
            else if (x < 3 && 3 <= y && y < 6)
            {
                return 2;
            }
            else if (x < 3 && 6 <= y)
            {
                return 3;
            }
            else if (3 <= x && x < 6 && y < 3)
            {
                return 4;
            }
            else if (3 <= x && x < 6 && 3 <= y && y < 6)
            {
                return 5;
            }
            else if (3 <= x && x < 6 && 6 <= y)
            {
                return 6;
            }
            else if (6 <= x && y < 3)
            {
                return 7;
            }
            else if (6 <= x && 3 <= y && y < 6)
            {
                return 8;
            }
            else if (6 <= x && 6 <= y)
            {
                return 9;
            }
            else
            {
                return 0;
            }
        }

        public static void ShowResult()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    Console.Write((Result.ElementAt(x)).ElementAt(y) + "\t");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        private static List<List<string>> Test1
        {
            get
            {
                //The input type is string to allow for Hex Values
                List<List<string>> input = new List<List<string>>(new List<string>[9] {
                    new List<string>(new string[9] { "8", ".", ".",       "9", "3", ".",      ".", ".", "2" }),
                    new List<string>(new string[9] {".", ".", "9",        ".", ".", ".",      ".", "4", "."}),
                    new List<string>(new string[9] {"7", ".", "2",        "1", ".", ".",      "9", "6", "."}),

                    new List<string>(new string[9] {"2", ".", ".",        ".", ".", ".",      ".", "9", "."}),
                    new List<string>(new string[9] {".", "6", ".",        ".", ".", ".",      ".", "7", "."}),
                    new List<string>(new string[9] {".", "7", ".",        ".", ".", "6",      ".", ".", "5"}),

                    new List<string>(new string[9] {".", "2", "7",        ".", ".", "8",      "4", ".", "6"}),
                    new List<string>(new string[9] {".", "3", ".",        ".", ".", ".",      "5", ".", "."}),
                    new List<string>(new string[9] {"5", ".", ".",        ".", "6", "2",      ".", ".", "8"})});
                return input;
            }
        }

        private static List<List<string>> Test2
        {
            get
            {
                //The input type is string to allow for Hex Values
                List<List<string>> input = new List<List<string>>(new List<string>[9] {
                    new List<string>(new string[9] {"5", ".", ".",        "6", ".", ".",      "2", ".", "." }),
                    new List<string>(new string[9] {".", "8", ".",        ".", ".", "7",      ".", "1", "3"}),
                    new List<string>(new string[9] {".", "9", "6",        ".", ".", ".",      "7", ".", "."}),

                    new List<string>(new string[9] {"6", ".", "4",        ".", ".", "1",      ".", "2", "5"}),
                    new List<string>(new string[9] {".", ".", ".",        ".", "4", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {"1", ".", ".",        ".", ".", ".",      "4", ".", "."}),

                    new List<string>(new string[9] {"2", ".", "9",        ".", "3", ".",      "1", ".", "."}),
                    new List<string>(new string[9] {"7", ".", "5",        "9", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {".", "6", ".",        "1", ".", ".",      ".", ".", "."})});
                return input;
            }
        }

        private static List<List<string>> Test3
        {
            get
            {
                //The input type is string to allow for Hex Values
                List<List<string>> input = new List<List<string>>(new List<string>[9] {
                    new List<string>(new string[9] {"3", "1", "6",        "4", "2", ".",      ".", ".", "7" }),
                    new List<string>(new string[9] {".", "2", ".",        "1", ".", ".",      "9", ".", "."}),
                    new List<string>(new string[9] {"8", "4", "9",        ".", ".", "6",      "1", ".", "."}),

                    new List<string>(new string[9] {".", ".", ".",        ".", "9", ".",      ".", ".", "5"}),
                    new List<string>(new string[9] {".", "5", ".",        "2", ".", "4",      ".", ".", "."}),
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      "4", "1", "9"}),

                    new List<string>(new string[9] {".", ".", ".",        "7", "1", ".",      ".", ".", "2"}),
                    new List<string>(new string[9] {".", ".", ".",        ".", "6", ".",      "7", ".", "8"}),
                    new List<string>(new string[9] {"6", "9", ".",        "8", ".", ".",      "5", ".", "."})});
                return input;
            }
        }

        private static List<List<string>> Test4
        {
            get
            {
                //The input type is string to allow for Hex Values
                List<List<string>> input = new List<List<string>>(new List<string>[9] {
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", "3", "8" }),
                    new List<string>(new string[9] {"2", ".", ".",        ".", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {"5", ".", "7",        ".", ".", ".",      "1", "4", "."}),

                    new List<string>(new string[9] {".", ".", "8",        "9", "4", ".",      ".", "6", "."}),
                    new List<string>(new string[9] {".", ".", ".",        "2", "3", ".",      "8", ".", "."}),
                    new List<string>(new string[9] {"7", ".", ".",        ".", ".", ".",      "9", ".", "."}),

                    new List<string>(new string[9] {".", "5", ".",        ".", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", "5",      "3.", ".", "."}),
                    new List<string>(new string[9] {"9", ".", ".",        ".", ".", "1",      "6", ".", "2"})});
                return input;
            }
        }

        private static List<List<string>> BlankGrid
        {
            get
            {
                //The input type is string to allow for Hex Values
                List<List<string>> input = new List<List<string>>(new List<string>[9] {
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", ".", "." }),
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", ".", "."}),

                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", ".", "."}),

                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", ".", "."})});
                return input;
            }
        }
    }
}
