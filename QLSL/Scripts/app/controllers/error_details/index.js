var errorDetailsController = function() {
    var cachedObj = {
       
    }
    this.initialize = function () {
        $.when()
            .done(function () {
                loadData();


            });

        registerEvents();

    }
    function registerEvents() {
        $('.panel').on('hidden.bs.collapse', toggleIcon);
        $('.panel').on('shown.bs.collapse', toggleIcon);
    }
    function toggleIcon(e) {
        $(e.target)
            .prev('.panel-heading')
            .find(".more-less")
            .toggleClass('fa-angle-double-down fa-angle-double-up');
    }

   


    function loadData() {
        
        $(".panelChart").html('<div class="text-center"><img src="/Images/loading.gif" height="100px" /></div>');
        $.ajax({
            type: "GET",
            url: "/Report/GetErrors",
            data: [],
            dataType: "json",
            beforeSend: function () {

            },
            success: function (rslt) {
                console.log(rslt);
                
                $(".panelTLE").html('');
                $(".panelCCTVE").html('');
                $(".panelVMSE").html('');
                $(".panelWIME").html('');
                var content;
                if (rslt.CCTVEs.length > 0) {
                    $("#headingCCTVE b").html("Camera <i>(" + rslt.CCTVEs.length + ")</i>");
                    //content = '<div class="row text-center" style="padding-left: 5px">' +
                    //                '<h4> Đèn tín hiệu </h4></div>' +
                    content = '<div class="table-responsive" id="tableCCTVE"></div>';
                    $(".panelCCTVE").append(content);
                    var newRow = '<table class="table table-striped table-bordered table-condensed table-hover">' +
                                    '<thead><tr><th class="width-3">No.</th>' +
                                    '<th class="width-10">Thời gian</th>' +
                                    '<th>Camera</th>' +
                                    '<th class="width-15">Lỗi</th></th>' +
                                    '<th>Chi tiết</th></th></tr></thead><tbody>';


                    $.each(rslt.CCTVEs,
                                  function (i, td) {
                                      newRow += '<tr><td>' + (i + 1) + '</td>' +
                                          '<td>' + formatDate(td.DateOccur) + '</td>' +
                                          '<td>' + td.Subject + '</td>' +
                                          '<td>' + td.Error + '</td>' +
                                          '<td>' + td.Detail + '</td></tr>';

                                  });
                    newRow += '</tbody></table>';
                    $('#tableCCTVE').append(newRow);
                }
     

                if (rslt.TLEs.length > 0) {
                    $("#headingTL b").html("Đèn tín hiệu <i>(" + rslt.TLEs.length+")</i>");
                    //content = '<div class="row text-center" style="padding-left: 5px">' +
                    //                '<h4> Đèn tín hiệu </h4></div>' +
                    content = '<div class="table-responsive" id="tableTLE"></div>';
                    $(".panelTLE").append(content);
                    var newRow = '<table class="table table-striped table-bordered table-condensed table-hover">' +
                                    '<thead><tr><th class="width-3">No.</th>' +
                                    '<th class="width-10">Thời gian</th>' +
                                    '<th>Chốt đèn</th>' +
                                    '<th class="width-15">Lỗi</th></th>' +
                                    '<th>Chi tiết</th></th></tr></thead><tbody>';

             
                    $.each(rslt.TLEs,
                                  function (i, td) {
                                      newRow += '<tr ><td>' + (i + 1) + '</td>' +
                                          '<td>' + formatDate(td.DateOccur) + '</td>' +
                                          '<td>' + td.Subject + '</td>' +
                                          '<td>' + td.Error + '</td>' +
                                          '<td>' + td.Detail + '</td></tr>';

                                  });
                    newRow += '</tbody></table>';
                    $('#tableTLE').append(newRow);
                }
                if (rslt.VMSEs.length > 0) {
                    $("#headingVMSE b").html("VMS <i>(" + rslt.VMSEs.length + ")</i>");
                    //content = '<div class="row text-center" style="padding-left: 5px">' +
                    //                '<h4> Đèn tín hiệu </h4></div>' +
                    content = '<div class="table-responsive" id="tableVMSE"></div>';
                    $(".panelVMSE").append(content);
                    var newRow = '<table class="table table-striped table-bordered table-condensed table-hover">' +
                                    '<thead><tr><th class="width-3">No.</th>' +
                                    '<th class="width-10">Thời gian</th>' +
                                    '<th>VMS</th>' +
                                    '<th class="width-15">Lỗi</th></th>' +
                                    '<th>Chi tiết</th></th></tr></thead><tbody>';


                    $.each(rslt.VMSEs,
                                  function (i, td) {
                                      newRow += '<tr ><td>' + (i + 1) + '</td>' +
                                          '<td>' + formatDate(td.DateOccur) + '</td>' +
                                          '<td>' + td.Subject + '</td>' +
                                          '<td>' + td.Error + '</td>' +
                                          '<td>' + td.Detail + '</td></tr>';

                                  });
                    newRow += '</tbody></table>';
                    $('#tableVMSE').append(newRow);
                }
                if (rslt.WIMEs.length > 0) {
                    $("#headingWIME b").html("Trạm cân <i>(" + rslt.WIMEs.length + ")</i>");
                    //content = '<div class="row text-center" style="padding-left: 5px">' +
                    //                '<h4> Đèn tín hiệu </h4></div>' +
                    content = '<div class="table-responsive" id="tableWIME"></div>';
                    $(".panelWIME").append(content);
                    var newRow = '<table class="table table-striped table-bordered table-condensed table-hover">' +
                                    '<thead><tr><th class="width-3">No.</th>' +
                                    '<th class="width-10">Thời gian</th>' +
                                    '<th>Trạm</th>' +
                                    '<th class="width-15">Lỗi</th></th>' +
                                    '<th>Chi tiết</th></th></tr></thead><tbody>';


                    $.each(rslt.WIMEs,
                                  function (i, td) {
                                      newRow += '<tr ><td>' + (i + 1) + '</td>' +
                                          '<td>' + formatDate(td.DateOccur) + '</td>' +
                                          '<td>' + td.Subject + '</td>' +
                                          '<td>' + td.Error + '</td>' +
                                          '<td>' + td.Detail + '</td></tr>';

                                  });
                    newRow += '</tbody></table>';
                    $('#tableWIME').append(newRow);
                }
            },
            error: function (status) {
                console.log(status);
            }
        });
    } 




    function formatDate(date) {
        var d = new Date(parseInt(date.substr(6))),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getYear() - 100;
        hours = '' + d.getHours(),
        minutes = '' + d.getMinutes();


        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;
        if (hours.length < 2) hours = '0' + hours;
        if (minutes.length < 2) minutes = '0' + minutes;

        return [day, month, year].join('/') + " " + [hours, minutes].join(':');
    }
}