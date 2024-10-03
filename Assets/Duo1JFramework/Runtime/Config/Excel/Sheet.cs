using OfficeOpenXml;
using System;

namespace Duo1JFramework.ExcelAPI
{
    /// <summary>
    /// 工作表
    /// </summary>
    public class Sheet
    {
        /// <summary>
        /// 所属Excel
        /// </summary>
        public Excel Excel { get; private set; }

        /// <summary>
        /// Excel工作表
        /// </summary>
        public ExcelWorksheet Worksheet { get; private set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name => Worksheet.Name;

        /// <summary>
        /// 行数
        /// </summary>
        public int RowCnt => Worksheet.Dimension.Rows;

        /// <summary>
        /// 列数
        /// </summary>
        public int ColCnt => Worksheet.Dimension.Columns;

        /// <summary>
        /// 获取单元格
        /// </summary>
        public Cell GetCell(int row, int col)
        {
            ExcelRange cell = Worksheet.Cells[row, col];
            return Cell.Create(cell);
        }

        /// <summary>
        /// 获取单元格
        /// </summary>
        public Cell this[int row, int col]
        {
            get => GetCell(row, col);
        }

        /// <summary>
        /// 设置单元格值
        /// </summary>
        public Sheet SetValue(int row, int col, string value)
        {
            Worksheet.SetValue(row, col, value);
            return this;
        }

        /// <summary>
        /// 获取单元格值
        /// </summary>
        public T GetValue<T>(int row, int col)
        {
            return Worksheet.GetValue<T>(row, col);
        }

        /// <summary>
        /// 获取单元格值
        /// </summary>
        public string GetValue(int row, int col)
        {
            return GetValue<string>(row, col);
        }

        /// <summary>
        /// 设置单元格公式值
        /// </summary>
        public Sheet SetFormula(int row, int col, string formula)
        {
            Cell cell = GetCell(row, col);
            cell.Formula = formula;
            return this;
        }

        /// <summary>
        /// 获取单元格公式值
        /// </summary>
        public string GetFormula(int row, int col)
        {
            return GetCell(row, col).Formula;
        }

        #region Iterator

        /// <summary>
        /// 遍历每行
        /// </summary>
        public void ForeachRow(Action<Cell> callback)
        {
            if (callback == null)
            {
                return;
            }

            for (int row = 1; row <= RowCnt; row++)
            {
                for (int col = 1; col <= ColCnt; col++)
                {
                    callback(GetCell(row, col));
                }
            }
        }

        /// <summary>
        /// 遍历每行
        /// </summary>
        public void ForeachRow(Action<ExcelRange> callback)
        {
            if (callback == null)
            {
                return;
            }

            for (int row = 1; row <= RowCnt; row++)
            {
                for (int col = 1; col <= ColCnt; col++)
                {
                    callback(Worksheet.Cells[row, col]);
                }
            }
        }

        /// <summary>
        /// 遍历每列
        /// </summary>
        public void ForeachCol(Action<Cell> callback)
        {
            if (callback == null)
            {
                return;
            }

            for (int col = 1; col <= ColCnt; col++)
            {
                for (int row = 1; row <= RowCnt; row++)
                {
                    callback(GetCell(row, col));
                }
            }
        }

        /// <summary>
        /// 遍历每列
        /// </summary>
        public void ForeachCol(Action<ExcelRange> callback)
        {
            if (callback == null)
            {
                return;
            }

            for (int col = 1; col <= ColCnt; col++)
            {
                for (int row = 1; row <= RowCnt; row++)
                {
                    callback(Worksheet.Cells[row, col]);
                }
            }
        }

        #endregion Iterator

        /// <summary>
        /// 创建工作表
        /// </summary>
        public static Sheet Create(Excel excel, ExcelWorksheet worksheet)
        {
            return new Sheet(excel, worksheet);
        }

        private Sheet(Excel excel, ExcelWorksheet worksheet)
        {
            Excel = excel;
            Worksheet = worksheet;
        }
    }
}