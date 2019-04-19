var commonMethod = {
    /**
     * 获取当前登录用户信息
     * @returns {} 
     */
    GetWorkUser: function () {
        var workUser = {};
        $.ajax({
            url: '/Admin/Common/GetWorkUserInfo',
            type: 'POST',
            async: false,
            success: function (data) {
                workUser = data.WorkUser;
            }
        });
        return workUser;
    },
    /**
     * 
     * @param {} id 
     * @param {} expressCode 
     * @returns {} 
     */
    GetExpressInfo: function (id, expressCode) {
        $.ajax({
            url: '/Admin/Common/ExpressCheck',
            type: 'POST',
            //async: false,
            data: {
                expressCode: expressCode
            },
            success: function (e) {
                console.log(e.success);
                $("#div_" + id).show();
                var out = "";
                if (e.success) {
                    out += '<ul class="state">';

                    for (var i = e.rows.length - 1; i >= 0; i--) {
                        var c = "";
                        o = "";
                        if (i == e.rows.length - 1) {
                            c = "firstChild current1";
                            o = "on"
                        }
                        out += '<li class="pr ' + c + '">';
                        var ch = e.rows[i]["AcceptStation"].replace(/\[/g, '<span style="color:red">[')
                            .replace(/\]/g, ']</span>');
                        out += '<div class="' + o + '">' + ch + '</div>';
                        out += '<div class="time">' + e.rows[i]["AcceptTime"] + '</div>';
                        out += '</li> ';
                    }
                    out += '    </ul>';
                } else {
                    out = "<div style='padding:20px '>：( 该单号暂无物流进展，请稍后再试，或检查公司和单号是否有误。</div>";
                }
                $("#img_" + id).html(out);
            }
        });
    },
    /**
     * 快递追踪详情HTML
     * @param {} id 
     * @param {} expressCode 快递单号
     * @returns {} 
     */
    ExpressHtml: function (id, expressCode) {
        var html = '<div class="input-group">' +
                      '<span class="input-group-addon">快递单号</span>'+
                     '<input type="text" id="' + id + '" class="form-control" disabled value="' + expressCode + '">' +
                        '<div class="input-group-btn">' +
                            '<button type="button" class="btn btn-default btn-flat" onclick="commonMethod.GetExpressInfo(\'' + id + '\',\'' + expressCode + '\')"><i class="fa  fa-search"></i></button>' +
                            '<div class="btn-group">' +
                                //'<button type="button" class="btn btn-default btn-flat dropdown-toggle" data-toggle="dropdown" aria-expanded="false" id="toggle_' + id + '" onclick="toggleDiv(this)">' +
                                //    '<span class="caret"></span>' +
                                //'</button>' +
                            '</div>' +
                         '</div>' +
                    '</div>' +
                    '<div id="div_' + id + '" style="display:none;"> <br>' +
                        '<div class="file-preview" style="border: 1px dashed #ddd">' +
                            '<div class=""> ' +
                                '<div class="file-preview-thumbnails" id="img_' + id + '">  ' +
                                '</div>' +
                                '<div class="clearfix">' +
                                '</div>' +
                            '</div>' +
                        '</div>' +
                    '</div>';
        return html;
    }

}