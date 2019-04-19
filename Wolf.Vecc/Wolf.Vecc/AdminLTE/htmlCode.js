var HtmlUtil = {
    /*1.用正则表达式实现html转码*/
    htmlEncodeByRegExp: function (str) {
        var s = "";
        if (str.length == 0) return "";
        s = str.replace(/&/g, "&amp;");
        s = s.replace(/</g, "&lt;");
        s = s.replace(/>/g, "&gt;");
        s = s.replace(/ /g, "&nbsp;");
        s = s.replace(/\'/g, "&#39;");
        s = s.replace(/\"/g, "&quot;");
        return s;
    },
    /*2.用正则表达式实现html解码*/
    htmlDecodeByRegExp: function (str) {
        var s = "";
        if (str.length == 0) return "";
        s = str.replace(/&amp;/g, "&");
        s = s.replace(/&lt;/g, "<");
        s = s.replace(/&gt;/g, ">");
        s = s.replace(/&nbsp;/g, " ");
        s = s.replace(/&#39;/g, "\'");
        s = s.replace(/&quot;/g, "\"");
        return s;
    },
    htmlReg : function(str){
        //$("#summernote").summernote('code').replace(new RegExp(/(<)/g), '&lt;');
        var _str = str.replace(new RegExp(/(<)/g), '&lt;');
        return _str.replace(new RegExp(/(>)/g), '&gt;');
    },
    strLength: function (str) {
        var s = "";
        if (str.length > 25) {
            s = str.substring(0, 25) + "....";
        }
        else {
            s = str;
        }
        return s;
    },
    getRadioValue:function(radioName) {
        var value = 0;
        var radio = document.getElementsByName(radioName);
        for (var i = 0; i < radio.length; i++) {
            if (radio[i].checked == true) {
                value = radio[i].value;
                break;
            }
        }
        return value;
    },
    unique1:function(array) {
      var n = []; //一个新的临时数组
      //遍历当前数组
      for(var i = 0; i < array.length; i++){
        //如果当前数组的第i已经保存进了临时数组，那么跳过，
        //否则把当前项push到临时数组里面
        if (n.indexOf(array[i]) == -1) n.push(array[i]);
      }
      return n;
    }
};

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

Date.prototype.format = function(format){ 
    var o = { 
        "M+" : this.getMonth()+1, //month 
        "d+" : this.getDate(), //day 
        "h+" : this.getHours(), //hour 
        "m+" : this.getMinutes(), //minute 
        "s+" : this.getSeconds(), //second 
        "q+" : Math.floor((this.getMonth()+3)/3), //quarter 
        "S" : this.getMilliseconds() //millisecond 
    } 
  
    if(/(y+)/.test(format)) { 
        format = format.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length)); 
    } 
  
    for(var k in o) { 
        if(new RegExp("("+ k +")").test(format)) { 
            format = format.replace(RegExp.$1, RegExp.$1.length==1 ? o[k] : ("00"+ o[k]).substr((""+ o[k]).length)); 
        } 
    } 
    return format; 
} 