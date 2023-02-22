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
        StudyLabyrinth();
        RepairLabyrinth();
    }

    private void StudyLabyrinth()
    {
        int cellCount = RowCount * ColumnCount * StoryCount;
        Debug.Log($"Total cells: {cellCount}");
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
        Debug.Log("checking can asc/desc...");
        if (direction is Direction.Up or Direction.Down) { return false; }
        
        Debug.Log($"attempting to move: {direction} and {directionVertical}");
        
        Debug.Log($"starting coordinates:\nstory {story}\nrow {row}\ncolumn: {column}");

        int targetStory = story + (directionVertical == Direction.Up ? 1 : -1);
        int targetColumn = column + (direction == Direction.Right ? 1 : direction == Direction.Left ? -1 : 0);
        int targetRow = row + (direction == Direction.Front ? 1 : direction == Direction.Back ? -1 : 0);
        Debug.Log($"target coordinates:\nstory {targetStory}\nrow {targetRow}\ncolumn: {targetColumn}");
        
        if (targetStory < 0 || targetStory == StoryCount) { return false; }
        if (targetColumn < 0 || targetColumn == ColumnCount) { return false; }
        if (targetRow < 0 || targetRow == RowCount) { return false; }
        
        return !GetLabyrinthCell(targetRow, targetColumn, targetStory).IsVisited 
                && !GetLabyrinthCell(row, column, targetStory).IsVisited;
        
        // bool neighborAtEdge = targetStory == 0 || targetStory + 1 == StoryCount;
        // if (row == 0 || row + 1 == RowCount || column == 0 || column + 1 == ColumnCount) { return false; }

        //check cells adjacent to target + 1 in horizontal direction
        // switch (direction)
        // {
        //     case Direction.Front:
        //         if (GetLabyrinthCell(row + 1, column + 1, neighborStory).IsVisited 
        //             || GetLabyrinthCell(row + 1, column - 1, neighborStory).IsVisited) { return false; }
        //
        //         if (!neighborAtEdge 
        //             && GetLabyrinthCell(row + 1, column, neighborStory + mappedDirection).IsVisited) { return false; }
        //         break;
        //     case Direction.Back:
        //         if (GetLabyrinthCell(row - 1, column + 1, neighborStory).IsVisited 
        //             || GetLabyrinthCell(row - 1, column - 1, neighborStory).IsVisited) { return false; }
        //         if (!neighborAtEdge 
        //             && GetLabyrinthCell(row - 1, column, neighborStory + mappedDirection).IsVisited) { return false; }
        //         break;
        //     case Direction.Right:
        //         if (GetLabyrinthCell(row + 1, column + 1, neighborStory).IsVisited 
        //             || GetLabyrinthCell(row - 1, column + 1, neighborStory).IsVisited) { return false; }
        //         if (!neighborAtEdge 
        //             && GetLabyrinthCell(row - 1, column, neighborStory + mappedDirection).IsVisited) { return false; }
        //         break;
        //     case Direction.Left:
        //         if (GetLabyrinthCell(row + 1, column - 1, neighborStory).IsVisited 
        //             || GetLabyrinthCell(row - 1, column - 1, neighborStory).IsVisited) { return false; }
        //         if (!neighborAtEdge 
        //             && GetLabyrinthCell(row - 1, column, neighborStory + mappedDirection).IsVisited) { return false; }
        //         break;
        // }Log

        //check cell above/below current and target cells
        // if (directionVertical == Direction.Up && targetStory + 1 < StoryCount)
        // {
        //     if (GetLabyrinthCell(row, column, targetStory + 1).IsVisited) { return false; }
        //
        //     if (story > 0)
        //     {
        //         if (GetLabyrinthCell(row, column, story - 1).IsVisited) { return false; }
        //     }
        // }
        // if (directionVertical == Direction.Down && story + 1 < StoryCount)
        // {
        //     if (GetLabyrinthCell(row, column, story + 1).IsVisited) { return false; }
        //
        //     if (targetStory > 0)
        //     {
        //         if (GetLabyrinthCell(row, column, targetStory - 1).IsVisited) { return false; }
        //     }   
        // }

        // return // adjacent cells
        //     (direction == Direction.Back || !GetLabyrinthCell(row + 1, column, story).IsVisited)
        //     && (direction == Direction.Front || !GetLabyrinthCell(row - 1, column, story).IsVisited)
        //     && (direction == Direction.Left || !GetLabyrinthCell(row, column + 1, story).IsVisited)
        //     && (direction == Direction.Right || !GetLabyrinthCell(row, column - 1, story).IsVisited)
        //     // adjacent cells of target cell
        //     && !GetLabyrinthCell(row + 1, column, targetStory).IsVisited
        //     && !GetLabyrinthCell(row - 1, column, targetStory).IsVisited
        //     && !GetLabyrinthCell(row, column + 1, targetStory).IsVisited
        //     && !GetLabyrinthCell(row, column - 1, targetStory).IsVisited;
    }

    private void VisitCell(int row, int column, int story, 
        Direction moveMade, LabyrinthCell prevCell = null, Direction prevMoveMade = Direction.Base)
    {
        Debug.Log("this cell is x" + column + ", y" + story + ", z" + row + "\n moved: " + moveMade);
        int movesAvailableCount = 0;
        bool createRamp = moveMade is Direction.Up or Direction.Down && prevCell is not null;
        LabyrinthCell thisCell = GetLabyrinthCell(row, column, story);
        Direction[] movesAvailable = new Direction[6];
        
        if (createRamp)
        {
            // has to go same direction as last iteration's moveMade (this iteration's prevModeMade)
            thisCell.Floor = prevCell.Floor = story == 0;
            thisCell.IsVisited = true;
            switch (prevMoveMade)
            {
                case Direction.Start:
                    break;
                case Direction.Right:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampRight = prevCell.WallBack = prevCell.WallFront =
                            thisCell.WallBack = thisCell.WallLeft = thisCell.WallFront = thisCell.Ceiling = true;
                        prevCell.WallRight = false;
                    }
                    else
                    {
                        prevCell.WallFront = prevCell.WallRight = prevCell.WallBack = prevCell.Ceiling =
                            thisCell.WallBack = thisCell.WallFront = thisCell.RampLeft = true;
                        thisCell.WallLeft = false;

                    }
                    VisitCell(row, column + 1, story, Direction.Right, thisCell, moveMade);
                    break;
                case Direction.Front:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampFront = prevCell.WallLeft = prevCell.WallRight =
                            thisCell.WallBack = thisCell.WallLeft = thisCell.WallRight = thisCell.Ceiling = true;
                        prevCell.WallFront = false;
                    }
                    else
                    {
                        prevCell.WallFront = prevCell.WallRight = prevCell.WallLeft = prevCell.Ceiling =
                            thisCell.WallLeft = thisCell.WallRight = thisCell.RampBack = true;
                        thisCell.WallBack = false;

                    }
                    VisitCell(row + 1, column, story, Direction.Front, thisCell, moveMade);
                    break;
                case Direction.Left:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampLeft = prevCell.WallFront = prevCell.WallBack =
                            thisCell.WallBack = thisCell.WallFront = thisCell.WallRight = thisCell.Ceiling = true;
                        prevCell.WallLeft = false;
                    }
                    else
                    {
                        prevCell.WallFront = prevCell.WallBack = prevCell.WallLeft = prevCell.Ceiling =
                            thisCell.WallBack = thisCell.WallFront = thisCell.RampRight = true;
                        thisCell.WallRight = false;

                    }
                    VisitCell(row, column - 1, story, Direction.Left, thisCell, moveMade);
                    break;
                case Direction.Back:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampBack = prevCell.WallLeft = prevCell.WallRight =
                            thisCell.WallLeft = thisCell.WallRight = thisCell.WallFront = thisCell.Ceiling = true;
                        prevCell.WallBack = false;
                    }
                    else
                    {
                        prevCell.WallRight = prevCell.WallBack = prevCell.WallLeft = prevCell.Ceiling =
                            thisCell.WallLeft = thisCell.WallRight = thisCell.RampFront = true;
                        thisCell.WallFront = false;

                    }
                    VisitCell(row - 1, column, story, Direction.Back, thisCell, moveMade);
                    break;
            }
        }
        else
        {
            do
            {
                movesAvailableCount = 0;

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

                // check up
                if (CanAscendOrDescend(row, column, story, moveMade, Direction.Up))
                {
                    Debug.Log("can ascend...");
                    movesAvailable[movesAvailableCount] = Direction.Up;
                    movesAvailableCount++;
                }
                else if (!thisCell.IsVisited && moveMade != Direction.Down)
                {
                    thisCell.Ceiling = true;
                }

                // check down
                if (CanAscendOrDescend(row, column, story, moveMade, Direction.Down))
                {
                    Debug.Log("can descend...");
                    movesAvailable[movesAvailableCount] = Direction.Down;
                    movesAvailableCount++;
                }
                else if ((!thisCell.IsVisited && moveMade != Direction.Up) || story == 0)
                {
                    thisCell.Floor = true;
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
                            VisitCell(row, column + 1, story, Direction.Right, thisCell, moveMade);
                            break;
                        case Direction.Front:
                            VisitCell(row + 1, column, story, Direction.Front, thisCell, moveMade);
                            break;
                        case Direction.Left:
                            VisitCell(row, column - 1, story, Direction.Left, thisCell, moveMade);
                            break;
                        case Direction.Back:
                            VisitCell(row - 1, column, story, Direction.Back, thisCell, moveMade);
                            break;
                        case Direction.Up:
                            VisitCell(row, column, story + 1, Direction.Up, thisCell, moveMade);
                            break;
                        case Direction.Down:
                            VisitCell(row, column, story - 1, Direction.Down, thisCell, moveMade);
                            break;
                    }
                }
            } while (movesAvailableCount > 0);

            GetLabyrinthCell(0, 0, 0).Door = true;
            GetLabyrinthCell(0, 0, 0).WallBack = false;
        }
    }
}