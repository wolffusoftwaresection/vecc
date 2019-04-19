var languagePlugin = {
    GetLanguage: function (url) {
        ajax.loading();
        var languageNos = [];
        $.each($("*[data-language]"),
            function (index, item) {
                var dataLanguage = $(item).attr("data-language");
                if (dataLanguage != "undefind" ||
                    dataLanguage != "" ||
                    dataLanguage != null) {
                    languageNos.push(dataLanguage);
                }
            });
        $.ajax({
            url:url,
            type: 'POST',
            async: false,
            data: {
                languageNos: languageNos
            },
            success: function (data) {
                $.each(data.list,
                    function (index, item) {
                        var no = item.languageNo;
                        var val = item.languageValue;
                        if (item.languageValue != "Unknown ID") {
                            //$("label[data-language=Common_Label_ExceptionCode]").html('aa');
                            $("*[data-language='" + no + "']").not("i").text(val);
                        }
                    });
            },
            error: function (data) {

            }
        });
        ajax.loadingClose();
    },

    CommonLanguageUrl: '/Admin/Common/GetCommonLanguageValue',
    PageLanguageUrl: '/Admin/Common/GetPageLanguageValue',
    //AllLanguageUrl: '/Admin/Common/GetAllLanguageValue',
    GetPageLanguage: function (url, key, value) {
        var val = "";
        $.ajax({
            url: url,
            type: 'POST',
            async: false,
            data: {
                languageNos: key
            },
            success: function (data) {
                //console.log(data);
                if (data != null && data.list.length>0) {
                    val = data.list[0].languageValue;
                }
            }   
        });
        if (val != "Unknown ID" && val=="") {
            val = value;
        }
        return val;
    }
};