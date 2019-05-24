using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Comm.Excel;
using Wolf.Vecc.Comm.TableDictionary;

namespace Wolf.Vecc.Comm.Helpers
{
    public class ExcelHelper : IDisposable
    {
        private string fileName = ""; //文件名
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;
        private string[] _fieldsNot = { "PageIndex", "PageSize", "IsImport", "IsExport", "StartTime", "EndTime" };

        public ExcelHelper()
        {

        }
        public ExcelHelper(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                this.fileName = fileName;
                if (!File.Exists(fileName))
                    File.Create(fileName).Close();
                disposed = false;
            }
        }
        public bool Export<T>(List<T> _list, params ExcelDictionary[] _dics) where T : class
        {
            var _t = typeof(T);
            var _props = _t.GetProperties().Where(c => c.GetMethod.IsVirtual == false && !_fieldsNot.Contains(c.Name)).ToArray();
            DataTable _dt = new DataTable();
            var _displayName = "";
            //遍历字段名
            foreach (var item in _props)
            {
                _displayName = item.GetCustomAttribute<DisplayAttribute>()?.Name;
                if (string.IsNullOrEmpty(_displayName))
                    _displayName = item.Name;
                _dt.Columns.Add(_displayName);
            }
            //数据转换成datatable
            foreach (var item in _list)
            {
                DataRow _dr = _dt.NewRow();
                for (int i = 0; i < _dt.Columns.Count; i++)
                {
                    var _value = _props[i].GetValue(item);
                    if (_props[i].Name == "IsDeleted")
                    {
                        _dr[i] = _value.ToString() == "0" ? "未删除" : "已删除";
                        continue;
                    }
                    var _dic = _dics?.FirstOrDefault(c => c.Name == _props[i].Name)?.Dic;
                    if (_dic != null && _dic.Count > 0)
                        _dr[i] = _dic[(int)_value];
                    else
                        _dr[i] = _value;
                }
                _dt.Rows.Add(_dr);
            }
            int _r = DataTableToExcel(_dt, "Sheet1", true);
            return _r > 0;
        }
        public List<T> Import<T>(List<string> _files) where T : class, new()
        {
            List<T> _list = new List<T>();
            var _props = typeof(T).GetProperties().Where(c => c.GetMethod.IsVirtual == false && !_fieldsNot.Contains(c.Name)).ToArray();
            var _dic = new Dictionary<string, string>();
            var _displayName = "";
            foreach (var item in _props)
            {
                _displayName = item.GetCustomAttribute<DisplayAttribute>()?.Name;
                if (string.IsNullOrEmpty(_displayName))
                    _displayName = item.Name;
                _dic.Add(_displayName, item.Name);
            }
            foreach (var item in _files)
            {
                ExcelHelper _excelHelper = new ExcelHelper(item);
                var _dt = _excelHelper.ExcelToDataTable();
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    T _t = new T();
                    for (int j = 0; j < _dt.Columns.Count; j++)
                    {
                        if (_dic[_dt.Columns[j].ColumnName] != null)
                        {
                            var _value = _dt.Rows[i][_dt.Columns[j].ColumnName];
                            var _pop = _t.GetType().GetProperty(_dic[_dt.Columns[j].ColumnName].ToString());
                            _t.GetType().GetProperty(_dic[_dt.Columns[j].ColumnName].ToString()).SetValue(_t, _value);
                        }
                    }
                    _list.Add(_t);
                }
            }
            return _list;
        }
        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;
            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();
            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }
                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    ICellStyle style = workbook.CreateCellStyle();
                    //style.FillBackgroundColor=NPOI.HSSF.Util.HSSFColor.Green.Index;
                    style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index;
                    style.FillPattern = FillPattern.SolidForeground;
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                        row.Cells[j].CellStyle = style;
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }
                for (i = 0; i < data.Rows.Count; ++i)
                {
                    if (data.Rows[i][0].ToString() != "")
                    {
                        IRow row = sheet.CreateRow(count);
                        for (j = 0; j < data.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                        }
                        ++count;
                    }
                }
                workbook.Write(fs); //写入到excel
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName = "Sheet1", bool isFirstRowColumn = true)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);
                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            DataColumn column = new DataColumn(firstRow.GetCell(i).StringCellValue);
                            data.Columns.Add(column);
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　
                        if (row.GetCell(0).ToString() == "") continue;
                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }
                fs = null;
                disposed = true;
            }
        }

        public MemoryStream CreatUserExcelTemplate(string[] head, string[] data, DataTable model = null)
        {
            //创建工作簿
            HSSFWorkbook wk = new HSSFWorkbook();

            ICellStyle styleCell = wk.CreateCellStyle();
            styleCell.Alignment = HorizontalAlignment.Center;
            IFont font = wk.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            styleCell.SetFont(font);

            ICellStyle styleCell2 = wk.CreateCellStyle();
            styleCell2.Alignment = HorizontalAlignment.Center;
            IFont font2 = wk.CreateFont();
            font2.Boldweight = (short)FontBoldWeight.Bold;
            font2.Color = (short)FontColor.Red;
            styleCell2.SetFont(font2);

            if (model != null)
            {
                ISheet sheet = wk.CreateSheet("职员信息");
                //创建行和单元格
                IRow row = sheet.CreateRow(0);
                //创建单元格
                ICell cell;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.CellStyle = styleCell;
                cell.SetCellValue("员工状态");
                cell = row.CreateCell(1);
                cell.CellStyle = styleCell;
                cell.SetCellValue("序号");
                cell = row.CreateCell(2);
                cell.CellStyle = styleCell;
                cell.SetCellValue("工号");
                cell = row.CreateCell(3);
                cell.CellStyle = styleCell;
                cell.SetCellValue("姓名");
                cell = row.CreateCell(4);
                cell.CellStyle = styleCell;
                cell.SetCellValue("性别");
                cell = row.CreateCell(5);
                cell.CellStyle = styleCell;
                cell.SetCellValue("出生年月");
                cell = row.CreateCell(6);
                cell.CellStyle = styleCell;
                cell.SetCellValue("民族");
                cell = row.CreateCell(7);
                cell.CellStyle = styleCell;
                cell.SetCellValue("籍贯");
                cell = row.CreateCell(8);
                cell.CellStyle = styleCell;
                cell.SetCellValue("身份证");
                cell = row.CreateCell(9);
                cell.CellStyle = styleCell;
                cell.SetCellValue("身份证到期日期");
                cell = row.CreateCell(10);
                cell.CellStyle = styleCell;
                cell.SetCellValue("年龄");
                cell = row.CreateCell(11);
                cell.CellStyle = styleCell;
                cell.SetCellValue("婚姻状况");
                cell = row.CreateCell(12);
                cell.CellStyle = styleCell;
                cell.SetCellValue("毕业院校");
                cell = row.CreateCell(13);
                cell.CellStyle = styleCell;
                cell.SetCellValue("专业");
                cell = row.CreateCell(14);
                cell.CellStyle = styleCell;
                cell.SetCellValue("学历");
                cell = row.CreateCell(15);
                cell.CellStyle = styleCell;
                cell.SetCellValue("学位");
                cell = row.CreateCell(16);
                cell.CellStyle = styleCell;
                cell.SetCellValue("家庭地址");
                cell = row.CreateCell(17);
                cell.CellStyle = styleCell;
                cell.SetCellValue("通信地址");
                cell = row.CreateCell(18);
                cell.CellStyle = styleCell;
                cell.SetCellValue("户籍地址");
                cell = row.CreateCell(19);
                cell.CellStyle = styleCell;
                cell.SetCellValue("户口性质");
                cell = row.CreateCell(20);
                cell.CellStyle = styleCell;
                cell.SetCellValue("联系电话");
                cell = row.CreateCell(21);
                cell.CellStyle = styleCell;
                cell.SetCellValue("政治面貌");
                cell = row.CreateCell(22);
                cell.CellStyle = styleCell;
                cell.SetCellValue("系统状态");
                cell = row.CreateCell(23);
                cell.CellStyle = styleCell;
                cell.SetCellValue("系统编号");
                cell = row.CreateCell(24);
                cell.CellStyle = styleCell;
                cell.SetCellValue("进公司日期");
                cell = row.CreateCell(25);
                cell.CellStyle = styleCell;
                cell.SetCellValue("到岗日期");
                cell = row.CreateCell(26);
                cell.CellStyle = styleCell;
                cell.SetCellValue("离职日期");
                cell = row.CreateCell(27);
                cell.CellStyle = styleCell;
                cell.SetCellValue("离职原因");
                cell = row.CreateCell(28);
                cell.CellStyle = styleCell;
                cell.SetCellValue("具体原因");
                cell = row.CreateCell(29);
                cell.CellStyle = styleCell;
                cell.SetCellValue("公司工龄");
                cell = row.CreateCell(30);
                cell.CellStyle = styleCell;
                cell.SetCellValue("合同类型");
                cell = row.CreateCell(31);
                cell.CellStyle = styleCell;
                cell.SetCellValue("合同期限");
                cell = row.CreateCell(32);
                cell.CellStyle = styleCell;
                cell.SetCellValue("合同起始日期");
                cell = row.CreateCell(33);
                cell.CellStyle = styleCell;
                cell.SetCellValue("合同终止日期");
                cell = row.CreateCell(34);
                cell.CellStyle = styleCell;
                cell.SetCellValue("合同试用期");
                cell = row.CreateCell(35);
                cell.CellStyle = styleCell;
                cell.SetCellValue("试用期考核结果");
                cell = row.CreateCell(36);
                cell.CellStyle = styleCell;
                cell.SetCellValue("转正日期");
                cell = row.CreateCell(37);
                cell.CellStyle = styleCell;
                cell.SetCellValue("用工性质");
                cell = row.CreateCell(38);
                cell.CellStyle = styleCell;
                cell.SetCellValue("所属公司");
                cell = row.CreateCell(39);
                cell.CellStyle = styleCell;
                cell.SetCellValue("部门名称");
                cell = row.CreateCell(40);
                cell.CellStyle = styleCell;
                cell.SetCellValue("工段");
                cell = row.CreateCell(41);
                cell.CellStyle = styleCell;
                cell.SetCellValue("班组");
                cell = row.CreateCell(42);
                cell.CellStyle = styleCell;
                cell.SetCellValue("现任岗位名称");
                cell = row.CreateCell(43);
                cell.CellStyle = styleCell;
                cell.SetCellValue("对应组织架构岗位");
                cell = row.CreateCell(44);
                cell.CellStyle = styleCell;
                cell.SetCellValue("岗位类别");
                cell = row.CreateCell(45);
                cell.CellStyle = styleCell;
                cell.SetCellValue("岗位等级");
                for (int i = 0; i < model.Rows.Count; i++)
                {
                    //创建行和单元格
                    row = sheet.CreateRow(i + 1);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(model.Rows[i]["UserStateStr"].ToString());
                    cell = row.CreateCell(1);
                    cell.SetCellValue(model.Rows[i]["UserNumber"].ToString());
                    cell = row.CreateCell(2);
                    cell.SetCellValue(model.Rows[i]["UserJobNumber"].ToString());
                    cell = row.CreateCell(3);
                    cell.SetCellValue(model.Rows[i]["Name"].ToString());
                    cell = row.CreateCell(4);
                    cell.SetCellValue(model.Rows[i]["Gender"].ToString());
                    cell = row.CreateCell(5);
                    cell.SetCellValue(model.Rows[i]["Birthday"].ToString());
                    cell = row.CreateCell(6);
                    cell.SetCellValue(model.Rows[i]["Family"].ToString());
                    cell = row.CreateCell(7);
                    cell.SetCellValue(model.Rows[i]["Province"].ToString());
                    cell = row.CreateCell(8);
                    cell.SetCellValue(model.Rows[i]["IDCard"].ToString());
                    cell = row.CreateCell(9);
                    cell.SetCellValue(model.Rows[i]["IDCardOverDue"].ToString());
                    cell = row.CreateCell(10);
                    cell.SetCellValue(model.Rows[i]["Age"].ToString());
                    cell = row.CreateCell(11);
                    cell.SetCellValue(model.Rows[i]["MaritalStatus"].ToString());
                    cell = row.CreateCell(12);
                    cell.SetCellValue(model.Rows[i]["School"].ToString());
                    cell = row.CreateCell(13);
                    cell.SetCellValue(model.Rows[i]["Major"].ToString());
                    cell = row.CreateCell(14);
                    cell.SetCellValue(model.Rows[i]["Education"].ToString());
                    cell = row.CreateCell(15);
                    cell.SetCellValue(model.Rows[i]["AcademicDegree"].ToString());
                    cell = row.CreateCell(16);
                    cell.SetCellValue(model.Rows[i]["Address"].ToString());
                    cell = row.CreateCell(17);
                    cell.SetCellValue(model.Rows[i]["MailingAddress"].ToString());
                    cell = row.CreateCell(18);
                    cell.SetCellValue(model.Rows[i]["PermanentAddress"].ToString());
                    cell = row.CreateCell(19);
                    cell.SetCellValue(model.Rows[i]["Nature"].ToString());
                    cell = row.CreateCell(20);
                    cell.SetCellValue(model.Rows[i]["UserMobile"].ToString());
                    cell = row.CreateCell(21);
                    cell.SetCellValue(model.Rows[i]["PoliticalOutlook"].ToString());
                    cell = row.CreateCell(22);
                    cell.SetCellValue(model.Rows[i]["SystemState"].ToString());
                    cell = row.CreateCell(23);
                    cell.SetCellValue(model.Rows[i]["SystemID"].ToString());
                    cell = row.CreateCell(24);
                    cell.SetCellValue(model.Rows[i]["IncorporationDate"].ToString());
                    cell = row.CreateCell(25);
                    cell.SetCellValue(model.Rows[i]["ArrivalDate"].ToString());
                    cell = row.CreateCell(26);
                    cell.SetCellValue(model.Rows[i]["LeaveDate"].ToString());
                    cell = row.CreateCell(27);
                    cell.SetCellValue(model.Rows[i]["LeaveReasons"].ToString());
                    cell = row.CreateCell(28);
                    cell.SetCellValue(model.Rows[i]["SpecificReasons"].ToString());
                    cell = row.CreateCell(29);
                    cell.SetCellValue(model.Rows[i]["CompanySeniority"].ToString());
                    cell = row.CreateCell(30);
                    cell.SetCellValue(model.Rows[i]["ContractType"].ToString());
                    cell = row.CreateCell(31);
                    cell.SetCellValue(model.Rows[i]["ContractPeriod"].ToString());
                    cell = row.CreateCell(32);
                    cell.SetCellValue(model.Rows[i]["ContractBeginDate"].ToString());
                    cell = row.CreateCell(33);
                    cell.SetCellValue(model.Rows[i]["ContractEndDate"].ToString());
                    cell = row.CreateCell(34);
                    cell.SetCellValue(model.Rows[i]["ContractTryDate"].ToString());
                    cell = row.CreateCell(35);
                    cell.SetCellValue(model.Rows[i]["ContractResult"].ToString());
                    cell = row.CreateCell(36);
                    cell.SetCellValue(model.Rows[i]["CompletionDate"].ToString());
                    cell = row.CreateCell(37);
                    cell.SetCellValue(model.Rows[i]["EmploymentNature"].ToString());
                    cell = row.CreateCell(38);
                    cell.SetCellValue(model.Rows[i]["OwnedCompany"].ToString());
                    cell = row.CreateCell(39);
                    cell.SetCellValue(model.Rows[i]["DepartmentName"].ToString());
                    cell = row.CreateCell(40);
                    cell.SetCellValue(model.Rows[i]["WorkshopSection"].ToString());
                    cell = row.CreateCell(41);
                    cell.SetCellValue(model.Rows[i]["WorkTeam"].ToString());
                    cell = row.CreateCell(42);
                    cell.SetCellValue(model.Rows[i]["CurrentPostName"].ToString());
                    cell = row.CreateCell(43);
                    cell.SetCellValue(model.Rows[i]["CorrespondingOrganizational"].ToString());
                    cell = row.CreateCell(44);
                    cell.SetCellValue(model.Rows[i]["PostCategory"].ToString());
                    cell = row.CreateCell(45);
                    cell.SetCellValue(model.Rows[i]["PostLevel"].ToString());
                }
                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
            }
            MemoryStream ms = new MemoryStream();
            wk.Write(ms);
            wk.Close();
            return ms;
        }

        /// <summary>
        /// 创建检测报告模板
        /// </summary>
        /// <returns></returns>
        public MemoryStream CreatExcelModelTemplate()
        {
            //创建工作簿
            HSSFWorkbook wk = new HSSFWorkbook();

            ICellStyle styleCell = wk.CreateCellStyle();
            styleCell.Alignment = HorizontalAlignment.Center;
            IFont font = wk.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            styleCell.SetFont(font);

            ICellStyle styleCell2 = wk.CreateCellStyle();
            styleCell2.Alignment = HorizontalAlignment.Center;
            IFont font2 = wk.CreateFont();
            font2.Boldweight = (short)FontBoldWeight.Bold;
            styleCell2.SetFont(font2);

            ICellStyle styleCell3 = wk.CreateCellStyle();
            styleCell3.Alignment = HorizontalAlignment.Center;
            IFont font3 = wk.CreateFont();
            font3.Boldweight = (short)FontBoldWeight.Bold;
            styleCell3.SetFont(font3);
            ICell cell;
            ISheet sheet1 = wk.CreateSheet("检验报告基本参数");

            IRow row = sheet1.CreateRow(0);
            sheet1.DefaultColumnWidth = 500 * 256;
            cell = row.CreateCell(0);
            cell.CellStyle = styleCell;
            cell.SetCellValue("报告编号");
            cell = row.CreateCell(1);
            cell.CellStyle = styleCell;
            cell.SetCellValue("内部编号");
            cell = row.CreateCell(2);
            cell.CellStyle = styleCell;
            cell.SetCellValue("产品名称");
            cell = row.CreateCell(3);
            cell.CellStyle = styleCell;
            cell.SetCellValue("产品商标");
            cell = row.CreateCell(4);
            cell.CellStyle = styleCell;
            cell.SetCellValue("产品型号");
            cell = row.CreateCell(5);
            cell.CellStyle = styleCell;
            cell.SetCellValue("受检单位");
            cell = row.CreateCell(6);
            cell.CellStyle = styleCell;
            cell.SetCellValue("检验类别");
            cell = row.CreateCell(7);
            cell.CellStyle = styleCell;
            cell.SetCellValue("发送日期");
            cell = row.CreateCell(8);
            cell.CellStyle = styleCell;
            cell.SetCellValue("检验单位名称");
            cell = row.CreateCell(9);
            cell.CellStyle = styleCell;
            cell.SetCellValue("检验单位地址");
            cell = row.CreateCell(10);
            cell.CellStyle = styleCell;
            cell.SetCellValue("检验单位电话");
            cell = row.CreateCell(11);
            cell.CellStyle = styleCell;
            cell.SetCellValue("检验单位传真");
            cell = row.CreateCell(12);
            cell.CellStyle = styleCell;
            cell.SetCellValue("检验单位邮编");
            cell = row.CreateCell(13);
            cell.CellStyle = styleCell;
            cell.SetCellValue("检验单位E_mail");
            cell = row.CreateCell(14);
            cell.CellStyle = styleCell;
            cell.SetCellValue("受检单位名称");
            cell = row.CreateCell(15);
            cell.CellStyle = styleCell;
            cell.SetCellValue("受检单位地址");
            cell = row.CreateCell(16);
            cell.CellStyle = styleCell;
            cell.SetCellValue("受检单位电话");
            cell = row.CreateCell(17);
            cell.CellStyle = styleCell;
            cell.SetCellValue("受检单位传真");
            cell = row.CreateCell(18);
            cell.CellStyle = styleCell;
            cell.SetCellValue("受检单位邮编");
            cell = row.CreateCell(19);
            cell.CellStyle = styleCell;
            cell.SetCellValue("受检单位E_mail");
            sheet1.DefaultColumnWidth = 500 * 256;

            ISheet sheet2 = wk.CreateSheet("检验结论");
            sheet2.DefaultColumnWidth = 500 * 256;
            row = sheet2.CreateRow(0);
            cell = row.CreateCell(0);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("样品名称");
            cell = row.CreateCell(1);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("型号");
            cell = row.CreateCell(2);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("商标");
            cell = row.CreateCell(3);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("生产单位");
            cell = row.CreateCell(4);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("生产日期");
            cell = row.CreateCell(5);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("送样者");
            cell = row.CreateCell(6);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("送样日期");
            cell = row.CreateCell(7);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("样品数量");
            cell = row.CreateCell(8);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("检验类别");
            cell = row.CreateCell(9);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("签发日期");
            cell = row.CreateCell(10);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("备注");
            cell = row.CreateCell(11);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("批准");
            cell = row.CreateCell(12);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("审核");
            cell = row.CreateCell(13);
            cell.CellStyle = styleCell2;
            cell.SetCellValue("主检");
            sheet2.DefaultColumnWidth = 500 * 256;

            ISheet sheet3 = wk.CreateSheet("检验车辆基本参数");
            sheet3.DefaultColumnWidth = 500 * 256;
            row = sheet3.CreateRow(0);
            cell = row.CreateCell(0);
            cell.CellStyle = styleCell3;
            cell.SetCellValue("VIN号");
            cell = row.CreateCell(1);
            cell.CellStyle = styleCell3;
            cell.SetCellValue("行驶里程(km)");
            cell = row.CreateCell(2);
            cell.CellStyle = styleCell3;
            cell.SetCellValue("轮胎数量(个)");
            cell = row.CreateCell(3);
            cell.CellStyle = styleCell3;
            cell.SetCellValue("编号");
            sheet3.DefaultColumnWidth = 500 * 256;
            //sheet1.AutoSizeColumn(0);
            //sheet1.AutoSizeColumn(1);
            //sheet2.AutoSizeColumn(0);
            //sheet2.AutoSizeColumn(1);
            //sheet3.AutoSizeColumn(0);
            //sheet3.AutoSizeColumn(1);
            MemoryStream ms = new MemoryStream();
            wk.Write(ms);
            wk.Close();
            return ms;
        }

        /// <summary>
        /// 创建模板
        /// </summary>
        /// <param name="head">头</param>
        /// <param name="data">默认数据</param>
        /// <param name="model">辅助数据（例如设备类型）</param>
        /// <returns></returns>
        public MemoryStream CreatExcelTemplate(string[] head, string[] data, DataTable model = null, List<ExcelSelectList> selectList = null)
        {
            //创建工作簿
            HSSFWorkbook wk = new HSSFWorkbook();

            ICellStyle styleCell = wk.CreateCellStyle();
            styleCell.Alignment = HorizontalAlignment.Center;
            IFont font = wk.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            styleCell.SetFont(font);

            ICellStyle styleCell2 = wk.CreateCellStyle();
            styleCell2.Alignment = HorizontalAlignment.Center;
            IFont font2 = wk.CreateFont();
            font2.Boldweight = (short)FontBoldWeight.Bold;
            font2.Color = (short)FontColor.Red;
            styleCell2.SetFont(font2);

            //创建一个Sheet
            ISheet sheet = wk.CreateSheet("Sheet1");
            //创建行和单元格
            IRow row = sheet.CreateRow(0);
            //创建单元格
            ICell cell;
            for (int i = 0; i < head.Count(); i++)
            {
                cell = row.CreateCell(i);
                cell.SetCellValue(head[i]);
                if (TableDictionaries.valiDictionary.Contains(head[i]))
                {
                    cell.CellStyle = styleCell2;
                }
                else
                {
                    cell.CellStyle = styleCell;
                }
                sheet.AutoSizeColumn(i);
            }

            //下拉框
            if (selectList != null)
            {
                foreach (var item in selectList)
                {
                    CellRangeAddressList regions = new CellRangeAddressList(1, 65535, item.colIndex, item.colIndex);
                    DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(item.items);
                    HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
                    sheet.AddValidationData(dataValidate);
                }
            }

            row = sheet.CreateRow(1);
            //默认数据
            for (int i = 0; i < data.Count(); i++)
            {
                cell = row.CreateCell(i);
                cell.SetCellValue(data[i]);
                cell.CellStyle = styleCell;
                sheet.AutoSizeColumn(i);
            }
            if (model != null)
            {
                //创建第二个Sheet
                ISheet sheet2 = wk.CreateSheet("设备型号参考");
                row = sheet2.CreateRow(0);
                cell = row.CreateCell(0);
                cell.CellStyle = styleCell;
                cell.SetCellValue("设备型号");
                cell = row.CreateCell(1);
                cell.CellStyle = styleCell;
                cell.SetCellValue("设备型号编号");
                cell = row.CreateCell(2);
                cell.CellStyle = styleCell;
                cell.SetCellValue("设备型号ID");
                for (int i = 0; i < model.Rows.Count; i++)
                {
                    //创建行和单元格
                    row = sheet2.CreateRow(i + 1);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(model.Rows[i]["EquipmentModelName"].ToString());
                    cell = row.CreateCell(1);
                    cell.SetCellValue(model.Rows[i]["EquipmentModelCode"].ToString());
                    cell = row.CreateCell(2);
                    cell.SetCellValue(model.Rows[i]["ID"].ToString());
                }
                sheet2.AutoSizeColumn(0);
                sheet2.AutoSizeColumn(1);
            }
            MemoryStream ms = new MemoryStream();
            wk.Write(ms);
            wk.Close();
            return ms;
        }

        ///// <summary>
        ///// 零件EXCEL模板
        ///// </summary>
        ///// <param name="head">表头</param>
        ///// <returns></returns>
        //public MemoryStream CreatPartExcelTemplate(string[] head,string[] data)
        //{
        //    //创建工作簿
        //    HSSFWorkbook wk = new HSSFWorkbook();
        //    ICellStyle styleCell = wk.CreateCellStyle();
        //    styleCell.Alignment = HorizontalAlignment.Center;
        //    IFont font = wk.CreateFont();
        //    font.Boldweight = (short)FontBoldWeight.Bold;
        //    styleCell.SetFont(font);

        //    //创建一个Sheet
        //    ISheet sheet = wk.CreateSheet("Sheet1");
        //    //创建行和单元格
        //    IRow row = sheet.CreateRow(0);
        //    //创建单元格
        //    ICell cell;
        //    for (int i = 0; i < head.Count(); i++)
        //    {
        //        cell = row.CreateCell(i);
        //        cell.SetCellValue(head[i]);
        //        cell.CellStyle = styleCell;
        //        sheet.AutoSizeColumn(i);
        //    }
        //    row = sheet.CreateRow(1);
        //    //默认数据
        //    for (int i = 0; i < data.Count(); i++)
        //    {
        //        cell = row.CreateCell(i);
        //        cell.SetCellValue(data[i]);
        //        cell.CellStyle = styleCell;
        //        sheet.AutoSizeColumn(i);
        //    }
        //    MemoryStream ms = new MemoryStream();
        //    wk.Write(ms);
        //    wk.Close();
        //    return ms;
        //}

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToTable(Stream stream, string[] headList, string fName, ref bool headIsRight, string sheetName = "Sheet1", bool isFirstRowColumn = true)
        {
            //判断表头是否和模板一致，一致为true
            headIsRight = true;
            IWorkbook wk = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                if (fName.IndexOf(".xlsx") > 0) // 2007版本
                    wk = new XSSFWorkbook(stream);
                else if (fName.IndexOf(".xls") > 0) // 2003版本
                    wk = new HSSFWorkbook(stream);
                if (sheetName != null)
                {
                    sheet = wk.GetSheet(sheetName);
                }
                else
                {
                    sheet = wk.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            DataColumn column = new DataColumn(firstRow.GetCell(i).StringCellValue);
                            data.Columns.Add(column);
                            //判断表头
                            if (headList[i].Trim() != firstRow.GetCell(i).StringCellValue.Trim())
                            {
                                headIsRight = false;
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //但表头一致时再取值
                    if (headIsRight)
                    {
                        //最后一列的标号
                        int rowCount = sheet.LastRowNum;
                        for (int i = startRow; i <= rowCount; ++i)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue; //没有数据的行默认是null　　　　　　　
                            if (row.GetCell(0) == null || row.GetCell(0).ToString() == "") continue;
                            DataRow dataRow = data.NewRow();
                            for (int j = row.FirstCellNum; j < cellCount; ++j)
                            {
                                if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                                            //dataRow[j] = row.GetCell(j).ToString();
                                {
                                    if (row.GetCell(j).CellType == CellType.Numeric)
                                    {
                                        if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                                        {
                                            dataRow[j] = Convert.ToDateTime(row.GetCell(j).DateCellValue).ToShortDateString();
                                        }
                                        else
                                        {
                                            dataRow[j] = row.GetCell(j).NumericCellValue;
                                        }
                                    }
                                    else//其他数字类型
                                    {
                                        if (row.GetCell(j).ToString() == "#N/A")
                                        {
                                            dataRow[j] = "";
                                        }
                                        else
                                        {
                                            dataRow[j] = row.GetCell(j).ToString();
                                        }
                                    }
                                }
                            }
                            data.Rows.Add(dataRow);
                        }
                    }
                }
                else
                {
                    headIsRight = false;
                }
                wk.Close();
                stream.Close();
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
    }
}
