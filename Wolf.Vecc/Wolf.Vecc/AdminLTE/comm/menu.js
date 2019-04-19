var menu = menu ? menu : {
    initMenu: function () {
        $("ul.menu > li").off("click").on("click", function (e) {
            var $this = $(this);
            $("ul.menu > li.open > a >.glyphicon").removeClass("glyphicon-menu-up").addClass("glyphicon-menu-down");
            if ($this.hasClass("open")) {
                $this.removeClass("open");
                $this.children("ul").slideUp("fast");
            } else {
                $("ul.menu > li.open").children("ul").slideUp("fast");
                $("ul.menu > li.open").removeClass("open");
                $this.addClass("open");
                //$this.find(".glyphicon").addClass("glyphicon-menu-up");
                $("ul.menu > li.open > a >.glyphicon").addClass("glyphicon-menu-up");
                $this.children("ul").slideDown("fast");
            }
            return false;
        });
        $("ul.menu > li > ul.submenu > li").off("click").on("click", function (e) {
            var $this = $(this);
            if (e && e.stopPropagation) {
                e.stopPropagation();
            } else {
                window.event.cancelBubble = true;
            }
            var $a = $this.find("a");
            var id = $a.data("id");
            var href = $a.attr("href");
            var name = $a.text();
            menu.addTab(id, href, name);
            return false;
        });
    },
    addTab: function (id, href, name) {
        if ($(window).width() <= 767) {
            $(window).resize();
        }
        var $document = $(top.document);
        var $tab = $document.find(".tabs>.nav-tabs>li>#tab-" + id);
        if ($tab.length !== 0) {
            menu.showTab(id, $document);
            return;
        }
        ajax.loading()
        $document.find(".tabs>.nav-tabs").append('<li role="presentation"><a id="tab-' + id + '" href="#tab-content-' + id + '" aria-controls="tab-content-' + id + '" role="tab" data-toggle="tab"><span class="tab-title">' + name + '</span><span class="glyphicon glyphicon-remove-circle ml5 tab-close" aria-hidden="true"></span></a></li>');
        $document.find(".tabs>.tab-content").append('<div role="tabpanel" class="tab-pane" id="tab-content-' + id + '"><iframe class="tab-iframe block" frameborder="0" scrolling="auto" width="100%" height="100%" src="' + href + '"></iframe></div>');
        $tab = $document.find(".tabs>.nav-tabs>li>#tab-" + id);
        menu.showTab(id, $document);
        $tab.on("mousedown", function (event) {
            if (event.which == 2) {
                menu.removeTab(id, $document);
            }
        });
        $tab.find(".tab-close").on("click", function () {
            menu.removeTab(id, $document);
        });
        var $iframe = $document.find("#tab-content-" + id).find(".tab-iframe");
        $tab.find(".tab-title").on("dblclick", function () {
            $iframe.attr('src', $iframe.attr('src'));
        });
        $iframe.on('load', function () {
            ajax.loadingClose();
        });
        return false;
    },
    removeTab: function (id, $document) {
        var $cur = $document.find("#tab-" + id).parent();
        var index = $document.find(".tabs>.nav-tabs>li[role='presentation']").index($cur);
        $cur.remove();
        $document.find("#tab-content-" + id).remove();
        if ($document.find(".tabs>.nav-tabs>li[role='presentation'].active").length == 0) {
            var showId = $document.find(".tabs>.nav-tabs>li[role='presentation']").eq(index - 1).find("a").attr("id");
            if (showId.length > 4) {
                menu.showTab(showId.substring(4), $document);
            }
        }
        return false;
    },
    showTab: function (id, $document) {
        $document.find(".tabs>.nav-tabs>li[role='presentation'].active").removeClass("active");
        $document.find(".tabs>.tab-content>.tab-pane[role='tabpanel'].active").removeClass("active");
        $document.find("#tab-" + id).parent().addClass("active");
        $document.find("#tab-content-" + id).addClass("active");
        return false;
    }
};