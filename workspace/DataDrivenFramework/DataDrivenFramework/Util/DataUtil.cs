using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDrivenFramework.Util
{
    public class DataUtil
    {
        public static object[][] getTestData(ExcelReaderFile xls, string testCaseName)
        {

            string sheetName = "Data";

            int testStartRowNum = 1;

            while (!xls.getCellData(sheetName, 0, testStartRowNum).Equals(testCaseName))
            {

                testStartRowNum++;


            }

            int colStartRowNum = 1 + testStartRowNum;
            int dataStartRowNum = 2 + testStartRowNum;
            //calcuate rows of data
            int rows = 0;
            while (!xls.getCellData(sheetName, 0, dataStartRowNum + rows).Equals(""))
            {

                rows++;
            }
            //calculate total col of data
            int col = 0;

            while (!xls.getCellData(sheetName, col, colStartRowNum + rows).Equals(""))
            {

                col++;
            }
            //Read data

            object[][] data = new object[rows][];
            int dataRow = 0;
            Dictionary<string, string> table = null;

            for (int rNum = dataStartRowNum; rNum < dataStartRowNum + rows; rNum++)
            {
                data[rNum - dataStartRowNum] = new object[1];
                table = new Dictionary<string, string>();

                for (int cNum = 0; cNum < col; cNum++)
                {
                    string key = xls.getCellData(sheetName, cNum, colStartRowNum);
                    string value = xls.getCellData(sheetName, cNum, rNum);
                    table.Add(key, value);

                }
                data[dataRow][0] = table;
                dataRow++;
            }

            return data;


        }

        public static bool isTestRunnable(string testCaseName, ExcelReaderFile xls)
        {
            string sheetName = "TestCases";
            int rows = xls.getRowCount(sheetName);
            for (int r = 2; r <= rows; r++)
            {
                string tName = xls.getCellData(sheetName, "TCID", r);
                if (tName.Equals(testCaseName))
                {

                    string runmode = xls.getCellData(sheetName, "Runmode", r);
                    if (runmode.Equals("Y"))
                     return true;
                    else
                        return false;
                }

            }
            return false;
        }
    }
}