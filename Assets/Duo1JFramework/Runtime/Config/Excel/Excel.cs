using System;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml;

namespace Duo1JFramework.ExcelAPI
{
    /// <summary>
    /// Excel表格
    /// </summary>
    public class Excel : IDisposable
    {
        /// <summary>
        /// Excel文件
        /// </summary>
        public ExcelPackage Package { get; private set; }

        /// <summary>
        /// Excel工作簿
        /// </summary>
        public ExcelWorkbook Workbook => Package.Workbook;

        /// <summary>
        /// 工作表集合
        /// </summary>
        public ExcelWorksheets Worksheets => Workbook.Worksheets;

        /// <summary>
        /// 工作表字典
        /// </summary>
        private Dictionary<object, Sheet> sheetDict;

        #region Sheet

        /// <summary>
        /// 通过表名获取工作表
        /// </summary>
        public Sheet GetSheet(string sheetName)
        {
            if (!sheetDict.TryGetValue(sheetName, out Sheet sheet))
            {
                ExcelWorksheet worksheet = Worksheets[sheetName];
                sheet = Sheet.Create(this, worksheet);
                sheetDict.Add(sheetName, sheet);
            }

            return sheet;
        }

        /// <summary>
        /// 通过索引获取工作表, 索引由0开始
        /// </summary>
        public Sheet GetSheet(int sheetIdx)
        {
            if (!sheetDict.TryGetValue(sheetIdx, out Sheet sheet))
            {
                ExcelWorksheet worksheet = Worksheets[sheetIdx];
                sheet = Sheet.Create(this, worksheet);
                sheetDict.Add(sheetIdx, sheet);
            }

            return sheet;
        }

        /// <summary>
        /// 添加工作表
        /// </summary>
        public Sheet AddSheet(string sheetName)
        {
            Worksheets.Add(sheetName);
            return GetSheet(sheetName);
        }

        public Sheet this[int sheetIdx]
        {
            get => GetSheet(sheetIdx);
        }

        public Sheet this[string sheetName]
        {
            get => GetSheet(sheetName);
            set => AddSheet(sheetName);
        }

        #endregion Sheet

        #region Save

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            Package.Save();
        }

        /// <summary>
        /// 另存为
        /// </summary>
        public void SaveAs(string filePath)
        {
            Package.SaveAs(filePath);
        }

        #endregion Save

        /// <summary>
        /// 通过路径打开Excel文件
        /// </summary>
        public static Excel Open(string filePath)
        {
            return new Excel(filePath);
        }

        /// <summary>
        /// 通过流打开Excel文件
        /// </summary>
        public static Excel Open(Stream stream)
        {
            return new Excel(stream);
        }

        /// <summary>
        /// 创建Excel文件
        /// </summary>
        /// <returns></returns>
        public static Excel Create()
        {
            return new Excel();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (Package != null)
            {
                Package.Dispose();
                Package = null;
            }
        }

        /// <summary>
        /// 设置框架许可证
        /// </summary>
        public static void SetLicense(LicenseContext license)
        {
            ExcelPackage.LicenseContext = license;
        }

        #region Inner

        private void InitInner()
        {
            SetLicense(LicenseContext.NonCommercial);

            sheetDict = new Dictionary<object, Sheet>();
        }

        private Excel(string filePath)
        {
            Package = new ExcelPackage(filePath);
            InitInner();
        }

        private Excel(Stream stream)
        {
            Package = new ExcelPackage(stream);
            InitInner();
        }

        private Excel()
        {
            Package = new ExcelPackage();
            InitInner();
        }

        public void Dispose()
        {
            Close();
        }

        #endregion Inner
    }
}