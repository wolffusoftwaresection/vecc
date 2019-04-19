var TableDetailed = {
    ShowDetailed: function (lay, detailedPath, id) {
        lay.open({
            type: 2,
            title: ['数据明细'],
            shadeClose: false,
            btn: ['关闭'],
            skin: '',
            area: ['60%', '85%'],
            content: [detailedPath + "?id=" + id],
            yes: function (index, layor) {
                lay.close(index);
            }
        });
    }
}