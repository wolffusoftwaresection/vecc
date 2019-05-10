using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.ResultModel
{
    /// <summary>
    /// 一 测试信息概况
    /// </summary>
    public class TestInfoModel
    {
        /// <summary>
        /// 海拔结果
        /// </summary>
        public string Alt_res_PF { get; set; }

        /// <summary>
        /// 工况结果
        /// </summary>
        public string Trips_res_PF { get; set; }

        /// <summary>
        /// 排放结果
        /// </summary>
        public string Emission_res_PF { get; set; }

        /// <summary>
        /// 排放级别
        /// </summary>
        public string Level_ems { get; set; }

        /// <summary>
        /// 排放余量
        /// </summary>
        public string P_ems_mrg { get; set; }

        /// <summary>
        /// 试验日期
        /// </summary>
        public string Date_test { get; set; }

        /// <summary>
        /// 测试时段
        /// </summary>
        public string Time_test { get; set; }

        /// <summary>
        /// 测试人员
        /// </summary>
        public string Tester { get; set; }

        /// <summary>
        /// 测试地点
        /// </summary>
        public string Place_test { get; set; }

        /// <summary>
        /// PEMS设备型号
        /// </summary>
        public string PEMS_Manufacturer { get; set; }

        /// <summary>
        /// 整车型号
        /// </summary>
        public string Vehicle_Model { get; set; }

        /// <summary>
        /// 整车类型
        /// </summary>
        public string Vehicle_Class { get; set; }

        /// <summary>
        /// 路线描述
        /// </summary>
        public string Route_Test { get; set; }

        /// <summary>
        /// 整备质量(KG)
        /// </summary>
        public string Mass_Curb { get; set; }

        /// <summary>
        /// 最大设计质量(KG)
        /// </summary>
        public string Mass_Max { get; set; }

        /// <summary>
        /// 整车载荷(%)
        /// </summary>
        public string R_Payload { get; set; }

        /// <summary>
        /// 最高海拔(M)
        /// </summary>
        public string Alt_Max { get; set; }

        /// <summary>
        /// 大气温度(℃)
        /// </summary>
        public string T_air_avg { get; set; }

        /// <summary>
        /// 试验类别
        /// </summary>
        public string Test_Class { get; set; }

        /// <summary>
        /// 总测试里程
        /// </summary>
        public string Odo_Test { get; set; }

        /// <summary>
        /// 总测试时间
        /// </summary>
        public string Duration_Test { get; set; }

        /// <summary>
        /// 最大功率
        /// </summary>
        public string Power_Max { get; set; }

        /// <summary>
        /// WHTC参考循环功
        /// </summary>
        public string Work_Ref { get; set; }
    }
}
