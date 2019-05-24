using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wolf.Vecc.Comm.Helpers;
using Wolf.Vecc.Comm.TableDictionary;

namespace Wolf.Vecc.Comm.Excel
{
    public static class UploadExcelCommon
    {
        public static DataTable dt = new DataTable();
        /// <summary>
        /// 验证上传的EXCEL数据合法性
        /// </summary>
        /// <param name="dt">原EXCEL数据</param>
        /// <param name="companyCode">公司Code</param>
        /// <param name="userName">工程师的登录名</param>
        /// <param name="flag">验证数据是否有错</param>
        /// <returns></returns>
        public static DataTable UploadExcel(DataTable dt, string userName, ref bool flag)
        {
            //验证数据是否有错，默认没有错误true
            flag = true;
            //对应新的DT
            DataTable newDt = new DataTable();
            newDt.Columns.Add("Line", typeof(string));
            foreach (DataColumn dc in dt.Columns)
            {
                //根据中文列名通过字典找出对应的英文名创建列
                newDt.Columns.Add(TableDictionaries.TableMapDictionary.GetValue(dc.ColumnName), typeof(string));
            }
            newDt.Columns.Add("Error", typeof(string));
            //先将多有行创建出来
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow newDr = newDt.NewRow();
                newDr["Line"] = i + 2;
                newDt.Rows.Add(newDr);
            }

            //以列为单位循环
            foreach (DataColumn dc in dt.Columns)
            {
                DataTable dt1 = new DataTable();
                List<string> list = new List<string>();

                List<string> userNameList = new List<string>();
                List<string> userList = new List<string>();

                //中文列名
                var cn = dc.ColumnName;
                //将转进来的DT里所有的数据赋值到新的DT中
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //取出字典中列对应的英文名，然后赋值
                    newDt.Rows[i][TableDictionaries.TableMapDictionary.GetValue(dc.ColumnName)] = dt.Rows[i][cn];
                }
                //循环行
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //判断必填值（valiDictionary存放必填项）
                    if (TableDictionaries.valiDictionary.Contains(cn) && (dt.Rows[i][cn] == null || dt.Rows[i][cn].ToString() == ""))
                    {
                        newDt.Rows[i]["Error"] += cn + "不能为空 /";
                        flag = false;
                    }
                    //判断某值是否存在
                    if (TableDictionaries.TableDictionary.Keys.Contains(cn) && dt.Rows[i][cn] != null && dt.Rows[i][cn].ToString() != "" && !list.Contains(dt.Rows[i][cn]))
                    {
                        newDt.Rows[i]["Error"] += cn + "不存在 /";
                        flag = false;
                    }
                    if (TableDictionaries.NumDictionary.Contains(cn) && dt.Rows[i][cn] != null && dt.Rows[i][cn].ToString() != "" && !IsNumeric(Convert.ToString(dt.Rows[i][cn])))
                    {
                        newDt.Rows[i]["Error"] += cn + "必须是整数 /";
                        flag = false;
                    }
                    if (TableDictionaries.PriceDictionary.Contains(cn) && dt.Rows[i][cn] != null && dt.Rows[i][cn].ToString() != "" && !IsPrice(Convert.ToString(dt.Rows[i][cn])))
                    {
                        newDt.Rows[i]["Error"] += cn + "格式不对 /";
                        flag = false;
                    }
                    //判断日期（模板中列必须包含日期两个字，其他非日期列不要出现日期两个字）
                    if (cn.Contains("日期") && !CheckIsDate(dt.Rows[i][cn].ToString()))
                    {
                        newDt.Rows[i]["Error"] += cn + "格式不正确 /";
                        flag = false;
                    }
                    /*******用户**********/
                    if (cn == "登录账号")
                    {
                        int num = dt.Select(" 登录账号 <> '' AND 登录账号 = '" + dt.Rows[i][cn].ToString() + "'").Count();
                        if (num > 1)
                        {
                            newDt.Rows[i]["Error"] += "登录账号在EXCEL中重复 /";
                            flag = false;
                        }

                        if (dt.Rows[i][cn] != null && dt.Rows[i][cn].ToString() != "" && userNameList.Contains(dt.Rows[i][cn]))
                        {
                            newDt.Rows[i]["Error"] += "重复的登录账号 /";
                            flag = false;
                        }
                    }
                }
            }
            return newDt;

        }

        /// <summary>
        /// 判断是否是日期类型
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static bool CheckIsDate(string dateStr)
        {
            DateTime dt;
            if (dateStr == null || dateStr == "" || DateTime.TryParse(dateStr, out dt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否是正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            Regex reg = new Regex(@"^[0-9]\d*$");
            return reg.IsMatch(str);
        }

        /// <summary>
        /// 判断两位数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPrice(string str)
        {
            Regex reg = new Regex(@"^(?!0+(?:\.0+)?$)(?:[0-9]\d*|0)(?:\.\d{1,2})?$");
            return reg.IsMatch(str) || str == "0" || str == "0.0" || str == "0.00";
        }
    }
}
