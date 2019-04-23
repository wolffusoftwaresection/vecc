var vecc = {
    //账号审核状态
    //1待审批 ，2未通过，3审批通过
    veccStatusToStr: function (_int) {
        var str = "";
        switch (_int) {
            case 1:
                str = "待审批";
                break;
            case 2:
                str = "未通过";
                break;
            default:
                str = "审批通过";
        }
        return str;
    },
    //1是工程师注册 2为企业用户注册 3为vecc添加的检测机构
    veccUserTypeToStr: function (_int) {
        var str = "";
        switch (_int) {
            case 1:
                str = "工程师";
                break;
            case 2:
                str = "企业用户";
                break;
            default:
                str = "检测机构";
        }
        return str;
    }
}