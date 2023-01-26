using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static bool MatchedAt(this Dot dot, Dot[,] dots, int column, int row)
    {
        if (column > 1 && row > 1)
        {
            if ((dots[column - 1, row].GetDotType == dot.GetDotType && dots[column - 2, row].GetDotType == dot.GetDotType) ||
                (dots[column, row - 1].GetDotType == dot.GetDotType && dots[column, row - 2].GetDotType == dot.GetDotType))
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1 && dots[column, row - 1].GetDotType == dot.GetDotType && dots[column, row - 2].GetDotType == dot.GetDotType)
            {
                return true;
            }
            if (column > 1 && dots[column - 1, row].GetDotType == dot.GetDotType && dots[column - 2, row].GetDotType == dot.GetDotType)
            {
                return true;
            }
        }

        return false;
    }
}
