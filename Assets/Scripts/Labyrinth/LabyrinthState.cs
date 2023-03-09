
public static class LabyrinthState
{
    public static int rows { get; set; }
    public static int columns { get; set; }
    public static int stories { get; set; }
    
    public static int pathLength { get; set; }
    
    public static LabyrinthCell[,,] LabyrinthMap;

    public static LabyrinthCell GetLabyrinthCellMap(int row, int column, int story)
    {
        if (row >= 0 && column >= 0 && story >= 0 &&
            row < rows && column < columns && story < stories)
        {
            return LabyrinthMap[row, column, story];
        }

        throw new System.ArgumentOutOfRangeException();
    }
}