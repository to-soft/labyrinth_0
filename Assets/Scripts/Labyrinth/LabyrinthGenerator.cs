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
        VisitCell(0, 0, 0, Direction.Start);
    }

    private void VisitCell(int row, int column, int story, Direction moveMade)
    {
        Direction[] movesAvailable = new Direction[6];
        int movesAvailableCount = 0;

        do
        {
            movesAvailableCount = 0;

            // check right
            if (column + 1 < ColumnCount && !GetLabyrinthCell(row, column + 1, story).IsVisited)
            {
                movesAvailable[movesAvailableCount] = Direction.Right;
                movesAvailableCount++;
            } 
            else if (!GetLabyrinthCell(row, column, story).IsVisited && moveMade != Direction.Left)
            {
                GetLabyrinthCell(row, column, story).WallRight = true;
            }

            // check forward
            if (row + 1 < RowCount && !GetLabyrinthCell(row + 1, column, story).IsVisited)
            {
                movesAvailable[movesAvailableCount] = Direction.Front;
                movesAvailableCount++;
            }
            else if (!GetLabyrinthCell(row, column, story).IsVisited && moveMade != Direction.Back)
            {
                GetLabyrinthCell(row, column, story).WallFront = true;
            }
            
            // check left
            if (column >= 1 && !GetLabyrinthCell(row, column - 1, story).IsVisited)
            {
                movesAvailable[movesAvailableCount] = Direction.Left;
                movesAvailableCount++;
            }
            else if (!GetLabyrinthCell(row, column, story).IsVisited && moveMade != Direction.Right)
            {
                GetLabyrinthCell(row, column, story).WallLeft = true;
            }
            
            // check backward
            if (row >= 1 && !GetLabyrinthCell(row - 1, column, story).IsVisited)
            {
                movesAvailable[movesAvailableCount] = Direction.Back;
                movesAvailableCount++;
            }
            else if (!GetLabyrinthCell(row, column, story).IsVisited && moveMade != Direction.Front)
            {
                GetLabyrinthCell(row, column, story).WallBack = true;
            }
            
            // check up
            if (story + 1 < StoryCount && !GetLabyrinthCell(row, column, story + 1).IsVisited)
            {
                movesAvailable[movesAvailableCount] = Direction.Up;
                movesAvailableCount++;
            }
            else if (!GetLabyrinthCell(row, column, story).IsVisited && moveMade != Direction.Down)
            {
                GetLabyrinthCell(row, column, story).Ceiling = true;
            }
            
            // check down
            if (story >= 1 && !GetLabyrinthCell(row, column, story - 1).IsVisited)
            {
                movesAvailable[movesAvailableCount] = Direction.Down;
                movesAvailableCount++;
            }
            else if (!GetLabyrinthCell(row, column, story).IsVisited && moveMade != Direction.Up)
            {
                GetLabyrinthCell(row, column, story).Floor = true;
            }

            GetLabyrinthCell(row, column, story).IsVisited = true;

            if (movesAvailableCount > 0)
            {
                switch (movesAvailable[Random.Range(0, movesAvailableCount)])
                {
                    case Direction.Start:
                        break;
                    case Direction.Right:
                        VisitCell(row, column + 1, story, Direction.Right);
                        break;
                    case Direction.Front:
                        VisitCell(row + 1, column, story, Direction.Front);
                        break;
                    case Direction.Left:
                        VisitCell(row, column - 1, story, Direction.Left);
                        break;
                    case Direction.Back:
                        VisitCell(row - 1, column, story, Direction.Back);
                        break;
                    case Direction.Up:
                        VisitCell(row, column, story + 1, Direction.Up);
                        break;
                    case Direction.Down:
                        VisitCell(row, column, story - 1, Direction.Down);
                        break;
                    
                }
            }

        } 
        while (movesAvailableCount > 0);
        
    }
}