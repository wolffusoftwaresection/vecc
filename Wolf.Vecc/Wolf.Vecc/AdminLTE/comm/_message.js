var msg = {
    confirm: function (_msg, _confirmCallback, _cancelCallback) {
        if (_cancelCallback) {
            layer.confirm(_msg, { cancel: _cancelCallback }, _confirmCallback);
        }
        else {
            layer.confirm(_msg, _confirmCallback);
        }

    },
    info: function (msg, title) {
        if (!title) {
            title = "提示";
        }
        layer.open({
            title: title,
            content: msg
        });
    },
    error: function (msg, title) {
        if (!title) {
            title = "提示";
        }
        layer.open({
            skin: 'layersmallerror',
            title: title,
            content: msg
        });
    },
    load: function (url, title,callback) {
        var width = "70%";
        var height = "80%";
        if ($(window).width() <= 768) {
            width = "100%";
            height = "100%";
        }

        var len = arguments.length;
        var id = "";
        if (len == 3)
        id = 'uploadiframe';
        layer.open({
            type: 2,
            title: title,
            content: path + url,
            id:id,
            maxmin: true,
            area: [width, height],
            moveOut: true,
            //move: false,
            cancel: function () {
            	if (len == 3) {
            		callback();
            	} else {
            		//layer.closeAll();
            	}

            }
        });

        return false;
    },
    sizeload: function (url, title,wh, callback) {
        var width = wh[0] + '%';
        var height = wh[1] + '%';
        //console.log(wh);
        if ($(window).width() <= 768) {
            width = "100%";
            height = "100%";
        }

        var len = arguments.length;
        var id = "secontWin";
        if (len == 4)
            id = 'uploadiframe';
        layer.open({
            type: 2,
            title: title,
            content: path + url,
            id: id,
            maxmin: true,
            area: [width, height],
            moveOut: true,

            btn: ["确定","取消"],
            cancel: function () {
                if (len == 4) {
                    callback();
                } else {
                    //layer.closeAll();
                }

            },
            yes: function (index,layero) { 
                var iframeid = $("#" + id).find("iframe").attr("id"); 
               $("#" + iframeid).contents().find(".submitbutton").click();
                //$("#" + iframeid).contents().find("form").submit();
                //layer.close(index);
            }
        });

        return false;
    },
    close: function () {
    	
        var index = parent.layer.getFrameIndex(window.name);
        parent.layer.close(index);

    }

}

 