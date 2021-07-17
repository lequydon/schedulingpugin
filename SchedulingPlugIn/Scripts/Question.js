var taggrid = "";
function grid_in_tabtrip(idPage) {
    $(document).ready(function () {
        var grid = $("#grid" + idPage + "").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: "/question/getlisttap?PageID=" + idPage
                    },
                    update: {
                        data: function (update) {
                            console.log(update);
                            if (update.Question == "") {
                                console.log(update.Question);
                                $(".error").css("display", "inline-block");
                            }
                            else {
                                $("#error").css("display", "none");
                                update.Question = escapeHtmlEntities(update.Question);
                                $.ajax({
                                    type: "post",
                                    url: "/Question/UpdateQuestions",
                                    data: { up: update },
                                    success: function (e) {
                                        $("#grid" + idPage + "").getKendoGrid().dataSource.read();
                                        $("#grid" + idPage + "").data('kendoGrid').refresh();
                                        const Toast = swal.mixin({
                                            toast: true,
                                            position: 'top-left',
                                            showConfirmButton: false,
                                            timer: 3000
                                        })

                                        Toast.fire({
                                            type: 'success',
                                            title: 'Edit the question success!'
                                        })

                                        //if (d == -2) {
                                        //    kendo.alert("The page name is duplicate.");
                                        //}

                                    }

                                })

                            }
                            //  });
                            //}
                            // return true;
                        }
                    },
                    create: {
                        data: function (create) {
                            //console.log(idPage);
                            $.ajax({
                                type: "post",
                                url: "/question/Create",
                                data: { contentQuestion: create.Question, pageID: idPage },
                                success: function () {
                                    const Toast = swal.mixin({
                                        toast: true,
                                        position: 'top-left',
                                        showConfirmButton: false,
                                        timer: 3000
                                    })

                                    Toast.fire({
                                        type: 'success',
                                        title: 'Create a new question success!'
                                    })
                                    $("#grid" + idPage + "").getKendoGrid().dataSource.read();
                                    $("#grid" + idPage + "").data('kendoGrid').refresh();
                                }
                            });
                        }
                    },
                    
                    parameterMap: function (options, operation) {

                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        }
                    }
                },
                
                Cancel: function (e) {
                    console.log(e);
                    //var temp = e.model.ID;
                    //var temp_array = e.sender.dataSource._pristineData;
                    //for (i in temp_array) {
                    //    if (temp == temp_array[i].ID)
                    //        record = i;
                    //}
                    //console.log(record);
                },
                schema: {
                    model: {
                        id: "ID",
                        fields: {
                            pagerequest:{ type:"string"},
                            Question: { type: "string", validation: { required: true } },
                            OrderQuestion: { type: "int", validation: { required: true } }
                        }
                    }
                },
                pageSize: 10
            },
            save: function (e) {

                // update.Question = escapeHtmlEntities(update.Question);
                if (e.model.Question == "" || e.model.Question.trim() == "") {
                    e.preventDefault();
                    $("#error").css("display", "inline-block");
                }
            },
            edit: function (e) {
                var temp = e.model.ID;
                var temp_array = e.sender.dataSource._pristineData;
                for (i in temp_array) {
                    if (temp == temp_array[i].ID)
                        record = i;
                }
                // console.log($("#error").css("display"));

                $("#text_key").keypress(function () {
                    $("#error").css("display", "none");
                });
            },
            pageSize: 10,
            height: 540,

            sortable: true,
            reorderable: true,
            groupable: false,
            resizable: true,
            filterable: true,
            columnMenu: false,
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 20, 40],
                buttonCount: 5
            },
            toolbar: [{ name: "create", text: "create" }], 
            columns: [
                {
                    field: "pagerequest",
                    hidden: true
                },
                {
                    field: "ID",
                    hidden: true
                },
                {
                    title: "#",
                    template: "#= ++record #",
                    width: 40
                }, {
                    field: "Question",
                    title: "Question",
                    width: 690


                }, {
                    command: [
                        { name: "edit", text: { edit: "Edit", update: "Save", cancel: "Cancel" } },
                        {
                        text: "Delete", iconClass: "fa fa-close", className: "btn-decline-grid",
                        click: function (e) {
                            $('.textareaSaved').val("");
                            dialogstart.data("kendoDialog").open();
                            e.preventDefault();
                            var tr = $(e.target).closest("tr");
                            var data = this.dataItem(tr);
                            taggrid = data.pagerequest;
                            item = data.ID;
                            tempp = data.Question;
                        },

                    }],
                    title: "Commands",
                    width: 180
                }],
            editable:"inline",
            //editable: {
            //    mode: "inline"
            //   // template: kendo.template($("#popup-editor").html())
            //},
            dataBinding: function () {
                //console.log((this.dataSource.page() - 1) * this.dataSource.pageSize());
                record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
                //console.log(this);

            }
        }).data("kendoGrid");
        grid.table.kendoSortable({
            filter: ">tbody >tr:not('.k-grid-edit-row')",
            hint: function (element) { //customize the hint
                var table = $('<table style="width: 600px;" class="k-grid k-widget"></table>'),
                    hint;

                table.append(element.clone()); //append the dragged element
                table.css("opacity", 0.7);
                return table; //return the hint element
            },
            cursor: "move",
            placeholder: function (element) {
                return $('<tr colspan="4" class="placeholder"></tr>');
            },
            change: function (e) {
                var skip = grid.dataSource.skip(),
                    oldIndex = e.oldIndex + skip,
                    newIndex = e.newIndex + skip,
                    data = grid.dataSource.data(),
                    dataItem = grid.dataSource.getByUid(e.item.data("uid"));
                dialog.data("kendoDialog").open();
                grid.dataSource.remove(dataItem);
                grid.dataSource.insert(newIndex, dataItem);
                oldndex = oldIndex;
                newndex = newIndex;
                datadrag = dataItem.pagerequest;


            },

        });
        var dialog = $('#dialog');

        dialog.kendoDialog({
            width: "450px",
            closable: false,
            modal: false,
            content: "Do you want to change the order of the question?",
            visible: false,
            actions: [
                { text: 'Close', action: onCancel },
                { text: 'OK', primary: true, action: onDrag }
            ]
        });
        var dialogstart = $("#dialog-content");
        dialogstart.kendoDialog({
            width: "450px",
            closable: false,
            modal: false,
            content: "Are You Sure You Want to Delete This Question?",
            visible: false,
            actions: [
                { text: 'Close' },
                { text: 'OK', primary: true, action: onOK }
            ]


        });

    });
    function onCancel() {
        $("#grid" + datadrag + "").getKendoGrid().dataSource.read();
        $("#grid" + datadrag + "").data('kendoGrid').refresh();
    }
    function onDrag() {
        console.log(oldndex, newndex);
        $.ajax({
            type: "post",
            url: "/Question/Drag",
            data: { oldindex: oldndex, newindex: newndex, PageRequestid1: datadrag },
            success: function () {
                const Toast = swal.mixin({
                    toast: true,
                    position: 'top-left',
                    showConfirmButton: false,
                    timer: 3000
                })

                Toast.fire({
                    type: 'success',
                    title: 'Change question order success!'
                })
                $("#grid" + datadrag + "").getKendoGrid().dataSource.read();
                $("#grid" + datadrag + "").data('kendoGrid').refresh();

            },

        });
        return true;
    }
    function onOK() {
        item = escapeHtmlEntities(item);
        tempp = escapeHtmlEntities(tempp);
        $.ajax({
            type: "post",
            url: "/question/Delete",
            data: { deleteid: item },
            success: function (e) {
                const Toast = swal.mixin({
                    toast: true,
                    position: 'top-left',
                    showConfirmButton: false,
                    timer: 3000
                })

                Toast.fire({
                    type: 'success',
                    title: 'Delete a question success!'
                })
                $("#grid" + taggrid + "").getKendoGrid().dataSource.read();
                $("#grid" + taggrid + "").data('kendoGrid').refresh();


            }
        });
        return true;
    }
}
var item;
var tempp;
var oldndex;
var newndex;
var datadrag;
var value;
$(document).ready(function () {
    // create DropDownList from input HTML element
    $("#color").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "PageID",
        dataSource: {
            transport: {
                read: {
                    url: "/Question/getlistdata"
                }
            }
        },
        index: 0,
        select: onSelect,
        dataBound: onDataBound
    });
    function onDataBound(e) {
        value = e.sender.dataSource._pristineData[0].PageID;
    };
    function onSelect(e) {
        // console.log(e);
        value = e.dataItem.PageID;
        $("#cap").toggleClass("black-cap");
    };


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
$(".page_request").next().click(function () {

    $('#alertNull').remove();
    $('#text').keyup(function () {
        console.log($('#text').val());
        if ($(this).val().trim() != "" && $('#alertNull').length > 2) {
            $('#alertNull').remove();
        }
    })
})