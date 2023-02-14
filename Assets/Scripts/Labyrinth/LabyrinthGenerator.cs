using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LabyrinthGenerator : LabyrinthContainer
{
    public LabyrinthGenerator(int rows, int columns, int stories) : base(rows, columns, stories)
    {
        
    }

    public override void GenerateLabyrinth()
    {
        VisitCell(0, 0, 0, 
            Direction.Start, GetLabyrinthCell(0, 0, 0), Direction.Start);
    }

    private bool CanAscend(int row, int column, Direction direction)
    {
        switch (direction)
        {
            case Direction.Front:
                return row + 1 < RowCount;
            case Direction.Back:
                return row > 0;
            case Direction.Right:
                return column + 1 < ColumnCount;
            case Direction.Left:
                return column > 0;
            default:
                return false;
        }
    }

    private void VisitCell(int row, int column, int story, 
        Direction moveMade, LabyrinthCell prevCell = null, Direction prevMoveMade = Direction.Base)
    {
        Direction[] movesAvailable = new Direction[6];
        int movesAvailableCount = 0;
        bool createRamp = moveMade == Direction.Up && prevCell is not null;
        LabyrinthCell thisCell = GetLabyrinthCell(row, column, story);

        if (createRamp)
        {
            // has to go same direction as last iteration's moveMade (this iteration's prevModeMade)
            prevCell.Floor = thisCell.Floor = false;
            thisCell.IsVisited = true;
            switch (prevMoveMade)
            {
                case Direction.Start:
                    break;
                case Direction.Right:
                    prevCell.RampRight = thisCell.Ceiling =
                        GetLabyrinthCell(row, column + 1, story).Floor =
                            thisCell.WallBack = thisCell.WallLeft = thisCell.WallFront = true;
                    VisitCell(row, column + 1, story, Direction.Right, thisCell, moveMade);
                    break;
                case Direction.Front:
                    prevCell.RampFront = thisCell.Ceiling = 
                        GetLabyrinthCell(row + 1, column, story).Floor =
                            thisCell.WallBack = thisCell.WallLeft = thisCell.WallRight = true;
                    VisitCell(row + 1, column, story, Direction.Front, thisCell, moveMade);
                    break;
                case Direction.Left:
                    prevCell.RampLeft = thisCell.Ceiling = 
                        GetLabyrinthCell(row, column - 1, story).Floor =
                            thisCell.WallBack = thisCell.WallFront = thisCell.WallRight = true;
                    VisitCell(row, column - 1, story, Direction.Left, thisCell, moveMade);
                    break;
                case Direction.Back:
                    prevCell.RampBack = thisCell.Ceiling = 
                        GetLabyrinthCell(row - 1, column, story).Floor =
                            thisCell.WallFront = thisCell.WallLeft = thisCell.WallRight = true;
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
                if (story + 1 < StoryCount
                    && !GetLabyrinthCell(row, column, story + 1).IsVisited
                    && CanAscend(row, column, moveMade))
                {
                    movesAvailable[movesAvailableCount] = Direction.Up;
                    movesAvailableCount++;
                }
                else if (!thisCell.IsVisited && moveMade != Direction.Down)
                {
                    thisCell.Ceiling = true;
                    if (story + 1 < StoryCount)
                    {
                        GetLabyrinthCell(row, column, story + 1).Floor = true;
                    }
                }

                // check down
                if (story >= 1 && !GetLabyrinthCell(row, column, story - 1).IsVisited)
                {
                    movesAvailable[movesAvailableCount] = Direction.Down;
                    movesAvailableCount++;
                }
                else if (!thisCell.IsVisited && moveMade != Direction.Up)
                {
                    thisCell.Floor = true;
                    if (story > 0)
                    {
                        GetLabyrinthCell(row, column, story - 1).Ceiling = true;
                    }
                }

                thisCell.IsVisited = true;

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
                            thisCell.Ceiling = false;
                            VisitCell(row, column, story + 1, Direction.Up, thisCell, moveMade);
                            break;
                        case Direction.Down:
                            VisitCell(row, column, story - 1, Direction.Down, thisCell, moveMade);
                            break;
                    }
                }
            } while (movesAvailableCount > 0);

            GetLabyrinthCell(0, 0, 0).WallBack = false;
        }
    }
}