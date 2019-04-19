function CollectFunction(optionModel, optionName) {
    if (!(this instanceof CollectFunction)) {
        return new CollectFunction(optionModel, optionName);
    }
    this.optionModel = optionModel;
    this.optionName = optionName;
}
CollectFunction.prototype = {
    GetRightKey: function () {
        var that = this;
        var options = that.optionModel.find("[name='" + this.optionName + "']");
        var optionsNums = "";
        if (options.length > 0) {
            var n = 0;
            while (n < options.length) {
                if ($(options[n]).is(':checked')) {
                    if (optionsNums != "") {
                        optionsNums += "," + n;
                    } else {
                        optionsNums = n.toString();
                    }
                }
                n++;
            }
        }
        //console.log(optionsNums);
        return optionsNums;
    },
    GetOptions: function () {
        var that = this;
        var textareas = that.optionModel.find("textarea");
        var textStr = "";
        if (textareas.length > 0) {
            var i = 0;
            while (i < textareas.length) {
                //console.log($(textareas[i]));
                if (textStr != "") {
                    textStr += '|' + $(textareas[i]).val();
                } else {
                    textStr = $(textareas[i]).val();
                }
                i++;
            }
        }
        console.log(textStr);
        return textStr;
    },
    GetAirAnswer: function () {
        var that = this;
        var options = that.optionModel.find("[name='" + this.optionName + "']");
        var airStr = "";
        if (options.length > 0) {
            var j = 0;
            while (j < options.length) {
                if (airStr != "") {
                    airStr += "|" + $(options[j]).val().trim();
                }
                else {
                    airStr = $(options[j]).val().trim();
                }
                j++;
            }
        }
        //console.log(airStr);
        return airStr;
    }
}