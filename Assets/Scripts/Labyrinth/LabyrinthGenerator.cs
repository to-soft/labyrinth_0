using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PossibleDirection
{
    Start,
    Right,
    Front,
    Left,
    Back,
    Down,
    Up,
}

public class LabyrinthGenerator : LabyrinthContainer
{
    public LabyrinthGenerator(int rows, int columns, int stories) : base(rows, columns, stories)
    {
        
    }

    public LabyrinthCell EscapeCell;
    public int furthestDistance = 0;
    public int prevCellDistance = 0;
    public int startingColumn;

    public override void GenerateLabyrinth()
    {
        startingColumn = (int)Math.Ceiling(ColumnCount / 2f) - 1;
        VisitCell(0, startingColumn, 0, 
            Direction.Start, GetLabyrinthCell(0, startingColumn, 0), Direction.Base);
        Debug.Log($"Furthest cell: {furthestDistance}");
        StudyLabyrinth();
        RepairLabyrinth();
        SolveLabyrinth();
        CartographLabyrinth();
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

    private void CartographLabyrinth()
    {
        for (int story = 0; story < StoryCount; story++)
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int column = 0; column < ColumnCount; column++)
                {
                    LabyrinthState.LabyrinthMap[row, column, story] = GetLabyrinthCell(row, column, story);
                    Debug.Log($"new labyrinth state count: {LabyrinthState.LabyrinthMap.Length}");
                }
            }
        }
    }
    
    private void Backtrack(int row, int column, int story, int prevDistance, Direction prevMove)
    {
        
        if (story + 1 > StoryCount || story < 0
            || column + 1 > ColumnCount || column < 0 
            || row + 1 > RowCount || row < 0)
        {
            Debug.Log("SolveLabyrinth: Out of bounds error");
            return;
        }
        
        LabyrinthCell cell = GetLabyrinthCell(row, column, story);
        if (cell.IsVisited) { return; }
        cell.IsVisited = true;
        
        int distanceFromGoal = prevDistance + 1;
        cell.DistanceFromGoal = distanceFromGoal;
        if (!HasWall(cell, Direction.Right) && prevMove != Direction.Left)
        {
            Backtrack(row, column + 1, story, distanceFromGoal, Direction.Right);
        }

        if (!HasWall(cell, Direction.Left) && prevMove != Direction.Right)
        {
            Backtrack(row, column - 1, story, distanceFromGoal, Direction.Left);
        }

        if (!HasWall(cell, Direction.Front) && prevMove != Direction.Back)
        {
            Backtrack(row + 1, column, story, distanceFromGoal, Direction.Front);
        }

        if (!HasWall(cell, Direction.Back) && prevMove != Direction.Front)
        {
            Backtrack(row - 1, column, story, distanceFromGoal, Direction.Back);
        }

        if (!HasWall(cell, Direction.Up) && prevMove != Direction.Down)
        {
            Backtrack(row, column, story + 1, distanceFromGoal, Direction.Up);
        }

        if (!HasWall(cell, Direction.Down) && prevMove != Direction.Up)
        {
            Backtrack(row, column, story - 1, distanceFromGoal, Direction.Down);
        }
    }

    private void SolveLabyrinth()
    {
        Debug.Log("SolveLabyrinth: Beginning backtracker...");
        Debug.Log($"SolveLabyrinth: Escape cell: " +
                  $"row {EscapeCell.Row} col {EscapeCell.Column} story {EscapeCell.Story}");
        Backtrack(EscapeCell.Row, EscapeCell.Column, EscapeCell.Story, -1, Direction.Start);
        LabyrinthCell start = GetLabyrinthCell(0, startingColumn, 0);
        Debug.Log($"SolveLabyrinth done: Estimated length from start to finish: {start.DistanceFromGoal}");
    }

    private bool HasWall(LabyrinthCell cell, Direction direction)
    {
        switch (direction)
        {
            case Direction.Front:
                if (cell.WallFront) { return true; }
                if (cell.Row + 1 >= RowCount) { return true; }
                return GetLabyrinthCell(cell.Row + 1, cell.Column, cell.Story).WallBack;
            case Direction.Back:
                if (cell.Door) { return true; }
                if (cell.WallBack) { return true; }
                if (cell.Row <= 0) { return true; }
                return GetLabyrinthCell(cell.Row - 1, cell.Column, cell.Story).WallFront;
            case Direction.Right:
                if (cell.WallRight) { return true;}
                if (cell.Column + 1 >= ColumnCount) { return true; }
                return GetLabyrinthCell(cell.Row, cell.Column + 1, cell.Story).WallLeft;
            case Direction.Left:
                if (cell.WallLeft) { return true;}
                if (cell.Column <= 0) { return true; }
                return GetLabyrinthCell(cell.Row, cell.Column - 1, cell.Story).WallRight;
            case Direction.Up:
                if (cell.Ceiling) { return true;}
                if (cell.Story + 1 >= StoryCount) { return true; }
                return GetLabyrinthCell(cell.Row, cell.Column, cell.Story + 1).Floor;
            case Direction.Down:
                if (cell.Floor) { return true;}
                if (cell.Story <= 0) { return true; }
                return GetLabyrinthCell(cell.Row, cell.Column, cell.Story - 1).Ceiling;
            default:
                return true;
        }
    }
    private bool HasAdjacentWall(LabyrinthCell cell, Direction direction)
    {
        switch (direction)
        {
            case Direction.Front:
                if (cell.Row + 1 >= RowCount) { return false; }
                return GetLabyrinthCell(cell.Row + 1, cell.Column, cell.Story).WallBack;
            case Direction.Back:
                if (cell.Row <= 0) { return false; }
                return GetLabyrinthCell(cell.Row - 1, cell.Column, cell.Story).WallFront;
            case Direction.Right:
                if (cell.Column + 1 >= ColumnCount) { return false; }
                return GetLabyrinthCell(cell.Row, cell.Column + 1, cell.Story).WallLeft;
            case Direction.Left:
                if (cell.Column <= 0) { return false; }
                return GetLabyrinthCell(cell.Row, cell.Column - 1, cell.Story).WallRight;
            case Direction.Up:
                if (cell.Story + 1 >= StoryCount) { return false; }
                return GetLabyrinthCell(cell.Row, cell.Column, cell.Story + 1).Floor;
            case Direction.Down:
                if (cell.Story <= 0) { return false; }
                return GetLabyrinthCell(cell.Row, cell.Column, cell.Story - 1).Ceiling;
            default:
                return false;
        }
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
                    
                    // fill in unvisited cells
                    if (!cell.IsVisited)
                    {
                        Debug.Log($"filling in cell: x{column} y{story} z{row}");
                        cell.WallFront = cell.WallRight =
                            cell.WallBack = cell.WallLeft = cell.Ceiling = cell.Floor = true;
                    }
                    else
                    {
                        cell.IsVisited = false;
                    }
                        
                    // remove overlapping walls between neighbor cells
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

        LabyrinthCell goal = GetLabyrinthCell(EscapeCell.Row, EscapeCell.Column, EscapeCell.Story);
        goal.IsGoal = true;
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

    private void VisitCell(int row, int column, int story, Direction moveMade, 
        LabyrinthCell prevCell = null, Direction prevMoveMade = Direction.Base)
    {
        // Debug.Log("visiting cell: x" + column + ", y" + story + ", z" + row + "\n moved: " + moveMade);
        LabyrinthCell thisCell = GetLabyrinthCell(row, column, story);
        // Debug.Log("is visited: " + thisCell.IsVisited);
        
        int currentDistance = prevCellDistance + 1;
        if (currentDistance > furthestDistance)
        {
            // Debug.Log($"new furthest distance: {currentDistance}");
            furthestDistance = currentDistance;
            EscapeCell = thisCell;
        }
        
        int movesAvailableCount = 0;
        Direction[] movesAvailable = new Direction[6];
        
        thisCell.Floor = story == 0;
        if (moveMade == Direction.Start)
        {
            thisCell.Door = true;
            thisCell.WallBack = false;
        }
        
        bool createRamp = moveMade is Direction.Up or Direction.Down && prevCell is not null;

        if (createRamp)
        {
            // has to go same direction as last iteration's moveMade (this iteration's prevModeMade)
            // prevCell.Floor = story == 0;
            thisCell.IsVisited = true;
            prevCellDistance = currentDistance;
            switch (prevMoveMade)
            {
                case Direction.Start:
                    break;
                case Direction.Right:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampRight = prevCell.WallBack = prevCell.WallFront = prevCell.WallRight = prevCell.Floor =
                            thisCell.WallBack = thisCell.WallLeft = thisCell.WallFront = thisCell.Ceiling = true;
                        // Debug.Log("Moving (up and) right...");
                    }
                    else
                    {
                        prevCell.WallFront = prevCell.WallRight = prevCell.WallBack = prevCell.Ceiling = thisCell.Floor =
                            thisCell.WallLeft = thisCell.WallBack = thisCell.WallFront = thisCell.RampLeft = true;
                        // Debug.Log("Moving (down and) right...");
                    }
                    VisitCell(row, column + 1, story, Direction.Right, thisCell, moveMade);
                    break;
                case Direction.Front:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampFront = prevCell.WallLeft = prevCell.WallRight = prevCell.WallFront = prevCell.Floor =
                            thisCell.WallBack = thisCell.WallLeft = thisCell.WallRight = thisCell.Ceiling = true;
                        // Debug.Log("Moving (up and) forward...");
                    }
                    else
                    {
                        prevCell.WallFront = prevCell.WallRight = prevCell.WallLeft = prevCell.Ceiling = thisCell.Floor =
                            thisCell.WallBack = thisCell.WallLeft = thisCell.WallRight = thisCell.RampBack = true;
                        // Debug.Log("Moving (down and) forward...");
                    }
                    VisitCell(row + 1, column, story, Direction.Front, thisCell, moveMade);
                    break;
                case Direction.Left:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampLeft = prevCell.WallFront = prevCell.WallBack = prevCell.WallLeft = prevCell.Floor =
                            thisCell.WallBack = thisCell.WallFront = thisCell.WallRight = thisCell.Ceiling = true;
                        // Debug.Log("Moving (up and) left...");
                    }
                    else
                    {
                        prevCell.WallFront = prevCell.WallBack = prevCell.WallLeft = prevCell.Ceiling = thisCell.Floor =
                            thisCell.WallRight = thisCell.WallBack = thisCell.WallFront = thisCell.RampRight = true;
                        // Debug.Log("Moving (down and) left...");
                    }
                    VisitCell(row, column - 1, story, Direction.Left, thisCell, moveMade);
                    break;
                case Direction.Back:
                    if (moveMade == Direction.Up)
                    {
                        prevCell.RampBack = prevCell.WallLeft = prevCell.WallRight = prevCell.WallBack = prevCell.Floor =
                            thisCell.WallLeft = thisCell.WallRight = thisCell.WallFront = thisCell.Ceiling = true;
                    // Debug.Log("Moving (up and) backward...");
                    }
                    else
                    {
                        prevCell.WallRight = prevCell.WallBack = prevCell.WallLeft = prevCell.Ceiling = thisCell.Floor =
                            thisCell.WallFront = thisCell.WallLeft = thisCell.WallRight = thisCell.RampFront = true;
                        // Debug.Log("Moving (down and) backward...");
                    }
                    VisitCell(row - 1, column, story, Direction.Back, thisCell, moveMade);
                    break;
            }
        }
        else
        {
            bool overrideMovement = false;
            bool overrideVerticalMovement = false;
            do
            {
                // Debug.Log($"Current distance is: {currentDistance}");
                movesAvailableCount = 0;
                if (overrideMovement) { return; }

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
                else if (!thisCell.IsVisited && moveMade != Direction.Front && moveMade != Direction.Start)
                {
                    thisCell.WallBack = true;
                }

                if (!overrideVerticalMovement)
                {
                    // check up
                    if (CanAscendOrDescend(row, column, story, moveMade, Direction.Up))
                    {
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
                        movesAvailable[movesAvailableCount] = Direction.Down;
                        movesAvailableCount++;
                    }
                    else if ((!thisCell.IsVisited && moveMade != Direction.Up) || story == 0)
                    {
                        thisCell.Floor = true;
                    }
                }
                else
                {
                    thisCell.Floor = thisCell.Ceiling = true;
                }
            
                thisCell.IsVisited = true;
                
                // Debug.Log("moves available: " + movesAvailableCount);

                if (movesAvailableCount > 0)
                {
                    prevCellDistance = currentDistance;
                    switch (movesAvailable[Random.Range(0, movesAvailableCount)])
                    {
                        case Direction.Start:
                            break;
                        case Direction.Right:
                            // Debug.Log("Moving right...");
                            overrideVerticalMovement = true;
                            VisitCell(row, column + 1, story, Direction.Right, thisCell, moveMade);
                            break;
                        case Direction.Front:
                            // Debug.Log("Moving forward...");
                            overrideVerticalMovement = true;
                            VisitCell(row + 1, column, story, Direction.Front, thisCell, moveMade);
                            break;
                        case Direction.Left:
                            // Debug.Log("Moving left...");
                            overrideVerticalMovement = true;
                            VisitCell(row, column - 1, story, Direction.Left, thisCell, moveMade);
                            break;
                        case Direction.Back:
                            // Debug.Log("Moving backward...");
                            overrideVerticalMovement = true;
                            VisitCell(row - 1, column, story, Direction.Back, thisCell, moveMade);
                            break;
                        case Direction.Up:
                            // Debug.Log("Moving up...");
                            overrideMovement = true;
                            VisitCell(row, column, story + 1, Direction.Up, thisCell, moveMade);
                            break;
                        case Direction.Down:
                            // Debug.Log("Moving down...");
                            overrideMovement = true;
                            VisitCell(row, column, story - 1, Direction.Down, thisCell, moveMade);
                            break;
                    }
                }
            } while (movesAvailableCount > 0);
        }
    }
}