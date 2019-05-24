using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.TableDictionary
{
    public static class TableDictionaries
    {
        //根据列名 找出数据库存在的数据
        public static Dictionary<string, string> TableDictionary = new Dictionary<string, string>
        {
            //{"客户Code","T_MstCustomerDetail,CustomerDetailCode" },
            //{"设备型号ID","T_MstEquipmentModel,ID" },
            //{"设备型号代码","T_MstEquipmentModel,EquipmentModelCode"},
            //{"工具类别","T_MstTypeTool,TypeToolName"},
            //{"所在库位","T_MstWarehouse,WarehouseName"},
            //{"工程师账号","T_SysUser,UserName"},
            //{"零件号","T_SparePartType,SAPCode"},
            //{"登录账号","T_SysUser,UserName"}
        };

        //模板列名对应的英文名
        public static Dictionary<string, string> TableMapDictionary = new Dictionary<string, string>
        {
            //用户导入模板
            {"登录账号", "UserName"},
            {"登录密码", "UserPwd"},
            {"员工状态", "UserState"},
            {"序号", "UserNumber"},
            {"工号", "UserJobNumber"},
            {"姓名", "Name"},
            {"性别", "Gender"},
            {"出生年月", "Birthday"},
            {"民族", "Family"},
            {"籍贯", "Province"},
            {"身份证", "IDCard"},
            {"身份证到期日期", "IDCardOverDue"},
            {"年龄", "Age"},
            {"婚姻状况", "MaritalStatus"},
            {"毕业院校", "School"},
            {"专业", "Major"},
            {"学历", "Education"},
            {"学位", "AcademicDegree"},
            {"家庭地址", "Address"},
            {"通信地址", "MailingAddress"},
            {"户籍地址", "PermanentAddress"},
            {"户口性质", "Nature"},
            {"联系电话", "UserMobile"},
            {"政治面貌", "PoliticalOutlook"},
            {"系统状态", "SystemState"},
            {"系统编号", "SystemID"},
            {"进公司日期", "IncorporationDate"},
            {"到岗日期", "ArrivalDate"},
            {"离职日期", "LeaveDate"},
            {"离职原因", "LeaveReasons"},
            {"具体原因", "SpecificReasons"},
            {"公司工龄", "CompanySeniority"},
            {"合同类型", "ContractType"},
            {"合同期限", "ContractPeriod"},
            {"合同起始日期", "ContractBeginDate"},
            {"合同终止日期", "ContractEndDate"},
            {"合同试用期", "ContractTryDate"},
            {"试用期考核结果", "ContractResult"},
            {"转正日期", "CompletionDate"},
            {"用工性质", "EmploymentNature"},
            {"所属公司", "OwnedCompany"},
            {"部门名称", "DepartmentName"},
            {"工段", "WorkshopSection"},
            {"班组", "WorkTeam"},
            {"现任岗位名称", "CurrentPostName"},
            {"对应组织架构岗位", "CorrespondingOrganizational"},
            {"岗位类别", "PostCategory"},
            {"岗位等级", "PostLevel"},
            //考题导入模板
            ///////////////////////////////4444444444444444444444444444444444/////////////////////////////////////////////////
            {"题目类别","QuestionAccess" },
            {"题目类型","QuestionType" },
            {"题目标题","QuestionTitle" },
            {"A","OptionA" },
            {"B","OptionB" },
            {"C","OptionC" },
            {"D","OptionD" },
            {"答案","Answer" },
            {"分数","Point" },
            {"试题解析", "Analysis" }
        };

        //必填项
        public static List<string> valiDictionary = new List<string>
        {
            //用户导入模板
            "题目类别",
            "登录账号",
            "登录密码",
            "姓名",
            //考题导入模板
            "题目类型",
            "题目标题"//,
            //"A",
            //"B",
            //"C",
            //"D"
        };

        //整数
        public static List<string> NumDictionary = new List<string>
        {
            //考题导入模板
            "分数"
        };

        //价格
        public static List<string> PriceDictionary = new List<string>
        {

        };

        #region 导入模板 列头 默认数据
        //用户模板EXCEL模板列头
        public static string[] ReportTemplateHeadList = {
                                                                "报告编号",
                                                                "内部编号",
                                                                "产品名称",
                                                                "产品商标",
                                                                "产品型号",
                                                                "受检单位",
                                                                "检验类别",
                                                                "发送日期",
                                                                "检验单位名称",
                                                                "检验单位地址",
                                                                "检验单位电话",
                                                                "检验单位传真",
                                                                "检验单位邮编",
                                                                "检验单位E_mail",
                                                                "受检单位名称",
                                                                "受检单位地址",
                                                                "受检单位电话",
                                                                "受检单位传真",
                                                                "受检单位邮编",
                                                                "受检单位E_mail"
                                                            };
     
        /// <summary>
        /// ///////////////////////////////////////////1111111111111111111111111111/////////////////////////////////////////////////////////////////
        /// </summary>
        //考题EXCEL模板列头
        public static string[] TestConclusionTemplateHeadList = {
                                                                "样品名称",
                                                                "型号",
                                                                "商标",
                                                                "生产单位",
                                                                "生产日期",
                                                                "送样者",
                                                                "送样日期",
                                                                "样品数量",
                                                                "检验类别",
                                                                "签发日期",
                                                                "备注",
                                                                "批准",
                                                                "审核",
                                                                "主检"
                                                            };

        /// <summary>
        /// /////////////////////////////2222222222222222222222222222222222//////////////////////////////////////////////////////
        /// </summary>
        //考题模板数据
        public static string[] InspectionVehicleTempHeadList = {
                                                                "VIN号",
                                                                "行驶里程(km)",
                                                                "轮胎数量(个)",
                                                                "编号"
                                                         };
        #endregion
    }
}
