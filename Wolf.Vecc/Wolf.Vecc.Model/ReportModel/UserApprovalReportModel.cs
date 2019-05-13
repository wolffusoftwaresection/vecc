using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.ReportModel
{
    /// <summary>
    /// 每月注册的人数统计类
    /// </summary>
    public class UserMonthRegister
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 当月注册总人数
        /// </summary>
        public int RegisterCount { get; set; }
    }

    public class ReportUserMonthRegisterInfo
    {
        public string Pending { get; set; }
        public string Fail { get; set; }
        public string Passed { get; set; }
    }

    public class CalculatingSuccessRate
    {
        public string Month { get; set; }
        public string Number { get; set; }
    }

    public class TempUserMonthRegister
    {
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sept { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
    }

    public class UserApprovalReportModel
    {
        //每个月申请人数及审批通过被拒绝的人数
        /// <summary>
        /// 月份
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 当月注册总人数
        /// </summary>
        public int RegisterCount { get; set; }

        /// <summary>
        /// 当月
        /// </summary>
        public int PassCount { get; set; }

    }
}
