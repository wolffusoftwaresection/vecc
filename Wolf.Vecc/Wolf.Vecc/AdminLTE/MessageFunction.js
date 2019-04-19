function MessageClass(msgNum, messageMenu,msgDown, num) {
    if (!(this instanceof MessageClass)) {
        return new MessageClass(msgNum, messageMenu,msgDown, num);
    }
    this.msgNum = msgNum;
    this.messageMenu = messageMenu;
    this.num = num || 0;
    this.msgDown = msgDown;
    this.clearMsgList = function () {
        $(msgNum).empty();
        $(messageMenu).empty();
    }
    this.init();
}

MessageClass.prototype = {
    init: function () {
        this.initElements();
        this.initEvent();
    },
    initElements: function () {
        this.clearMsgList();
        this.msg = '<li id={0} title="{1}"><a href="#"><i class="fa fa-users" style="color:#00c0ef;"></i>{2}</a></li>';
        //if (this.num > 0) {
        this.setMsgNum(this.num);
        //}
    },
    getMsgNum: function () {
        return $(this.msgNum).text();
    },
    setMsgNum: function (_msgNum) {
        var that = this;
        $(that.msgNum).empty();
        $(that.msgNum).append(_msgNum);
    },
    createMsgList: function () {
        var that = this;
        if (arguments.length > 0) {
            (function (arguments, that) {
                var _msgNum = arguments[0].length;
                if (_msgNum) {
                    if (_msgNum > 0) {
                        var i = 0;
                        while (i < _msgNum) {
                            var _msgStr = $(that.msg.format(arguments[0][i].MsgId,
                            arguments[0][i].Url, arguments[0][i].MessageTitle));
                            that.bindEvent(_msgStr);
                            $(that.messageMenu).append(_msgStr);
                            i++;
                        }
                    }
                }
            })(arguments, that);
        }
    },
    initEvent: function () {
        var that = this;
        $(this.msgDown).on("click", function (event) {
            $(messageMenu).empty();
            var __data = that.getMsgList();
            that.createMsgList(__data);
        });
    },
    getMsgList: function () {
        var _data;
        $.ajax({
            url: '/Home/GetMessageList',
            type: 'POST',
            async: false,
            success: function (data) {
                if (data.item) {
                    if (data.item.length > 0)
                        _data = data.item;
                }
            }
        });
        return _data;
    },
    bindEvent: function ($ent) {
        var that = this;
        $ent.on("click", function (event) {
            //$(this).find('i').css("color", "#00c0ef");
            var _numNew = that.getMsgNum() - 1;
            $(that.msgNum).empty();
            if (_numNew > 0) {
                $(that.msgNum).append(_numNew);   
            }
            if (that.updateIsRead($(this).attr("Id"))) {
                showPage($(this).attr("title"), '', '', '', this);
                //TableDetailed.ShowDetailed(layer, "/Admin/Message/Detailed", $(this).attr("id"));
                $(this).remove();
            }
            event.stopPropagation();
        });
    },
    updateIsRead: function (_id) {
        var bol;
        $.ajax({
            url: '/Admin/Message/UpdateIsRead',
            type: 'POST',
            data: { id: _id },
            async: false,
            success: function (data) {
                if (data) {
                    bol = data.result;
                }
            }
        });
        return bol;
    },
    numConvert: function (num) {
        var _countNum = 0;
        if (num) {
            if (num != "" || num != null)
                _countNum = num;
        }
        return _countNum;
    },
    instantMessage: function () {
        var that = this;
        
    }
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