using System;
using UnityEngine;
using System.Collections;


public abstract class LabyrinthContainer
{
    // obtain Row and Column from private variables
    public int RowCount
    {
        get { return mLabyrinthRows; }
    }

    public int ColumnCount
    {
        get { return mLabyrinthColumns; }
    }

    private int mLabyrinthRows;
    private int mLabyrinthColumns;
    private LabyrinthCell[,] mLabyrinth;
    
    // constructor to make rows and columns non-zero
    // and instantiate a new LabyrinthCell at that coordinate point (rank & range)

    public LabyrinthContainer(int rows, int columns)
    {
        mLabyrinthRows = Mathf.Abs(rows);
        mLabyrinthColumns = Mathf.Abs(columns);
        
        if (mLabyrinthRows == 0)
        {
            mLabyrinthRows = 1;
        }
        if (mLabyrinthColumns == 0)
        {
            mLabyrinthColumns = 1;
        }

        mLabyrinth = new LabyrinthCell[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                mLabyrinth[row, column] = new LabyrinthCell();
            }
        }
    }
    
    // called by the algorithm class to start the algorithm

    public abstract void GenerateLabyrinth();

    public LabyrinthCell GetLabyrinthCell(int row, int column)
    {
        if (row >= 0 && column >= 0 && row < mLabyrinthRows && column < mLabyrinthColumns)
        {
            return mLabyrinth[row, column];
        }
        else
        {
            Debug.Log(row + " " + column);
            throw new System.ArgumentOutOfRangeException();
        }
    }
}
