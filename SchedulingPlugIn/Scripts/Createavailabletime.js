$(document).ready(function () {
    $("#off0").hide();
    $("#off1").hide();
    $("#off2").hide();
    $("#off3").hide();
    $("#off4").hide();
    $("#on5").hide();
    $("#on6").hide();
    $("#edit-time0").hide();
    $("#edit-time1").hide();
    $("#edit-time2").hide();
    $("#edit-time3").hide();
    $("#edit-time4").hide();
    $("#edit-time5").hide();
    $("#edit-time6").hide();
    $("#edit-button5").attr("disabled", "disabled");
    $("#edit-button6").attr("disabled", "disabled");
    var valueDurationDefault = $("#duration-main").val();
    var ArrayStore = [{ Duration: valueDurationDefault, DateStart: "09:00 AM", DateEnd: "11:00 AM" }, { Duration: valueDurationDefault, DateStart: "02:00 PM", DateEnd: "05:00 PM" }];
    var StorageSaveTime = [{ DayOfWeek: 0, TimeSlotOfDay: [] }, { DayOfWeek: 1, TimeSlotOfDay: [] }, { DayOfWeek: 2, TimeSlotOfDay: [] }, { DayOfWeek: 3, TimeSlotOfDay: [] }, { DayOfWeek: 4, TimeSlotOfDay: [] }, { DayOfWeek: 5, TimeSlotOfDay: [] }, { DayOfWeek: 6, TimeSlotOfDay: [] }];
    for (i in StorageSaveTime) {
        //console.log(SessionStorageSaveTime[i].TimeSlotOfDay);
        if (StorageSaveTime[i].TimeSlotOfDay.length == 0) {
            //$('#info-add6').html("");
            //$('#on6').html("");
            StorageSaveTime[i].TimeSlotOfDay.push(ArrayStore[0]);
            StorageSaveTime[i].TimeSlotOfDay.push(ArrayStore[1]);
            if (StorageSaveTime[i].DayOfWeek == 0) {
                for (y in StorageSaveTime[i].TimeSlotOfDay) {
                    $('#info-add6').append("<div class='remove-div' style='display: inline-flex;align-items: center;'><label class='label-time' style='margin-bottom: 0'>From</label> <input class='fromthis' value='" + StorageSaveTime[i].TimeSlotOfDay[y].DateStart + "' data-format='hh: mm tt' /><label class='label-time' style='margin-bottom: 0'>To</label> <input class='tothis' value='" + StorageSaveTime[i].TimeSlotOfDay[y].DateEnd + "'  data-format='hh:mm tt' /><label class='custom-duration' style='margin-bottom: 0'>Duration</label> <input  class='duration-dayofweek' value=" + StorageSaveTime[i].TimeSlotOfDay[y].Duration + " step=5 min=5 max=120 type='number' /><label class='label-time'style='margin-bottom: 0'>minutes</label><i class='k-icon k-i-minus-outline' ></i></div>");
                    $('#on6').append('<label class="timeshowlable" style="margin - bottom: 0">' + StorageSaveTime[i].TimeSlotOfDay[y].DateStart + '-' + StorageSaveTime[i].TimeSlotOfDay[y].DateEnd + '</label><br />');
                }
            }
            if (StorageSaveTime[i].DayOfWeek != 0) {
                $('#info-add' + (StorageSaveTime[i].DayOfWeek - 1)).html("");
                $('#on' + (StorageSaveTime[i].DayOfWeek - 1)).html("");
                for (y in StorageSaveTime[i].TimeSlotOfDay) {

                    $('#info-add' + (StorageSaveTime[i].DayOfWeek - 1)).append("<div class='remove-div' style='display:inline-flex; align-items: center;'><label class='label-time' style='margin-bottom: 0'>From</label> <input class='fromthis' value='" + StorageSaveTime[i].TimeSlotOfDay[y].DateStart + "' data-format='hh:mm tt' /><label class='label-time' style='margin-bottom: 0'>To</label> <input class='tothis' value='" + StorageSaveTime[i].TimeSlotOfDay[y].DateEnd + "'  data-format='hh:mm tt' /><label class='custom-duration' style='margin-bottom: 0'>Duration</label> <input  class='duration-dayofweek' value=" + StorageSaveTime[i].TimeSlotOfDay[y].Duration + " step=5 min=5 max=120 type='number' /><label class='label-time' style='margin-bottom: 0'>minutes</label><i class='k-icon k-i-minus-outline' ></i></div>");
                    $('#on' + (StorageSaveTime[i].DayOfWeek - 1)).append('<label class="timeshowlable" style="margin - bottom: 0">' + StorageSaveTime[i].TimeSlotOfDay[y].DateStart + '-' + StorageSaveTime[i].TimeSlotOfDay[y].DateEnd + '</label><br />');
                }
            }
        }

    }
    var dropDownListZone = $("#timeZone").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        // filter: "startswith",

        dataSource: {
            serverFiltering: true,

            transport: {
                read: {
                    // url: "https://raw.githubusercontent.com/darkgunz1998/ahaha/master/timezoneServerJson"
                    url: "https://jsonstorage.net/api/items/26392365-eee8-4e45-ae7a-548a34b5ad0a"
                    // url: "/Scripts/TimeZoneJson.json"
                }
            }
        },
        //index: -1,
        change: function (e) {

            zone = this.value();

            //// console.log(this.value());
            //if ($('#viewType .k-input').text() == "Work Week") {
            //    hideWeek2();
            //    formatWeek();
            //}
            var text = this.text();

            $.getJSON("https://jsonstorage.net/api/items/26392365-eee8-4e45-ae7a-548a34b5ad0a", function (obj) {

                timezone = getOffsetFromTimeZone(text, obj);
                console.log(text);
                //    //setTimeZone(timezone);
                //    //updateTime(timezone);
            });
        }
    }).data("kendoDropDownList");
    // dropDownListZone.text("Select timezone..");
    //console.log("Antarctica/Davis");
    dropDownListZone.value(getTimezoneName());
    //function updateTime() {
    //    var currtime = new Date().toLocaleString(["en-us"], { timeZone: timezone });
    //    for (i in currtime) {
    //        if (currtime[i] == ",")
    //            break;
    //    }
    //    i++;
    //    var currtime1 = currtime.slice(i, currtime.length);
    //    $("#time").html(currtime1);
    //}
    function getOffsetFromTimeZone(textTimeZone, obj) {
        var temp;
        for (i in obj) {
            if (obj[i].text == textTimeZone) {
                temp = obj[i].utc;
                timezone1 = obj[i].value;
                return temp[0];
            }
        }
        return null;
    }
    function getTimezoneName() {
        var timeSummer = new Date(Date.UTC(2005, 6, 30, 0, 0, 0, 0));
        var summerOffset = -1 * timeSummer.getTimezoneOffset();
        var timeWinter = new Date(Date.UTC(2005, 12, 30, 0, 0, 0, 0));
        var winterOffset = -1 * timeWinter.getTimezoneOffset();
        var timeZoneHiddenField;

        if (-720 == summerOffset && -720 == winterOffset) { timeZoneHiddenField = 'Dateline Standard Time'; }
        else if (-660 == summerOffset && -660 == winterOffset) { timeZoneHiddenField = 'UTC-11'; }
        else if (-660 == summerOffset && -660 == winterOffset) { timeZoneHiddenField = 'Samoa Standard Time'; }
        else if (-660 == summerOffset && -600 == winterOffset) { timeZoneHiddenField = 'Hawaiian Standard Time'; }
        else if (-570 == summerOffset && -570 == winterOffset) { timeZoneHiddenField.value = 'Pacific/Marquesas'; }
        //                else if (-540 == summerOffset && -600 == winterOffset) { timeZoneHiddenField.value = 'America/Adak'; }
        //                else if (-540 == summerOffset && -540 == winterOffset) { timeZoneHiddenField.value = 'Pacific/Gambier'; }
        else if (-480 == summerOffset && -540 == winterOffset) { timeZoneHiddenField = 'Alaskan Standard Time'; }
        //                else if (-480 == summerOffset && -480 == winterOffset) { timeZoneHiddenField = 'Pacific/Pitcairn'; }
        else if (-420 == summerOffset && -480 == winterOffset) { timeZoneHiddenField = 'Pacific Standard Time'; }
        else if (-420 == summerOffset && -420 == winterOffset) { timeZoneHiddenField = 'US Mountain Standard Time'; }
        else if (-360 == summerOffset && -420 == winterOffset) { timeZoneHiddenField = 'Mountain Standard Time'; }
        else if (-360 == summerOffset && -360 == winterOffset) { timeZoneHiddenField = 'Central America Standard Time'; }
        //                else if (-360 == summerOffset && -300 == winterOffset) { timeZoneHiddenField = 'Pacific/Easter'; }
        else if (-300 == summerOffset && -360 == winterOffset) { timeZoneHiddenField = 'Central Standard Time'; }
        else if (-300 == summerOffset && -300 == winterOffset) { timeZoneHiddenField = 'SA Pacific Standard Time'; }
        else if (-240 == summerOffset && -300 == winterOffset) { timeZoneHiddenField = 'Eastern Standard Time'; }
        else if (-270 == summerOffset && -270 == winterOffset) { timeZoneHiddenField = 'Venezuela Standard Time'; }
        else if (-240 == summerOffset && -240 == winterOffset) { timeZoneHiddenField = 'SA Western Standard Time'; }
        else if (-240 == summerOffset && -180 == winterOffset) { timeZoneHiddenField = 'Central Brazilian Standard Time'; }
        else if (-180 == summerOffset && -240 == winterOffset) { timeZoneHiddenField = 'Atlantic Standard Time'; }
        else if (-180 == summerOffset && -180 == winterOffset) { timeZoneHiddenField = 'Montevideo Standard Time'; }
        else if (-180 == summerOffset && -120 == winterOffset) { timeZoneHiddenField = 'E. South America Standard Time'; }
        else if (-150 == summerOffset && -210 == winterOffset) { timeZoneHiddenField = 'Mid-Atlantic Standard Time'; }
        else if (-120 == summerOffset && -180 == winterOffset) { timeZoneHiddenField = 'America/Godthab'; }
        else if (-120 == summerOffset && -120 == winterOffset) { timeZoneHiddenField = 'SA Eastern Standard Time'; }
        else if (-60 == summerOffset && -60 == winterOffset) { timeZoneHiddenField = 'Cape Verde Standard Time'; }
        else if (0 == summerOffset && -60 == winterOffset) { timeZoneHiddenField = 'Azores Daylight Time'; }
        else if (0 == summerOffset && 0 == winterOffset) { timeZoneHiddenField = 'Morocco Standard Time'; }
        else if (60 == summerOffset && 0 == winterOffset) { timeZoneHiddenField = 'GMT Standard Time'; }
        else if (60 == summerOffset && 60 == winterOffset) { timeZoneHiddenField = 'Africa/Algiers'; }
        else if (60 == summerOffset && 120 == winterOffset) { timeZoneHiddenField = 'Namibia Standard Time'; }
        else if (120 == summerOffset && 60 == winterOffset) { timeZoneHiddenField = 'Central European Standard Time'; }
        else if (120 == summerOffset && 120 == winterOffset) { timeZoneHiddenField = 'South Africa Standard Time'; }
        else if (180 == summerOffset && 120 == winterOffset) { timeZoneHiddenField = 'GTB Standard Time'; }
        else if (180 == summerOffset && 180 == winterOffset) { timeZoneHiddenField = 'E. Africa Standard Time'; }
        else if (240 == summerOffset && 180 == winterOffset) { timeZoneHiddenField = 'Russian Standard Time'; }
        else if (240 == summerOffset && 240 == winterOffset) { timeZoneHiddenField = 'Arabian Standard Time'; }
        else if (270 == summerOffset && 210 == winterOffset) { timeZoneHiddenField = 'Iran Standard Time'; }
        else if (270 == summerOffset && 270 == winterOffset) { timeZoneHiddenField = 'Afghanistan Standard Time'; }
        else if (300 == summerOffset && 240 == winterOffset) { timeZoneHiddenField = 'Pakistan Standard Time'; }
        else if (300 == summerOffset && 300 == winterOffset) { timeZoneHiddenField = 'West Asia Standard Time'; }
        else if (330 == summerOffset && 330 == winterOffset) { timeZoneHiddenField = 'India Standard Time'; }
        else if (345 == summerOffset && 345 == winterOffset) { timeZoneHiddenField = 'Nepal Standard Time'; }
        else if (360 == summerOffset && 300 == winterOffset) { timeZoneHiddenField = 'N. Central Asia Standard Time'; }
        else if (360 == summerOffset && 360 == winterOffset) { timeZoneHiddenField = 'Central Asia Standard Time'; }
        else if (390 == summerOffset && 390 == winterOffset) { timeZoneHiddenField = 'Myanmar Standard Time'; }
        else if (420 == summerOffset && 360 == winterOffset) { timeZoneHiddenField = 'North Asia Standard Time'; }
        else if (420 == summerOffset && 420 == winterOffset) { timeZoneHiddenField = 'SE Asia Standard Time'; }
        else if (480 == summerOffset && 420 == winterOffset) { timeZoneHiddenField = 'North Asia East Standard Time'; }
        else if (480 == summerOffset && 480 == winterOffset) { timeZoneHiddenField = 'China Standard Time'; }
        else if (540 == summerOffset && 480 == winterOffset) { timeZoneHiddenField = 'Yakutsk Standard Time'; }
        else if (540 == summerOffset && 540 == winterOffset) { timeZoneHiddenField = 'Tokyo Standard Time'; }
        else if (570 == summerOffset && 570 == winterOffset) { timeZoneHiddenField = 'Cen. Australia Standard Time'; }
        else if (570 == summerOffset && 630 == winterOffset) { timeZoneHiddenField = 'Australia/Adelaide'; }
        else if (600 == summerOffset && 540 == winterOffset) { timeZoneHiddenField = 'Asia/Yakutsk'; }
        else if (600 == summerOffset && 600 == winterOffset) { timeZoneHiddenField = 'E. Australia Standard Time'; }
        else if (600 == summerOffset && 660 == winterOffset) { timeZoneHiddenField = 'AUS Eastern Standard Time'; }
        else if (630 == summerOffset && 660 == winterOffset) { timeZoneHiddenField = 'Australia/Lord_Howe'; }
        else if (660 == summerOffset && 600 == winterOffset) { timeZoneHiddenField = 'Tasmania Standard Time'; }
        else if (660 == summerOffset && 660 == winterOffset) { timeZoneHiddenField = 'West Pacific Standard Time'; }
        else if (690 == summerOffset && 690 == winterOffset) { timeZoneHiddenField = 'Central Pacific Standard Time'; }
        else if (720 == summerOffset && 660 == winterOffset) { timeZoneHiddenField = 'Magadan Standard Time'; }
        else if (720 == summerOffset && 720 == winterOffset) { timeZoneHiddenField = 'Fiji Standard Time'; }
        else if (720 == summerOffset && 780 == winterOffset) { timeZoneHiddenField = 'New Zealand Standard Time'; }
        else if (765 == summerOffset && 825 == winterOffset) { timeZoneHiddenField = 'Pacific/Chatham'; }
        else if (780 == summerOffset && 780 == winterOffset) { timeZoneHiddenField = 'Tonga Standard Time'; }
        else if (840 == summerOffset && 840 == winterOffset) { timeZoneHiddenField = 'Pacific/Kiritimati'; }
        else { timeZoneHiddenField = 'US/Pacific'; }
        return timeZoneHiddenField;
    }

    // create DatePicker from input HTML element
    $("#datepicker").kendoDatePicker({
        format: "MM/dd/yyyy",
        value: getValueDate(),
        change: function () {
            $("#datepickerr").data("kendoDatePicker").min(kendo.toString($("#datepicker").data("kendoDatePicker").value(), "MM/dd/yyyy"));
            $("#datepickerr").val(kendo.toString(new Date(new Date($("#datepicker").data("kendoDatePicker").value()).getFullYear(), new Date($("#datepicker").data("kendoDatePicker").value()).getMonth(), new Date($("#datepicker").data("kendoDatePicker").value()).getDate() + 6), "MM/dd/yyyy"));

        },
        min: new Date()
    });
    $("#datepicker").attr('readonly', true);
    function getValueDate() {
        var now = new Date();
        if (now.getDay() == 0) {
            $("#datepicker").val(kendo.toString(new Date(now.getFullYear(), now.getMonth(), now.getDate() + 1), "MM/dd/yyyy"));
        }
        else if (now.getDay() == 6) {
            $("#datepicker").val(kendo.toString(new Date(now.getFullYear(), now.getMonth(), now.getDate() + 2), "MM/dd/yyyy"));
        }
        else {
            $("#datepicker").val(kendo.toString(new Date(), "MM/dd/yyyy"));
        }
    }
    var dateFrom = kendo.toString($("#datepicker").data("kendoDatePicker").value(), "MM/dd/yyyy");

    $("#datepickerr").kendoDatePicker({
        format: "MM/dd/yyyy",
        min: Date(dateFrom),
        change: function () {
            $("#datepicker").data("kendoDatePicker").max(kendo.toString($("#datepickerr").data("kendoDatePicker").value(), "MM/dd/yyyy"));
        }
    });
    $("#datepickerr").attr('readonly', true);
    $("#datepickerr").val(kendo.toString(new Date(new Date(dateFrom).getFullYear(), new Date(dateFrom).getMonth(), new Date(dateFrom).getDate() + 6), "MM/dd/yyyy"));

    $("#notifications-switch0").kendoSwitch({
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
    });
    $("#notifications-switch1").kendoSwitch({
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
    });
    $("#notifications-switch2").kendoSwitch({
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
    });
    $("#notifications-switch3").kendoSwitch({
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
    });
    $("#notifications-switch4").kendoSwitch({
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
    });
    $("#notifications-switch5").kendoSwitch({
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
    });
    $("#notifications-switch6").kendoSwitch({
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
    });

    $("input.fromthis").each(function () {
        $(this).kendoTimePicker({
            format: "hh:mm tt",
            interval: 15,
            min: "8:00 AM",
            max: "08:45 PM",
            change: function () {
                var numberTemp = $(this)[0]._value;
                var tothis = $($($(this)[0].element[0]).parents(".remove-div").find("input.tothis")).data("kendoTimePicker");
                var buttonsave = $($($(this)[0].element[0]).parents(".view").find(".k-update-button")[0]);
                tothis.min(numberTemp);
                var durationthis = parseInt($($(this)[0].element[0]).parents(".remove-div").find(".duration-dayofweek")[0].value) * 60 * 1000;
                if (tothis.value() - numberTemp < durationthis) {
                    buttonsave.attr("disabled", true);
                    $($($(this)[0].element[0]).parents(".remove-div").find(".text")[0]).remove();;
                    $($(this)[0].element[0]).parents(".remove-div").append("<p class='text' style='color:red; margin-bottom: 0; padding-left:5px;'>Conflig this time slot. Please change it </p>");
                }
                else {
                    buttonsave.attr("disabled", false);
                    $($($(this)[0].element[0]).parents(".remove-div").find(".text")[0]).remove();;
                }




            }
        }).data("kendoTimePicker");
    })
    $("input.tothis").each(function () {
        $(this).kendoTimePicker({
            format: "hh:mm tt",
            interval: 15,
            min: "08:15 AM",
            max: "9:00 PM",
            change: function () {
                var numberTemp = $(this)[0]._value;
                var fromthis = $($($(this)[0].element[0]).parents(".remove-div").find("input.fromthis")).data("kendoTimePicker");
                var buttonsave = $($($(this)[0].element[0]).parents(".view").find(".k-update-button")[0]);
                var durationthis = parseInt($($(this)[0].element[0]).parents(".remove-div").find(".duration-dayofweek")[0].value) * 60 * 1000;
                if (numberTemp - fromthis.value() < durationthis) {
                    buttonsave.attr("disabled", true);
                    $($($(this)[0].element[0]).parents(".remove-div").find(".text")[0]).remove();;
                    $($(this)[0].element[0]).parents(".remove-div").append("<p class='text' style='color:red; margin-bottom: 0; padding-left:5px;'>Conflig this time slot. Please change it </p>");
                }
                else {
                    buttonsave.attr("disabled", false);
                    $($($(this)[0].element[0]).parents(".remove-div").find(".text")[0]).remove();;
                }



            }
        }).data("kendoTimePicker");
    });

    var variblecheckClick = 0;
    var checkedit = true;
    $(".k-edit-button").click(function (e) {
        e.preventDefault();
        checkCloseEdit++;
        if (checkCloseEdit != 0) {
            console.log(checkCloseEdit);
            $("#save-button").css("background-color", "grey");
            $("#save-button").attr("disabled", true);
        }
        var dem = 0;
        var on = "on" + parseInt(sessionStorage.getItem("id"));
        var duration = $("#duration-dayofweek" + parseInt(sessionStorage.getItem("id"))).val($("#duration-main").val());
        var off = "off" + parseInt(sessionStorage.getItem("id"));
        var edit = "edit-time" + parseInt(sessionStorage.getItem("id"));
        var editbutton = "edit-button" + parseInt(sessionStorage.getItem("id"));
        var from = "time-from" + parseInt(sessionStorage.getItem("id"));
        var to = "time-to" + parseInt(sessionStorage.getItem("id"));
        var duration = "duration-dayofweek" + parseInt(sessionStorage.getItem("id"));
        var addEdit = "info-add" + parseInt(sessionStorage.getItem("id"));
        var time = "time" + parseInt(sessionStorage.getItem("id"));

        $(".k-timepicker").removeClass("k-input");
        $("#" + on).hide();
        $('#' + on).empty();
        $("#" + off).hide();
        $("#" + edit).show();
        $("#" + editbutton).hide();
        //$("#" + addEdit).append("<div class='remove-div'><label class='label-time'>From</label> <input id=" + from + '1' + " class='fromthis' data-format='hh:mm tt' /><label class='label-time'>To</label> <input id=" + to + '1' + " class='tothis' data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input class='duration-dayofweek' value='" + parseInt($("#duration-main").val()) + "' step=5 min=5 max=120 type='number' /><label>minutes</label><span class='k-icon k-i-minus-outline'></span></div><div class='remove-div'><label class='label-time'>From</label> <input id=" + from + '2' + " class='fromthis' data-format='hh:mm tt' /><label class='label-time'>To</label> <input id=" + to + "2" + " class='tothis' data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input class='duration-dayofweek' value='" + parseInt($("#duration-main").val()) + "' step=5 min=5 max=120 type='number' /><label>minutes</label><span class='k-icon k-i-minus-outline'></span></div>");

        var nineClock = (20 * 3600 + 45 * 60) * 1000;

        $(this).parent(".view").find(".remove-div").each(function () {
            dem++;
        });
        if (variblecheckClick == 0) {
            clickRemoveTimeSlot(nineClock);
        };

        sessionStorage.clear();
        $("input.fromthis").attr('readonly', true);
        $("input.tothis").attr('readonly', true);
        var Counts = $(this).parents(".view").find(".remove-div");

        var lastID = Counts[Counts.length - 1].children[3].children[0].children[0].defaultValue;
        var getIdparent = $(this).parents('.view')[0].children[3].id;
        if (checkedit == true) {
            checkedit = false;
            var valueFirst = $($($(this).parents(".view").find('input.tothis')[0])).data('kendoTimePicker').value()
            valueFirst.setMinutes(valueFirst.getMinutes() + 15);
            $($($(this).parents(".view").find('input.fromthis')[1])).data('kendoTimePicker').min(valueFirst);
        }

        var numberTemp = new Date(Counts[Counts.length - 1].children[3].children[0].children[0].value);
        var timeAdd = (numberTemp.getHours() * 3600 + numberTemp.getMinutes() * 60 + numberTemp.getSeconds()) * 1000;
        if (timeAdd < nineClock) {
            $("#" + getIdparent + " .k-plus-button").show();
        }
        $('.duration-dayofweek').keydown(function (e) {
            return false;
        })
    });

    $(".k-cancel-button").click(function (e) {
        e.preventDefault();
        checkCloseEdit--;
        if (checkCloseEdit == 0) {
            console.log(checkCloseEdit);
            $("#save-button").css("background-color", "#4bc34b");
            $("#save-button").attr("disabled", false);
        }
        variblecheckClick = 0;
        var infoAdd = "info-add" + parseInt(sessionStorage.getItem("cancel"));
        var on = "on" + parseInt(sessionStorage.getItem("cancel"));
        var edit = "edit-time" + parseInt(sessionStorage.getItem("cancel"));
        var editbutton = "edit-button" + parseInt(sessionStorage.getItem("cancel"));
        var timefrom = "time-from" + parseInt(sessionStorage.getItem("cancel"));
        var timeto = "time-to" + parseInt(sessionStorage.getItem("cancel"));
        var time = "time" + parseInt(sessionStorage.getItem("cancel"));
        var ArrayAllTimeSlosts = [];
        $("#" + on).show();
        $("#" + edit).hide();
        $("#" + editbutton).show();
        var i = 1;
        var dem = 0;
        $("#" + infoAdd).html("");
        $("#" + on).html("");
        var Arraystore = [];
        var countdatepicker = 0;
        if (parseInt(sessionStorage.getItem("cancel")) == 6)
            Arraystore = StorageSaveTime.find(findname => findname.DayOfWeek === 0).TimeSlotOfDay;
        else
            Arraystore = StorageSaveTime.find(findname => findname.DayOfWeek === parseInt(sessionStorage.getItem("cancel")) + 1).TimeSlotOfDay;
        for (item in Arraystore) {
            $("#" + infoAdd).append("<div class='remove-div picker" + countdatepicker + "'  style='display:inline-flex'><label class='label-time'>From</label> <input class='fromthis' value='" + Arraystore[item].DateStart + "' data-format='hh:mm tt' /><label class='label-time'>To</label> <input class='tothis' value='" + Arraystore[item].DateEnd + "'  data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input class='duration-dayofweek' value=" + Arraystore[item].Duration + " step=5 min=5 max=120 type='number' /><label class='label-time'>minutes</label><i class='k-icon k-i-minus-outline' ></i></div>")
            $("#" + on).append('<label class="timeshowlable">' + Arraystore[item].DateStart + '-' + Arraystore[item].DateEnd + '</label><br />');
            $('#' + infoAdd + ' .picker' + countdatepicker + ' .fromthis').kendoTimePicker({
                dateInput: true
            });
            $('#' + infoAdd + ' .picker' + countdatepicker + ' .tothis').kendoTimePicker({
                dateInput: true
            });
            countdatepicker++;
        }
        $("#" + on).css("height", "auto")
        $(".view").css("height", "auto")
        sessionStorage.clear();
        dem = 0;
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

    $(".k-save-button").click(function () {
        if (checkCloseEdit == 0) {
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
                url: "/Calendar/CreateAvailableTimeSlot",
                data: { sessionTimeSlot: sesioncreate },
                success: function (e) {

                    if (e == 1) {
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
                        const Toast = swal.mixin({
                            toast: false,
                            position: 'top-left',
                            showConfirmButton: false,
                            timer: 3000
                        })

                        Toast.fire({
                            type: 'error',
                            title: 'Error!'
                        })
                    }
                }
            })
        }

    });

    $(".k-plus-button").click(function () {
        variblecheckClick = 1;
        var dem = 1;
        $(this).parents(".view").find(".remove-div").each(function () {
            dem++;
        });
        var count = $(this).parents(".view").find(".remove-div");
        var add = "info-add" + parseInt(sessionStorage.getItem("add"));
        var timefrom = "time-from" + parseInt(sessionStorage.getItem("add"));
        //var IdlastTime = parseInt($(this).parents(".view").find(".remove-div")[count - 1].children[1].children[0].children[0].id.slice(10));
        var timeto = "time-to" + parseInt(sessionStorage.getItem("add"));
        $("#" + add).append("<div class='remove-div' style='display: inline-flex; align-items: center;'><label class='label-time' style='margin-bottom: 0'>From</label> <input class='fromthis' data-format='hh:mm tt'  /><label class='label-time' style='margin-bottom: 0'>To</label> <input class='tothis' data-format='hh:mm tt' /><label class='custom-duration' style='margin-bottom: 0'>Duration</label> <input class='duration-dayofweek' value='" + parseInt($("#duration-main").val()) + "' step=5 min=5 max=120 type='number'/><label class='label-time' style='margin-bottom: 0'>minutes</label><span class='k-icon k-i-minus-outline'></span></div>");

        if (dem > 1) {
            $(".k-i-minus-outline").show();
        }

        var number = count[count.length - 1].children[3].children[0].children[0].value;
        var numberTime = new Date("10/10/2019 " + number);
        var getMinutes = numberTime.getMinutes();
        var nineClock = (20 * 3600 + 45 * 60) * 1000;
        numberTime.setMinutes(getMinutes + 15);
        var count2 = $(this).parents(".view").find(".remove-div");
        console.log(count2);
        $(count2[count2.length - 1].children[1]).kendoTimePicker({
            format: "hh:mm tt",
            value: numberTime,
            interval: 15,
            min: numberTime,
            max: "8:45 PM",
            change: function () {
                var numberTemp = $(this)[0]._value;
                $(count2[count2.length - 1].children[3].children[0].children[0]).data("kendoTimePicker").min(numberTemp);
                var tothis = $($($(this)[0].element[0]).parents(".remove-div").find("input.tothis")).data("kendoTimePicker");
                var buttonsave = $($($(this)[0].element[0]).parents(".view").find(".k-update-button")[0]);
                var durationthis = parseInt($($(this)[0].element[0]).parents(".remove-div").find(".duration-dayofweek")[0].value) * 60 * 1000;
                if (tothis.value() - numberTemp < durationthis) {
                    buttonsave.attr("disabled", true);
                    $($($(this)[0].element[0]).parents(".remove-div").find(".text")[0]).remove();;
                    $($(this)[0].element[0]).parents(".remove-div").append("<p class='text' style='color:red;margin-bottom: 0; padding-left:5px;'>Conflig this time slot. Please change it </p>");
                }
                else {
                    buttonsave.attr("disabled", false);
                    $($($(this)[0].element[0]).parents(".remove-div").find(".text")[0]).remove();;
                }

            }
        }).data("kendoTimePicker");
        $(count2[count2.length - 1].children[3]).kendoTimePicker({
            format: "hh:mm tt",
            value: "9:00 PM",
            interval: 15,
            min: $(count2[count2.length - 1].children[1].children[0].children[0]).data("kendoTimePicker").value(),
            max: "9:00 PM",
            change: function () {
                var numberTemp = $(this)[0]._value;
                $(count2[count2.length - 1].children[1].children[0].children[0]).data("kendoTimePicker").max(numberTemp);
                var timeAdd = (numberTemp.getHours() * 3600 + numberTemp.getMinutes() * 60 + numberTemp.getSeconds()) * 1000;
                var nextthis = $($($(this)[0].element[0]).parents(".remove-div")[0]).next();
                var prevthis = $($($(this)[0].element[0]).parents(".remove-div")[0]).prev();
                console.log(numberTemp.getHours());
                if (nextthis.length == 0) {
                    if (timeAdd < nineClock) {
                        $($($($(this)[0].element[0]).parents(".view")[0]).find(".k-plus-button")).show();
                    } else {
                        $($($($(this)[0].element[0]).parents(".view")[0]).find(".k-plus-button")).hide();
                    }
                }
                if (nextthis.length != 0) {
                    if (numberTemp.getHours() == 20 && numberTemp.getMinutes() == 45 || numberTemp.getHours() == 21) {
                        nextthis.remove();
                    } else {
                        numberTemp.setMinutes(numberTemp.getMinutes() + 15);
                        $($(nextthis[0]).find('input.fromthis')).data("kendoTimePicker").min(numberTemp);
                        $($(nextthis[0]).find('input.fromthis')).data("kendoTimePicker").value(numberTemp);
                        numberTemp.setMinutes(numberTemp.getMinutes() + 15);
                        $($(nextthis[0]).find('input.tothis')).data("kendoTimePicker").min(numberTemp);
                    }
                }
                var fromthis = $($($(this)[0].element[0]).parents(".remove-div").find("input.fromthis")).data("kendoTimePicker");
                var buttonsave = $($($(this)[0].element[0]).parents(".view").find(".k-update-button")[0]);
                var durationthis = parseInt($($(this)[0].element[0]).parents(".remove-div").find(".duration-dayofweek")[0].value) * 60 * 1000;
                if (numberTemp - fromthis.value() < durationthis) {
                    buttonsave.attr("disabled", true);
                    $($($(this)[0].element[0]).parents(".remove-div").find(".text")[0]).remove();;
                    $($(this)[0].element[0]).parents(".remove-div").append("<p class='text' style='color:red; margin-bottom: 0; padding-left:5px;'>Conflig this time slot. Please change it </p>");
                }
                else {
                    buttonsave.attr("disabled", false);
                    $($($(this)[0].element[0]).parents(".remove-div").find(".text")[0]).remove();;
                }
            }
        }).data("kendoTimePicker");

        var numberTemp = $(count2[count2.length - 1].children[3].children[0].children[0]).data("kendoTimePicker").value();
        var timeAdd = (numberTemp.getHours() * 3600 + numberTemp.getMinutes() * 60 + numberTemp.getSeconds()) * 1000;
        if (timeAdd >= nineClock) {
            $(this).hide();
        }
        sessionStorage.clear();
        if (variblecheckClick == 1) {
            clickRemoveTimeSlot(nineClock);
        }


        $("input.fromthis").attr('readonly', true);
        $("input.tothis").attr('readonly', true);


    });


    function getValueFrom(a) {
        var timefromnext = kendo.toString($("#time-from" + timefrom + (a - 1)).val(), "hh:mm tt");

    }
    function clickRemoveTimeSlot(nineClock) {
        $('.k-i-minus-outline').click(function () {
            var dem = 0;
            $(this).parents(".view").find(".remove-div").each(function () {
                dem++;
            });
            var Counts = $(this).parents(".view").find(".remove-div");
            var lastID;
            if (Counts.length > 1) {
                lastID = Counts[Counts.length - 2].children[3].children[0].children[0];
            } else if (Counts.length == 1) {
                lastID = Counts[Counts.length - 1].children[3].children[0].children[0];
            }
            try {
                var getIdparent = $(this).parents('.view')[0].children[3].id;
                var numberTemp = $(lastID).data('kendoTimePicker').value();
                var timeAdd = (numberTemp.getHours() * 3600 + numberTemp.getMinutes() * 60 + numberTemp.getSeconds()) * 1000;
            } catch (er) {
                console.log("double");
            }


            var checkPosition = $(this).parents(".remove-div").next();
            if (checkPosition.length == 0) {
                if (timeAdd < nineClock) {
                    $("#" + getIdparent + " .k-plus-button").show();
                }
                if (dem > 1) {
                    $("#" + getIdparent + " .k-i-minus-outline").show();
                    $(this).parent('.remove-div').remove();
                    dem--;
                    if (dem == 1) {
                        $("#" + getIdparent + " .k-i-minus-outline").hide();
                    }
                }
            } else {
                if (dem > 1) {
                    $("#" + getIdparent + " .k-i-minus-outline").show();
                    $(this).parent('.remove-div').remove();
                    dem--;
                }
            }

        });
    }
    var checkCloseEdit = 0;
    $(".k-update-button").click(function (e) {
        e.preventDefault();
        checkCloseEdit--;
        if (checkCloseEdit == 0) {
            console.log(checkCloseEdit);
            $("#save-button").css("background-color", "#4bc34b");
            $("#save-button").attr("disabled", false);
        }
        variblecheckClick = 0;
        var infoAdd = "info-add" + parseInt(sessionStorage.getItem("save"));
        var on = "on" + parseInt(sessionStorage.getItem("save"));
        var edit = "edit-time" + parseInt(sessionStorage.getItem("save"));
        var editbutton = "edit-button" + parseInt(sessionStorage.getItem("save"));
        var timefrom = "time-from" + parseInt(sessionStorage.getItem("save"));
        var timeto = "time-to" + parseInt(sessionStorage.getItem("save"));
        var time = "time" + parseInt(sessionStorage.getItem("save"));
        var ArrayAllTimeSlosts = [];
        $("#" + on).show();
        $("#" + edit).hide();
        $("#" + editbutton).show();
        var i = 1;
        var dem = 0;
        var objTimeslotofweek = $(this).parents(".view").find(".remove-div");
        objTimeslotofweek.each(function () {
            var flag = 0;
            var thisremovediv = $(this);

            console.log(thisremovediv.find('input.tothis')/*[1].defaultValue*/);
            ObjEachTimeslost = {
                DateEnd: thisremovediv.find('input.tothis')[0].value,
                DateStart: thisremovediv.find('input.fromthis')[0].value,
                Duration: thisremovediv.find('input.duration-dayofweek').val()
            }
            ArrayAllTimeSlosts.push(ObjEachTimeslost);
            dem++;
            i++;
        });
        if (parseInt(sessionStorage.getItem("save")) == 6)
            StorageSaveTime.find(findname => findname.DayOfWeek === 0).TimeSlotOfDay = ArrayAllTimeSlosts;
        else
            StorageSaveTime.find(findname => findname.DayOfWeek === parseInt(sessionStorage.getItem("save")) + 1).TimeSlotOfDay = ArrayAllTimeSlosts;
        $("#" + infoAdd).html("");
        $("#" + on).html("");
        var Arraystore = [];
        var countdatepicker = 0;
        if (parseInt(sessionStorage.getItem("save")) == 6)
            Arraystore = StorageSaveTime.find(findname => findname.DayOfWeek === 0).TimeSlotOfDay;
        else
            Arraystore = StorageSaveTime.find(findname => findname.DayOfWeek === parseInt(sessionStorage.getItem("save")) + 1).TimeSlotOfDay;
        for (item in Arraystore) {
            $("#" + infoAdd).append("<div class='remove-div picker" + countdatepicker + "'  style='display:inline-flex'><label class='label-time'>From</label> <input class='fromthis' value='" + Arraystore[item].DateStart + "' data-format='hh:mm tt' /><label class='label-time'>To</label> <input class='tothis' value='" + Arraystore[item].DateEnd + "'  data-format='hh:mm tt' /><label class='custom-duration'>Duration</label> <input class='duration-dayofweek' value=" + Arraystore[item].Duration + " step=5 min=5 max=120 type='number' /><label class='label-time'>minutes</label><i class='k-icon k-i-minus-outline' ></i></div>")
            $("#" + on).append('<label class="timeshowlable">' + Arraystore[item].DateStart + '-' + Arraystore[item].DateEnd + '</label><br />');
            $('#' + infoAdd + ' .picker' + countdatepicker + ' .fromthis').kendoTimePicker({
                dateInput: true
            });
            $('#' + infoAdd + ' .picker' + countdatepicker + ' .tothis').kendoTimePicker({
                dateInput: true
            });
            countdatepicker++;
        }
        $("#" + on).css("height", "auto")
        $(".view").css("height", "auto")
        //objTimeslotofweek.remove();
        sessionStorage.clear();
        dem = 0;
    });
    $('#duration-main').change(function () {
        var durationValueMain = $(this).val();
        console.log(durationValueMain);
        $('.duration-dayofweek').val(durationValueMain);
    })
    $('#duration-main').keydown(function (e) {
        return false;
    })

});