var tablePlugins = {
    /**
     * 
     * @param {} tableId 表格ID
     * @param {} urlPath ajax地址
     * @param {} toolBarId 工具栏ID #toolbar_toolBarId
     * @param {} tableColumns 列
     * @param {} queryParams 参数
     * @param {} sidePagination 分页方式server/client
     * @returns {} bootstraptable
     */
    InitTables: function (tableId, urlPath, toolBarId, tableColumns, queryParams, sidePagination, height) {
        if (height == 'undefined' || height == null) {
            height = 500;
        }
        $("#" + tableId).bootstrapTable({
            url: urlPath,           //请求后台的URL（*）
            method: 'get',                                              //请求方式（*）
            toolbar: toolBarId,                                //工具按钮用哪个容器
            striped: true,                                              //是否显示行间隔色
            cache: false,                                               //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                                           //是否显示分页（*）
            sortable: true,                                             //是否启用排序
            sortOrder: "asc",                                           //排序方式
            queryParams: queryParams,
            sidePagination: sidePagination,                                   //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                                              //初始化加载第一页，默认第一页
            pageSize: 15,                                               //每页的记录行数（*）
            pageList: [10, 25, 50, 100],                                //可供选择的每页的行数（*）
            search: false,                                              //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: false,
            showColumns: true,                                         //是否显示所有的列
            showRefresh: true,                                          //是否显示刷新按钮
            minimumCountColumns: 2,                                     //最少允许的列数
            clickToSelect: true,                                        //是否启用点击选中行
            height: height,                                                //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "Id",                                             //每一行的唯一标识，一般为主键列
            showToggle: true,                                          //是否显示详细视图和列表视图的切换按钮
            cardView: false,                                            //是否显示详细视图
            columns: tableColumns,
            onLoadSuccess: function () {
                //languagePlugin.GetLanguage(languagePlugin.AllLanguageUrl);
            }
            //onPageChange: function (number, size) {
            //},
            //onClickRow: function (row, element, field) {
            //}
            
            //,
            //onDblClickRow: function (row, obj) {
                //TableDetailed.ShowDetailed(layer, detailedPath, row.Id);
            //}
        });
    },
    TableQueryParams: function (params) {                            //传递参数（*）
        var temp = {
            PageSize: params.limit,                             //页面显示行数
            PageIndex: params.offset / params.limit + 1,        //页码*页面显示行数=offset
            OrderName: params.order,                            //desc、asc
            SortName: params.sort                               //列名
        }
        return temp;
    },
    //获取列
    TableColums: function (url) {
        var tableColumns = [];
        $.ajax({
            url: url,
            type: 'POST',
            async: false,
            success: function (data) {
                tableColumns = data.tc;
            },
            error: function (data) {

            }
        });
        return tableColumns;
    },
    //获取搜索列
    GetSearchList: function (url) {
        var search = {};
        search.SearchHtml = '';
        search.SearchFieldList = [];
        $.ajax({
            url: url,
            type: 'POST',
            async: false,
            success: function (data) {
                search.SearchHtml = data.result.SearchHtml;
                search.SearchFieldList = data.result.SearchFieldList;
            }
        });
        return search;
    },
    //删除列
    IsDeleteColumn: {
        field: 'IsDeleted',
        //title: [languagePlugin.GetPageLanguage(languagePlugin.AllLanguageUrl, 'Common_Label_IsDeleted', "是否删除")],
        align: 'center',
        width: '50px',
        formatter: function (value, row, index) {
            var deleteHtml = '<div class="switch">' +
                '<div class="checkbox" style="margin-top: 0px;  margin-bottom: 0px; ">' +
                '<input type="checkbox" ' + (value == 1 ? 'checked' : '') + ' onclick="deleteItem(\'' + row.Id + '\',\'' + value + '\',this)">' +
                '<label></label>' +
                '</div>' +
                '</div>';
            return deleteHtml;
        }
    },
    DeleteConfirm: function (id, value, props, tableId, deleteUrl) {
        var status = ($(props).is(':checked'));
        value = value == 1 ? 2 : 1;
        var result = false;
        layer.confirm('确认操作', {
            shadeClose: true,
            title: ["提示"],
            btn: ["确定","取消"],
            skin: 'layersmall',
            area: ['345px', '200px'],
            btn1: function (index) {
                $.ajax({
                    url: deleteUrl,
                    type: 'POST',
                    async: false,
                    data: {
                        id: id,
                        value: value
                    },
                    success: function (data) {
                        result = data.Success;
                        if (result) {
                            $("#" + tableId).bootstrapTable('refresh');
                            layer.close(index);
                        }
                    },
                    error: function (data) {

                    }
                });
            },
            btn2: function () {
                $(props).prop('checked', !status);
            },
            end: function () {
                $(props).prop('checked', !status);
            }
        });
    }
}