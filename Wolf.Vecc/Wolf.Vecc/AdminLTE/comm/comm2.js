var ajax = {
    Post: function (url, data, callback) {
        $.ajax({
            type: 'POST',
            url: path + url,
            data: data,
            //dataType: "json",
            cache: false,
            beforeSend: function () {
                ajax.Loading();
            },
            success: function (data) {
                if (callback) {
                    callback(data);
                }
                else {
                    msg.info(data.Msg);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ajax.LoadingEnd();
                msg.confirm("网络连接超时..", function () {

                    location.href = location.href;
                })
                // console.log("Status：" + XMLHttpRequest.status + ",readyState：" + XMLHttpRequest.readyState + ",textStatus：" + textStatus);
            },
            complete: function () {
                ajax.LoadingEnd();
            }
        });
    },
    Get: function (url, data, callback) {
        $.ajax({
            type: 'GET',
            url: path + url,
            data: data,
            //dataType: "json",
            cache: false,
            beforeSend: function () {
                ajax.Loading();
            },
            success: function (data) {
                if (callback) {
                    callback(data);
                }
                else {
                    msg.info(data.Msg);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ajax.LoadingEnd();
                msg.confirm("网络连接超时..", function () {

                    location.href = location.href;
                })
            },
            complete: function () {
                ajax.LoadingEnd();
            }
        });
    },
    loading: function () {
        var htm = '<div id="divLoading" class="position-absolute top-0 left-0 width-100-percent text-align-center height-100-percent"  style="z-index:9999;" >';
        htm += '<img src="' + path + 'Images/comm/load3.jpg" style="position:fixed;top:48%;left:45%"  />';
        htm += '</div>';
        $("body").append(htm);
    },
    loadingEnd: function () {
        $("#divLoading").remove();
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
                if (data.Success) {
                    callback(data);
                }
                else {
                    msg.error(data.Msg);
                }
            }
        };
        ajax.Loading();
        //form.setAttribute('encoding', 'multipart/form-data');
        //form.setAttribute('enctype', 'multipart/form-data');
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
    }
}
var msg = {
    confirm: function (msg, callback) {
        $.confirm({
            title: '友情提醒',
            backgroundDismiss: false,
            theme: 'white',
            content: msg,
            confirmButton: '确认',
            cancelButton: '取消',
            //autoClose: 'cancel|4000',
            confirm: function () {
                callback();
            },
            cancel: function () {
            }
        });
    },
    confirmPayWx: function (msg, callback) {
        $.confirm({
            title: '友情提醒',
            backgroundDismiss: false,
            theme: 'white',
            content: msg,
            confirmButton: '付款完成',
            cancelButton: '取消',
            //autoClose: 'cancel|4000',
            confirm: function () {
                callback();
            },
            cancel: function () {
            }
        });
    },
    error: function (msg, callback) {
        $.alert({
            title: '友情提醒',
            //backgroundDismiss: false,
            theme: 'white',
            content: msg,
            confirmButton: '确认',
            confirm: function () {
            }
        });
    },
    info: function (msg, callback) {
        var msgObj = {
            title: '友情提醒',
            //backgroundDismiss: false,
            theme: 'white',
            content: msg,
            confirmButton: '确认',
            confirm: function () {
                if (callback) {
                    callback();
                }
            }
        };
        if (callback) {
            msgObj.columnClass = "col-md-12";
        }
        // $.alert(msgObj);
    },
    infoLg: function (msg, callback) {
        var msgObj = {
            title: '友情提醒',
            //backgroundDismiss: false,
            theme: 'white',
            columnClass: "col-md-6 col-md-offset-3",
            content: msg,
            confirmButton: '确认',
            confirm: function () {
                if (callback) {
                    callback();
                }
            }
        };
        if (callback) {
            msgObj.columnClass = "col-md-12";
        }
        //  $.alert(msgObj);
    },
    load: function (url, title) {
        $.confirm({
            content: "url:" + path + url,
            title: title,
            contentLoaded: function (data) {
            }
        });
    },
    loadHtml: function (title, htm, callback) {
        $.confirm({
            title: title,
            backgroundDismiss: false,
            columnClass: "col-md-12",
            theme: 'white',
            content: htm,
            confirmButton: '确认',
            cancelButton: '取消',
            confirm: function () {
                if (callback) {
                    callback();
                    return false;
                }
            }
        });
    },
    loadSelect: function (title, htm, callback) {
        $.confirm({
            title: title,
            backgroundDismiss: false,
            columnClass: "col-md-8 col-md-offset-2",
            theme: 'white',
            content: htm,
            confirmButton: '确认',
            cancelButton: '取消',
            confirm: function () {
                if (callback) {
                    callback();
                }
            }
        });
    },
    closeAll: function () {
        var _jconfirmLength = $('.jconfirm').length;
        $('.jconfirm').remove();
        jconfirm.record.currentlyOpen -= _jconfirmLength;
        if (jconfirm.record.currentlyOpen < 1) {
            $('body').removeClass('jconfirm-noscroll')
        }
    }
}
var regex = {
    IsDecimal: function (str) {
        var reg = /^[0-9]+\.{0,1}[0-9]{0,2}$/;
        return reg.test(str);
    }

}
var comm = {
    jsonTimePaser: function (time) {
        return timeStamp2String(time);
    },
    GetCity: function (provinceId, container, selectCityName) {
        if (!provinceId) {
            return;
        }
        ajax.Post("SysData/GetCity", { _provinceId: provinceId }, function (data) {
            if (data.Success) {
                var list = data.List;
                var htm = '<option value="">请选择</option>';
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
    },
    clickAddAnimate: function (obj) {
        if (obj) {
            var htm = '<div id="div-click-antimate" style="color:#00ffff;position:absolute;font-size:16px;"> +1</div>';
            $("body").append(htm);
            var _top = $(obj).offset().top;
            var _left = $(obj).offset().left;
            $("#div-click-antimate").css({ "top": _top + "px", "left": _left + 10 + "px" }).addClass("niceIn");
            setTimeout(function () {
                $("#div-click-antimate").remove();
            }, 700);
        }
    },
    getNowFormatDate: function () {
        var date = new Date();
        var seperator1 = "-";
        var seperator2 = ":";
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
                + " " + date.getHours() + seperator2 + date.getMinutes()
                + seperator2 + date.getSeconds();
        return currentdate;
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
String.prototype.isDateBig = function (endDate) {
    var _end = new Date(this.replace("-", "/").replace("-", "/"));
    var _start = new Date(endDate.replace("-", "/").replace("-", "/"));
    return _end >= _start
}
function CommCreateImgs(container, data, width, isPrevDelete) {
    if (isPrevDelete) {
        if (data.Success) {
            container.children(".uploadFile").remove();
        }
    }
    var htm = ""
    var list = data.List;
    var url = data.Url;
    for (var i = 0; i < list.length; i++) {
        htm += "<img class='uploadFile'  onmouseover='CommDeleteImg(this,event)'  src='" + url + list[i] + "' style='width:" + width + "px;height:" + width + "px;' />";
    }
    container.prepend(htm).find("input[type=file]").val("");
}
function CommDeleteImg(obj, e) {
    var offSet = $(obj).offset();
    var _width = $(obj).width();
    var _height = $(obj).height();
    $(obj).addClass("delete-flag")
    var _scrollHeight = $(document).scrollTop();
    if (!$("#divDeleteImg").length) {
        var divHml = '<div id="divDeleteImg"  style="z-index:9999; text-align:center; line-height:' + _height + 'px; width:' + _width + 'px;height:'
            + _height + 'px; font-size:50px;position:absolute;top:' + (offSet.top) + 'px;left:' + offSet.left
            + 'px"> <span  type="button" value="删除">×</span><div style="Position: absolute;background-color: black;width:' +
              _width + 'px;height:' + _height + 'px;top:0;left:0;opacity: 0.3;z-index:1;" onclick=" $(\'.delete-flag\').remove();$(\'#divDeleteImg\').remove();"   onmouseout=" $(\'#divDeleteImg\').remove();$(\'.delete-flag\').removeClass(\'delete-flag\'); "></div></div>';
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
String.prototype.trim = function () {
    return this.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
}
$(function () {
    $("input[type=number][maxlength]").keydown(function (e) {
        if (this.value.length >= this.maxLength && e.keyCode != 8 && e.keyCode != 46) {

            return false;
        }

    }).keyup(function () {
        if (this.value.length > this.maxLength) {
            this.value = this.value.substring(0, this.maxLength);
        }

    })
    $("input[type=number]").keydown(function (e) {
        if (e.keyCode == 109 || e.keyCode == 189) {
            return false;
        }
    })
})