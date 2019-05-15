using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Data.DataService;
using Wolf.Vecc.IService.IVeccReportService;
using Wolf.Vecc.Model.ReportModel;

namespace Wolf.Vecc.Service.VeccReportService
{
    public class ReportService : IReportService
    {
        private readonly IDbServiceReposity _dbServiceReposity;
        public ReportService(IDbServiceReposity dbServiceReposity)
        {
            _dbServiceReposity = dbServiceReposity;
        }

        #region 注册用户报表
        public TempUserMonthRegister CalculatingSuccessNumberView(string year)
        {
            string sql = @"select 
                            sum(case month(systask.create_time) when '1' then 1 else 0 end) as 'Jan',
                            sum(case month(systask.create_time) when '2' then 1 else 0 end) as 'Feb',
                            sum(case month(systask.create_time) when '3' then 1 else 0 end) as 'Mar',
                            sum(case month(systask.create_time) when '4' then 1 else 0 end) as 'Apr',
                            sum(case month(systask.create_time) when '5' then 1 else 0 end) as 'May',
                            sum(case month(systask.create_time) when '6' then 1 else 0 end) as 'Jun',
                            sum(case month(systask.create_time) when '7' then 1 else 0 end) as 'Jul',
                            sum(case month(systask.create_time) when '8' then 1 else 0 end) as 'Aug',
                            sum(case month(systask.create_time) when '9' then 1 else 0 end) as 'Sept',
                            sum(case month(systask.create_time) when '10' then 1 else 0 end) as 'Oct',
                            sum(case month(systask.create_time) when '11' then 1 else 0 end) as 'Nov',
                            sum(case month(systask.create_time) when '12' then 1 else 0 end) as 'Dec'
                        from sys_task systask where 
                        date_format(systask.create_time, '%Y') = '{0}'";
            return _dbServiceReposity.Query<TempUserMonthRegister>(string.Format(sql, year), new System.Data.SqlClient.SqlParameter[0]).FirstOrDefault();
        }

        public TempUserMonthRegister GetReportUserMonthRegister(string year)
        {
            string sql = @"select 
                sum(case month(sysuser.create_date) when '1' then 1 else 0 end) as 'Jan',
	            sum(case month(sysuser.create_date) when '2' then 1 else 0 end) as 'Feb',
	            sum(case month(sysuser.create_date) when '3' then 1 else 0 end) as 'Mar',
	            sum(case month(sysuser.create_date) when '4' then 1 else 0 end) as 'Apr',
	            sum(case month(sysuser.create_date) when '5' then 1 else 0 end) as 'May',
	            sum(case month(sysuser.create_date) when '6' then 1 else 0 end) as 'Jun',
	            sum(case month(sysuser.create_date) when '7' then 1 else 0 end) as 'Jul',
	            sum(case month(sysuser.create_date) when '8' then 1 else 0 end) as 'Aug',
	            sum(case month(sysuser.create_date) when '9' then 1 else 0 end) as 'Sept',
	            sum(case month(sysuser.create_date) when '10' then 1 else 0 end) as 'Oct',
	            sum(case month(sysuser.create_date) when '11' then 1 else 0 end) as 'Nov',
	            sum(case month(sysuser.create_date) when '12' then 1 else 0 end) as 'Dec'
            from sys_user sysuser where
            sysuser.IsDel = 0 and date_format(sysuser.create_date, '%Y') = '{0}'";

            var tempUserMonthRegister = _dbServiceReposity.Query<TempUserMonthRegister>(string.Format(sql, year), new System.Data.SqlClient.SqlParameter[0]).FirstOrDefault();
            return tempUserMonthRegister;
        }

        public ReportUserMonthRegisterInfo ReportUserMonthRegisterInfo(string year)
        {
            string sql = @"SELECT 
                     sum(case account_status when 1 then 1 else 0 end) as 'pending',
                     sum(case account_status when 2 then 1 else 0 end) as 'fail',
                     sum(case account_status when 3 then 1 else 0 end) as 'passed'
                     FROM sys_user WHERE IsDel = 0 and DATE_FORMAT(create_date,'%Y-%m') = '{0}'";
            var reportUserMonthRegisterInfo = _dbServiceReposity.Query<ReportUserMonthRegisterInfo>(string.Format(sql, year), new System.Data.SqlClient.SqlParameter[0]).FirstOrDefault();
            return reportUserMonthRegisterInfo;
        }
        #endregion

        #region 上传数据报表
        public TempDataMonthRegister GetReportDataMonthRegister(string year)
        {
            string sql = @"select 
                sum(case month(sysdata.upload_date) when '1' then 1 else 0 end) as 'Jan',
	            sum(case month(sysdata.upload_date) when '2' then 1 else 0 end) as 'Feb',
	            sum(case month(sysdata.upload_date) when '3' then 1 else 0 end) as 'Mar',
	            sum(case month(sysdata.upload_date) when '4' then 1 else 0 end) as 'Apr',
	            sum(case month(sysdata.upload_date) when '5' then 1 else 0 end) as 'May',
	            sum(case month(sysdata.upload_date) when '6' then 1 else 0 end) as 'Jun',
	            sum(case month(sysdata.upload_date) when '7' then 1 else 0 end) as 'Jul',
	            sum(case month(sysdata.upload_date) when '8' then 1 else 0 end) as 'Aug',
	            sum(case month(sysdata.upload_date) when '9' then 1 else 0 end) as 'Sept',
	            sum(case month(sysdata.upload_date) when '10' then 1 else 0 end) as 'Oct',
	            sum(case month(sysdata.upload_date) when '11' then 1 else 0 end) as 'Nov',
	            sum(case month(sysdata.upload_date) when '12' then 1 else 0 end) as 'Dec'
            from sys_data sysdata where
            sysdata.IsDel = 0 and date_format(sysdata.upload_date, '%Y') = '{0}'";

            var tempDataMonthRegister = _dbServiceReposity.Query<TempDataMonthRegister>(string.Format(sql, year), new System.Data.SqlClient.SqlParameter[0]).FirstOrDefault();
            return tempDataMonthRegister;
        }

        public ReportDataMonthUploadInfo ReportDataMonthUploadInfo(string year)
        {
            string sql = @"SELECT 
                     sum(case data_status when 1 then 1 else 0 end) as 'pending',
                     sum(case data_status when 2 then 1 else 0 end) as 'fail',
                     sum(case data_status when 3 then 1 else 0 end) as 'passed'
                     FROM sys_data WHERE IsDel = 0 and DATE_FORMAT(upload_date,'%Y-%m') = '{0}'";
            var reportDataMonthUploadInfo = _dbServiceReposity.Query<ReportDataMonthUploadInfo>(string.Format(sql, year), new System.Data.SqlClient.SqlParameter[0]).FirstOrDefault();
            return reportDataMonthUploadInfo;
        }
        #endregion
    }
}
