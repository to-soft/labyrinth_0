using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LabyrinthGenerator : LabyrinthContainer
{
    public LabyrinthGenerator(int rows, int columns) : base(rows, columns)
    {
        
    }

    public override void GenerateLabyrinth()
    {
        VisitCell(0, 0, Direction.Start);
    }

    private void VisitCell(int row, int column, Direction moveMade)
    {
        Direction[] movesAvailable = new Direction[4];
        int movesAvailableCount = 0;

        do
        {
            movesAvailableCount = 0;

            // check right
            if (column + 1 < ColumnCount && !GetLabyrinthCell(row, column + 1).IsVisited)
            {
                movesAvailable[movesAvailableCount] = Direction.Right;
                movesAvailableCount++;
            } 
            else if (!GetLabyrinthCell(row, column).IsVisited && moveMade != Direction.Left)
            {
                GetLabyrinthCell(row, column).WallRight = true;
            }

            // check forward
            if (row + 1 < RowCount && !GetLabyrinthCell(row + 1, column).IsVisited)
            {
                movesAvailable[movesAvailableCount] = Direction.Front;
                movesAvailableCount++;
            }
            else if (!GetLabyrinthCell(row, column).IsVisited && moveMade != Direction.Back)
            {
                GetLabyrinthCell(row, column).WallFront = true;
            }
            
            // check left
            if (column >= 1 && !GetLabyrinthCell(row, column - 1).IsVisited)
            {
                movesAvailable[movesAvailableCount] = Direction.Left;
                movesAvailableCount++;
            }
            else if (!GetLabyrinthCell(row, column).IsVisited && moveMade != Direction.Right)
            {
                GetLabyrinthCell(row, column).WallLeft = true;
            }
            
            // check backward
            if (row >= 1 && !GetLabyrinthCell(row - 1, column).IsVisited)
            {
                movesAvailable[movesAvailableCount] = Direction.Back;
                movesAvailableCount++;
            }
            else if (!GetLabyrinthCell(row, column).IsVisited && moveMade != Direction.Front)
            {
                GetLabyrinthCell(row, column).WallBack = true;
            }

            GetLabyrinthCell(row, column).IsVisited = true;

            if (movesAvailableCount > 0)
            {
                switch (movesAvailable[Random.Range(0, movesAvailableCount)])
                {
                    case Direction.Start:
                        break;
                    case Direction.Right:
                        VisitCell(row, column + 1, Direction.Right);
                        break;
                    case Direction.Front:
                        VisitCell(row + 1, column, Direction.Front);
                        break;
                    case Direction.Left:
                        VisitCell(row, column - 1, Direction.Left);
                        break;
                    case Direction.Back:
                        VisitCell(row - 1, column, Direction.Back);
                        break;
                }
            }

        } 
        while (movesAvailableCount > 0);
        
    }
}