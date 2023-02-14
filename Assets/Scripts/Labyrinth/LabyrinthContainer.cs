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

    public int StoryCount
    {
        get { return mLabyrinthStories; }
    }

    private int mLabyrinthRows;
    private int mLabyrinthColumns;
    private int mLabyrinthStories;
    private LabyrinthCell[,,] mLabyrinth;
    
    // constructor to make rows and columns non-zero
    // and instantiate a new LabyrinthCell at that coordinate point (rank & range)

    public LabyrinthContainer(int rows, int columns, int stories)
    {
        mLabyrinthRows = Mathf.Abs(rows);
        mLabyrinthColumns = Mathf.Abs(columns);
        mLabyrinthStories = Mathf.Abs(stories);
        
        if (mLabyrinthRows == 0)
        {
            mLabyrinthRows = 1;
        }
        if (mLabyrinthColumns == 0)
        {
            mLabyrinthColumns = 1;
        }
        if (mLabyrinthStories == 0)
        {
            mLabyrinthStories = 1;
        }

        mLabyrinth = new LabyrinthCell[rows, columns, stories];

        for (int story = 0; story < stories; story++)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    mLabyrinth[row, column, story] = new LabyrinthCell();
                }
            }
        }
    }
    
    // called by the algorithm class to start the algorithm

    public abstract void GenerateLabyrinth();

    public LabyrinthCell GetLabyrinthCell(int row, int column, int story)
    {
        if (row >= 0 && column >= 0 && story >= 0 &&
            row < mLabyrinthRows && column < mLabyrinthColumns && story < mLabyrinthStories)
        {
            return mLabyrinth[row, column, story];
        }

        Debug.Log(row + " " + column + " " + story);
        throw new System.ArgumentOutOfRangeException();
    }
}
