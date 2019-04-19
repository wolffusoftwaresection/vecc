function CreateExaminationFunction(examinationList, examinationID, btnModel, questionsModel, questionTypeModel, questionTitle, subMitExamination) {
    if (!(this instanceof CreateExaminationFunction)) {
        return new CreateExaminationFunction(examinationList, examinationID, btnModel, questionsModel, questionTypeModel, questionTitle, subMitExamination);
    }
    this.examinationList = examinationList;
    this.examinationID = examinationID;
    this.btnModel = btnModel;
    this.questionsModel = questionsModel;
    this.questionTypeModel = questionTypeModel;
    this.questionTitle = questionTitle;
    this.subMitExamination = subMitExamination;
    this.init();
}

CreateExaminationFunction.prototype = {
    init: function () {
        this.initElements();
        this.initEvent();
    },
    initElements: function () {
        this.enArray = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z');
        this.btn = '<input type="button" value="{0}" id="QID_{1}" name="optionBtn_{1}" class="btn btn-default" style="width:50px;height:50px;color:white; margin-right:5px; margin-top:5px;background-color:#FFA54F"/>';
        this.optionRadio = '<div class="col-md-12"><div class="form-group">{0}. <input type="radio" id="optionOnly_{1}" name="optionsItem_{3}" class="mgr mgr-success mgr-lg" /><label for="optionOnly_{1}" style="padding-left:5px;">{2}</label></div></div>';

        //this.optionCheckBox = '<div class="col-md-12"><div class="form-group"><label class="col-md-2 control-label">选项{0} <input name="optionCB" class="mgc mgc-success mgc-lg" type="checkbox" /></label></div></div>'
        this.optionCheckBox = '<div class="col-md-12"><div class="form-group">{0}. <input type="checkbox" id="optionMulti_{1}" class="mgc mgc-success mgc-lg" name="optionMulti_{3}" /><label for="optionMulti_{3}">{2}</label></div></div>';

        //this.optionCheckBox = '<div class="col-md-12"><div class="form-group"><label class="col-md-2 control-label">{0}. {2} <input name="optionCB" class="mgc mgc-success mgc-lg" type="checkbox" /></label><div class="col-md-5"><textarea class="col-md-11">{1}</textarea></div></div></div>'

        this.optionJudge = '<div class="col-md-12"><div class="form-group"><input type="radio" id="judgeR_{0}" class="mgr mgr-success mgr-lg" name="judgeGroup_{1}" /><label for="judgeR_{0}">{2} </label></div></div>';
    },
    initEvent: function () {
        this.createExaminationBtn();
        this.createExamination();
        this.inintBindEvent();
    },
    inintBindEvent: function () {
        var that = this;
        $("#subMitExamination").on("click", function (event) {
            //提交试卷先判断是否所有题目都已答完
            var ckLen = that.checkExamination();
            if (ckLen.length == 0) {
                //根据this.examinationList获取用户答题数据详情并上传服务器
                swal({
                    title: '确认提交操作!',
                    text: "提交后将结束此次考试!",
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: '提交',
                    cancelButtonText: '取消'
                }).then(function (isConfirm) {
                    if (isConfirm) {
                        that.subMitUserExaminationData();
                    }
                });
            }
            else {
                that.changBtnBGColor();
                //ckLen中的题目QID都是没有答过将对应按钮显示未答前颜色
                for (var i = 0; i < ckLen.length; i++) {
                    $("#QID_" + ckLen[i]).css("background-color", "#FFA54F");
                }
            }
        });
    },
    btnBindEvent: function ($ent) {
        var that = this;
        $ent.on("click", function (event) {
            //console.log($(this).attr("id"));
            //设置点击后按钮颜色
            $(this).css("background-color", "#00c0ef");
            var qid = $(this).attr("id").split('_')[1];
            var index = $(this).val();
            that.createExamination(qid, index);
        });
    },
    changBtnBGColor: function () {
        $(this.btnModel).find('input').each(function () {
            $(this).css("background-color", "#00c0ef");
        });
    },
    checkExamination: function () {
        var ck = new Array();
        for (var i = 0; i < this.examinationList.length; i++) {
            var _result = true;
            $.each(this.examinationList[i].QuestionOptions, function (index, value, array) {
                if (value.SelAnswer == 1) {
                    _result = false;
                }
            });
            if (_result) {
                ck.push(this.examinationList[i].QID);
            }
        }
        return ck;
    },
    subMitUserExaminationData: function () {
        var _examinationList = [];
        var _optionItem;
        for (var i = 0; i < this.examinationList.length; i++) {
            var answers = "";
            if (this.examinationList[i].QuestionType <= 3) {
                for (var j = 0; j < this.examinationList[i].QuestionOptions.length; j++) {
                    if (this.examinationList[i].QuestionOptions[j].SelAnswer > 0) {
                        if (answers != "") {
                            answers += "," + this.examinationList[i].QuestionOptions[j].Id;
                        }
                        else {
                            answers = this.examinationList[i].QuestionOptions[j].Id;
                        }
                    }
                }
            }
            else {
                for (var k = 0; k < this.examinationList[i].QuestionOptions.length; k++) {
                    if (answers != "") {
                        answers += "|" + this.examinationList[i].QuestionOptions[k].QuestionOption;
                    }
                    else {
                        answers = this.examinationList[i].QuestionOptions[k].QuestionOption;
                    }
                }
            }
            _optionItem = { 'ExaminationID': this.examinationID, 'QID': this.examinationList[i].QID, 'QuestionType': this.examinationList[i].QuestionType, 'AnswerIDStr': answers };
            _examinationList.push(_optionItem);
        }
        //console.log(JSON.stringify(_examinationList));
        $.ajax({
            url: "/Admin/Examination/UserExamination",
            data: { examination: JSON.stringify(_examinationList) },
            type: "POST",
            async: false,
            success: function (data) {
                if (data.result) {
                    Cookie.del('starttime');
                    $("#equipmentTime").empty();
                    $("#equipmentTime").hide();
                    swal({
                        title: '考试结束!',
                        text: "得分" + data.point,
                        type: 'success',
                        showCancelButton: true,
                        showCancelButton: false,
                    }).then(function (isConfirm) {
                        if (isConfirm) {
                            window.close();
                        }
                    });
                }
                else {
                    swal({
                        title: '提交错误!',
                        text: data.msg,
                        type: 'error',
                        showCancelButton: false
                    })
                    //    .then(function (isConfirm) {
                    //    if (isConfirm) {
                    //        //window.close();
                    //        return;
                    //    }
                    //});
                }
            },
            error: function (data) {
            }
        });
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
                var _QID = arguments[0];
                //btn的value
                var _index = arguments[1];
                if (_QID > 0) {
                    //查询题目列表中对应qid的问题作为按钮点击的返回值参数qid为按钮对应的绑定ID
                    var btnForExamination = that.selExaminationListByQID(_QID);
                    //console.log(btnForExamination);
                    that.clearQuestion();
                    $(that.questionTypeModel).append(that.getTypeStr(btnForExamination.QuestionType));
                    if (btnForExamination.QuestionType == 4) {
                        that.createAirSpaceInput(_index, btnForExamination);
                    }
                    else {
                        //添加标题
                        $(that.questionTitle).append(_index + "." + HtmlUtil.htmlDecodeByRegExp(btnForExamination.QuestionTitle));
                        //判断是否有图片有就添加
                        if (btnForExamination.QuestionImageStr != "") {
                            var img = btnForExamination.QuestionImageStr.split('|');
                            if (img.length > 0) {
                                $(that.questionTitle).append('<img src="/Upload/QuestionImage/' + img[0] + '"/>');
                            }
                        }
                        that.createOptions(btnForExamination);
                    }
                }
            })(arguments, that);
        }
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
    selOptionsByID: function (qid, optionId) {
        //console.log(qid + optionId);
        var examinationItem = this.selExaminationListByQID(qid);
        //console.log(examinationItem);
        var optionItem;
        if (examinationItem) {
            var _option;
            if (optionId) {
                if (optionId > 0) {
                    $.each(examinationItem.QuestionOptions, function (index, value, array) {
                        if (value.Id == optionId) {
                            optionItem = value;
                        }
                    });
                }
            }
        }
        return optionItem;
    },
    //设置点击项的SelAnswer为1前遍历题目所有单选项设置SelAnswer为-1
    inintExaminationOptionsSelAnswer: function (qid) {
        var _listOption = this.selExaminationListByQID(qid);
        if (_listOption) {
            $.each(_listOption.QuestionOptions, function (index, value, array) {
                value.SelAnswer = -1;
            });
        }
    },
    createOptions: function () {
        var that = this;
        if (arguments.length > 0) {
            (function (arguments, that) {
                var examination = arguments[0];
                if (examination) {
                    //生成单选
                    //console.log(examination);
                    var arrayOptions = examination.QuestionOptions;
                    var qid = examination.QID;
                    //console.log(arrayOptions);
                    var optLen = arrayOptions.length;
                    var i = 0;
                    var _option;
                    while (i < optLen) {
                        if (examination.QuestionType == 1) {
                            //console.log(arrayOptions[i]);
                            _option = $(that.optionRadio.format(that.enArray[i],
                                arrayOptions[i].Id, arrayOptions[i].QuestionOption, qid));
                        }
                        else if (examination.QuestionType == 2) {
                            _option = $(that.optionCheckBox.format(that.enArray[i],
                                 arrayOptions[i].Id, arrayOptions[i].QuestionOption, qid));
                        }
                        else if (examination.QuestionType == 3) {
                            //console.log(arrayOptions[i]);
                            _option = $(that.optionJudge.format(
                                 arrayOptions[i].Id, qid, arrayOptions[i].QuestionOption));
                        }
                        if (examination.QuestionType <= 3) {
                            that.bindOptionEvent(_option, examination.QuestionType);
                        }
                        //遍历集合中的QuestionOptions的item的SelAnswer如果为1将选中
                        if (arrayOptions[i].SelAnswer > 0) {
                            //console.log($(_option).find('input'));
                            var __input = $(_option).find('input');
                            $(__input).prop('checked', true);
                        }
                        $(that.questionsModel).append(_option);
                        i++;
                    }
                }
            })(arguments, that);
        };
    },
    createAirSpaceInput: function (index, _btnForExamination) {
        $(this.questionTitle).append(index + "." + this.replaceAirSpace(HtmlUtil.htmlDecodeByRegExp(_btnForExamination.QuestionTitle), _btnForExamination.QID, _btnForExamination.QuestionOptions));
        this.bindInputEvent(this.questionTitle);
    },
    clearQuestion: function () {
        //每点一次button清空当前的题目加载新的题目
        $(this.questionTitle).empty();
        $(this.questionTypeModel).empty();
        $(this.questionsModel).empty();
    },
    bindOptionEvent: function ($ent, _questionType) {
        var that = this;
        $ent.on("click", function (event) {
            var ev = event || window.event;
            var elm = ev.target || ev.srcElement;
            if (elm.tagName === 'LABEL' || elm.tagName === 'DIV') { return; }
            //设置对应题目（QID）的选中
            var _input = $(this).find('input');
            var _qid = $(_input).attr("name").split('_')[1];
            var _optionId = $(_input).attr("id").split('_')[1];
            var _optionItem = that.selOptionsByID(_qid, _optionId);
            
            //var _questionType = that.selExaminationListByQID(_qid).QuestionType;
            //如果时单选需要恢复SelAnswer都为-1在重新赋值给点击的option;如果是checkbox允许多个SelAnswer为1(即选中)
            if (_questionType == 1 || _questionType == 3) {
                that.inintExaminationOptionsSelAnswer(_qid);
                _optionItem.SelAnswer = 1;
                $(_input).prop('checked', true);
            }
            else if (_questionType == 2) {
                _optionItem.SelAnswer = _optionItem.SelAnswer == 1 ? -1 : 1;
                //if ($(_input).is(':checked')) {
                //    $(_input).prop('checked', true);
                //} else {
                //    $(_input).prop('checked', false);
                //}
            }
            //console.log(that.examinationList);
        });
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
    },
    replaceAirSpace: function (str, qid, questionOptions) {
        //{1}为题目的ID
        var _input = '<input type="text" id="airSpace_{0}" name="airSpaceTxt_{1}" value="{2}" style="border:none #FFFFFF;huerreson:e xpression(this.width=this.scrollWidth); border-bottom:#777777 solid 1px;background:transparent;"/>';
        for (var i = 0; i < questionOptions.length; i++) {
            var _val = this.selOptionsByID(qid, questionOptions[i].Id).QuestionOption;
            var _optionInput = _input.format(questionOptions[i].Id, qid, _val);
            //console.log(_optionInput);
            str = str.replace(/\[AirSpace\]/, _optionInput);
        }
        return str;
    },
    bindInputEvent: function ($QuestionTitle) {
        var that = this;
        //console.log($($QuestionTitle).find('input').length);
        $($QuestionTitle).find('input').each(function (index) {
            $(this).on('blur', function () {
                var _optionId = $(this).attr("id").split('_')[1];
                var _qid = $(this).attr("name").split('_')[1];
                var _optionItem = that.selOptionsByID(_qid, _optionId);
                if ($(this).val().trim() == "") {
                    _optionItem.SelAnswer = -1;
                    _optionItem.QuestionOption = "";
                }
                else {
                    _optionItem.SelAnswer = 1;
                    _optionItem.QuestionOption = $(this).val();
                }
                //console.log(_optionItem);
            })
        })
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