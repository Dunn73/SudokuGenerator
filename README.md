# Sudoku Generator

## Description

* Creates a random valid sudoku board by generating a random number on each cell with a 1/3 chance.
* Checks the randomized sudoku board to see if there is exactly one valid solution.
* If there is exactly one valid solution, will write both the unsolved and solved puzzle to a text file.
* If there is no solution or more than one solution that iteration will end.

## Elements to Change

* Number of iterations
  * Currently set to 1 million, took over 30 minutes to run to completion, generating 124 valid sudokus
* Likelihood of a cell being filled with a number
  * Currently set to 1/3 chance. The lower the chance, the harder it will be to find a valid sudoku
   
