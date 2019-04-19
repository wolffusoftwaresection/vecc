var commonListPlugin = {
    /**
      * @param {} titleName 标题
      * @param {} addBtnFuncName 新增方法  例："add('0')"
      * @param {} tableId 表格ID  
      * @param {} treeDivId 树容器ID 
      */
    BuilderListHtml: function (titleName, addBtnFuncName, tableId, treeDivId) {
        var listHtml = '<section class="content-header">' +
    '<ol class="breadcrumb" id="title_' + titleName + '"></ol>' +
    '<script>' +
        '$("#title_' + titleName + '").html(\'<li><a>\' +' +
            '$(".tab-panel.active").attr("parentname") +' +
            '\'</a></li><li class="active">\' +' +
            '$(".tab-panel.active").attr("name") +' +
            '\'</li>\');' +
    '</script>' +
        '</section>' +
'<div class="content tsmodal-content" id="pageSection" style="background: white; min-height: 10px;">' +
    '<div class="row">' +
        '<div class="col-lg-12" id="toolbar_btns">' +
            '<button type="button" class="btn btn-default btn-flat" data-toggle="collapse" data-parent="#accordion" href="#collapseToolSearch" data-language="Common_Btn_SearchFilter">筛选项</button>' +
            '<button type="button" id="btn_create" class="btn btn-dropbox btn-flat" onclick="' + addBtnFuncName + '" style="margin-left:5px;" data-language="Common_Btn_New">新增</button>' +
        '</div>' +
        '<div class="col-lg-12">' +
                '<div id="collapseToolSearch" class="panel-collapse collapse">' +
                    '<div class="border-dashed">' +
                        '<form class="form-horizontal">' +
                            '<div class="box-body">' +
                                '<div class="row" id="SearchList">' +

                                '</div>' +
                            '</div>' +
                            '<div class="box-footer pull-right">' +
                                '<button type="button" class="btn btn-dropbox btn-flat" id="screeningBtn" onclick="screeningSearch()" data-language="Common_Btn_Search">筛选</button>' +
                            '</div>' +
                        '</form>' +
                    '</div>' +
                '</div>' +
        '</div>' +
    '</div>' +
        '</div>' +
'<div class="box-body" style="padding:15px;">' +
            '<div id="toolbar_' + tableId + '">' +
            ' <div class="columns columns-right btn-group pull-left" style="margin-top: 10px; margin-left: 10px;">' +
            '<button class="btn btn-default" id="' + tableId + '_Stop" type="button" name="refresh" title="停用" data-language="Common_Button_Stop" onclick="">停用</button>' +
            '<button class="btn btn-default" id="' + tableId + '_Start" type="button" name="toggle" title="启用" data-language="Common_Button_Open" onclick="">启用</button>' +
            '</div>' +
            '</div>' +
            '<div class="row">' +
            ((treeDivId == '' || treeDivId == null) ? '' : '<div id="' + treeDivId + '" class="col-lg-3">' +
                '' +
                '</div>') +
                '<div class="' + ((treeDivId == '' || treeDivId == null) ? "col-lg-12" : "col-lg-9") + '">' +
            '<table id="' + tableId + '" class="table-no-bordered"></table>' +
            '</div>' +
            '</div>' +

        //'<table id="' + tableId + '" class="table-no-bordered"></table>' +
'</div>';
        $("#tab-content").append(listHtml);
    }
}