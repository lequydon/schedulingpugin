$(document).ready(function () {

    // dialog delete 
    var dialog_decline = $("#dialog_decline");
    dialog_decline.kendoDialog({
        title: false,
        width: "500",
        closable: false,
        visible: false,
    });
    $('.button_show_dialog').click(function () {
        $('#dialog_decline').css('display', 'block');
        dialog_decline.data("kendoDialog").open();
    })
    $('.button_fromCancel').click(function () {
        dialog_decline.data("kendoDialog").close();
    });
    $('.button_fromDelete').click(function () {
        //alert();
        var check = 0;
        if ($('.checktest')[0].checked == true)
            check = 1;

        $.ajax({
            type: "post",
            url: "/TimeSlots/DeleteSessionTimeSlot",
            data: { ID: IDavailableTime, check: check },
            success: function () {
                dialog_decline.data("kendoDialog").close();
                dialog_edit.data("kendoDialog").close();

                const Toast = swal.mixin({
                    toast: true,
                    position: 'top-left',
                    showConfirmButton: false,
                    timer: 3000
                })

                Toast.fire({
                    type: 'success',
                    title: 'Successfully Delete this record!'
                })
                $("#grid_available").getKendoGrid().dataSource.read();
                $("#grid_available").data('kendoGrid').refresh();
            }
        });
    })
    // dialog edit
    var dialog_edit = $("#dialog_edit");
    dialog_edit.kendoDialog({
        title: "Edit Available Time",
        closable: true,
        visible: false,
    });
    $('.button_fromCancelEdit').click(function () {
        //$('.remove-div').each(function () {
        //    if ($(this).attr('canceltest') != 1)
        //        $(this).remove();
        //})

        dialog_edit.data("kendoDialog").close();
    });
    $('.button_fromSave').click(function () {
        //alert();
        $.ajax({
            type: "post",
            url: "/TimeSlots/DeleteSessionTimeSlot",
            data: { ID: IDavailableTime, check: check },
            success: function () {
                dialog_edit.data("kendoDialog").close();
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
                $("#grid_available").getKendoGrid().dataSource.read();
                $("#grid_available").data('kendoGrid').refresh();
            }
        });
    })



    // global variable
    var IDavailableTime = null;
    var variablecheckCloseEdit = 0;
    $(document).ready(function () {

        $("#grid_available").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: "/Timeslots/GetListSesionTimeSlot"
                    }
                },
                //data: data1,
                schema: {
                    parse: function (data) {
                        var events = [];
                        for (var i = 0; i < data.length; i++) {
                            var DateStart = data[i].SessionStart;
                            var DateEnd = data[i].SessionEnd;
                            DateStart = new Date(DateStart.slice(6, 16) * 1000);
                            DateEnd = new Date(DateEnd.slice(6, 16) * 1000);
                            data[i].SessionStart = kendo.toString(DateStart, 'MM/dd/yyyy');
                            data[i].SessionEnd = kendo.toString(DateEnd, 'MM/dd/yyyy');

                        }
                        return data;
                    },
                    model: {
                        fields: {
                            ID: { type: "string" },
                            DateStart: { type: "date" },
                            DateEnd: { type: "date" },
                            TimeZoneDefault: { type: "string" },
                            DurationDefault: { type: "number" }
                        }
                    }
                },
                pageSize: 20
            },
            groupable: true,
            sortable: true,
            filterable: {
                mode: "row",
                ui: function () {
                    element.kendoDatePicker({
                        format: '{0: dd-MM-yyyy}'
                    })
                }
            },
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },
            columns: [
                {
                    hidden: true,
                    field: "ID"
                },
                {
                    field: "SessionStart",
                    title: "Date Start",
                    width: 120,
                    //format: "{0:MM/dd/yyyy}"
                    template: "#: kendo.toString(kendo.parseDate(SessionStart), 'MM/dd/yyyy')#"
                },
                {
                    field: "SessionEnd",
                    title: "Date End",
                    width: 120,
                    //format: "{0:MM/dd/yyyy}"
                    template: "#: kendo.toString(kendo.parseDate(SessionEnd), 'MM/dd/yyyy')#"
                },
                //{
                //    field: "TimeZone",
                //    title: "Time Zone Default",
                //    width: 120,
                //},
                {
                    field: "Duration",
                    title: "Duration Default",
                    width: 150,
                },
                {
                    title: "Commands",
                    command: [
                        {
                            text: "Edit", iconClass: "fas fa-pencil-alt", className: "btn-accept-grid",
                            click: function (e) {
                                $('#dialog_edit').css('display', 'block');
                                dialog_edit.data("kendoDialog").open();
                                e.preventDefault();
                                var tr = $(e.target).closest("tr");
                                var data = this.dataItem(tr);
                                IDavailableTime = data.ID;
                                var datestart = data.SessionStart;
                                var dateend = data.SessionEnd;
                                datestart = datestart.slice(6, 16);
                                datestart = new Date(datestart * 1000);
                                dateend = dateend.slice(6, 16);
                                dateend = new Date(dateend * 1000);
                                $('.startdate').text(kendo.toString(datestart, "MM/dd/yyyy"));
                                $('.enddate').text(kendo.toString(dateend, "MM/dd/yyyy"));
                                $.ajax({
                                    type: "get",
                                    url: "/TimeSlots/GetListDetail",
                                    data: { ID: IDavailableTime },
                                    success: function (da) {
                                        console.log(da);
                                        edit_dalog_page(da);
                                    }
                                });
                                return true;
                            }
                        },
                        {
                            text: "Delete", iconClass: "fa fa-close", className: "btn-decline-grid",
                            click: function (e) {
                                $('#dialog_decline').css('display', 'block');
                                dialog_decline.data("kendoDialog").open();
                                e.preventDefault();
                                var tr = $(e.target).closest("tr");
                                var data = this.dataItem(tr);
                                IDavailableTime = data.ID;
                                var datestart = data.SessionStart;
                                var dateend = data.SessionEnd;
                                datestart = datestart.slice(6, 16);
                                datestart = new Date(datestart * 1000);
                                dateend = dateend.slice(6, 16);
                                dateend = new Date(dateend * 1000);
                                $('.startdate').text(kendo.toString(datestart, "MM/dd/yyyy"));
                                $('.enddate').text(kendo.toString(dateend, "MM/dd/yyyy"));
                            }
                        },
                        {
                            text: "Details", iconClass: "fa fa-info-circle", className: "btn-details-grid2",
                            click: function (e) {
                                $('#dialog_detail').css('display', 'block');
                                $("input").css("max-width", "29em");
                                //$(".k-switch-container").css('border', '3px solid #d6d6d6');
                                dialog_detail.data("kendoDialog").open();
                                e.preventDefault();
                                var timeform = "time-from-detail" + parseInt(sessionStorage.getItem("timeform"));
                                var timeto = "time-to-detail" + parseInt(sessionStorage.getItem("timeto"));
                                var duration = "duration-dayofweek-detail" + parseInt(sessionStorage.getItem("duration"));
                                var tr = $(e.target).closest("tr");
                                var data = this.dataItem(tr);
                                IDavailableTime = data.ID;
                                idPage = IDavailableTime;
                                $.ajax({
                                    type: "Post",
                                    url: "/TimeSlots/GetListDetail",
                                    data: { ID: IDavailableTime }
                                }).done(function (da) {
                                    console.log(da);
                                    detail_dialog_page(da);
                                });
                                return true;
                            }
                        }
                    ],
                    width: 310,
                }]
        })
    });
    //kendo switch globle variable

    function getTimeZone(tz) {
        $.getJSON("https://jsonstorage.net/api/items/9a792bfc-1fa3-425e-a6db-d7df883f0de0", function (obj) {
            var utc = null;
            for (var i in obj) {
                if (tz == obj[i].standardname) {
                    utc = obj[i].UTC
                }
            }
            $("#timeZone-detail").val(utc);
        });
    }

    function getTimeZoneEdit(tz) {
        $.getJSON("https://jsonstorage.net/api/items/9a792bfc-1fa3-425e-a6db-d7df883f0de0", function (obj) {
            var utc = null;
            for (var i in obj) {
                if (tz == obj[i].standardname) {
                    utc = obj[i].UTC
                }
            }
            $("#timeZone").val(utc);
        });
    }
    var switchInstance0;
    var switchInstance1;
    var switchInstance2;
    var switchInstance3;
    var switchInstance4;
    var switchInstance5;
    var switchInstance6;
    //create kendo switch
    switchInstance0 = $("#notifications-switch0").kendoSwitch({
        checked: false,
        change: function (e) {
            var abc = ((e.checked ? "checked" : "unchecked"));
            if (abc == "checked") {
                $("#on0").show();
                $("#off0").hide();
                $("#edit-time0").hide();
                $("#edit-button0").show();
                $("#edit-button0").removeAttr("disabled", "disabled");
            }
            else {
                $("#off0").show();
                $("#on0").hide();
                $("#edit-time0").hide();
                $("#edit-button0").show();
                $("#edit-button0").attr("disabled", "disabled");
            }
        }
    }).data("kendoSwitch");
    switchInstance1 = $("#notifications-switch1").kendoSwitch({
        checked: false,
        change: function (e) {
            var abc = ((e.checked ? "checked" : "unchecked"));
            if (abc == "checked") {
                $("#on1").show();
                $("#off1").hide();
                $("#edit-time1").hide();
                $("#edit-button1").show();
                $("#edit-button1").removeAttr("disabled", "disabled");
            }
            else {
                $("#off1").show();
                $("#on1").hide();
                $("#edit-time1").hide();
                $("#edit-button1").show();
                $("#edit-button1").attr("disabled", "disabled");
            }
        }
    }).data("kendoSwitch");
    switchInstance2 = $("#notifications-switch2").kendoSwitch({
        checked: false,
        change: function (e) {
            var abc = ((e.checked ? "checked" : "unchecked"));
            if (abc == "checked") {
                $("#on2").show();
                $("#off2").hide();
                $("#edit-time2").hide();
                $("#edit-button2").show();
                $("#edit-button2").removeAttr("disabled", "disabled");
            }
            else {
                $("#off2").show();
                $("#on2").hide();
                $("#edit-time2").hide();
                $("#edit-button2").show();
                $("#edit-button2").attr("disabled", "disabled");
            }
        }
    }).data("kendoSwitch");
    switchInstance3 = $("#notifications-switch3").kendoSwitch({
        checked: false,
        change: function (e) {
            var abc = ((e.checked ? "checked" : "unchecked"));
            if (abc == "checked") {
                $("#on3").show();
                $("#off3").hide();
                $("#edit-time3").hide();
                $("#edit-button3").show();
                $("#edit-button3").removeAttr("disabled", "disabled");
            }
            else {
                $("#off3").show();
                $("#on3").hide();
                $("#edit-time3").hide();
                $("#edit-button3").show();
                $("#edit-button3").attr("disabled", "disabled");
            }
        }
    }).data("kendoSwitch");
    switchInstance4 = $("#notifications-switch4").kendoSwitch({
        checked: false,
        change: function (e) {
            var abc = ((e.checked ? "checked" : "unchecked"));
            if (abc == "checked") {
                $("#on4").show();
                $("#off4").hide();
                $("#edit-time4").hide();
                $("#edit-button4").show();
                $("#edit-button4").removeAttr("disabled", "disabled");
            }
            else {
                $("#off4").show();
                $("#on4").hide();
                $("#edit-time4").hide();
                $("#edit-button4").show();
                $("#edit-button4").attr("disabled", "disabled");
            }
        }
    }).data("kendoSwitch");
    switchInstance5 = $("#notifications-switch5").kendoSwitch({
        checked: false,
        change: function (e) {
            var abc = ((e.checked ? "checked" : "unchecked"));
            if (abc == "checked") {
                $("#on5").show();
                $("#off5").hide();
                $("#edit-time5").hide();
                $("#edit-button5").show();
                $("#edit-button5").removeAttr("disabled", "disabled");
            }
            else {
                $("#off5").show();
                $("#on5").hide();
                $("#edit-time5").hide();
                $("#edit-button5").show();
                $("#edit-button5").attr("disabled", "disabled");
            }
        }
    }).data("kendoSwitch");
    switchInstance6 = $("#notifications-switch6").kendoSwitch({
        checked: false,
        change: function (e) {
            var abc = ((e.checked ? "checked" : "unchecked"));
            if (abc == "checked") {
                $("#on6").show();
                $("#off6").hide();
                $("#edit-time6").hide();
                $("#edit-button6").show();
                $("#edit-button6").removeAttr("disabled", "disabled");
            }
            else {
                $("#off6").show();
                $("#on6").hide();
                $("#edit-time6").hide();
                $("#edit-button").show();
                $("#edit-button6").attr("disabled", "disabled");
            }
        }
    }).data("kendoSwitch");
    //show dialog edit page

    function edit_dalog_page(datadetail) {//
        // event changer input type number
        $('#duration-main').change(function () {
            $('.duration-dayofweek').val($('#duration-main').val());
        })
        $('.box-edit').hide();
        $('.status').show();
        $('.k-edit-button').show();
        for (var i = 0; i < 7; i++) {
            $("#on" + i).hide();
            $("#edit-time" + i).hide();
            $("#edit-button" + i).attr("disabled", "disabled");
            $("#edit-button" + i).show();

        }
        //kendo switch first create not checked
        switchInstance0.check(false);
        switchInstance1.check(false);
        switchInstance2.check(false);
        switchInstance3.check(false);
        switchInstance4.check(false);
        switchInstance5.check(false);
        switchInstance6.check(false);
        getTimeZoneEdit(datadetail[0].TimeZone);

        // create DatePicker from input HTML element

        //timespan to date
        datadetail[0].SessionStart = datadetail[0].SessionStart.slice(6, 16);
        datadetail[0].SessionStart = new Date(datadetail[0].SessionStart * 1000);
        datadetail[0].SessionEnd = datadetail[0].SessionEnd.slice(6, 16);
        datadetail[0].SessionEnd = new Date(datadetail[0].SessionEnd * 1000);
        $("#datepicker").val(kendo.toString(datadetail[0].SessionStart, "MM/dd/yyyy"));


        //var dateFrom = $("#datepicker").data("kendoDatePicker").value();
        $("#datepickerr").val(kendo.toString(datadetail[0].SessionEnd, "MM/dd/yyyy"));
        // set value duration section
        $('#duration-main').val(datadetail[0].SessionDuration);
        //set time day of week in sectiontime
        var countadd = 0;
        //Array store create
        ArrayStore = [{ Duration: datadetail[0].SessionDuration, DateStart: "09:00 AM", DateEnd: "11:00 AM" }, { Duration: datadetail[0].SessionDuration, DateStart: "02:00 PM", DateEnd: "05:00 PM" }];
        //sessionstorage data save
        var StorageSaveTime = [{ DayOfWeek: 0, TimeSlotOfDay: [] }, { DayOfWeek: 1, TimeSlotOfDay: [] }, { DayOfWeek: 2, TimeSlotOfDay: [] }, { DayOfWeek: 3, TimeSlotOfDay: [] }, { DayOfWeek: 4, TimeSlotOfDay: [] }, { DayOfWeek: 5, TimeSlotOfDay: [] }, { DayOfWeek: 6, TimeSlotOfDay: [] }];
        for (i in datadetail[0].DaysOfWeek) {
            //console.log(datadetail[0].DaysOfWeek[i].TimeSlotOfDay);
            if (datadetail[0].DaysOfWeek[i].DayOfWeek != 0) {
                $('#on' + (datadetail[0].DaysOfWeek[i].DayOfWeek - 1) + '').html("");
                $("#info-add" + (datadetail[0].DaysOfWeek[i].DayOfWeek - 1) + "").html("");
                for (y in datadetail[0].DaysOfWeek[i].TimeSlotOfDay) {

                    datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart = datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart.slice(6, 16);
                    datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart = new Date(datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart * 1000);
                    datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd = datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd.slice(6, 16);
                    datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd = new Date(datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd * 1000);
                    console.log(datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart);
                    var timestart = kendo.toString(datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart, "hh:mm tt");
                    var timeend = kendo.toString(datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd, "hh:mm tt");
                    var durationthis = datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].Duration;
                    //console.log('#on'+ (datadetail[0].DaysOfWeek[i].DayOfWeek - 1)+'');
                    //add to sessionstorage data save
                    InfoTimeSlosts = {
                        Duration: durationthis,
                        DateEnd: timeend,
                        DateStart: timestart
                    }
                    StorageSaveTime.find(findname => findname.DayOfWeek === datadetail[0].DaysOfWeek[i].DayOfWeek).TimeSlotOfDay.push(InfoTimeSlosts);

                    $('#on' + (datadetail[0].DaysOfWeek[i].DayOfWeek - 1) + '').append('<label class="timeshowlable" countadd=' + countadd + '>' + timestart + '-' + timeend + '</label><br />');
                    $('#on' + (datadetail[0].DaysOfWeek[i].DayOfWeek - 1) + '').show();
                    $("#off" + (datadetail[0].DaysOfWeek[i].DayOfWeek - 1) + "").hide();
                    $("#edit-button" + (datadetail[0].DaysOfWeek[i].DayOfWeek - 1) + "").removeAttr("disabled", "disabled");
                    //$("#notifications-switch" + (datadetail[0].DaysOfWeek[i].DayOfWeek - 1) + "").kendoSwitch(

                    $("#info-add" + (datadetail[0].DaysOfWeek[i].DayOfWeek - 1) + "").append("<div class='remove-div' testshow='-1' countadd=" + countadd + " style='display:inline-flex'><label class='label-time'>From</label> <input class='fromthis'  value='" + timestart + "' data-format='hh:mm tt' /><label class='label-time'>To</label> <input class='tothis'  value='" + timeend + "'  data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input id='duration-dayofweek' class='duration-dayofweek' value=" + durationthis + " step=5 min=5 max=120 type='number' /><label class='label-time'>minutes</label><i class='k-icon k-i-minus-outline' ></i></div>");
                    countadd++;
                    $('.k-i-minus-outline').click(function () {

                        var thistag = $(this).closest('.remove-div');
                        var daySlostTag = $(this).closest('.info-date').find('.remove-div');
                        $(this).closest('.info-date').find('.remove-div').each(function () {
                            var test = new Date($(this).find('input.tothis').data('kendoTimePicker').value());
                            if (thistag == $(this))
                                console.log("ok");
                        }) 
                        //var parent = $(this).parent('.remove-div');
                        $(this).closest('.box-edit').attr('id');
                        $(this).parent('.remove-div').remove();
                        //$(daySlostTag).each(function (day) {
                        //    console.log("ok");
                        //    //var test = new Date($(day).find('input.tothis').data('kendoTimePicker').value());
                        //    //console.log(test.getHours());
                        //})
                    });
                }
                if (datadetail[0].DaysOfWeek[i].DayOfWeek - 1 == 0)
                    switchInstance0.check(true);
                if (datadetail[0].DaysOfWeek[i].DayOfWeek - 1 == 1)
                    switchInstance1.check(true);
                if (datadetail[0].DaysOfWeek[i].DayOfWeek - 1 == 2)
                    switchInstance2.check(true);
                if (datadetail[0].DaysOfWeek[i].DayOfWeek - 1 == 3)
                    switchInstance3.check(true);
                if (datadetail[0].DaysOfWeek[i].DayOfWeek - 1 == 4)
                    switchInstance4.check(true);
                if (datadetail[0].DaysOfWeek[i].DayOfWeek - 1 == 5)
                    switchInstance5.check(true);
            }
            if (datadetail[0].DaysOfWeek[i].DayOfWeek == 0) {
                $("#info-add6").html("");
                $('#on6').html("");
                for (y in datadetail[0].DaysOfWeek[i].TimeSlotOfDay) {
                    datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart = datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart.slice(6, 16);
                    datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart = new Date(datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart * 1000);
                    datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd = datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd.slice(6, 16);
                    datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd = new Date(datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd * 1000);
                    var timestart = kendo.toString(datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart, "hh:mm tt");
                    var timeend = kendo.toString(datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd, "hh:mm tt");
                    var durationthis = datadetail[0].DaysOfWeek[i].TimeSlotOfDay[y].Duration;
                    InfoTimeSlosts = {
                        Duration: durationthis,
                        DateEnd: timeend,
                        DateStart: timestart
                    }
                    StorageSaveTime.find(findname => findname.DayOfWeek === datadetail[0].DaysOfWeek[i].DayOfWeek).TimeSlotOfDay.push(InfoTimeSlosts);
                    $('#on6').append('<label class="timeshowlable" countadd=' + countadd + '>' + timestart + '-' + timeend + '</label><br />');
                    $('#on6').show();
                    $("#off6").hide();
                    $("#edit-button6").removeAttr("disabled", "disabled");
                    $("#info-add6").append("<div class='remove-div' style='display:inline-flex' testshow='-1' countadd=" + countadd + "><label class='label-time'>From</label> <input class='fromthis'  value='" + timestart + "' value='" + timestart + "' data-format='hh:mm tt' /><label class='label-time'>To</label> <input class='tothis'  value='" + timeend + "'  data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input id='duration-dayofweek' class='duration-dayofweek' value=" + durationthis + " step=5 min=5 max=120 type='number' /><label class='label-time'>minutes</label><i class='k-icon k-i-minus-outline' ></i></div>");
                    countadd++;
                }
                switchInstance6.check(true);
            }
        }
        // add time slosts to time slosts free
        for (i in StorageSaveTime) {
            if (StorageSaveTime[i].TimeSlotOfDay.length == 0) {
                StorageSaveTime[i].TimeSlotOfDay.push(ArrayStore[0]);
                StorageSaveTime[i].TimeSlotOfDay.push(ArrayStore[1]);
                if (StorageSaveTime[i].DayOfWeek == 0) {
                    $('#info-add6').html("");
                    $('#on6').html("");
                    for (y in StorageSaveTime[i].TimeSlotOfDay) {
                        $('#info-add6').append("<div class='remove-div' style='display: inline-flex'><label class='label - time'>From</label> <input class='fromthis'  value='" + StorageSaveTime[i].TimeSlotOfDay[y].DateStart + "' data-format='hh: mm tt' /><label class='label-time'>To</label> <input class='tothis'  value='" + StorageSaveTime[i].TimeSlotOfDay[y].DateEnd + "'  data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input id='duration-dayofweek' class='duration-dayofweek' value=" + StorageSaveTime[i].TimeSlotOfDay[y].Duration + " step=5 min=5 max=120 type='number' /><label class='label-time'>minutes</label><i class='k-icon k-i-minus-outline' ></i></div>");
                        $('#on6').append('<label class="timeshowlable" >' + StorageSaveTime[i].TimeSlotOfDay[y].DateStart + '-' + StorageSaveTime[i].TimeSlotOfDay[y].DateEnd + '</label><br />');
                    }
                }
                if (StorageSaveTime[i].DayOfWeek != 0) {
                    $('#info-add' + (StorageSaveTime[i].DayOfWeek - 1)).html("");
                    $('#on' + (StorageSaveTime[i].DayOfWeek - 1)).html("");
                    for (y in StorageSaveTime[i].TimeSlotOfDay) {

                        $('#info-add' + (StorageSaveTime[i].DayOfWeek - 1)).append("<div class='remove-div' style='display:inline-flex'><label class='label-time'>From</label> <input class='fromthis'  value='" + StorageSaveTime[i].TimeSlotOfDay[y].DateStart + "' data-format='hh:mm tt' /><label class='label-time'>To</label> <input class='tothis' value='" + StorageSaveTime[i].TimeSlotOfDay[y].DateEnd + "'   data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input id='duration-dayofweek' class='duration-dayofweek' value=" + StorageSaveTime[i].TimeSlotOfDay[y].Duration + " step=5 min=5 max=120 type='number' /><label class='label-time'>minutes</label><i class='k-icon k-i-minus-outline' ></i></div>");
                        $('#on' + (StorageSaveTime[i].DayOfWeek - 1)).append('<label class="timeshowlable" >' + StorageSaveTime[i].TimeSlotOfDay[y].DateStart + '-' + StorageSaveTime[i].TimeSlotOfDay[y].DateEnd + '</label><br />');
                    }
                }
            }

        }
        console.log(StorageSaveTime);
        $('.k-i-minus-outline').click(function () {
            // $(this).parent('.remove-div').remove();
            //var TagParentButon = $(this).closest('.info-date').first();
            var TagParentButon = $(this).closest('.info-date').attr('id');
            $(this).parent('.remove-div').remove();
            //show or hide button decline in line timeslost
            var CountLine = 0;
            $('#' + TagParentButon + ' .remove-div').each(function () {
                CountLine++;
            })
            if (CountLine == 1)
                $('#' + TagParentButon + ' .k-i-minus-outline').css('display', 'none');
            else
                $('#' + TagParentButon + ' .k-i-minus-outline').css('display', 'block');
        });
        //sessionstorage.clear();
        $('.fromthis').kendoTimePicker({
            format: "hh:mm tt",
            dateInput: true
        });
        $('.tothis').kendoTimePicker({
            format: "hh:mm tt",
            dateInput: true
        });
        //var flag = 0;
        // turn on dislay inline-flex for time slost in day
        $('.remove-div').css('display', 'inline-flex');
        $(".k-edit-button").click(function (e) {
            variablecheckCloseEdit++;
            if (variablecheckCloseEdit != 0) {
                console.log(variablecheckCloseEdit);
                $("#save-button").css("background-color", "grey");
                $("#save-button").attr("disabled", true);
            }
            $('.duration-dayofweek').keydown(function (e) {
                return false;
            })
            $("input.fromthis").attr('readonly', true);
            $("input.tothis").attr('readonly', true);
            var CountLine = 0;
            e.preventDefault();
            var on = "on" + parseInt(sessionStorage.getItem("id"));
            var off = "off" + parseInt(sessionStorage.getItem("id"));
            var edit = "edit-time" + parseInt(sessionStorage.getItem("id"));
            var editbutton = "edit-button" + parseInt(sessionStorage.getItem("id"));
            var info = "info-add" + parseInt(sessionStorage.getItem("id"));
            // show and hide add slost button 
            var flagcheckslost = 0;
            $('#' + info + ' .remove-div input.tothis').each(function () {
                var endslost = $(this).data("kendoTimePicker").value();
                if (endslost.getHours() == 21) {
                    $('.k-plus-button').hide();
                    flagcheckslost = 1;
                }
                if (flagcheckslost == 0)
                    $('.k-plus-button').show();
            })
            //var endslost = $('#' + info + ' .remove-div:last input.tothis').data("kendoTimePicker").value();
            //if (endslost.getHours() == 21)
            //    $('.k-plus-button').hide();
            //else
            //    $('.k-plus-button').show();
            $("#" + on).hide();
            $("#" + off).hide();
            $("#" + edit).show();
            $("#" + editbutton).hide();
            //show or hide button decline in line time slosts
            $('#' + info + ' .remove-div').each(function () {
                CountLine++;
            })
            if (CountLine == 1)
                $('#' + info + ' .k-i-minus-outline').css('display', 'none');
            else
                $('#' + info + ' .k-i-minus-outline').css('display', 'block');
            
        });
        $(".k-cancel-button").click(function (e) {
            e.preventDefault();
            variablecheckCloseEdit--;
            if (variablecheckCloseEdit == 0) {
                console.log(variablecheckCloseEdit);
                $("#save-button").css("background-color", "#4bc34b");
                $("#save-button").attr("disabled", false);
            }
            testshow = 0;
            var on = "on" + parseInt(sessionStorage.getItem("cancel"));
            var edit = "edit-time" + parseInt(sessionStorage.getItem("cancel"));
            var editbutton = "edit-button" + parseInt(sessionStorage.getItem("cancel"));
            var info = "info-add" + parseInt(sessionStorage.getItem("cancel"));
            $("#" + info).html("");
            $("#" + on).html("");

            var Arraystore = [];
            var countdatepicker = 0;
            //if (parseInt(sessionStorage.getItem("cancel")) == 6)
            if (parseInt(sessionStorage.getItem("cancel")) == 6)
                Arraystore = StorageSaveTime.find(findname => findname.DayOfWeek === 0).TimeSlotOfDay;
            else
                Arraystore = StorageSaveTime.find(findname => findname.DayOfWeek === parseInt(sessionStorage.getItem("cancel")) + 1).TimeSlotOfDay;
            for (item in Arraystore) {
                $("#" + info).append("<div class='remove-div picker" + countdatepicker + "'  style='display:inline-flex'><label class='label-time'>From</label> <input class='fromthis' value='" + Arraystore[item].DateStart + "' data-format='hh:mm tt' /><label class='label-time'>To</label> <input class='tothis' value='" + Arraystore[item].DateEnd + "'  data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input class='duration-dayofweek' value=" + Arraystore[item].Duration + " step=5 min=5 max=120 type='number' /><label class='label-time'>minutes</label><i class='k-icon k-i-minus-outline' style=' width: 1.125em; height: 1.125em; margin-left: 1em;'></i></div>")
                $("#" + on).append('<label class="timeshowlable">' + Arraystore[item].DateStart + '-' + Arraystore[item].DateEnd + '</label><br />');
                $('#' + info + ' .picker' + countdatepicker + ' .fromthis').kendoTimePicker({
                    format: "hh:mm tt",
                    dateInput: true
                });
                $('#' + info + ' .picker' + countdatepicker + ' .tothis').kendoTimePicker({
                    format: "hh:mm tt",
                    dateInput: true
                });
                countdatepicker++;
            }
            $('.k-i-minus-outline').click(function () {
                var TagParentButon = $(this).closest('.info-date').attr('id');
                $(this).parent('.remove-div').remove();
                //show or hide button decline in line timeslost
                var CountLine = 0;
                $('#' + TagParentButon + ' .remove-div').each(function () {
                    CountLine++;
                })
                if (CountLine == 1)
                    $('#' + TagParentButon + ' .k-i-minus-outline').css('display', 'none');
                else
                    $('#' + TagParentButon + ' .k-i-minus-outline').css('display', 'block');

            });

            $("#" + on).show();
            $("#" + edit).hide();
            $("#" + editbutton).show();
            sessionStorage.clear();
        });
        function escapeHtmlEntities(str) {
            if (typeof jQuery !== 'undefined') {
                // Create an empty div to use as a container,
                // then put the raw text in and get the HTML
                // equivalent out.
                return jQuery('<div/>').text(str).html();
            }

            // No jQuery, so use string replace.
            return str
                .replace(/&/g, '&amp;')
                .replace(/>/g, '&gt;')
                .replace(/</g, '&lt;')
                .replace(/"/g, '&quot;')
                .replace(/'/g, '&apos;');
        }
        var timeFrom1 = "9:00 AM";
        var timeFrom2 = "2:00 PM";
        var timeTo1 = "11:00 AM";
        var timeTo2 = "5:00 PM";
        $(".k-save-button").click(function () {

            var SessionStart = new Date($("#datepicker").val());//escapeHtmlEntities($("#datepicker").val());
            var SessionEnd = new Date($("#datepickerr").val());
            SessionStart = SessionStart.toJSON();
            SessionEnd = SessionEnd.toJSON();
            var SessionDuration = parseInt($("#duration-main").val());//escapeHtmlEntities($("#duration-main").val());
            var TimeZone = escapeHtmlEntities($("#timeZone").val());
            var DaysOfWeek = new Array();
            var id = 0;
            var i = 1;
            $(".view").each(function () {
                if (id != 7)
                    if ($("#notifications-switch" + id)[0].checked == true) {
                        var dayOfWeek = $(this).children('#name').text();
                        var TimeSlotOfDay = new Array();
                        $('#info-add' + id + ' .remove-div').each(function () {
                            var Timeslotofday_object = {
                                Duration: $(this).find('.duration-dayofweek').val(),
                                DateStart: $(this).find('.fromthis')[1].value,
                                DateEnd: $(this).find('.tothis')[1].value
                            }
                            TimeSlotOfDay.push(Timeslotofday_object);
                        })
                        var day_name;
                        switch (dayOfWeek) {
                            case "Sunday":
                                day_name = 0;
                                break;
                            case "Monday":
                                day_name = 1;
                                break;
                            case "Tuesday":
                                day_name = 2;
                                break;
                            case "Wednesday":
                                day_name = 3;
                                break;
                            case "Thursday":
                                day_name = 4;
                                break;
                            case "Friday":
                                day_name = 5;
                                break;
                            case "Saturday":
                                day_name = 6;
                        }

                        //var DayOfWeek = new Array();
                        var DayOfWeek = {
                            DayOfWeek: day_name,
                            TimeSlotOfDay: TimeSlotOfDay
                        };

                        DaysOfWeek.push(DayOfWeek);
                    }
                i++;
                id++;
            });

            var sesioncreate = { SessionStart, SessionEnd, SessionDuration, TimeZone, DaysOfWeek };
            console.log(sesioncreate);
            $.ajax({
                type: "POST",
                url: "/TimeSlots/UpdateSessionTimeSlot",
                data: { sessionTimeSlot: sesioncreate, ID_session: IDavailableTime, check:1 },
                success: function (e) {
                    console.log(e);
                    if (e == 1) {
                        dialog_edit.data("kendoDialog").close();
                        const Toast = swal.mixin({
                            toast: true,
                            position: 'top-left',
                            showConfirmButton: false,
                            timer: 3000
                        })

                        Toast.fire({
                            type: 'success',
                            title: 'Successfully !'
                        })
                    }
                    else {
                        dialog_edit.data("kendoDialog").close();
                        const Toast = swal.mixin({
                            toast: true,
                            position: 'top-left',
                            showConfirmButton: false,
                            timer: 3000
                        })

                        Toast.fire({
                            type: 'error',
                            title: 'error !'
                        })
                    }


                }
            })
        })
        $(".k-update-button").click(function (e) {
            e.preventDefault();
            variablecheckCloseEdit --;
            if (variablecheckCloseEdit == 0) {
                console.log(variablecheckCloseEdit);
                $("#save-button").css("background-color", "#4bc34b");
                $("#save-button").attr("disabled", false);
            }
            var on = "on" + parseInt(sessionStorage.getItem("update"));
            var edit = "edit-time" + parseInt(sessionStorage.getItem("update"));
            var editbutton = "edit-button" + parseInt(sessionStorage.getItem("update"));
            var infoadd = "info-add" + parseInt(sessionStorage.getItem("update"));
            var ArrayAllTimeSlosts = [];
            $('#' + infoadd).find('.remove-div').each(function () {
                var flag = 0;
                var thisremovediv = $(this);
                console.log(thisremovediv.find('.tothis')[1]);
                ObjEachTimeslost = {
                    //neu loi sua o day
                    DateEnd: thisremovediv.find('input.tothis')[0].value,
                    DateStart: thisremovediv.find('input.fromthis')[0].value,
                    Duration: thisremovediv.find('input.duration-dayofweek').val()
                }
                ArrayAllTimeSlosts.push(ObjEachTimeslost);
            })
            if (parseInt(sessionStorage.getItem("update")) == 6)
                StorageSaveTime.find(findname => findname.DayOfWeek === 0).TimeSlotOfDay = ArrayAllTimeSlosts;
            else
                StorageSaveTime.find(findname => findname.DayOfWeek === parseInt(sessionStorage.getItem("update")) + 1).TimeSlotOfDay = ArrayAllTimeSlosts;
            //append date to time slost
            $("#" + infoadd).html("");
            $("#" + on).html("");
            var Arraystore = [];
            var countdatepicker = 0;
            if (parseInt(sessionStorage.getItem("update")) == 6)
                Arraystore = StorageSaveTime.find(findname => findname.DayOfWeek === 0).TimeSlotOfDay;
            else
                Arraystore = StorageSaveTime.find(findname => findname.DayOfWeek === parseInt(sessionStorage.getItem("update")) + 1).TimeSlotOfDay;
            for (item in Arraystore) {
                $("#" + infoadd).append("<div class='remove-div picker" + countdatepicker + "'  style='display:inline-flex'><label class='label-time'>From</label> <input class='fromthis' value='" + Arraystore[item].DateStart + "' data-format='hh:mm tt' /><label class='label-time'>To</label> <input class='tothis' value='" + Arraystore[item].DateEnd + "'  data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input id='duration-dayofweek' class='duration-dayofweek' value=" + Arraystore[item].Duration + " step=5 min=5 max=120 type='number' /><label class='label-time'>minutes</label><i class='k-icon k-i-minus-outline' ></i></div>")
                $("#" + on).append('<label class="timeshowlable">' + Arraystore[item].DateStart + '-' + Arraystore[item].DateEnd + '</label><br />');
                $('#' + infoadd + ' .picker' + countdatepicker + ' .fromthis').kendoTimePicker({
                    format: "hh:mm tt",
                    dateInput: true
                });
                $('#' + infoadd + ' .picker' + countdatepicker + ' .tothis').kendoTimePicker({
                    format: "hh:mm tt",
                    dateInput: true
                });
                countdatepicker++;
            }
            $('.k-i-minus-outline').click(function () {
                var TagParentButon = $(this).closest('.info-date').attr('id');
                $(this).parent('.remove-div').remove();
                //show or hide button decline in line timeslost
                var CountLine = 0;
                $('#' + TagParentButon + ' .remove-div').each(function () {
                    CountLine++;
                })
                if (CountLine == 1)
                    $('#' + TagParentButon + ' .k-i-minus-outline').css('display', 'none');
                else
                    $('#' + TagParentButon + ' .k-i-minus-outline').css('display', 'block');

            });
            $("#" + on).show();
            $("#" + edit).hide();
            $("#" + editbutton).show();
            sessionStorage.clear();
        })
        var i = 1;
        var testshow = 0;
        // create relationship between timeslosts
        function createRelationship(idday) {
            var flagConsiderFirstSlost = 0;
            $('#info-add' + idday).find('.remove-div').each(function () {
                if (flagConsiderFirstSlost == 0) {
                    $(this).find('input.tothis').data('kendoTimePicker').min(new Date("2011-04-11T08:00:00"));
                    $(this).find('input.tothis').data('kendoTimePicker').max(new Date("2011-04-11T21:00:00"));
                    $(this).find('input.fromthis').data('kendoTimePicker').min(new Date("2011-04-11T08:00:00"));
                    $(this).find('input.fromthis').data('kendoTimePicker').max(new Date("2011-04-11T20:45:00"));
                    flagConsiderFirstSlost = 1;
                }

            })
        }
        $(".k-plus-button").click(function () {
            var add = "info-add" + parseInt(sessionStorage.getItem("add"));
            var timefrom = "time-from" + parseInt(sessionStorage.getItem("add"));
            var timeto = "time-to" + parseInt(sessionStorage.getItem("add"));
            $("#" + add).append("<div class='remove-div' style='display:inline-flex' newadd='1' testshow='" + testshow + "'><label class='label-time'>From</label> <input class='fromthis' id=" + timefrom + i + " data-format='hh:mm tt' /><label class='label-time'>To</label> <input class='tothis' id=" + timeto + i + " data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input class='duration-dayofweek' value=" + $("#duration-main").val() + " step=5 min=5 max=120 type='number' /><label class='label-time'>minutes</label><i class='k-icon k-i-minus-outline' ></i></div>");
            testshow++;

            $('.k-plus-button').hide();
            $('.k-i-minus-outline').click(function () {
                var TagParentButon = $(this).closest('.info-date').attr('id');
                $(this).parent('.remove-div').remove();

                //show or hide button decline in line timeslost
                var CountLine = 0;
                $('#' + TagParentButon + ' .remove-div').each(function () {
                    CountLine++;
                })
                if (CountLine == 1)
                    $('#' + TagParentButon + ' .k-i-minus-outline').css('display', 'none');
                else
                    $('#' + TagParentButon + ' .k-i-minus-outline').css('display', 'block');
            });
            //set value next time add
            var timePrevSlost = new Date($("#" + timefrom + i).closest('.remove-div').prev().find('input.tothis').data('kendoTimePicker').value());
            var minutePrevSlost = timePrevSlost.getMinutes();
            timePrevSlost.setMinutes(minutePrevSlost + 15);
            $("#" + timefrom + i).kendoTimePicker({
                format: "hh:mm tt",
                value: timePrevSlost,
                interval: 15
            }).data("kendoTimePicker");
            
            $("#" + timeto + i).kendoTimePicker({
                format: "hh:mm tt",
                value: "9:00 PM",
                interval: 15
            }).data("kendoTimePicker");
            //createRelationship(sessionStorage.getItem("add"));
            //show and hide button add a time slost
            var endslost = $('#' + add + ' .remove-div:last input.tothis').data("kendoTimePicker").value();
            if (endslost.getHours() == 21)
                sessionStorage.clear();
            i++;
            //show and hide add a timeslost
            var endslost = $('#' + add + ' .remove-div:last input.tothis').data("kendoTimePicker").value();
            if (endslost.getHours() == 21)
                $('.k-plus-button').hide();
            // show or hide button decline

            var CountLine = 0;
            $('#' + add + ' .remove-div').each(function () {
                CountLine++;
            })
            if (CountLine == 1)
                $('#' + add + ' .k-i-minus-outline').css('display', 'none');
            else
                $('#' + add + ' .k-i-minus-outline').css('display', 'block');
            $('.duration-dayofweek').keydown(function (e) {
                return false;
            })
            $("input.fromthis").attr('readonly', true);
            $("input.tothis").attr('readonly', true);
        });
        
    }

    var switch_detail0 = $("#notifications-switch-detail0").kendoSwitch().data("kendoSwitch");
    var switch_detail1 = $("#notifications-switch-detail1").kendoSwitch().data("kendoSwitch");
    var switch_detail2 = $("#notifications-switch-detail2").kendoSwitch().data("kendoSwitch");
    var switch_detail3 = $("#notifications-switch-detail3").kendoSwitch().data("kendoSwitch");
    var switch_detail4 = $("#notifications-switch-detail4").kendoSwitch().data("kendoSwitch");
    var switch_detail5 = $("#notifications-switch-detail5").kendoSwitch().data("kendoSwitch");
    var switch_detail6 = $("#notifications-switch-detail6").kendoSwitch().data("kendoSwitch");

    function detail_dialog_page(detail) {
        $('.box-detail').hide();
        $('.busyday').show();
        $('.k-view-button-detail').show();
        for (var i = 0; i < 7; i++) {
            $("#on-detail" + i).hide();
            $("#edit-time-detail" + i).hide();
            $("#edit-button-detail" + i).attr("disabled", "disabled");
            $("#edit-button-detail" + i).show();
            $("#view-button-detail" + i).hide();
        }

        switch_detail0.check(false);
        switch_detail1.check(false);
        switch_detail2.check(false);
        switch_detail3.check(false);
        switch_detail4.check(false);
        switch_detail5.check(false);
        switch_detail6.check(false);

        getTimeZone(detail[0].TimeZone);

        detail[0].SessionStart = detail[0].SessionStart.slice(6, 16);
        detail[0].SessionStart = new Date(detail[0].SessionStart * 1000);
        detail[0].SessionEnd = detail[0].SessionEnd.slice(6, 16);
        detail[0].SessionEnd = new Date(detail[0].SessionEnd * 1000);
        $("#dateStart-detail").val(kendo.toString(detail[0].SessionStart, "MM/dd/yyyy"));

        $("#dateEnd-detail").val(kendo.toString(detail[0].SessionEnd, "MM/dd/yyyy"));

        $('#duration-main-detail').val(detail[0].SessionDuration);
        for (i in detail[0].DaysOfWeek) {
            if (detail[0].DaysOfWeek[i].DayOfWeek != 0) {
                $('#on-detail' + (detail[0].DaysOfWeek[i].DayOfWeek - 1) + '').html("");
                $("#info-add-detail" + (detail[0].DaysOfWeek[i].DayOfWeek - 1) + "").html("");
                for (y in detail[0].DaysOfWeek[i].TimeSlotOfDay) {
                    detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart = detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart.slice(6, 16);
                    detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart = new Date(detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart * 1000);
                    detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd = detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd.slice(6, 16);
                    detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd = new Date(detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd * 1000);

                    var timestart = kendo.toString(detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart, "hh:mm tt");
                    var timeend = kendo.toString(detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd, "hh:mm tt");
                    var durationthis = detail[0].DaysOfWeek[i].TimeSlotOfDay[y].Duration;

                    $('#on-detail' + (detail[0].DaysOfWeek[i].DayOfWeek - 1) + '').append('<label>' + timestart + '-' + timeend + '</label><br />');
                    var onDetail = $('#on-detail' + (detail[0].DaysOfWeek[i].DayOfWeek - 1) + '').height();
                    $('#view-detail-dayofweek' + (detail[0].DaysOfWeek[i].DayOfWeek - 1) + '').css("height", onDetail);
                    $('#on-detail' + (detail[0].DaysOfWeek[i].DayOfWeek - 1) + '').show();
                    $("#off-detail" + (detail[0].DaysOfWeek[i].DayOfWeek - 1) + "").hide();
                    $("#edit-button-detail" + (detail[0].DaysOfWeek[i].DayOfWeek - 1) + "").removeAttr("disabled", "disabled");

                    $("#info-add-detail" + (detail[0].DaysOfWeek[i].DayOfWeek - 1) + "").append("<div class='remove-div-detail'><label class='label-time-detail'>From</label> <input class='timefrom-detail' value='" + timestart + "' data-format='hh:mm tt' readonly/><label class='label-time-detail'>To</label> <input class='timeto-detail' value='" + timeend + "'  data-format='hh:mm tt' readonly/><label class='custom-duration'>Duration</label> <input class='duration-dayofweek-detail' value=" + durationthis + " readonly/><label class='label-time'>minutes</label></div>");
                    $("#view-button-detail" + (detail[0].DaysOfWeek[i].DayOfWeek - 1) + "").show();
                }
                if (detail[0].DaysOfWeek[i].DayOfWeek - 1 == 0)
                    switch_detail0.check(true);
                if (detail[0].DaysOfWeek[i].DayOfWeek - 1 == 1)
                    switch_detail1.check(true);
                if (detail[0].DaysOfWeek[i].DayOfWeek - 1 == 2)
                    switch_detail2.check(true);
                if (detail[0].DaysOfWeek[i].DayOfWeek - 1 == 3)
                    switch_detail3.check(true);
                if (detail[0].DaysOfWeek[i].DayOfWeek - 1 == 4)
                    switch_detail4.check(true);
                if (detail[0].DaysOfWeek[i].DayOfWeek - 1 == 5)
                    switch_detail5.check(true);
                if (detail[0].DaysOfWeek[i].DayOfWeek - 1 == 6)
                    switch_detail6.check(true);
            }
            if (detail[0].DaysOfWeek[i].DayOfWeek == 0) {
                $("#info-add-detail6").html("");
                $('#on-detail6').html("");
                for (y in detail[0].DaysOfWeek[i].TimeSlotOfDay) {
                    detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart = detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart.slice(6, 16);
                    detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart = new Date(detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart * 1000);
                    detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd = detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd.slice(6, 16);
                    detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd = new Date(detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd * 1000);

                    var timestart = kendo.toString(detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateStart, "hh:mm tt");
                    var timeend = kendo.toString(detail[0].DaysOfWeek[i].TimeSlotOfDay[y].DateEnd, "hh:mm tt");
                    var durationthis = detail[0].DaysOfWeek[i].TimeSlotOfDay[y].Duration;

                    $('#on-detail6').append('<label>' + timestart + '-' + timeend + '</label><br />');
                    var onDetail6 = $('#on-detail6').height();
                    $('#view-detail-dayofweek6').css("height", onDetail6);
                    $('#on-detail6').show();
                    $("#off-detail6").hide();
                    $("#edit-button-detail6").removeAttr("disabled", "disabled");

                    $("#info-add-detail6").append("<div class='remove-div-detail'><label class='label-time-detail'>From</label> <input class='timefrom-detail' value='" + timestart + "' data-format='hh:mm tt' readonly /><label class='label-time-detail'>To</label> <input class='timeto-detail' value='" + timeend + "'  data-format='hh:mm tt' readonly/><label class='custom-duration'>Duration</label> <input class='duration-dayofweek-detail' value=" + durationthis + " readonly/><label class='label-time'>minutes</label></div>");
                    $("#view-button-detail6").show();
                }
                switch_detail6.check(true);

            }
        }

        $(".k-view-button-detail").click(function (e) {
            e.preventDefault();
            var ondetail = "on-detail" + parseInt(sessionStorage.getItem("view"));
            var offdetail = "off-detail" + parseInt(sessionStorage.getItem("view"));
            var editdetail = "edit-time-detail" + parseInt(sessionStorage.getItem("view"));
            var viewbuttondetail = "view-button-detail" + parseInt(sessionStorage.getItem("view"));
            $('#view-detail-dayofweek' + parseInt(sessionStorage.getItem("view"))).css("height", "+=6em");
            //var from = "time-from-detail" + parseInt(sessionStorage.getItem("view"));
            //var to = "time-to-detail" + parseInt(sessionStorage.getItem("view"));
            $("#" + ondetail).hide();
            $("#" + offdetail).hide();
            $("#" + editdetail).show();
            $("#" + viewbuttondetail).hide();
            sessionStorage.clear();
        });
        $(".k-close-button-detail").click(function (e) {
            e.preventDefault();
            var ondetail = "on-detail" + parseInt(sessionStorage.getItem("close"));
            var editdetail = "edit-time-detail" + parseInt(sessionStorage.getItem("close"));
            var viewbuttondetail = "view-button-detail" + parseInt(sessionStorage.getItem("close"));
            $('#view-detail-dayofweek' + parseInt(sessionStorage.getItem("close"))).css("height", "-=6em");
            //var removediv = "remove-div" + parseInt(sessionStorage.getItem("close"));
            $("#" + ondetail).show();
            $("#" + editdetail).hide();
            $("#" + viewbuttondetail).show();
            //$('.' + removediv).remove();
            sessionStorage.clear();
        });
    }
    //
    //show kendoWindow detail page 
    var dialog_detail = $("#dialog_detail");

    dialog_detail.kendoDialog({
        title: "Available Time's Details",
        closable: true,
        visible: false
    });

    $('.k-cancel-button-detail').click(function () {
        dialog_detail.data("kendoDialog").close();
    });

    $(".k-window-action").click(function () {
    });

    var idPage = null;
    $(".k-edit-button-detail").click(function (e) {
        dialog_detail.data("kendoDialog").close();
        $("input").css("max-width", "");
        $('#dialog_edit').css('display', 'block');
        dialog_edit.data("kendoDialog").open();
        e.preventDefault();
        console.log(idPage);
        $.ajax({
            type: "get",
            url: "/TimeSlots/GetListDetail",
            data: { ID: idPage },
            success: function (da) {
                edit_dalog_page(da);
            }
        });

    });
    $('#duration-main').keydown(function (e) {
        return false;
    });
});
