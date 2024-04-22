using System.Diagnostics;

SudokuGeneration sudokuGeneration = new();
Stopwatch stopwatchFullDuration = new();

int solutionsFound = 0;

int currentPercent = 0;
for (int i = 0; i < 1000000; i++) {
    stopwatchFullDuration.Start();
    if (i%10000 == 0){
        currentPercent++;
        Console.WriteLine($"Loading...{currentPercent}%");
        double elapsedSeconds = Math.Round(stopwatchFullDuration.Elapsed.TotalSeconds, 2);
        Console.WriteLine($"Total Solutions Found: {solutionsFound} in {elapsedSeconds}");
    }
    if (sudokuGeneration.SudokuGenerator()){
        solutionsFound ++;
    }
}


Console.ReadKey();

class SudokuGeneration {
    string sudokuSolution = "";
    public bool SudokuGenerator() {
        char[,] sudoku = GenerateRandomSudoku();
        string sudokuHolder = "";
        foreach (var element in sudoku) {
            if (element == '1' || element == '2' || element == '3' || element == '4' || element == '5' || element == '6'
                || element == '7' || element == '8' || element == '9') {
                sudokuHolder += element;
            }
            else {
                sudokuHolder += '.';
            }
        }

        // Solve the Sudoku puzzle
        if (SolveSudoku(sudoku)) {
            string fullSolution = "";
            // Combines the puzzle and solution into one string, separated by a comma
            fullSolution += sudokuHolder;
            fullSolution += ',';
            fullSolution += sudokuSolution;

            using (StreamWriter writer = new StreamWriter("ValidSudokus.txt", true)){   // True adds more lines to the file
                writer.WriteLine(fullSolution);
            }

            Console.WriteLine("\nSolved Sudoku Puzzle:");
            return true;
        }
        else {
            return false;
        }
    }

    static char[,] GenerateRandomSudoku() {
        // Create a blank Sudoku grid
        char[,] sudoku = new char[9, 9];

        // Generate a random initial configuration
        Random random = new Random();
        for (int row = 0; row < 9; row++) {
            for (int col = 0; col < 9; col++) {
                // Randomly fill 1/3 of the grid
                if (random.Next(0, 2) == 0) {
                    // Generate a random valid number for the current cell
                    char randomNumber = GetValidRandomNumber(sudoku, row, col, random);
                    sudoku[row, col] = randomNumber;
                }
            }
        }
        return sudoku;
    }

    static char GetValidRandomNumber(char[,] sudoku, int row, int col, Random random) {
        List<char> validNumbers = new List<char>("123456789");

        // Remove numbers already used in the same row and column
        for (int i = 0; i < 9; i++) {
            validNumbers.Remove(sudoku[row, i]);
            validNumbers.Remove(sudoku[i, col]);
        }

        // Remove numbers already used in the same 3x3 subgrid
        int startRow = row - row % 3;
        int startCol = col - col % 3;
        for (int i = startRow; i < startRow + 3; i++) {
            for (int j = startCol; j < startCol + 3; j++) {
                validNumbers.Remove(sudoku[i, j]);
            }
        }

        // Select a random valid number from the remaining list
        int index = random.Next(0, validNumbers.Count);
        if (validNumbers.Count > 0){
            return validNumbers[index];
        }
        try {
            return validNumbers[index];
        }
        catch (ArgumentOutOfRangeException) {
            //Console.WriteLine($"{e}: No valid combinations remaining");
        }
        return '_';

    }
    public bool SolveSudoku(char[,] sudoku) {
        // Create a list to store empty cells
        List<(int, int)> emptyCells = new List<(int, int)>();

        // Find empty cells and add them to the list
        for (int row = 0; row < 9; row++) {
            for (int col = 0; col < 9; col++) {
                if (sudoku[row, col] == '\0') {
                    emptyCells.Add((row, col));
                }
            }
        }

        // Initialize the solution count
        int solutionCount = 0;

        // Solve the Sudoku puzzle recursively
        SolveSudokuRecursive(sudoku, emptyCells, 0, ref solutionCount);

        // Return true if exactly one solution is found
        return solutionCount == 1;
    }

    public void SolveSudokuRecursive(char[,] sudoku, List<(int, int)> emptyCells, int index, ref int solutionCount) {
        // Base case: All empty cells are filled
        if (index == emptyCells.Count) {
            // Increment the solution count
            sudokuSolution = "";
            foreach (var element in sudoku){
                sudokuSolution += element;
            }
            solutionCount++;
            return;
        }

        // Get the row and column of the current empty cell
        int row = emptyCells[index].Item1;
        int col = emptyCells[index].Item2;

        // Try each digit from '1' to '9'
        for (char digit = '1'; digit <= '9'; digit++) {
            // Check if the current digit is valid
            if (IsValidMove(sudoku, row, col, digit)) {
                // Assign the digit to the current cell
                sudoku[row, col] = digit;

                // Recursively solve the Sudoku puzzle
                SolveSudokuRecursive(sudoku, emptyCells, index + 1, ref solutionCount);

                // Stop the search if more than one solution is found
                if (solutionCount > 1) {
                    return;
                }

                // Backtrack: Undo the current move
                sudoku[row, col] = '\0';
            }
        }
    }

    static bool IsValidMove(char[,] sudoku, int row, int col, char digit) {
        // Check row and column
        for (int i = 0; i < 9; i++) {
            if (sudoku[row, i] == digit || sudoku[i, col] == digit) {
                return false;
            }
        }

        // Check 3x3 subgrid
        int startRow = row - row % 3;
        int startCol = col - col % 3;
        for (int i = startRow; i < startRow + 3; i++) {
            for (int j = startCol; j < startCol + 3; j++) {
                if (sudoku[i, j] == digit) {
                    return false;
                }
            }
        }

        return true;
    }

    static void PrintSudoku(char[,] sudoku) {
        for (int row = 0; row < 9; row++) {
            for (int col = 0; col < 9; col++) {
                Console.Write(sudoku[row, col] + " ");
            }
            Console.WriteLine();
        }
    }
}