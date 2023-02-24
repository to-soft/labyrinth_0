using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class LabyrinthGenerator : LabyrinthContainer
{
    public LabyrinthGenerator(int rows, int columns, int stories) : base(rows, columns, stories)
    {
        
    }

    public override void GenerateLabyrinth()
    {
        VisitCell(0, 0, 0, 
            Direction.Start, GetLabyrinthCell(0, 0, 0), Direction.Base);
        OpenLabyrinth();
        StudyLabyrinth();
        RepairLabyrinth();
    }

    private void StudyLabyrinth()
    {
        Debug.Log($"\n###LABYRINTH RESEARCH###\n");
        int cellCount = RowCount * ColumnCount * StoryCount;
        Debug.Log($"Total cells: {cellCount}");
        
        int forgottenCellCount = 0;
        LabyrinthCell[] forgottenCells = new LabyrinthCell[cellCount];
        for (int story = 0; story < StoryCount; story++)
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int column = 0; column < ColumnCount; column++)
                {
                    LabyrinthCell cellInQuestion = GetLabyrinthCell(row, column, story);
                    if (!cellInQuestion.IsVisited)
                    {
                        forgottenCells[forgottenCellCount] = cellInQuestion;
                        forgottenCellCount++;
                    }
                }
            }
        }
        Debug.Log($"Forgotten cell count: {forgottenCellCount}");
        Debug.Log($"Forgotten cells: {forgottenCellCount}");
        Debug.Log($"\n###END RESEARCH###\n");
    }

    private void OpenLabyrinth()
    {
        GetLabyrinthCell(0, 0, 0).Door = true;
        GetLabyrinthCell(0, 0, 0).WallBack = false;
    }

    private void RepairLabyrinth()
    {
        for (int story = 0; story < StoryCount; story++)
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int column = 0; column < ColumnCount; column++)
                {
                    LabyrinthCell cell = GetLabyrinthCell(row, column, story);
                    if (!cell.IsVisited)
                    {
                        Debug.Log($"filling in cell: x{column} y{story} z{row}");
                        cell.WallFront = cell.WallRight =
                            cell.WallBack = cell.WallLeft = cell.Ceiling = cell.Floor = true;
                    }
                        
                    if (cell.WallFront && row + 1 < RowCount) 
                    {
                        GetLabyrinthCell(row + 1, column, story).WallBack = false;
                    }
                    if (cell.WallRight && column + 1 < ColumnCount) 
                    {
                        GetLabyrinthCell(row, column + 1, story).WallLeft = false;
                    }

                    if (cell.Ceiling && story + 1 < StoryCount)
                    {
                        LabyrinthCell above = GetLabyrinthCell(row, column, story + 1);
                        above.Floor = false;
                        if (above.RampFront || above.RampBack || above.RampRight || above.RampLeft)
                        {
                            cell.Ceiling = false;
                        }
                    }
                }
            }
        }
    }

    private bool CanAscendOrDescend(int row, int column, int story, Direction direction, Direction directionVertical)
    {
        if (direction is Direction.Up or Direction.Down or Direction.Start) { return false; }
        // Debug.Log($"checking move: {direction} and {directionVertical}");
        // Debug.Log($"starting coordinates: story {story} row {row} column: {column}");
        
        int targetStory = story + (directionVertical == Direction.Up ? 1 : -1);
        int targetColumn = column + (direction == Direction.Right ? 1 : direction == Direction.Left ? -1 : 0);
        int targetRow = row + (direction == Direction.Front ? 1 : direction == Direction.Back ? -1 : 0);
        // Debug.Log($"target coordinates:\nstory {targetStory}\nrow {targetRow}\ncolumn: {targetColumn}");
        
        if (targetStory < 0 || targetStory == StoryCount) { return false; }
        if (targetColumn < 0 || targetColumn == ColumnCount) { return false; }
        if (targetRow < 0 || targetRow == RowCount) { return false; }
        
        return !GetLabyrinthCell(targetRow, targetColumn, targetStory).IsVisited 
                && !GetLabyrinthCell(row, column, targetStory).IsVisited;
    }

    private void VisitCell(int row, int column, int story, 
        Direction moveMade, LabyrinthCell prevCell = null, Direction prevMoveMade = Direction.Base)
    {
        Debug.Log("this cell is x" + column + ", y" + story + ", z" + row + "\n moved: " + moveMade);
        int movesAvailableCount = 0;
        bool createRamp = moveMade is Direction.Up or Direction.Down && prevCell is not null;
        LabyrinthCell thisCell = GetLabyrinthCell(row, column, story);
        Debug.Log("is visited: " + thisCell.IsVisited);
        Direction[] movesAvailable = new Direction[6];
        
        thisCell.Floor = story == 0;
        if (createRamp)
        {
            // has to go same direction as last iteration's moveMade (this iteration's prevModeMade)
            // prevCell.Floor = story == 0;
            thisCell.IsVisited = true;
            switch (prevMoveMade)
            {
                case Direction.Start:
                    break;
                case Direction.Right:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampRight = prevCell.WallBack = prevCell.WallFront = prevCell.WallRight =
                            thisCell.WallBack = thisCell.WallLeft = thisCell.WallFront = thisCell.Ceiling = true;
                        Debug.Log($"Making ceiling at: s{story} r{row} c{column} line 173");
                        Debug.Log("Moving (up and) right...");
                    }
                    else
                    {
                        prevCell.WallFront = prevCell.WallRight = prevCell.WallBack = prevCell.Ceiling = 
                            thisCell.WallLeft = thisCell.WallBack = thisCell.WallFront = thisCell.RampLeft = true;
                        Debug.Log($"Making ceiling at: s{story} r{row} c{column} line 180");
                        Debug.Log("Moving (down and) right...");
                    }
                    VisitCell(row, column + 1, story, Direction.Right, thisCell, moveMade);
                    break;
                case Direction.Front:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampFront = prevCell.WallLeft = prevCell.WallRight = prevCell.WallFront = 
                            thisCell.WallBack = thisCell.WallLeft = thisCell.WallRight = thisCell.Ceiling = true;
                        Debug.Log($"Making ceiling at: s{story} r{row} c{column} line 190");
                        Debug.Log("Moving (up and) forward...");
                    }
                    else
                    {
                        prevCell.WallFront = prevCell.WallRight = prevCell.WallLeft = prevCell.Ceiling =
                            thisCell.WallBack = thisCell.WallLeft = thisCell.WallRight = thisCell.RampBack = true;
                        Debug.Log($"Making ceiling at: s{story} r{row} c{column} line 197");
                        Debug.Log("Moving (down and) forward...");
                    }
                    VisitCell(row + 1, column, story, Direction.Front, thisCell, moveMade);
                    break;
                case Direction.Left:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampLeft = prevCell.WallFront = prevCell.WallBack = prevCell.WallLeft = 
                            thisCell.WallBack = thisCell.WallFront = thisCell.WallRight = thisCell.Ceiling = true;
                        Debug.Log($"Making ceiling at: s{story} r{row} c{column} line 207");
                        Debug.Log("Moving (up and) left...");
                    }
                    else
                    {
                        prevCell.WallFront = prevCell.WallBack = prevCell.WallLeft = prevCell.Ceiling = 
                            thisCell.WallRight = thisCell.WallBack = thisCell.WallFront = thisCell.RampRight = true;
                        Debug.Log($"Making ceiling at: s{story} r{row} c{column} line 214");
                        Debug.Log("Moving (down and) left...");
                    }
                    VisitCell(row, column - 1, story, Direction.Left, thisCell, moveMade);
                    break;
                case Direction.Back:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampBack = prevCell.WallLeft = prevCell.WallRight = prevCell.WallBack = 
                            thisCell.WallLeft = thisCell.WallRight = thisCell.WallFront = thisCell.Ceiling = true;
                        Debug.Log($"Making ceiling at: s{story} r{row} c{column} line 224");
                    Debug.Log("Moving (up and) backward...");
                    }
                    else
                    {
                        prevCell.WallRight = prevCell.WallBack = prevCell.WallLeft = prevCell.Ceiling =
                            thisCell.WallFront = thisCell.WallLeft = thisCell.WallRight = thisCell.RampFront = true;
                        Debug.Log($"Making ceiling at: s{story} r{row} c{column} line 231");
                        Debug.Log("Moving (down and) backward...");
                    }
                    VisitCell(row - 1, column, story, Direction.Back, thisCell, moveMade);
                    break;
            }
        }
        else
        {
            // made sole move... (moved up or down, can't move anywhere else, return to prev in stack...
            // moved l/r/f/b, can't move up or down but can move l/r/f/b
            bool overrideMovement = false;
            bool overrideVerticalMovement = false;
            do
            {
                movesAvailableCount = 0;
                if (overrideMovement)
                {
                    return;
                }

                // check if we are at the edge of the grid, and that we haven't been here yet
                if (column + 1 < ColumnCount && !GetLabyrinthCell(row, column + 1, story).IsVisited)
                {
                    movesAvailable[movesAvailableCount] = Direction.Right;
                    movesAvailableCount++;
                }
                // if we are at the edge of the grid, and ensure we don't create a loop
                else if (!thisCell.IsVisited && moveMade != Direction.Left)
                {
                    thisCell.WallRight = true;
                }

                // check forward
                if (row + 1 < RowCount && !GetLabyrinthCell(row + 1, column, story).IsVisited)
                {
                    movesAvailable[movesAvailableCount] = Direction.Front;
                    movesAvailableCount++;
                }
                else if (!thisCell.IsVisited && moveMade != Direction.Back)
                {
                    thisCell.WallFront = true;
                }

                // check left
                if (column >= 1 && !GetLabyrinthCell(row, column - 1, story).IsVisited)
                {
                    movesAvailable[movesAvailableCount] = Direction.Left;
                    movesAvailableCount++;
                }
                else if (!thisCell.IsVisited && moveMade != Direction.Right)
                {
                    thisCell.WallLeft = true;
                }

                // check backward
                if (row >= 1 && !GetLabyrinthCell(row - 1, column, story).IsVisited)
                {
                    movesAvailable[movesAvailableCount] = Direction.Back;
                    movesAvailableCount++;
                }
                else if (!thisCell.IsVisited && moveMade != Direction.Front)
                {
                    thisCell.WallBack = true;
                }

                if (!overrideVerticalMovement)
                {
                    // check up
                    if (CanAscendOrDescend(row, column, story, moveMade, Direction.Up))
                    {
                        // Debug.Log("can ascend...");
                        movesAvailable[movesAvailableCount] = Direction.Up;
                        movesAvailableCount++;
                    }
                    else if (!thisCell.IsVisited && moveMade != Direction.Down)
                    {
                        Debug.Log($"Making ceiling at: s{story} r{row} c{column} line 308");
                        thisCell.Ceiling = true;
                    }

                    // check down
                    if (CanAscendOrDescend(row, column, story, moveMade, Direction.Down))
                    {
                        // Debug.Log("can descend...");
                        movesAvailable[movesAvailableCount] = Direction.Down;
                        movesAvailableCount++;
                    }
                    else if ((!thisCell.IsVisited && moveMade != Direction.Up) || story == 0)
                    {
                        Debug.Log($"Creating floor at: story {story} row {row} column {column}");
                        thisCell.Floor = true;
                    }
                }
            
                thisCell.IsVisited = true;
                
                // Debug.Log("moves available: " + movesAvailableCount);

                if (movesAvailableCount > 0)
                {
                    switch (movesAvailable[Random.Range(0, movesAvailableCount)])
                    {
                        case Direction.Start:
                            break;
                        case Direction.Right:
                            Debug.Log("Moving right...");
                            overrideVerticalMovement = true;
                            VisitCell(row, column + 1, story, Direction.Right, thisCell, moveMade);
                            break;
                        case Direction.Front:
                            Debug.Log("Moving forward...");
                            overrideVerticalMovement = true;
                            VisitCell(row + 1, column, story, Direction.Front, thisCell, moveMade);
                            break;
                        case Direction.Left:
                            Debug.Log("Moving left...");
                            overrideVerticalMovement = true;
                            VisitCell(row, column - 1, story, Direction.Left, thisCell, moveMade);
                            break;
                        case Direction.Back:
                            Debug.Log("Moving backward...");
                            overrideVerticalMovement = true;
                            VisitCell(row - 1, column, story, Direction.Back, thisCell, moveMade);
                            break;
                        case Direction.Up:
                            Debug.Log("Moving up...");
                            overrideMovement = true;
                            VisitCell(row, column, story + 1, Direction.Up, thisCell, moveMade);
                            break;
                        case Direction.Down:
                            Debug.Log("Moving down...");
                            overrideMovement = true;
                            VisitCell(row, column, story - 1, Direction.Down, thisCell, moveMade);
                            break;
                    }
                }
            } while (movesAvailableCount > 0);
        }
    }
}