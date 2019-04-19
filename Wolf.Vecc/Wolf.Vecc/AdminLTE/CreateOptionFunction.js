function CreateOption(type, rootOption_object, optionsStr_array) {
    if (!(this instanceof CreateOption)) {
        return new CreateOption(type, rootOption_object, optionsStr_array);
    }
    this.type = type;
    this.rootOption_object = rootOption_object;
    this.optionsStr_array = optionsStr_array;
    this.init();
}

CreateOption.prototype = {
    init: function () {
        this.initElements();
        this.initEvent();
    },
    initElements: function () {
        //this.optionsStr_array = new Array("选项E ", "选项F ", "选项G ", "选项H ", "选项I ", "选项J ", "选项K ", "选项L ", "选项M ", "选项N ", "选项O ", "选项P ", "选项Q ", "选项R ", "选项S ", "选项T ", "选项U ", "选项V ", "选项W ", "选项X ", "选项Y ", "选项Z ");
        //this.enArray = new Array("选项I ", "选项G ", "选项K ", "选项L ", "选项M ", "选项N ", "选项O ", "选项P ", "选项Q ", "选项R ", "选项S ", "选项T ", "选项U ", "选项V ", "选项W ", "选项X ", "选项Y ", "选项Z ");
        this.radioHtml = '<div class="col-md-12"><div class="form-group"><label class="col-md-2 control-label">{0}<input class="mgr mgr-success mgr-lg" name="optionOnly" type="radio" /></label>' +
                                '<div class="col-md-5"><textarea class="col-md-11"></textarea></div></div></div>';

        this.checkBoxHtml = '<div class="col-md-12"><div class="form-group"><label class="col-md-2 control-label">{0}<input class="mgc mgc-success mgc-lg" name="optionCB" type="checkbox" /></label>' +
                                '<div class="col-md-5"><textarea class="col-md-11"></textarea></div></div></div>';

        this.deleteOptionBtn = $('<a class="tm_ico_delete col-sm-2" style="margin-top:12px;"></a>');
    },
    initEvent: function () {

    },
    bindEvent: function ($ent) {
        var that = this;
        $ent.on("click", function (event) {
            //$(this).find('i').css("color", "#00c0ef");
            $(this).parent().remove();
            event.stopPropagation();
        });
    },
    createOption: function () {
        var that = this;
        (function (that) {
            var _len = that.getRootLength();
            var _outLen = _len - 4;
            //console.log(that.optionsStr_array[_outLen]);
            //if (_outLen < 4) {
            //var _optionStr = $(that.radioHtml.format(that.optionsStr_array[_outLen]));
            if (that.optionsStr_array.length > 0) {
                var _optionStr;
                if (that.type == "radio") {
                    _optionStr = $(that.radioHtml.format(that.optionsStr_array[0]));
                }
                else {
                    _optionStr = $(that.checkBoxHtml.format(that.optionsStr_array[0]));
                }
                that.optionsStr_array.splice(0, 1);
                //绑定删除事件
                that.bindEvent(that.deleteOptionBtn);
                $(that.rootOption_object).append($(_optionStr).find('.form-group').append(that.deleteOptionBtn));
            }
            //that.optionsStr_array.splice(_outLen, 1);
            //}
        })(that);
    },
    getRootLength: function () {
        return $(this.rootOption_object).find("[name='optionOnly']").length;
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

var OptionsStr = {
    _optionsStr: new Array("选项E ", "选项F ", "选项G ", "选项H ", "选项I ", "选项J ", "选项K ", "选项L ", "选项M ", "选项N ", "选项O ", "选项P ", "选项Q ", "选项R ", "选项S ", "选项T ", "选项U ", "选项V ", "选项W ", "选项X ", "选项Y ", "选项Z ")
};
Object.defineProperty(OptionsStr, "optionsStr", {
    get: function () {
        //return this._optionsStr.split(',');
        return this._optionsStr;
    },
    set: function (value) {
        this._optionsStr = value;
    }
});

var OptionsStr1 = {
    //_optionsStr : "选项E,选项F,选项G,选项H", "选项I ", "选项G ", "选项K "
    _optionsStr1: new Array("选项E ", "选项F ", "选项G ", "选项H ", "选项I ", "选项J ", "选项K ", "选项L ", "选项M ", "选项N ", "选项O ", "选项P ", "选项Q ", "选项R ", "选项S ", "选项T ", "选项U ", "选项V ", "选项W ", "选项X ", "选项Y ", "选项Z ")
};
Object.defineProperty(OptionsStr1, "optionsStr1", {
    get: function () {
        //return this._optionsStr.split(',');
        return this._optionsStr1;
    },
    set: function (value) {
        this._optionsStr1 = value;
    }
});