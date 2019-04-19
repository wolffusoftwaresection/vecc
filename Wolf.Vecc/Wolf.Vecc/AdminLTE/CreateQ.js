function CreateQ(examinationList, questionsModel, questionTypeModel, questionTitle) {
    if (!(this instanceof CreateQ)) {
        return new CreateQ(examinationList, questionsModel, questionTypeModel, questionTitle);
    }
    this.examinationList = examinationList;
    this.questionsModel = questionsModel;
    this.questionTypeModel = questionTypeModel;
    this.questionTitle = questionTitle;
    this.init();
}

CreateQ.prototype = {
    init: function () {
        this.initElements();
        this.initEvent();
    },
    initElements: function () {
        this.enArray = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z');
        this.optionRadio = '<div class="col-md-12"><div class="form-group">{0}. <input type="radio" id="optionOnly_{1}" name="optionsItem_{3}" class="mgr mgr-success mgr-lg" /><label for="optionOnly_{1}" style="padding-left:5px;">{2}</label></div></div>';
        this.optionCheckBox = '<div class="col-md-12"><div class="form-group">{0}. <input type="checkbox" id="optionMulti_{1}" class="mgc mgc-success mgc-lg" name="optionMulti_{3}" /><label for="{1}">{2}</label></div></div>';
        this.optionJudge = '<div class="col-md-12"><div class="form-group"><input type="radio" id="judgeR_{0}" name="judgeGroup_{1}" /><label for="judgeR_{0}">{2} </label></div></div>';
        this.optionAir = '<div class="col-md-3"><div class="form-group"><input type="text" name="airAnswerGroup" value={0} class="col-md-8"/></div></div>'
    },
    initEvent: function () {
        this.createExamination();
    },
    createExamination: function () {
        var that = this;
        (function (that) {
            //查询题目列表中对应qid的问题作为按钮点击的返回值参数qid为按钮对应的绑定ID
            var btnForExamination = that.examinationList;
            var optLen = btnForExamination.QuestionOptions.length;
            var i = 0;
            var _option;
            //var _answer = btnForExamination.Answers.split(',');
            $(that.questionType).append(that.getTypeStr(btnForExamination.QuestionType));
            if (btnForExamination.QuestionType == 4) {
                that.createAirSpaceInput(btnForExamination);
            }
            else {
                //添加标题
                $(that.questionTitle).append(HtmlUtil.htmlDecodeByRegExp(btnForExamination.QuestionTitle));
                //判断是否有图片有就添加
                if (btnForExamination.QuestionImageStr != "") {
                    var img = btnForExamination.QuestionImageStr.split('|');
                    if (img.length > 0) {
                        $(that.questionTitle).append('<img src="/Upload/QuestionImage/' + img[0] + '"/>');
                    }
                }
                that.createOptions(btnForExamination);
            }
        })(that);
    },
    createAirSpaceInput: function (_btnForExamination) {
        $(this.questionTitle).append(this.replaceAirSpace(HtmlUtil.htmlDecodeByRegExp(_btnForExamination.QuestionTitle), _btnForExamination.QID, _btnForExamination.QuestionOptions, _btnForExamination.Answers));
    },
    createOptions: function () {
        var that = this;
        if (arguments.length > 0) {
            (function (arguments, that) {
                var examination = arguments[0];
                if (examination) {
                    //生成单选
                    var arrayOptions = examination.QuestionOptions;
                    var qid = examination.QID;
                    var optLen = arrayOptions.length;
                    var i = 0;
                    var _option;
                    var _answer = examination.Answers.split(',');
                    while (i < optLen) {
                        if (examination.QuestionType == 1) {
                            _option = $(that.optionRadio.format(that.enArray[i],
                                arrayOptions[i].Id, arrayOptions[i].QuestionOption, qid));
                        }
                        else if (examination.QuestionType == 2) {
                            _option = $(that.optionCheckBox.format(that.enArray[i],
                                 arrayOptions[i].Id, arrayOptions[i].QuestionOption, qid));
                        }
                        else if (examination.QuestionType == 3) {
                            _option = $(that.optionJudge.format(
                                 arrayOptions[i].Id, qid, arrayOptions[i].QuestionOption));
                        }
                        var _sel = $.inArray(i.toString(), _answer) >= 0;
                        $(_option).find("input").prop("checked", _sel);
                        $(that.questionsModel).append(_option);
                        i++;
                    }
                }
            })(arguments, that);
        };
    },
    replaceAirSpace: function (str, qid, questionOptions, answers) {
        //{1}为题目的ID
        var _answers = answers.split('|');
        var _input = '<input type="text" id="airSpace_{0}" name="airSpaceTxt_{1}" value="{2}" style="border:none #FFFFFF;huerreson:e xpression(this.width=this.scrollWidth); border-bottom:#777777 solid 1px;background:transparent;"/>';
        for (var i = 0; i < questionOptions.length; i++) {
            var _optionInput = _input.format(questionOptions[i].Id, qid, _answers[i]);
            str = str.replace(/\[AirSpace\]/, _optionInput);
        }
        return str;
    },
    getTypeStr: function (type) {
        var str = "单选题";
        switch (type) {
            case 1:
                str = "单选题";
                break;
            case 2:
                str = "多选题";
                break;
            case 3:
                str = "判断题";
                break;
            case 4:
                str = "填空题";
                break;
        }
        return str;
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