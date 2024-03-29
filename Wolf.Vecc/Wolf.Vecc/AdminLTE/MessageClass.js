﻿function MessageC(msgNum, messageMenu, data) {
    if (!(this instanceof MessageC)) {
        return new MessageC(msgNum, messageMenu, data);
    }
    BaseList.apply(this, arguments);
    this.clearMsgList = function () {
        $(msgNum).empty();
        $(messageMenu).empty();
    }
    this.init();
}

MessageC.prototype = new BaseList();
MessageC.prototype.initElements = function () {
    this.clearMsgList();
    this.msg = '<li id={0}><a href="#"><i class="fa fa-users" style="color:#00c0ef;"></i>{1}</a></li>';
    this.createMsgList(this.data);
}
MessageC.prototype.initEvent = function ($ent) {
    var that = this;
    $ent.on("click", function (event) {
        //$(this).find('i').css("color", "#00c0ef");
        var _numNew = $(that.msgNum).text() - 1;
        $(that.msgNum).empty();
        if (_numNew > 0) {
            $(that.msgNum).append(_numNew);
        }
        $(this).remove();
        TableDetailed.ShowDetailed(layer, "/Admin/Message/Detailed", $(this).attr("Id"));
    });
}
MessageC.prototype.createMsgList = function () {
    var that = this;
    if (arguments.length > 0) {
        (function (arguments, that) {
            var _msgNum = arguments[0].rows.length;
            if (_msgNum) {
                if (_msgNum > 0) {
                    $(that.msgNum).append(_msgNum);
                    var i = 0;
                    while (i < _msgNum) {
                        var _msgStr = $(that.msg.format(arguments[0].rows[i].Id,
                            arguments[0].rows[i].MessageTitle));
                        that.initEvent(_msgStr);
                        $(that.messageMenu).append(_msgStr);
                        i++;
                    }
                }
                else {
                    $(that.msgNum).append("0");
                }
            }
        })(arguments, that);
    }
}
MessageC.prototype.init = function () {
    this.initElements();
}

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
    function (m, i) {
        return args[i];
    });
}

String.format = function () {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}