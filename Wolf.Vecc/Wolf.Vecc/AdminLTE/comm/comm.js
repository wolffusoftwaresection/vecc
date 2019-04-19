

var comm = {
    jsonTimePaser: function (time) {
        return timeStamp2String(time);
    },
    GetCity: function (provinceId, container, selectCityName) {
        ajax.Post("SysData/GetCity", { _provinceId: provinceId }, function (data) {
            if (data.Success) {
                var list = data.List;
                var htm = '<option value="0">请选择</option>';
                for (var i = 0; i < list.length; i++) {
                    if (list[i].CityName == selectCityName) {
                        htm += '<option selected="selected" value="' + list[i].CityID + '">' + list[i].CityName + '</option>';
                    }
                    else {
                        htm += '<option value="' + list[i].CityID + '">' + list[i].CityName + '</option>';
                    }
                }
                $("#" + container).html(htm);
            }
        })
    }
}
function timeStamp2String(time) {
    if (time) {
        var datetime = new Date(parseInt(time.replace("/Date(", "").replace(")/", ""), 10));
        var year = datetime.getFullYear();
        var month = datetime.getMonth() + 1 < 10 ? "0" + (datetime.getMonth() + 1) : datetime.getMonth() + 1;
        var date = datetime.getDate() < 10 ? "0" + datetime.getDate() : datetime.getDate();
        var hour = datetime.getHours() < 10 ? "0" + datetime.getHours() : datetime.getHours();
        var minute = datetime.getMinutes() < 10 ? "0" + datetime.getMinutes() : datetime.getMinutes();
        var second = datetime.getSeconds() < 10 ? "0" + datetime.getSeconds() : datetime.getSeconds();
        return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
    }
}
Number.prototype.toPercent = function () {
    return (Math.round(this * 10000) / 100).toFixed(2) + '%';
}
function CommCreateImgs(container, data, hiddenId, isPrevDelete) {
    if (isPrevDelete) {
        container.children(".uploadFile").remove();
    }
    var htm = ""
    var list = data.List;
    var url = data.Url;
    for (var i = 0; i < list.length; i++) {
        htm += "<div style='position:relative;display:inline-block;'> <img class='uploadFile'  onmouseover='CommDeleteImg(this,event)'  src=" + url + list[i] + " width='87' height='87'  />";
        htm += "<div style='position:absolute;width:100%;bottom:0;left:0px;'>";
        htm += "<progress value='0' max='100' style='width:87px;'></progress>";
        htm += "</div></div>";
    }
    container.prepend(htm);
}
function SetImgUrlInHidden(hiddenElement, data) {
    var list = data.List;
    var url = data.Url;
    var imgUrl = url + list[0];
    hiddenElement.val(hiddenElement.val() + imgUrl + ",");

}
function ReSetImgUrlInHidden() {

    var _container = $(".delete-flag").parent().parent();
    var hiddenElement = _container.find("input[type=hidden]")[0];
    var _removeUrl = $(".delete-flag").attr("title");
    hiddenElement.value = hiddenElement.value.replace(_removeUrl, "");
    //var _files = _container.find(".uploadFile");
    //var _removeIndex = -1;
    //for (var i = 0; i < _files.length; i++) {
    //    if ($(_files[0]).hasClass("delete-flag")) {
    //        _removeIndex = _files.length-i;
    //    }
    //}

    //var _urls = hiddenElement.value.substring(0, hiddenElement.value.length-1).split(",");
    //_urls.splice(_removeIndex);
    //var _newUrl = "";
    //for (var i = 0; i < _urls.length; i++) {
    //    _newUrl += _urls[i];
    //}
    //hiddenElement.value = _newUrl;
}
function CommDeleteImg(obj, e) {
    var offSet = $(obj).offset();
    var _width = $(obj).width();
    var _height = $(obj).height();
    $(obj).addClass("delete-flag")
    var _scrollHeight = $(document).scrollTop();
    if (!$("#divDeleteImg").length) {
        var divHml = '<div id="divDeleteImg"  style="text-align:center; line-height:' + _height + 'px; width:' + _width + 'px;height:'
            + _height + 'px; font-size:50px;position:absolute;top:' + (offSet.top) + 'px;left:' + offSet.left
            + 'px"> <span  type="button" value="删除">×</span><div style="Position: absolute;background-color: black;width:' +
              _width + 'px;height:' + _height + 'px;top:0;left:0;opacity: 0.3;z-index:1;" onclick="ReSetImgUrlInHidden(); $(\'.delete-flag\').parent().remove();$(\'#divDeleteImg\').remove();"   onmouseout=" $(\'#divDeleteImg\').remove();$(\'.delete-flag\').removeClass(\'delete-flag\'); "></div></div>';
        $('body').append(divHml);
    }
}
function CommCheckBoxSingle(container, groupName) {
    if (groupName) {
        container.find("input[type=checkbox][name=" + groupName + "]").each(function (index, element) {
            $(document).on("click", 'input', function () {
                $(this).attr("checked", true).siblings().attr("checked", false);
            })
        })
    }
    else {
        container.find("input[type=checkbox]").each(function (index, element) {
            $(document).on("click", 'input', function () {
                $(this).attr("checked", true).siblings().attr("checked", false);
            })
        })
    }
}
function ShowImg(imgName, obj, e) {
    var offSet = $(obj).offset();
    lastX = e.pageX;
    lastY = e.pageY;
    if (!$("#divShowImg").length) {
        var divHml = '<div id="divShowImg" class="position-fixed width-20-percent display-none">';
        divHml += '<img id="divShowImg_img" class="width-100-percent" /></div>'
        $('body').append(divHml);
    }
    $("#divShowImg_img").attr("src", imgName).parent().css({ "top": lastY + 10, "left": lastX + 10 }).removeClass("display-none")
}
function MoveImg(e) {
    moveX = e.pageX - lastX;
    moveY = e.pageY - lastY;
    lastX = e.pageX;
    lastY = e.pageY;
    $("#divShowImg").css({ "top": lastY + 10, "left": lastX + 10 });
}
function HideImg() {
    $("#divShowImg").addClass("display-none")
}

function com_request(paras, url) {
	if (arguments.length == 1)
		url = window.location.href;
	var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
	var paraObj = {};
	for (var i = 0; i < paraString.length; i++) {
		paraObj[paraString[i].substring(0, paraString[i].indexOf("=")).toLowerCase()] = paraString[i].substring(paraString[i].indexOf("=") + 1, paraString[i].length);
	}
	var returnValue = paraObj[paras.toLowerCase()];
	if (typeof (returnValue) == "undefined") {
		return "";
	} else {
		return returnValue;
	}
}

function searchUserList(textID, valueID) {
    //textID：显示用户名的文本框ID
    //var textID = 'txtUserName';
    //valueID：用于存放用户ID的文本框ID
    //var valueID = 'UserID';
    layer.open({
        type: 2,
        title: ['人员列表'],
        shadeClose: true,
        skin: 'layerlarge',
        area: ['70%', '98%'],
        content: ['/Admin/Common/SearchUserList?textID=' + textID + '&valueID=' + valueID, 'yes']
    });
}

function searchMoreUserList(textID, valueID,Ids) {
    layer.open({
        type: 2,
        title: ['人员列表'],
        shadeClose: true,
        skin: 'layerlarge',
        area: ['70%', '98%'],
        content: ['/Admin/Common/SearchMoreUserList?textID=' + textID + '&valueID=' + valueID + "&UserIds=" + Ids, 'yes']
    });
}

var fileUploadPlugins = {
    /**
    * 文件上传
    * @param {} type 0文件类型不受限制，1图片类型
    */
    uploadFiles: function (type, fileDirectory) {
        layer.open({
        type: 2,
        title: ['选择文件'],
        shadeClose: true,
        btn: [],
        skin: 'layerlarge',
        area: ['90%', '98%'],
        maxmin: true,
        //fileType":0文件类型不受限制，1图片类型
        content: ['/Admin/Common/Upload?fileType=' + type + '&fileDirectory=' + fileDirectory, "yes"]
        });
    },
    /**
   * 删除图片
   */
    deleteImg: function (obj) {
        var imgpath = $(obj).prev().attr("src");
        //$.ajax({
        //    url: "/Admin/Common/DeleteFile",
        //    data: { path: imgpath },
        //    type: "post",
        //    success: function (data)
        //    {
        //        $(obj).parent().remove();
        //    },
        //    error: function (data) {

        //    }
        //});
        $(obj).parent().remove();
    },
    //删除文件
    deleteFile: function (obj) {
        $(obj).parent().parent().remove();
    },
    /**
   * 获取文件ID
   */
    getFileID: function ()
    {
        debugger
        var fileIDs = "";
        $.each($("#imgShowDiv .fileShow"),
            function (index, item) {
                fileIDs += $(item).attr("fileID")+",";
            });
        if (fileIDs != "")
        {
            fileIDs = fileIDs.substr(0, fileIDs.length-1);
        }
        return fileIDs;
    },
    /**
   * 初始化图片
   */
    initFile: function (id, TableName) {
        $.ajax({
            url: "/File/GetFile?TableName="+TableName,
            data: { id: id },
            type: "post",
            success: function (data) {
                var html = "";
                for (var i = 0; i < data.length; i++) {
                    html += "<div class='fileItem'>";
                    html += "<img src='" + data[i].FilePath + "' fileID='" + data[i].Id + "' class='imgShow'>";
                    html += "<i class='fa fa-trash status' onclick='fileUploadPlugins.deleteImg(this)'></i></div>";
                }
                $("#imgShowDiv").html(html);
            },
            error: function (data) {

            }
        });

    },
    /**
   * 初始化文件
   */
    initFileList: function (id, TableName) {
        var flag = false;
        $.ajax({
            url: "/File/GetFile?TableName=" + TableName,
            data: { id: id },
            type: "post",
            success: function (data) {
                var html = '<table class="table table-hover table-bordered table-striped">';
                html += '<thead><tr>';
                html += '<th>文件名</th>';
                html += '<th>操作</th>';
                html += '</tr></thead>';
                html += '<tbody>';
                for (var i = 0; i < data.length; i++) {
                    html += '<tr><td>' + data[i].FileName + '</td><td><a fileID=\'' + data[i].Id + '\' class="fileShow" href="/Admin/Common/DownLoadUrlFile?filePath=' + data[i].FilePath + '&fileName=' + data[i].FileName + '">下载</a></td></tr>';
                    flag = true;
                }
                html += '</tbody></table>';
                $("#imgShowDiv").html(html);
            },
            error: function (data) {

            }
        });
        return flag;
    },
    /**
  * 初始化图片
  */
    initMoreFile: function (id, TableName) {
        $.ajax({
            url: "/File/GetFile?TableName=" + TableName,
            data: { id: id },
            type: "post",
            success: function (data) {
                var html = "";
                for (var i = 0; i < data.length; i++) {
                    html += "<div class='fileItem'>";
                    html += "<img src='" + data[i].FilePath + "' fileID='" + data[i].Id + "' class='imgShow'>";
                    html += "</div>";
                }
                $("#imgShowDiv_" + id).html(html);
            },
            error: function (data) {

            }
        });

    }
}
