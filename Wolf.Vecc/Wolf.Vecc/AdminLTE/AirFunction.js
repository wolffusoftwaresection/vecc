var AirFunction = (function () {
    var _instance = null;
    function AirAnswerFunction() {
        return {
            createAirAnswer: function (_airAnswerModel) {
                var aAirAnswer = $('<a class="tm_ico_delete col-sm-2" style="margin-top:10px;"></a>');
                var divAirAnswer = $('<div class="col-md-3"></div>');
                var divChildAirAnswer = $('<div class="form-group"><input type="text" name="airAnswerGroup" class="col-md-8"/></div>');
                this.bindEventDel(aAirAnswer);
                $(divChildAirAnswer).append(aAirAnswer);
                $(divAirAnswer).append(divChildAirAnswer);
                $(_airAnswerModel).append(divAirAnswer);
                return this;
            },
            bindEventDel: function ($del) {
                $($del).on("click", function (event) {
                    $(this).parent().parent().remove();
                    event.stopPropagation();
                });
                return this;
            },
            getAirAnswerNum: function (_airAnswerModel) {
               return $(_airAnswerModel).find('input').length;
            },
            getAirSpace: function (_summernote) {
                var _oldStr = $(_summernote).summernote('code');
                var _as = "AirSpace"; // 要计算的字符
                var regex = new RegExp(_as, 'g'); // 使用g表示整个字符串都要匹配
                var result = _oldStr.match(regex);
                var count = !result ? 0 : result.length;
                return count;
            },
            airNumRight: function (_airAnswerModel, _summernote) {
                var _bool = false;
                if (this.getAirAnswerNum(_airAnswerModel) === this.getAirSpace(_summernote)) {
                    _bool = true;
                }
                return _bool;
            }
        }
    }
    return function () {
        if (!_instance) {
            _instance = AirAnswerFunction();
        }
        return _instance;
    }
})();