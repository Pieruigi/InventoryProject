using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OMTB.Utility
{
    public class MathUtil
    {
        

        public static void FromIndexToGrid(int index, int rows, int cols, out int rowNum, out int colNum)
        {
            if (index < 0)
                throw new Exception(string.Format("Index is less than zero: {0}.", index));

            if (rows <= 0)
                throw new Exception(string.Format("Rows is less or equal to zero: {0}.", rows));

            if (cols <= 0)
                throw new Exception(string.Format("Cols is less or equal to zero: {0}.", cols));

            if (index >= rows * cols)
                throw new Exception(String.Format("Index is to higher: {0}, columns: {1}, rows: {2}.", cols, rows));

            rowNum = index / cols;

            colNum = index % cols;

        }


        public static int FromGridToIndex(int cols, int rowNum, int colNum)
        {
            
            if (cols <= 0)
                throw new Exception(string.Format("Cols is less or equal to zero: {0}.", cols));

            if (rowNum < 0)
                throw new Exception(string.Format("RowNum is less than zero: {0}.", rowNum));
            
            if (colNum < 0)
                throw new Exception(string.Format("ColNum is less than zero: {0}.", colNum));


            return rowNum * cols + colNum % cols;
        }
    }

}
