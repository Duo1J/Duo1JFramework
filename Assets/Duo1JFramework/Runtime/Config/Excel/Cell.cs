using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

namespace Duo1JFramework.ExcelAPI
{
    /// <summary>
    /// 单元格
    /// </summary>
    public struct Cell
    {
        /// <summary>
        /// 单元格数据
        /// </summary>
        public ExcelRange Range { get; private set; }

        /// <summary>
        /// 单元格样式
        /// </summary>
        public ExcelStyle Style => Range.Style;

        /// <summary>
        /// 单元格值
        /// </summary>
        public string Value
        {
            get
            {
                object val = Range.Value;
                return val == null ? string.Empty : val.ToString();
            }
            set
            {
                Range.Value = value;
            }
        }

        /// <summary>
        /// 单元格公式值
        /// </summary>
        public string Formula
        {

            get
            {
                object formula = Range.Formula;
                return formula == null ? string.Empty : formula.ToString();
            }
            set
            {
                Range.Formula = value;
            }
        }

        #region Color

        /// <summary>
        /// 填充颜色
        /// </summary>
        public string FillColor => Style.Fill.BackgroundColor.Rgb;

        /// <summary>
        /// 填充主题
        /// </summary>
        public eThemeSchemeColor? FillTheme => Style.Fill.BackgroundColor.Theme;

        /// <summary>
        /// 字体颜色
        /// </summary>
        public string FontColor => Style.Font.Color.Rgb;

        #endregion Color

        /// <summary>
        /// 创建单元格
        /// </summary>
        public static Cell Create(ExcelRange range)
        {
            return new Cell(range);
        }

        private Cell(ExcelRange range)
        {
            Range = range;
        }
    }
}