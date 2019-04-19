var table = {
    initial: function () {

        $(".no-more-tables table").each(function () {
            var ths = [];
            $(this).find("thead tr th").each(function () {
                ths.push($(this).text().trim());
            });
            $(this).find("tbody tr").each(function () {
                var tds = $(this).find("td");
                for (var i = 0; i < tds.length && i < ths.length; i++) {
                    if (ths[i] != "" && $(tds[i]).attr("data-title") == null) {
                        $(tds[i]).attr("data-title", ths[i]);
                    }
                }
            });
        });
    }

}