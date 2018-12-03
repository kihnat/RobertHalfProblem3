using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertHalfProblem3
{
    class Program
    {
        /*
         My Sudoku skills are quite limited.  The obvious approach is to start brute forcing numbers in and then make it check the solution.
         This seems really inelegant and computationally expensive. 
         I made the program to follow the steps that I follow when solving a Sudoku, and then make a series of reasonably educated guesses and checking those guesses.
         It's still brute forcing it, but it should provide a solution faster then straight brute force.
         Given the time constraints of this challenge I am not going to learn how to be a master of sudoku techniques and then program them.
         I realize that the brute force is a complete solution on its own, but I think this shows off greater coding skill.
         I'm sure it is possible to program enough Sudoku techniques to not need any brute force, but I am not 

        */

        private List<SquareOptions> OptionsList
        {
            get; set;
        }

        static void Main(string[] args)
        {
            SolvePuzzle(Test5);
        }

        private static void SolvePuzzle(List<List<string>> input)
        {
            var optionsList = ConvertToSquareOptions(input);
            var puzzle = SolvePuzzleLogically(optionsList);


            var unsolvedSquares = puzzle.Where(x => x.Value == ".").Count();
            if (unsolvedSquares != 0)
            {
                puzzle = SolvePuzzleWithBruteForce(puzzle);

                unsolvedSquares = puzzle.Where(x => x.Value == ".").Count();
                if (unsolvedSquares != 0)
                {
                    Console.WriteLine("I failed to solve the puzzle completely.  This is what I have.");
                    Console.WriteLine("My programming is insufficient or a solution is not possible.");
                }

            }
            if (CheckBoardState(puzzle))
            {
                ShowResult(puzzle);
            }
            else
            {
                Console.WriteLine("Oops, something went wrong");
            }

        }

        private static List<SquareOptions> SolvePuzzleLogically(List<SquareOptions> optionsList)
        {
            var ret = optionsList;
            bool progressBeingMade = true;

            while (progressBeingMade)
            {
                progressBeingMade = false;

                var solvedCells = ret.Where(x => x.Value != ".").ToList();

                //Update cell options based on what is in their row/column/box
                foreach (var cell in solvedCells)
                {
                    string solvednumber = cell.Value;

                    var matchingX = ret.Where(x => x.XCoordinate == cell.XCoordinate && x.YCoordinate != cell.YCoordinate);
                    foreach (var xMatchedCell in matchingX)
                    {
                        xMatchedCell.Options.Remove(solvednumber);
                    }

                    var matchingY = ret.Where(x => x.YCoordinate == cell.YCoordinate && x.XCoordinate != cell.XCoordinate);
                    foreach (var yMatchedCell in matchingY)
                    {
                        yMatchedCell.Options.Remove(solvednumber);
                    }

                    var matchingZ = ret.Where(x => x.BoxNumber == cell.BoxNumber && (x.YCoordinate != cell.YCoordinate || x.XCoordinate != cell.XCoordinate));
                    foreach (var boxMatchedCell in matchingZ)
                    {
                        boxMatchedCell.Options.Remove(solvednumber);
                    }
                }

                //Update Cell Options based on each row needing something of every value
                for (int i = 0; i < 9; i++)
                {
                    var rowCells = ret.Where(x => x.XCoordinate == i).ToList();
                    SearchForOnlyOptionInGroup(rowCells);
                }

                //Update Cell Options based on each column needing something of every value
                for (int i = 0; i < 9; i++)
                {
                    var columnCells = ret.Where(x => x.YCoordinate == i).ToList();
                    SearchForOnlyOptionInGroup(columnCells);
                }

                //Update Cell Options based on each box needing something of every value
                for (int i = 1; i <= 9; i++)
                {
                    var boxCells = ret.Where(x => x.BoxNumber == i).ToList();
                    SearchForOnlyOptionInGroup(boxCells);
                }

                //Fill in the solved cells
                var solvableCells = ret.Where(x => x.Value == "." && x.Options.Count() == 1).ToList();
                foreach (var cell in solvableCells)
                {
                    var cellValue = cell.Options.First();
                    cell.Value = cellValue;
                    progressBeingMade = true;
                }
            }
            return ret;

        }

        private static List<SquareOptions> SolvePuzzleWithBruteForce(List<SquareOptions> puzzle)
        {
            //Make a list of guesses.  That's empty cells paired with a valid option from their options list

            //Pick a guess.  Make that guess.

            //Run that guess through the logic.

            //Either it will fill in the bord completely or it won't. 

            //Check the answer. 

            //If it is incomplete, hop down another level and make another guess
            bool isCorrect = CheckBoardState(puzzle);
            return puzzle;
        }

        private static bool CheckBoardState(List<SquareOptions> puzzle)
        {
            var ret = true;
            //Check Rows
            for (int i = 0; i < 9; i++)
            {
                var rowCells = puzzle.Where(x => x.XCoordinate == i).ToList();
                ret = ret && CheckSubsetForDuplicates(rowCells);
            }

            //Check Columns
            for (int i = 0; i < 9; i++)
            {
                var columnCells = puzzle.Where(x => x.YCoordinate == i).ToList();
                ret = ret && CheckSubsetForDuplicates(columnCells);
            }

            //Check Boxes
            for (int i = 0; i < 9; i++)
            {
                var boxCells = puzzle.Where(x => x.BoxNumber == i).ToList();
                ret = ret && CheckSubsetForDuplicates(boxCells);
            }

            //Check Options
            var unfillableCells = puzzle.Where(x => x.Options.Count() == 0).Count();
            ret = ret && unfillableCells == 0;

            return ret;
        }

        private static bool CheckSubsetForDuplicates(List<SquareOptions> set)
        {
            for (int j = 1; j <= 9; j++)
            {
                var searchValue = j.ToString();
                var results = set.Where(x => x.Value == searchValue);
                if (results.Count() > 1)
                {
                    return false;
                }
            }

            return true;
        }

        private static void SearchForOnlyOptionInGroup(List<SquareOptions> cellGroup)
        {
            for (int j = 1; j <= 9; j++)
            {
                var searchValue = j.ToString();
                if (cellGroup.Where(x => x.Value == searchValue).Count() == 0)
                {
                    var possibleCells = cellGroup.Where(x => x.Options.Contains(searchValue));
                    if (possibleCells.Count() == 1)
                    {
                        var value = new List<string> { searchValue };
                        possibleCells.First().Options = value;
                    }
                }
            }
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

        public static void ShowResult(List<SquareOptions> result)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    var element = result.Where(i => i.XCoordinate == x && i.YCoordinate == y).First().Value;
                    Console.Write(element + "\t");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        private static List<List<string>> Test1
        {
            get
            {
                //The input type is string to allow for the placeholder
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
                //Easy Level Puzzle
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
                //Medium Level Puzzle
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
                //Hard Level Puzzle
                List<List<string>> input = new List<List<string>>(new List<string>[9] {
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", "3", "8" }),
                    new List<string>(new string[9] {"2", ".", ".",        ".", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {"5", ".", "7",        ".", ".", ".",      "1", "4", "."}),

                    new List<string>(new string[9] {".", ".", "8",        "9", "4", ".",      ".", "6", "."}),
                    new List<string>(new string[9] {".", ".", ".",        "2", "3", ".",      "8", ".", "."}),
                    new List<string>(new string[9] {"7", ".", ".",        ".", ".", ".",      "9", ".", "."}),

                    new List<string>(new string[9] {".", "5", ".",        ".", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", "5",      "3", ".", "."}),
                    new List<string>(new string[9] {"9", ".", ".",        ".", ".", "1",      "6", ".", "2"})});
                return input;
            }
        }

        private static List<List<string>> Test5
        {
            get
            {
                //This is left blank to make it easier to insert new puzzles
                List<List<string>> input = new List<List<string>>(new List<string>[9] {
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      "6", "8", "." }),
                    new List<string>(new string[9] {".", ".", ".",        ".", "7", "3",      ".", ".", "9"}),
                    new List<string>(new string[9] {"3", ".", "9",        ".", ".", ".",      ".", "4", "5"}),

                    new List<string>(new string[9] {"4", "9", ".",        ".", ".", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {"8", ".", "3",        ".", "5", ".",      "9", ".", "2"}),
                    new List<string>(new string[9] {".", ".", ".",        ".", ".", ".",      ".", "3", "6"}),

                    new List<string>(new string[9] {"9", "6", ".",        ".", ".", ".",      "3", ".", "8"}),
                    new List<string>(new string[9] {"7", ".", ".",        "6", "8", ".",      ".", ".", "."}),
                    new List<string>(new string[9] {".", "2", "8",        ".", ".", ".",      ".", ".", "."})});
                return input;
            }
        }

        private static List<List<string>> BlankGrid
        {
            get
            {
                //This is left blank to make it easier to insert new puzzles
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
