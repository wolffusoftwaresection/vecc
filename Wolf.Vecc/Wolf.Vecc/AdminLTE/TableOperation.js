var m_pagerow = 0;
//初始化Table
function InitTable(tb, actionUrl, dbQueryParams, tbColumns, id, showdetail) {
	var toolbar = "#toolbar";
	if (arguments.length === 5) {
		toolbar = "#" + id
	}
	$(tb).bootstrapTable({
	    url: actionUrl,         //请求后台的URL（*）
        dynurl:true,
		method: 'get',                      //请求方式（*）
		toolbar: toolbar,                //工具按钮用哪个容器
		striped: true,                      //是否显示行间隔色
		cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
		pagination: true,                   //是否显示分页（*）
		sortable: false,                     //是否启用排序
		sortOrder: "asc",                   //排序方式
		queryParams: dbQueryParams,//传递参数（*）
		sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
		pageNumber: 1,                       //初始化加载第一页，默认第一页
		pageSize: 15,                       //每页的记录行数（*）
		//pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
		search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
		strictSearch: false,
		showColumns: true,                  //是否显示所有的列
		showRefresh: true,                  //是否显示刷新按钮
		minimumCountColumns: 2,             //最少允许的列数
		clickToSelect: true,                //是否启用点击选中行
		height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
		uniqueId: "Id",                     //每一行的唯一标识，一般为主键列
		showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
		cardView: false,                    //是否显示详细视图
		detailView: showdetail,                   //是否显示父子表
		columns: tbColumns,
		onPageChange: function (number, size) {
			m_pagerow = (number - 1) * size;
		}
	});
};

//表格行多选删除
function MultDelete(table, actionUrl) {
	var arrselections = $(table).bootstrapTable('getSelections');
	if (arrselections.length <= 0) {
		toastr.warning('请选择有效数据');
		return;
	}
	WinMsg.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
		if (!e) {
			return;
		}
		$.ajax({
			type: "post",
			url: actionUrl,
			data: { "arrselections": JSON.stringify(arrselections) },
			success: function (result) {
				if (result.ResultType == 0) {
					toastr.success(result.Message);
					$(table).bootstrapTable('refresh');
				}
				else {
					toastr.error(result.Message);
				}
			},
			error: function () {
				toastr.error('网络错误，请重新提交！');
			}
		});
	});
}

//重置密码
function MultReset(table, actionUrl) {
	var arrselections = $(table).bootstrapTable('getSelections');
	if (arrselections.length <= 0) {
		toastr.warning('请选择有效数据');
		return;
	}
	WinMsg.confirm({ message: "确定要将已选择用户的密码重置为初始密码“123456”吗？" }).on(function (e) {
		if (!e) {
			return;
		}
		$.ajax({
			type: "post",
			url: actionUrl,
			data: { "arrselections": JSON.stringify(arrselections) },
			success: function (result) {
				if (result.ResultType == 0) {
					toastr.success(result.Message);
					//$(table).bootstrapTable('refresh');
				}
				else {
					toastr.error(result.Message);
				}
			},
			error: function () {
				toastr.error('网络错误，请重新提交！');
			}
		});
	});
}

function openNewPage(url, name, id,size) {
	if ($("#templatequeryid").length > 0) $("#templatequeryid").remove();
	if (arguments.length === 4) {
		var str = '<input type="hidden" id="templatequeryid" value="' + id + '"/>';
		$("body").append(str);
	}
	if (size == "L")
	{
	    msg.sizeload(url, name, ['90', '85']);
	}
	else if(size == "M")
	{
	    msg.sizeload(url, name, ['86', '85']);
	}
	else if (size == "LL") {
	    msg.sizeload(url, name, ['30', '40']);
	}
	else
	{
	    msg.sizeload(url, name, ['30', '60']);
	}
    
}

function deleteInfo(url, name, id) {
	if ($("#templatequeryid").length > 0) $("#templatequeryid").remove();
	if (arguments.length === 3) {
		var str = '<input type="hidden" id="templatequeryid" value="' + id + '"/>';
		$("body").append(str);
	}
	msg.confirm(name,
        function () {
            ajax.post(url, "",
                function(data) {
                    $("#" + $("#templatequeryid").val()).click();
                    $("#templatequeryid").remove();
                    msg.info(data.Msg);
                });
	});
}

function GetPageItemsValue(ControllerID) {
	var search = new Array();
	var obj = [];
	var _query = "";
	$(".search_" + ControllerID).each(function () {
		var _id = $(this).attr("id").substring($(this).attr("id").indexOf("_") + 1);
		_query += "&" + _id + "=" + $(this).val();
	});
	return _query;
}
function ShowTableAndQuery(Controller, ControllerID,Size, Operate, ID) { //CompanyName搜索框名称 

	var dbQueryParams = function (params) {
		var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
			PageSize: params.limit,   //页面显示行数
			PageIndex: params.offset / params.limit + 1//页码*页面显示行数=offset
		};
		return temp;
	};
	var actionUrl = "/" + ControllerID + "/SearchLists?" + GetPageItemsValue(ControllerID) + "&id=" + ID;//
	var str = "[{title: 'ID',align: 'center',formatter: function (value, row, index) {return m_pagerow + index + 1; }},";
	
	var key = eval('dyKey_' + ControllerID);
	var btn = 'btn_query_' + ControllerID;

	str = "[{field: 'checkboxState', checkbox: true},";
	//str += "{field: '',title: '',align: 'center',formatter: function (value, row, index) {return '<a class=\"detail-icon\" href=\"javascript:\"><i class=\"glyphicon glyphicon-plus icon-plus\"></i></a>' }},";

	str += eval('dyTableCol_' + ControllerID);
	str = str.substring(0, str.length - 1);
	//console.log(str);
	if (Operate[0] != 0 || Operate[1] != 0 || Operate[2] != 0) {
	    str += ",{field: 'operate',title: '" + languagePlugin.GetPageLanguage(languagePlugin.AllLanguageUrl, 'Common_Btn_Setting', "设置") + "',align: 'center',formatter: function (value, row, index) { var a = '';var b = '';var c = '';var d = '';var  id= eval('row.'+'" + key + "');"
		str += "  var url = Controller+'/" + ControllerID + "/Edit/' + id; var url1 = Controller+'/" + ControllerID + "/Delete/' + id;var url2 = Controller+'/" + ControllerID + "/Add/' + id;var url3 = Controller+'/" + ControllerID + "/List/' + id;"
		if (Operate[0] == 1 || Operate[0] == 2)
		    str += "a = '<button type=button class=\"btn btn-sm btn-info\"  onclick=\"openNewPage(\\\'' + url + '\\\', \\\'" +languagePlugin.GetPageLanguage(languagePlugin.AllLanguageUrl, 'Common_Btn_Edit', "编辑") + "\\\', \\\'' + btn + '\\\', \\\'' + Size + '\\\');\">编辑</button>';"
		//if (Operate[1] == 1)
			//str += "b = '<a class=setgroup href=javascript:void(0) onclick=\"deleteInfo(\\\'' + url1 + '\\\', \\\'确认删除\\\', \\\'' + btn + '\\\');\"><i class=\"fa  fa-trash\"/></a>&nbsp;&nbsp;';"
		if (Operate[0] == 2) {
			// str += "c= '<a class=setgroup href=javascript:void(0) onclick=\"openNewPage(\\\'' + url2 + '\\\', \\\'新增\\\', \\\'' + btn + '\\\');\"><i class=\"fa  fa-plus-square\"/></a>&nbsp;&nbsp;';"
		    str += "d= '&nbsp;&nbsp;<button type=button class=\"btn btn-sm btn-info\" onclick=\"openNewPage(\\\'' + url3 + '\\\', \\\'表单\\\', \\\'' + btn + '\\\', \\\'' + Size + '\\\');\">表单</button>';"
		}
		str += "return a + b+ c+ d;} } ";
	}
	//str += ",{   field: 'useID',formatter: function (value, row, index) {var  id= eval('row.'+'" + key + "');return \"<span class=\'useID\'>\"+id+\"</span>\";}}";
	str += "]";

    
	var showdetail = false
	if (Operate[0] == 2) showdetail = false;
	InitTable($("#tb_" + ControllerID), actionUrl, dbQueryParams, eval(str), 'toolbar_' + ControllerID, showdetail);
	$("#btn_query_" + ControllerID).click(function () {
		var actionUrl = "/" + ControllerID + "/SearchLists?" + GetPageItemsValue(ControllerID) + "&id=" + ID;
		m_pagerow = 0;
		$("#tb_" + ControllerID).bootstrapTable('refresh', { url: actionUrl });

	});

}

function ShowTableAndQueryTree(Controller, ControllerID, Size, Operate, ID) { //CompanyName搜索框名称 

    var dbQueryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            PageSize: params.limit,   //页面显示行数
            PageIndex: params.offset / params.limit + 1//页码*页面显示行数=offset
        };
        return temp;
    };
    var actionUrl = "/" + ControllerID + "/SearchLists?" + GetPageItemsValue(ControllerID) + "&parentCode=" + ID;//
    var str = "[{title: 'ID',align: 'center',formatter: function (value, row, index) {return m_pagerow + index + 1; }},";

    var key = eval('dyKey_' + ControllerID);
    var btn = 'btn_query_' + ControllerID;

    str = "[{checkbox: true},";
    //str += "{field: '',title: '',align: 'center',formatter: function (value, row, index) {return '<a class=\"detail-icon\" href=\"javascript:\"><i class=\"glyphicon glyphicon-plus icon-plus\"></i></a>' }},";

    str += eval('dyTableCol_' + ControllerID);
    str = str.substring(0, str.length - 1);
    //console.log(str);
    if (Operate[0] != 0 || Operate[1] != 0 || Operate[2] != 0) {
        str += ",{field: 'operate',title: '" + languagePlugin.GetPageLanguage(languagePlugin.AllLanguageUrl, 'Common_Btn_Setting', "设置") + "',align: 'center',formatter: function (value, row, index) { var a = '';var b = '';var c = '';var d = '';var  id= eval('row.'+'" + key + "');"
        str += "  var url = Controller+'/" + ControllerID + "/Edit/' + id; var url1 = Controller+'/" + ControllerID + "/Delete/' + id;var url2 = Controller+'/" + ControllerID + "/Add/' + id;var url3 = Controller+'/" + ControllerID + "/List/' + id;"
        if (Operate[0] == 1 || Operate[0] == 2)
            str += "a = '<button type=button class=\"btn btn-sm btn-info\"  onclick=\"openNewPage(\\\'' + url + '\\\', \\\'编辑\\\', \\\'' + btn + '\\\', \\\'' + Size + '\\\');\">编辑</button>';"
        //if (Operate[1] == 1)
        //str += "b = '<a class=setgroup href=javascript:void(0) onclick=\"deleteInfo(\\\'' + url1 + '\\\', \\\'确认删除\\\', \\\'' + btn + '\\\');\"><i class=\"fa  fa-trash\"/></a>&nbsp;&nbsp;';"
        if (Operate[0] == 2) {
            // str += "c= '<a class=setgroup href=javascript:void(0) onclick=\"openNewPage(\\\'' + url2 + '\\\', \\\'新增\\\', \\\'' + btn + '\\\');\"><i class=\"fa  fa-plus-square\"/></a>&nbsp;&nbsp;';"
            str += "d= '&nbsp;&nbsp;<button type=button class=\"btn btn-sm btn-info\" onclick=\"openNewPage(\\\'' + url3 + '\\\', \\\'表单\\\', \\\'' + btn + '\\\', \\\'' + Size + '\\\');\">表单</button>';"
        }
        str += "return a + b+ c+ d;} } ";
    }
    //str += ",{   field: 'useID',formatter: function (value, row, index) {var  id= eval('row.'+'" + key + "');return \"<span class=\'useID\'>\"+id+\"</span>\";}}";
    str += "]";


    var showdetail = false
    if (Operate[0] == 2) showdetail = false;
    InitTable($("#tb_" + ControllerID), actionUrl, dbQueryParams, eval(str), 'toolbar_' + ControllerID, showdetail);
    if (ID != null) {
        $("#tb_" + ControllerID).bootstrapTable('refresh', { url: actionUrl });
    }

}

function getPageurl(table) {
    var ControllerID = table.replace("toolbar_", "");
    var actionUrl = "/" + ControllerID + "/SearchLists?" + GetPageItemsValue(ControllerID);
    return actionUrl;
}
function getnewurl(url) {
    
    url = url.substring(1, url.lastIndexOf("/"));
    var actionUrl = "/" + url + "/SearchLists?" + GetPageItemsValue(url);

    return actionUrl;
}
//closeOropenItemsCheck
function closeOropenItems(Controller, ControllerID,value) {
    var key = eval('dyKey_' + ControllerID);
    var arrselections = $("#tb_" + ControllerID).bootstrapTable('getSelections');
    if (arrselections.length <= 0) {
        layer.msg('请选择有效数据');
        return;
    }
    var delid = new Array();
    
    for (var i = 0; i < arrselections.length;i++){
        delid.push(eval('arrselections[i].' + key));
    }
     
   
    value = value;
    deleteInfo(Controller + "/" + ControllerID + "/Deletes?delid=" + delid + "&value=" + value, '确认操作', 'btn_query_' + ControllerID);
    //ajax.get("Admin/Deletes", { delid: delid, value: value }, function (e) {
         
    //    $("#" + id).html(str);

    //})

}
function detailFormatter(index, row) {

}

function writeLanguageOption(id) {

	ajax.get("Ajax/GetLanguages", null, function (e) {
		var len = e.rows.length;
		var str = "";
		//str += "<option value=''></option>";
		for (var i = 0; i < len; i++) {
			str += "<option value='" + e.rows[i].LRCultureCode + "'>" + e.rows[i].LRCultureName + "</option>";
		}
		$("#" + id).html(str);

	})
}
function writeAdminOption(url,id,value) { 
    ajax.get(url, null, function (e) {
        //console.log(JSON.)
        var len = e.id.length;
		var str = "";
		str += "<option value=''></option>";
		for (var i = 0; i < len; i++) {
		    if (value == e.id[i])
		        str += "<option value='" + e.id[i] + "' selected>" + e.name[i] + "</option>";
		    else
		        str += "<option value='" + e.id[i] + "'>" + e.name[i] + "</option>";
		}
		$("#" + id).html(str);

	})
}

function writeConfigOption(id, dom) {
	var params = {};
	params.id = id;
	ajax.get("Ajax/GetConfig", params, function (e) {
		var len = e.rows.length;
		var str = "";
		//str += "<option value=''></option>";
		for (var i = 0; i < len; i++) {

			str += "<option value='" + e.rows[i].ConfigTypeId + '_' + e.rows[i].ConfigValue + "' config='" + e.rows[i].ConfigId + "'>" + e.rows[i].ConfigText + "</option>";
		}
		$("#" + dom).html(str);

	})
}
function writeChildConfigOption(id, dom) {
	var params = {};
	params.id = id;
	ajax.get("Ajax/GetChildConfig", params, function (e) {
		var len = e.rows.length;
		var str = "";
		for (var i = 0; i < len; i++) {
			str += "<option value='" + e.rows[i].ConfigTypeId + '_' + e.rows[i].ConfigValue + "' config='" + e.rows[i].ConfigId + "'>" + e.rows[i].ConfigText + "</option>";
		}
		$("#" + dom).html(str);

	})
}

function changeConfig(obj) {
	clearSelect(obj);
	$("select[pcid='" + obj.attr("id") + "']").each(function () {
		var dom = $(this).attr("id");
		var id = obj.find("option:selected").attr("config");
		//alert(obj.find("option:selected").attr("config"))

		writeChildConfigOption(id, dom);
	});

}

function clearSelect(obj) {
	var children = $("select[pcid='" + obj.attr("id") + "']");
	if (children.length > 0) {
		var id = children.attr("id");
		children.html('<option value=""></option>');
		clearSelect($("#" + id));

	}
	else {
		children.html('<option value=""></option>');
	}
}

//获取所有可用menu
function getAllSecMenu(id) {
    var str='';
    $(".treeviewchoose").find(".treeview-menu").each(function () {
        $(this).find("li").each(function () {
            str += '<li><a onclick="showPage(\'' + $(this).find("a").attr("add") + '\',\'' + $(this).find("a").attr("id") + '\',\'' + $(this).find("a").find("span").html() + '\',\'' + $(this).find("a").attr("parentname") + '\')">' + $(this).find("a").find("span").html() + '</a></li>';
        })
        str += '<li class="divider"></li>';
    })
    //document.write(str);
    $("#"+id).html(str)
}

//  showPage("/Admin/SysUser",id,name);
function showPage(url, id, name, parentname,props) {
    //如果id 存在 则显示否则打开
    /*if($("#righttab_"+id).length>0){
      $("#tabul").find("li").removeClass("active");
      var  nid="#righttab_"+id;
      $("a[href='"+nid+"']").parent().addClass("active");
      $("#tab-content").find("div").removeClass("active");
      $("#righttab_"+id).addClass("active");
    return;
    }*/
    ajax.loading();
    $(".treeview-menu").find("li").removeClass("active");
    $(props).parent().addClass("active");
    $.ajax({
        url: url, dataType: "html", success: function (e) {
            /*  $("#tabul").find("li").removeClass("active");
              $("#tabul").append('<li class="active"><a href="#righttab_'+id+'" data-toggle="tab" aria-expanded="true">'+name+'<button  onclick="closetab(this)"  type="button" class="close closetab" data-dismiss="alert" aria-hidden="true">×</button></a></li>');
              $("#tab-content").find("div").removeClass("active");
              $("#tab-content").append('<div class="tab-pane active" id="righttab_'+id+'">'+e+'</div>');*/
            $("#tab-content").html('<div class="tab-panel active" id="righttab_' + id + '" name="' + name + '" parentname="' + parentname + '">' + e + '</div>');
           ajax.loadingClose();
        }, error: function () {
           ajax.loadingClose();
        }
    });
}


function closeOropenItemsCheck(ControllerID, Controller, id, obj) {
    var status = ($(obj).is(':checked'));
    var value = 1;
    layer.confirm('确认操作', {
        shadeClose: true,
        title: [languagePlugin.GetPageLanguage(languagePlugin.AllLanguageUrl, 'Common_Button_Mark', "提示")],
        btn: [languagePlugin.GetPageLanguage(languagePlugin.AllLanguageUrl, 'Common_Btn_Save', "确定"), languagePlugin.GetPageLanguage(languagePlugin.AllLanguageUrl, 'Common_Btn_Cancel', "取消")],
        //skin: 'layersmall',
        //area: ['345px', '200px'],
        btn1: function () {
            if (status === false) value = 0;
            var delid = new Array();

            for (var i = 0; i < 1; i++) {
                delid.push(id);
            }
            ajax.post(Controller + "/" + ControllerID + "/Deletes?delid=" + delid + "&value=" + value, "", function (data) { msg.info(data.Msg); $("#tb_" + ControllerID).bootstrapTable('refresh'); });
        },
        btn2: function () {
            $(obj).prop('checked', !status);
        },
        end: function () {
            $(obj).prop('checked', !status);
        }
    });

    //msg.confirm('确认操作', function () {
        
    //    if (status === false) value = 0;
    //    var delid = new Array();

    //    for (var i = 0; i < 1; i++) {
    //        delid.push(id);
    //    }
    //    ajax.post(Controller + "/" + ControllerID + "/Deletes?delid=" + delid + "&value=" + value, "", function (data) { msg.info(data.Msg); $("#tb_" + ControllerID).bootstrapTable('refresh'); });
    //}, function () {
    //    alert(11);
    //    $(obj).prop('checked', !status);

    //});
    
     

}

function closeOropenItemsChoose(a, b, c, d, value) {
    var str;
    if (d === 1) {
        str = '是';
    }else {
        str = '否'
    }
    return str;
}



var tempIconID;
function showIcon(id) {
    tempIconID = id;
    layer.open({
        type: 2,
        title: false,
        closeBtn: 0,
        area: ["80%", "80%"],
        fixed: false, //不固定
        maxmin: true,
        content: '/Home/Icon'
    });
}

function closeLayer(val) {
    $("#" + tempIconID).val(val)
    layer.closeAll();
}

function setTableChangePos(id) { // 当出现横向滚动条是表格变化
    //$("#" + id).on('load-success.bs.table', function (data) {
    //    var $table = $(this);
    //    setTimeout(function () {
    //        var $obj = $table.parent();
    //        var obj = $obj[0];

    //        if (obj.scrollHeight > obj.clientHeight || obj.offsetHeight > obj.clientHeight) {
    //            $table.bootstrapTable('toggleView');
    //        }
    //    }, 500);

    //});
}
function reloadController(classname){
    $("." + classname).each(function () {
         $(this).val("")
    })
}