using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Core.LambdaExt;
using Wolf.Vecc.Data.DataService;
using Wolf.Vecc.IService.ISysService;
using Wolf.Vecc.Model.SysModel;
using Wolf.Vecc.Model.ViewModel;

namespace Wolf.Vecc.Service.SysService
{
    public class SysDataService : ISysDataService
    {
        private readonly IDbServiceReposity _dbServiceReposity;
        public SysDataService(IDbServiceReposity dbServiceReposity)
        {
            _dbServiceReposity = dbServiceReposity;
        }

        public SysData GetDataById(int Id)
        {
            return _dbServiceReposity.FirstOrDefault<SysData>(d => d.Id == Id);
        }

        public List<SysData> GetSysDataList(DataApprovalViewModel dataApprovalViewModel)
        {
            Expression<Func<SysData, bool>> searchPredicate = PredicateExtensions.True<SysData>();
            searchPredicate = searchPredicate.And(d => d.IsDel == 0);
            if (dataApprovalViewModel.FileName != null)
            {
                searchPredicate = searchPredicate.And(d => d.DataName.Contains(dataApprovalViewModel.FileName));
            }
            if (dataApprovalViewModel.DataState > 0)
            {
                searchPredicate = searchPredicate.And(d => d.DataStatus == dataApprovalViewModel.DataState);
            }
            return _dbServiceReposity.GetWhereSearch(searchPredicate).ToList();
        }

        public int Update(SysData sysData)
        {
            return _dbServiceReposity.Update(sysData);
        }
    }
}
