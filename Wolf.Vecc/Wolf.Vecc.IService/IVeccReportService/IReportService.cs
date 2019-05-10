using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.ReportModel;

namespace Wolf.Vecc.IService.IVeccReportService
{
    public interface IReportService
    {
        /// <summary>
        /// 用户每月注册数量统计 柱状图
        /// </summary>
        /// <returns></returns>
        TempUserMonthRegister GetReportUserMonthRegister(string year);

        /// <summary>
        ///  获取某一年某一月的注册用户审批状态结果数量 饼图
        /// </summary>
        /// <param name="year"></param>
        ReportUserMonthRegisterInfo ReportUserMonthRegisterInfo(string year);
    }
}
