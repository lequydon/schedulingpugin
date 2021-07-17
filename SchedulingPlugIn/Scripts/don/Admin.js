//get data create heartmap and create Analysis Consultation
calendarfunc();
function calendarfunc() {
    $.ajax({
        url: "/Admin/GetListTimeCalendar",
        type: "get",
        success: function (e) {
            heartmap1(e);
        }
    })
}
function heartmap1(e) {
    $('#cal-heatmap').html("");
    //color for heartmap
    var color = ["FFFFFF", "#DEE499", "#A6C96B", "#6FA64A", "#6F8248", "#426D2F"];
    var level_number = [0, 2, 4, 6, 8];
    var DateTimeZone = new Date().toLocaleString("en-US", { timeZone: "America/New_York" });
   // var DateNow = new Date().toLocaleString("en-US", { timeZone: "America/New_York" });
    var DateNow = new Date(DateTimeZone);
    //console.log(DateNow);
    var DateTemp = new Date(DateTimeZone);
    console.log(DateTemp);
    var DateTempGetDay = new Date(DateTimeZone);
    // console.log(DateNow.getMonth());
   
    var tag = $('#cal-heatmap');
    //month in year display
    tag.append("<div class='month'></div>");
    $('.month').text(month_name(DateNow));
    $('.month').css({ "font-size": "23px", "margin-bottom": "10px", "text-align": "center" });
    var name_date = 1;
    // date name in week display
    var listday = ["Mo", "Tu", "We", "Th", "Fr", "Sa", "Su"];
    tag.append("<div class='date'></div>");
    for (var i = 0; i < 7; i++) {
        $(".date").append("<span class='DayInWeek" + name_date + "'>" + listday[i] + "</span>");
        name_date++;
    }
    //data in month display
    ///dislay date
    var NumberDay = getDaysOfMonth(DateNow.getFullYear(), DateNow.getMonth());   
    DateTemp.setDate(1);
    var dislaydateTemp = DateTemp.getDay();
    if (dislaydateTemp == 0) {
        dislaydateTemp = 7;
    }
    var CountDate = 1;
    var setdate = 1;
    for (var i = 1; i <= 42; i++) {
        if (i < dislaydateTemp || i > (dislaydateTemp + NumberDay)-1) {
            console.log(dislaydateTemp);
            $(".date").append("<span class='" + i + "'></span>");
        }
        else {
            var consulation = 0;
            for (tempd in e) {
                if (CountDate == e[tempd].time) {
                    consulation = e[tempd].value;
                    break;
                }
            }
            var color_tmp;
            //get color for day
            if (consulation == level_number[0])
                color_tmp = color[0];
            else
                for (var tempd = 0; tempd < level_number.length;tempd++) {
                    if (consulation > level_number[tempd - 1] && consulation <= level_number[tempd])
                        color_tmp = color[tempd];
                        //console.log(color);
                }
            if (consulation > level_number[level_number.length-1])
                color_tmp = color[level_number.length];
            
            DateTempGetDay.setDate(CountDate);
            var CountDateName = "";
            if (CountDate == 1 || CountDate == 21 || CountDate==31)
                CountDateName = CountDate + "st";
            if (CountDate == 2 || CountDate == 22)
                CountDateName = CountDate + "nd";
            if (CountDate == 3 || CountDate == 23)
                CountDateName = CountDate + "rd";
            if (CountDate != 1 && CountDate != 21 && CountDate != 31 && CountDate != 2 && CountDate != 22 && CountDate != 3 && CountDate != 23)
                CountDateName = CountDate + "th";
            if (i <= 35)
                $(".date").append("<div id='date_intime' style='background-color:" + color_tmp + "'title='" + GetFullNameDay(DateTempGetDay) + " " + CountDateName + " <br/> Consultations:" + consulation + "' day='day" + CountDate + "' class='" + i + "'></div>");
            else {
                $(".date ." + setdate + "").replaceWith("<div id='date_intime' style='background-color:" + color_tmp + "'title='" + GetFullNameDay(DateTempGetDay) + " " + CountDateName + " <br/> Consultations:" + consulation + "' day='day" + CountDate + "' class='" + i + "'></div>");
                setdate++;
            }
            CountDate++;
        }
    }
    $('.date #date_intime').css({"border":"0.5px solid #ccc"});
    $('.date').css({ 'display': 'grid', "grid-gap": "8px" });
    $('.date').css('grid-template-columns', '1fr 1fr 1fr 1fr 1fr 1fr 1fr');
   // $('.date div').css({ "width": "20px", "height": "20px" }); 
    $('.date #date_intime').css({ "border-radius": "5px", "width": "20px", "height": "20px" });
    //legend in heart map
    tag.append("<div class='legend'></div>");
    $('.legend').append("<div class='less'>Less</div>");
    
    for (var i = 0; i < 6; i++) {
        if (i == 0)
            $('.legend').append("<div class='level' id='level" + i + "' style='background-color:" + color[i] + "' title='Less " + level_number[i] + "'></div>");
        if (i >0 && i<5)
            $('.legend').append("<div class='level' id='level" + i + "' style='background-color:" + color[i] + "' title='between " + level_number[i-1] + " and " + level_number[i] + "'></div>");
        if (i == 5)
            $('.legend').append("<div class='level' id='level" + i + "' style='background-color:" + color[i] + "' title='More " + level_number[4] + "'></div>");
    }
    $('.legend .level').css({ "width": "20px", "height": "20px" });
    $('.legend .level').css({  "border-radius": "5px", "border": "0.5px solid #ccc" });
    $('.legend').append("<div class='more'>More</div>");
    $(".legend").css("font-size", "12px");
    $(".legend").css({"display":"grid","float":"right","margin-top":"15px"});
    $(".legend").css('grid-template-columns', '1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr');
    //tooltip
    $(document).ready(function () {
        var tooltip = $(".date").kendoTooltip({
            filter: "div",
            width: 120,
            position: "top",
            animation: {
                open: {
                    effects: "zoom",
                    duration: 150
                }
            }
        }).data("kendoTooltip");

    });

}
function month_name(dt) {
    mlist = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    return mlist[dt.getMonth()];
};
function GetFullNameDay(d) {
    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    return days[d.getDay()];
}
//function heartmap(datas) {
//    var parser = function (data) {
//        var stats = {};
//        for (var d in data) {
//            stats[data[d].date] = data[d].value;
//        }
//        return stats;
//    };
//    var cal = new CalHeatMap();
//    cal.init({
//        itemName: ["Consultation"],

//        data: datas,
//        afterLoadData: parser,
//        domain: "month",
//        subDomain: "x_day",
//        cellSize: 20,
//        cellPadding: 15,
//        cellRadius: 5,
//        range: 1,
//        label: {
//            position: "top"
//        },
//        displayLegend: true,
//        legendCellSize: 20,
//        legendCellPadding: 10,
//        legendHorizontalPosition: "right",
//        highlight: false,
//        tooltip: true,
//        legend: [1, 3, 5, 7, 9],
//        legendColors: {
//            min: "#F2F2F2",
//            max: "#133804"
//        },

//    });

//    // hertmap DOM
//}
//get data create analysis consultation 
analysis();
function analysis() {
    $.ajax({
        type: "get",
        url: "/admin/gettimeforanalysis",
        success: function (e) {
            for (i in e) {
                console.log(e[i].date);
                e[i].date = e[i].date.slice(6, 16);
                
            }
            AnalysisConsulation(e);
        }
    })
}

function getDaysOfMonth(year, month) {
    return new Date(year, month + 1, 0).getDate();
}
function AnalysisConsulation(e) {
    var total_this_month = 0;
    var total_remain = 0;
    var this_date = new Date();
    for (i in e) {
        var temp = new Date(e[i].date * 1000);
        if (temp.getMonth() == this_date.getMonth()) {
            total_this_month++;
        }

        if (temp > this_date)
            total_remain++;
    }
    var AVG_thismonth = 0;
    AVG_thismonth = total_this_month / getDaysOfMonth(this_date.getFullYear(), this_date.getMonth());
    var n = parseFloat(AVG_thismonth); AVG_thismonth = Math.round(n * 10) / 10;
    
    $('.Consultation_month').text(total_this_month);
    $('.Consultation_remain').text(total_remain);
    $('#AVG_Consultation_month_number').text(AVG_thismonth);
}
//get data create next consulation
var obj;
form();
function form() {
    $.ajax({
        url: "/Admin/listcontact",
        type: "get",
        success: function (e) {
            obj = e;
            if (e[0] != null) {
                $("#Consultation_date_date").html(e[0].DateNotStamp);
                $("#Consultation_date_time").html(e[0].Time);
                $('.Consultation_Name').html(e[0].FirstName);
                $('.Consultation_email').html(e[0].Email);
                $('.Consultation_phone').html(e[0].Phone);
                $('.Consultation_contact').html(e[0].PageRequestName);
            }
            else {
                $("#Consultation_date_date").html("");
                $("#Consultation_date_time").html("");
                $('.Consultation_Name').html("");
                $('.Consultation_email').html("");
                $('.Consultation_phone').html("");
                $('.Consultation_contact').html("");
            }
        },
        async: false
    })
}
//create kendo grid_Expired
$(document).ready(function () {
    $("#grid_finish").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: "/Admin/GetList?number=" + 1,
                }
            }
        },
        columns: [
            {
                hidden: true,
                field: "ID"
            },
            {
                field: "FirstName",
                title: "Full Name",
                width: 130,
            },
            {
                field: "DateNotStamp",
                title: "Date",
                width: 100,
            },
            {
                field: "Time",
                title: "Time",
                width: 130,
            },
            {
                field: "PageRequestName",
                title: "Page",
                width: 170,
            },
            {
                title: "Commands",
                width: 195,
                command: [
                    {
                        text: "Cancel", iconClass: "fa fa-close", className: "btn-Cancel-grid",
                        click: function (e) {
                            $('.textareaSaved').val("");
                            dialog_decline.data("kendoDialog").open();
                            e.preventDefault();
                            var tr = $(e.target).closest("tr");
                            var data = this.dataItem(tr);
                            total_grid(data);
                        }
                    },
                    {
                        text: "Details", iconClass: "fa fa-info-circle", className: "btn-details-grid1",
                        click: function (e) {
                            e.preventDefault();
                            var tr = $(e.target).closest("tr");
                            var data = this.dataItem(tr);
                            total_grid(data);
                            detailpopup(data.ID);
                            contactpopup(data.ID);
                            $('.button_fromAccept').css('display', 'none');
                            $(".button_fromDecline").hide();
                            $(".button_fromCancel").show();
                            dialog_start.data("kendoDialog").open();
                        }
                    }]
            }]
        ,
        height: 350,
        groupable: false,
        sortable: true,
        reorderable: true,
        resizable: true,
        columnMenu: false,

    });
});
//end grid_Expired
//create kendo grid fish_nish(after accept)
$(document).ready(function () {
    $("#grid_Expired").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: "/Admin/GetList?number=" + 3,
                }
            }
        },
        columns: [
            {
                hidden: true,
                field: "ID"
            },
            {
                field: "FirstName",
                title: "Full Name",
                width: 130,
            },
            {
                field: "DateNotStamp",
                title: "Date",
                width: 100,
            },
            {
                field: "Time",
                title: "Time",
                width: 130,
            },
            {
                field: "PageRequestName",
                title: "Page",
                width: 150,
            },
            {
                title: "Commands",
                width: 195,
                command: [
                    {
                        text: "Decline", iconClass: "fa fa-close", className: "btn-decline-grid",
                        click: function (e) {
                            $('.textareaSaved').val("");
                            dialog_decline.data("kendoDialog").open();
                            e.preventDefault();
                            var tr = $(e.target).closest("tr");
                            var data = this.dataItem(tr);
                            total_grid(data);
                        }
                    },
                    {
                        text: "Details", iconClass: "fa fa-info-circle", className: "btn-details-grid11",
                        click: function (e) {
                            e.preventDefault();
                            var tr = $(e.target).closest("tr");
                            var data = this.dataItem(tr);
                            total_grid(data);
                            detailpopup(data.ID);
                            contactpopup(data.ID);
                            $('.button_fromAccept').css('display', 'none');
                            $(".button_fromDecline").show();
                            $(".button_fromCancel").hide();
                            dialog_start.data("kendoDialog").open();
                        }
                    }]
            }]
        ,
        height: 350,
        groupable: false,
        sortable: true,
        reorderable: true,
        resizable: true,
        columnMenu: false,

    });
});
//create kendo grid start 
$(document).ready(function () {
    $("#grid_start").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: "/Admin/GetList?number=" + 0
                }
            }
        },
        columns: [
            {
                hidden: true,
                field: "ID"
            },
            {
                field: "FirstName",
                title: "Full Name",
                width: 130,
            },
            {
                field: "DateNotStamp",
                title: "Date",
                width: 100,
            },
            {
                field: "Time",
                title: "Time",
                width: 130,
            },
            {
                field: "PageRequestName",
                title: "Page",
                width: 150,
            },
            {
                title: "Commands",
                command: [
                    {
                        text: "Decline", iconClass: "fa fa-close", className: "btn-decline-grid",
                        click: function (e) {
                            $('.textareaSaved').val("");
                            dialog_decline.data("kendoDialog").open();
                            e.preventDefault();
                            var tr = $(e.target).closest("tr");
                            var data = this.dataItem(tr);
                            total_grid(data);
                        }
                    },
                    {
                        text: "Accept", iconClass: "fa fa-check", className: "btn-accept-grid",
                        click: function (e) {
                            e.preventDefault();
                            var tr = $(e.target).closest("tr");
                            var data = this.dataItem(tr);
                            total_grid(data);
                            $.ajax({
                                type: "post",
                                url: "/Admin/UpdateResson",
                                data: { idCalendar: data.ID, status: 1, reason: "" },
                                success: function (da) {
                                    const Toast = swal.mixin({
                                        toast: true,
                                        position: 'top-left',
                                        showConfirmButton: false,
                                        timer: 3000
                                    })

                                    Toast.fire({
                                        type: 'success',
                                        title: 'Successfully Accept this record!'
                                    })
                                    $("#grid_start").getKendoGrid().dataSource.read();
                                    $("#grid_start").data('kendoGrid').refresh();
                                    $("#grid_finish").getKendoGrid().dataSource.read();
                                    $("#grid_finish").data('kendoGrid').refresh();
                                    $("#grid_Expired").getKendoGrid().dataSource.read();
                                    $("#grid_Expired").data('kendoGrid').refresh();
                                    loadpage();
                                    senmail(data.ID, 1, "");
                                }
                            });
                            return true;
                        }
                    },
                    {
                        text: "Details", iconClass: "fa fa-info-circle", className: "btn-details-grid2",
                        click: function (e) {
                            e.preventDefault();
                            // e.target is the DOM element representing the button
                            var tr = $(e.target).closest("tr"); // get the current table row (tr)
                            // get the data bound to the current table row
                            var data = this.dataItem(tr);
                            total_grid(data);
                            detailpopup(data.ID);
                            contactpopup(data.ID);
                            $('.button_fromAccept').show();
                            $(".button_fromDecline").show();
                            $(".button_fromCancel").hide();
                            dialog_start.data("kendoDialog").open();

                        }
                    }
                ],
                width: 310,
            }],
        height: 350,
        groupable: false,
        sortable: true,
        reorderable: true,
        resizable: true,
        columnMenu: false,

    });
});
var total;
function total_grid(e) {
    total = e;
}
function senmail(id,status,reason) {
    $.ajax({
        type: "post",
        url: "/Admin/send",
        data: { idCalendar: id, status: status, reason: reason }
    });
}
$('.button_fromSaved').click(function () {
    if ($('.textareaSaved').val() != "") {
        $.ajax({
            type: "post",
            url: "/Admin/UpdateResson",
            data: { idCalendar: total.ID, status: -1, reason: $('.textareaSaved').val() },
            success: function () {
                const Toast = swal.mixin({
                    toast: true,
                    position: 'top-left',
                    showConfirmButton: false,
                    timer: 3000
                })

                Toast.fire({
                    type: 'success',
                    title: 'Successfully Saved this record!'
                })
                $("#grid_finish").getKendoGrid().dataSource.read();
                $("#grid_finish").data('kendoGrid').refresh();
                $("#grid_start").getKendoGrid().dataSource.read();
                $("#grid_start").data('kendoGrid').refresh();
                $("#grid_Expired").getKendoGrid().dataSource.read();
                $("#grid_Expired").data('kendoGrid').refresh();
                loadpage();
                senmail(total.ID, -1, $('.textareaSaved').val());
            }
        });
        
    }
    else
        $('.fail').css('display', 'block');
})
var FlagReload = 0;
function loadpage() {
    if (FlagReload == 0) {
        form();
        calendarfunc();
        analysis();
        FlagReload = 1;
    }
    form();
    calendarfunc();
    analysis();
    dialog_decline.data("kendoDialog").close();


    dialog_start.data("kendoDialog").close();
}
$('.textareaSaved').click(function () {
    $('.fail').css('display', 'none');
})
$('.button_fromAccept').click(function () {
    // if ($("#test_click").attr("temp") == "1")
    $.ajax({
        type: "post",
        url: "/Admin/UpdateResson",
        data: { idCalendar: total.ID, status: 1, reason: "" },
        success: function () {
            const Toast = swal.mixin({
                toast: true,
                position: 'top-left',
                showConfirmButton: false,
                timer: 3000
            })

            Toast.fire({
                type: 'success',
                title: 'Successfully Accept this record!'
            })
            $("#grid_start").getKendoGrid().dataSource.read();
            $("#grid_start").data('kendoGrid').refresh();
            $("#grid_finish").getKendoGrid().dataSource.read();
            $("#grid_finish").data('kendoGrid').refresh();
            $("#grid_Expired").getKendoGrid().dataSource.read();
            $("#grid_Expired").data('kendoGrid').refresh();
            
            loadpage();
            senmail(total.ID, 1, "");
        }
    });
    total = null;
    form();
    dialog_start.data("kendoDialog").close();
});
var dialog_start = $("#dialog_gridStart");
var dialog_decline = $("#dialog_decline");
var show_decline_dialog = $(".button_fromDecline");
var show_view_more = $(".view_more");
dialog_start.kendoDialog({
    width: "850px",
    resizable: true,
    modal: {
        preventScroll: true
    },
    title: "Consultation Detail",
    visible: false,
});
show_view_more.click(function () {
    // $("#test_click").attr("temp", "2");
    try {
        contactpopup(obj[0].ID);
        detailpopup(obj[0].ID);
        total_grid(obj[0]);
        if (obj[0].Status == 1)
            $('.button_fromAccept').css('display', 'none');
            $(".button_fromDecline").hide();
            $(".button_fromCancel").show();
        if (obj[0].Status == 0)
            $('.button_fromAccept').css('display', 'inline-block');
        dialog_start.data("kendoDialog").open();
    }
    catch (er){
        console.log("no data");
    }
});
show_decline_dialog.click(function () {
    dialog_decline.data("kendoDialog").open();
    $('.textareaSaved').val("");
});
dialog_decline.kendoDialog({
    width: "380px",
    title: "Decline this consultation?",
    closable: false,
    visible: false,
});

$('.button_fromClose').click(function () {
    dialog_start.data("kendoDialog").close();

});
$('.button_fromCancel').click(function () {
    dialog_decline.data("kendoDialog").close();
});
// code paging in popup
function contactpopup(id) {
    $('.info_person').css('display', 'grid');
    if (id != null) {
        $.ajax({
            type: "get",
            url: "/admin/PopUpContact?id=" + id,
            success: function (e) {
                $('.name_popup').html(e[0].FirstName);
                $('.phone_popup').html(e[0].Phone);
                $('.date_popup').html(e[0].DateNotStamp);
                $('.gmail_popup').html(e[0].Email);
                $('.time_popup').html(e[0].Time);
                $('.page_popup').html(e[0].PageRequestName);
            }
        })

    }
}
function detailpopup(id) {
    if (id != null) {

        $.ajax({
            type: "get",
            // url: "https://api.myjson.com/bins/1a4229",
            url: "/admin/ListAswer?id=" + id,
            success: function (e) {
                // add button paging
                //count auto 
                for (i in e) {
                    //console.log(e[i]);
                    if (e[i].Answer == null)
                        e[i].Answer = "";
                }
                var autocount = 0;
                var count_object = Object.keys(e).length;
                var temp_count;
                $('.ques_tion').html("");
                $('.Page2').html("");
                $(".page-button").html("");
                console.log(count_object);
                //$(".page-button").append('<input type="button" id="-1temp" value="<" style="display:inline-block;background-color:white;border:1px solid #ccc;font-size:15px" />');
                if (count_object <= 8)
                    $(".page-button").append('<input type="button" id="1temp" value="1" style="display:inline-block;background-color:white;border:1px solid #ccc;font-size:15px" />');
                else {
                    $(".page-button").append('<input type="button" id="1temp" value="1" style="display:inline-block;background-color:white;border:1px solid #ccc;font-size:15px"/>');
                    temp_count = parseInt((count_object - 8) / 10);
                    if (((count_object - 8) % 10) != 0)
                        temp_count++;
                    
                    for (var i = 1; i <= temp_count; i++) {
                        var t = i + 1;
                        
                        var display_t = "display:inline-block;";
                        //if (t != 2)
                        //    display_t = "display:none;";
                        $(".page-button").append('<input type="button" id="' + t + 'temp" value="' + t + '" style="' + display_t + 'background-color:white;border:1px solid #ccc;font-size:15px" />');
                    }
                    //$(".page-button").append('<input type="button" id="-2temp" value=">" style="display:inline-block;background-color:white;border:1px solid #ccc;font-size:15px" />');
                }
                //add paging 1
                var temp = 0;
                for (var i = 0; i <= 7; i++) {
                    
                    temp = i + 1;
                    if (e[i] != null && i <= 2) {


                        $('.ques_tion').append(' <div class="question_info"><div class="question" style="display: grid" ><div>Question ' + temp + '</div><div>' + e[i].Question + '</div></div><div class="reply">' + e[i].Answer + '</div></div>');
                    }
                    if (e[i] != null && i > 2) {

                        $('.Page2').append('<div class="question_info"><div class="question" style="display: grid" ><div>Question ' + temp + '</div><div>' + e[i].Question + '</div></div><div class="reply">' + e[i].Answer + '</div></div>');
                    }
                }
                temp_count++;
                //var prev1 = $('#-1temp');
                //var prev2 = $('#-2temp');
                //var count_number_button_page = $('#1temp');
                //var temp_button_page = $('#3temp');
                //var flag = 0;

                $('input').click(function () {
                    if ($(this).val() == 1) {
                        $('.ques_tion').html("");
                        $('.info_person').css('display', 'grid');
                        $('.Page2').html("");
                        for (var i = 0; i <= 7; i++) {
                            temp = i + 1;
                            if (e[i] != null && i <= 2) {

                                $('.ques_tion').append(' <div class="question_info"><div class="question" style="display: grid" ><div>Question ' + temp + '</div><div>' + e[i].Question + '</div></div><div class="reply">' + e[i].Answer + '</div></div>');
                            }
                            if (e[i] != null && i > 2) {

                                $('.Page2').append('<div class="question_info"><div class="question" style="display: grid" ><div>Question ' + temp + '</div><div>' + e[i].Question + '</div></div><div class="reply">' + e[i].Answer + '</div></div>');
                            }
                        }
                    }
                    if ($(this).val() != 1) {
                        $('.ques_tion').html("");
                        $('.info_person').css('display', 'none');
                        $('.Page2').html("");
                        for (var i = ($(this).val() * 10 - 3) - 10 + 1; i <= $(this).val() * 10 - 3; i++) {
                            temp = i + 1;
                            if (e[i] != null && i <= $(this).val() * 10 - 8) {
                                $('.ques_tion').append(' <div class="question_info"><div class="question" style="display: grid" ><div>Question ' + temp + '</div><div>' + e[i].Question + '</div></div><div class="reply">' + e[i].Answer + '</div></div>');
                            }
                            if (e[i] != null && i > $(this).val() * 10 - 8 && i <= $(this).val() * 10 - 2) {
                                $('.Page2').append('<div class="question_info"><div class="question" style="display: grid" ><div>Question ' + temp + '</div><div>' + e[i].Question + '</div></div><div class="reply">' + e[i].Answer + '</div></div>');
                            }
                        }
                    }
                })
            }
        })
    }
    
}