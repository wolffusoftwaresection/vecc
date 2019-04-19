var ajax = {
	post: function (url, data, callback) {
        $.ajax({
            type: 'POST',
            url: path + url,
            data: data,
            dataType: "json",
            cache: false,
            beforeSend: function () {
                ajax.loading();
            },
            success: function (data) {
                if (callback)
                    callback(data);
                else
                    msg.info(data.Msg);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ajax.loadingClose();
                msg.error(errorThrown);
            },
            complete: function () {
                ajax.loadingClose();
            }
        });
    },
    get: function (url, data, callback) {
        ajax.loading();
        $.get(path + url, data, function (data) {
            callback(data);
            ajax.loadingClose();
        })
    },
    dele: function (url, callback) {
        $.ajax({
            type: 'DELETE',
            url: path + url,
            dataType: "json",
            cache: false,
            beforeSend: function () {
                ajax.loading();
            },
            success: function (data) {
                if (!callback) {
                    callback(data);
                }
                msg.info(data.Msg);
                return false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ajax.loadingClose();
                msg.error(errorThrown);
            },
            complete: function () {
                ajax.loadingClose();
            }
        });
    },
    loading: function () {
        ajax.loadingClose();
        layer.load();
    },
    loadingClose: function () {
        layer.closeAll('loading');
    },
    searchSuccess: function (data) {
    },
    searchFailure: function (data) {
    },
    updateSuccess: function (data) {
        if (data.Success) {
            ajax.onReload(data);
            msg.close();
        }
        else {
            msg.info(data.Msg);
            return false;
        }
    },
    updateFailure: function (data) {
        msg.error(data.Msg);
    },
    onReload: function (data) {
    	
    	if (data.Success) {
    		if (window.parent.$("#templatequeryid").length > 0) {
    			window.parent.$("#" + window.parent.$("#templatequeryid").val()).click();
    		} else if ($("#templatequeryid").length > 0) {
    			$("#" + $("#templatequeryid").val()).click();
    		}else if (window.parent.$('#searchForm').length > 0)
                window.parent.$('#searchForm').submit();
            else
                $('#searchForm').submit();
        }
        else
    	    msg.error(data.Msg);

    	$("#templatequeryid").remove();
    	window.parent.$("#templatequeryid").remove();
    },
    onPageSizeChange: function () {
        $("#pagesize").val($("#pagesizeddl").val());
        $('#searchForm').submit();
    },
    File: function (form, url, callback) {
        //  var form = $("#"+formId);
        var url = path + url;
        var options = {
            url: url,
            iframe: true,
            dataType: 'text',
            type: "post",
            success: function (data) {
                ajax.LoadingEnd();
                var _list = $.parseJSON(data).Data;
                if (_list.Success) {
                    callback(_list);
                }
                else {
                    msg.error(_list.Msg);
                }
            }
        };
        ajax.Loading();
        form.ajaxSubmit(options);
    },
    filefromdata: function (files, url, obj, callback) {
        var baseFiles = files;
        var container = $(obj).parent();
        for (var i = 0; i < files.length; i++) {
            lrz(files[i], {
                width: 1920,
                quality: 70
            })
         .then(function (rst) {
             var _lrzImg = rst.base64;
             var _imgHtm = "";
             _imgHtm += "<div style='position:relative;display:inline-block;'> <img class='uploadFile'  onmouseover='CommDeleteImg(this,event)'  src=" + _lrzImg + " width='87' height='87'  />";
             _imgHtm += "<div style='position:absolute;width:100%;bottom:0;left:0px;'>";
             _imgHtm += "<progress value='0' max='100' style='width:87px;'></progress>";
             _imgHtm += "</div></div>";
             container.prepend(_imgHtm);
             var progress = container.find("progress")[0];
             var xhr = new XMLHttpRequest();
             xhr.open('POST', path + url);
             xhr.onload = function () {
                 var data = JSON.parse(xhr.response);
                 if (xhr.status === 200) {
                     // 上传成功
                     //img.src = rst.base64;
                     var list = data.List;
                     var url = data.Url;
                     var imgUrl = url + list[0];
                     callback(data);
                     progress.value = 100;

                     container.find(".uploadFile").attr("title", imgUrl + ",");
                 } else {

                     // 处理错误
                     progress.value = 0;
                 }
             };
             if (xhr.upload) {
                 try {
                     xhr.upload.addEventListener('progress', function (e) {
                         if (!e.lengthComputable) return false;
                         // 上传进度
                         progress.value = ((e.loaded / e.total) || 0) * 100;
                         if (progress.value == 100) {
                             $(progress).addClass("hidden");
                         }
                     });
                 } catch (err) {
                     console.error('进度展示出错了,似乎不支持此特性?', err);
                 }
             }
             xhr.send(rst.formData);
         });
        }
    },
    filefromdataCut: function (options, url, obj, callback) {
        var data = new FormData();
        var _base64 = options.base64;

        data.append("fileBase64", _base64);
        var _hiddenId = options.hiddenId;
        var container = $("#" + _hiddenId).parent();
        var progress = container.find("progress")[0];
        var xhr = new XMLHttpRequest();
        xhr.open('POST', path + url);
        xhr.onload = function () {
            var data = JSON.parse(xhr.response);
            if (xhr.status === 200) {
                // 上传成功
                //img.src = rst.base64;
                var list = data.List;
                var url = data.Url;
                var imgUrl = url + list[0];
                callback(data);
                progress.value = 100;

                $("#" + _hiddenId).attr("title", imgUrl + ",");
            } else {

                // 处理错误
                progress.value = 0;
            }
        };
        if (xhr.upload) {
            try {
                xhr.upload.addEventListener('progress', function (e) {
                    if (!e.lengthComputable) return false;
                    // 上传进度
                    progress.value = ((e.loaded / e.total) || 0) * 100;
                    if (progress.value == 100) {
                        $(progress).addClass("hidden");
                    }
                });
            } catch (err) {
                console.error('进度展示出错了,似乎不支持此特性?', err);
            }
        }
        xhr.send(data);
    },
    createKindEditor: function (selector) {
        KindEditor.create(selector, {
            uploadJson: o2o.getFullPath('/Kindeditor/FileUpload'),
            fileManagerJson: o2o.getFullPath('/Kindeditor/FileManager'),
            allowFileManager: true,
            urlType: 'domain',
            afterCreate: function () {
                this.sync();
            },
            afterBlur: function () {
                this.sync();
            }
        });
    },
    dataExport: function (url) {

        var _data = $("#btnExport").parents("form").serialize() + "&IsExport=true";

        ajax.post(url, _data, function (data) {
            if (data.Success)
                location.href = data.Url;
        })

    },
    dataImport: function (files, url) {
        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append("file" + i, files[i]);
        }
        var xhr = new XMLHttpRequest();
        xhr.open('POST', path + url);
        xhr.onload = function () {
            ajax.loadingClose();
            var data = JSON.parse(xhr.response);
            if (xhr.status === 200) {
                $("#searchForm").submit();
                msg.info(data.Msg);

            } else {
                msg.error("导入失败");
            }
        };

        ajax.loading()
        xhr.send(data);
    }

}