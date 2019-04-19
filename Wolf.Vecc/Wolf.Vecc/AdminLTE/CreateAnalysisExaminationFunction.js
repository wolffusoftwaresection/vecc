function CreateAnalysisExaminationFunction(examinationList, examinationID, btnModel, questionsModel, questionTypeModel, questionTitle, imgRW, analysis, rightAnswer) {
    if (!(this instanceof CreateAnalysisExaminationFunction)) {
        return new CreateAnalysisExaminationFunction(examinationList, examinationID, btnModel, questionsModel, questionTypeModel, questionTitle, imgRW, analysis, rightAnswer);
    }
    this.examinationList = examinationList;
    this.examinationID = examinationID;
    this.btnModel = btnModel;
    this.questionsModel = questionsModel;
    this.questionTypeModel = questionTypeModel;
    this.questionTitle = questionTitle;
    this.imgRW = imgRW;
    this.analysis = analysis;
    this.rightAnswer = rightAnswer;
    this.init();
}

CreateAnalysisExaminationFunction.prototype = {
    init: function () {
        this.initElements();
        this.initEvent();
    },
    initElements: function () {
        this.btn = '<input type="button" value="{0}" id="QID_{1}" name="optionBtn_{1}" class="btn btn-default" style="width:50px;height:50px;color:white; margin-right:5px; margin-top:5px;background-color:#FFA54F"/>';
        this.enArray = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z');
        this.optionRadio = '<div class="col-md-12"><div class="form-group">{0}. <input type="radio" id="optionOnly_{1}" name="optionsItem_{3}" class="mgr mgr-success mgr-lg" /><label for="optionOnly_{1}" style="padding-left:5px;">{2}</label></div></div>';
        this.optionCheckBox = '<div class="col-md-12"><div class="form-group">{0}. <input type="checkbox" id="optionMulti_{1}" class="mgc mgc-success mgc-lg" name="optionMulti_{3}" /><label for="{1}">{2}</label></div></div>';
        this.optionJudge = '<div class="col-md-12"><div class="form-group"><input type="radio" id="judgeR_{0}" name="judgeGroup_{1}" /><label for="judgeR_{0}">{2} </label></div></div>';
        this.optionAir = '<div class="col-md-3"><div class="form-group"><input type="text" name="airAnswerGroup" value={0} class="col-md-8"/></div></div>'
        this.optionRight = '<img src="/AdminLTE/dist/img/right.png" alt="正确" />';
        this.optionError = '<img src="/AdminLTE/dist/img/error.png" alt="错误" />';
    },
    initEvent: function () {
        this.createExaminationBtn();
        this.createExamination();
    },
    createExaminationBtn: function () {
        var that = this;
        (function (that) {
            var btnNum = that.examinationList.length;
            for (var i = 1; i <= btnNum; i++) {
                var _eBtn = $(that.btn.format(i, that.examinationList[i - 1].QID));
                that.btnBindEvent(_eBtn);
                $(that.btnModel).append(_eBtn);
                if (i == 1) {
                    _eBtn.click();
                }
            }
        })(that);
    },
    createExamination: function () {
        var that = this;
        if (arguments.length > 0) {
            (function (arguments, that) {
                //查询题目列表中对应qid的问题作为按钮点击的返回值参数qid为按钮对应的绑定ID
                var btnForExamination = that.selExaminationListByQID(arguments[0]);
                that.clearQuestion();
                var optLen = btnForExamination.QuestionOptions.length;
                var i = 0;
                var _option;
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
                //添加正确错误图片
                if (btnForExamination.ResultRecord > 0){
                    $(that.imgRW).append(that.optionError);
                }
                else {
                    $(that.imgRW).append(that.optionRight);
                }
                //添加正确答案
                $(that.rightAnswer).append(that.getEN(btnForExamination.Answers, btnForExamination.QuestionType));
                //添加解析
                $(that.analysis).append(btnForExamination.Analysis);
            })(arguments, that);
        }
    },
    getEN: function (answers, type) {
        var answerArr;
        var enStr = "";
        if (answers != "") {
            if (type == 4) {
                answerArr = answers.split('|');
                for (var i = 0; i < answerArr.length; i++) {
                    if (enStr != "") {
                        enStr += "," + answerArr[i];
                    }
                    else {
                        enStr = answerArr[i];
                    }
                }
            }
            else {
                answerArr = answers.split(',');
                for (var j = 0; j < answerArr.length; j++) {
                    if (enStr != "") {
                        enStr += "," + this.enArray[answerArr[j]];
                    }
                    else {
                        enStr = this.enArray[answerArr[j]];
                    }
                }
            }
        }
        return enStr;
    },
    clearQuestion: function () {
        //每点一次button清空当前的题目加载新的题目
        $(this.questionTitle).empty();
        $(this.questionTypeModel).empty();
        $(this.questionsModel).empty();
        $(this.rightAnswer).empty();
        $(this.analysis).empty();
        $(this.imgRW).empty();
    },
    selExaminationListByQID: function (qid) {
        var _list;
        if (qid) {
            if (qid > 0) {
                $.each(this.examinationList, function (index, value, array) {
                    if (value.QID == qid) {
                        _list = value;
                    }
                });
            }
        }
        return _list;
    },
    btnBindEvent: function ($ent) {
        var that = this;
        $ent.on("click", function (event) {
            //设置点击后按钮颜色
            $(this).css("background-color", "#00c0ef");
            var qid = $(this).attr("id").split('_')[1];
            var index = $(this).val();
            that.createExamination(qid, index);
        });
    },
    selExaminationListByQID: function (qid) {
        var _list;
        if (qid) {
            if (qid > 0) {
                $.each(this.examinationList, function (index, value, array) {
                    if (value.QID == qid) {
                        _list = value;
                    }
                });
            }
        }
        return _list;
    },
    createAirSpaceInput: function (_btnForExamination) {
        $(this.questionTitle).append(this.replaceAirSpace(HtmlUtil.htmlDecodeByRegExp(_btnForExamination.QuestionTitle), _btnForExamination.QID, _btnForExamination.QuestionOptions, _btnForExamination.RecordAnswerStr));
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
                    var _answer = examination.RecordAnswerStr.split(',');
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
                        var _sel = $.inArray((arrayOptions[i].Id).toString(), _answer) >= 0;
                        $(_option).find("input").prop("checked", _sel);
                        $(that.questionsModel).append(_option);
                        i++;
                    }
                }
            })(arguments, that);
        };
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