function CreateQuestionOptionsFunction(optionsList, answers, questionType, questionsModel) {
    if (!(this instanceof CreateQuestionOptionsFunction)) {
        return new CreateQuestionOptionsFunction(optionsList, answers, questionType, questionsModel);
    }
    this.optionsList = optionsList;
    this.answers = answers;
    this.questionType = questionType;
    this.questionsModel = questionsModel;
    this.init();
}

CreateQuestionOptionsFunction.prototype = {
    init: function () {
        this.initElements();
        this.initEvent();
    },
    initElements: function () {
        this.enArray = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z');
        this.optionRadio = '<div class="col-md-12"><div class="form-group"><label class="col-md-2 control-label">选项{0} <input name="optionOnly" class="mgr mgr-success mgr-lg" type="radio" /></label><div class="col-md-5"><textarea class="col-md-11">{1}</textarea></div></div></div>'
        this.optionCheckBox = '<div class="col-md-12"><div class="form-group"><label class="col-md-2 control-label">选项{0} <input name="optionCB" class="mgc mgc-success mgc-lg" type="checkbox" /></label><div class="col-md-5"><textarea class="col-md-11">{1}</textarea></div></div></div>'
        this.optionAir = '<div class="col-md-3"><div class="form-group"><input type="text" name="airAnswerGroup" value={0} class="col-md-8"/></div></div>'
        //this.deleteOptionBtn = $('<a class="tm_ico_delete col-sm-2" style="margin-top:12px;"></a>');
        //this.optionJudge = '<div class="col-md-12"><div class="form-group"><input type="radio" id="judgeR_{0}" name="judgeGroup_{1}" /><textarea class="col-md-12"> {2}</textarea></div></div>';
    },
    initEvent: function () {
        this.createOptions();
        //this.inintBindEvent();
    },
    createOptions: function () {
        var that = this;
        (function (that) {
            if (that.optionsList) {
                //console.log(that.optionsList);
                var arrayOptions = that.optionsList;
                var optLen = arrayOptions.length;
                var i = 0;
                var _option;
                var _answer = that.answers.split(',');
                if (that.questionType != 3) {
                    while (i < optLen) {
                        if (that.questionType == 1) {
                            _option = $(that.optionRadio.format(that.enArray[i],
                                arrayOptions[i].QuestionOption));
                            var _sel = $.inArray(i.toString(), _answer) >= 0;
                            $(_option).find("input").prop("checked", _sel);
                        }
                        else if (that.questionType == 2) {
                            _option = $(that.optionCheckBox.format(that.enArray[i],
                                 arrayOptions[i].QuestionOption));
                            var _sel = $.inArray(i.toString(), _answer) >= 0;
                            $(_option).find("input").prop("checked", _sel);
                        }
                            //else if (that.questionType == 3) {
                            //    var _options = $(that.questionsModel).find("input");
                            //    if (_answer[0] == i.toString()) {
                            //        console.log(_options[i]);
                            //        var judge = _options[i];
                            //        $(judge).prop("checked", true);
                            //    }
                            //}
                        else if (that.questionType == 4) {
                            var _answerAir = that.answers.split('|');
                            _option = $(that.optionAir.format(_answerAir[i]));
                        }
                        $(that.questionsModel).append(_option);
                        i++;
                    }
                }
                else {
                    var _options = $(that.questionsModel).find("input");
                    var judge0;
                    if (_answer[0] == "0") {
                        judge0 = _options[0];
                        $(judge0).prop("checked", true);
                    }
                }
            }
        })(that);
    },
    clearQuestion: function () {
        //每点一次button清空当前的题目加载新的题目
        //$(this.questionTitle).empty();
        //$(this.questionTypeModel).empty();
        //$(this.questionsModel).empty();
    }
}