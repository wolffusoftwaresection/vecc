using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.SysModel;
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.IService.ISysService
{
    public interface ISysDataService
    {
        /// <summary>
        /// 获取上传数据列表
        /// </summary>
        /// <returns></returns>
        List<SysData> GetSysDataList(DataApprovalViewModel dataApprovalViewModel);

        List<SysData> GetSysDataList(FindingsAuditViewModel findingsAuditViewModel);

        SysData GetDataById(int Id);

        int Update(SysData sysData);

        int BatchUpdataData(string sysDatas, int state);

        int BatchDelete(string Ids);

        int InsertData(SysData sysData);
    }
}
