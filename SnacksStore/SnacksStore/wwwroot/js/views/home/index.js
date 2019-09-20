var popup, dataTable;

$(document).ready(function () {
   

    function IsAdmin(res = 1) {
        if (res == 1) {
            dataTable = $('#SnacksTableAdmin').DataTable({
                "ajax": {
                    "url": "/api/products",
                    "type": "GET",
                    "datatype": "json"
                },
                "columns": [
                    { "data": "productName" },
                    { "data": "price" },
                    { "data": "details" },
                    {
                        "data": "productID",
                        "render": function (data) {
                            return "<a class='btn btn-default btn-sm' onclick=ShowPopup('/home/AddEditProduct/" + data + "')><i class='fa fa-pencil'></i> Edit</a><a class='btn btn-danger btn-sm' style='margin-left:5px' onclick=DeleteProduct(" + data + ")><i class='fa fa-trash'></i> Delete</a><a class='btn btn-success btn-sm' style='margin-left:5px' onclick=BuyProduct(" + data + ")><i class='fa fa-dollar'></i> Buy Product</a>";
                        }
                    }
                ],
                "language": {
                    "emptyTable": "no data found."
                }
            });

        } else {
            dataTable = $('#SnacksTableAdmin').DataTable({
                "ajax": {
                    "url": "/api/products",
                    "type": "GET",
                    "datatype": "json"
                },
                "columns": [
                    { "data": "productName" },
                    { "data": "price" },
                    { "data": "details" },
                    {
                        "data": "productID",
                        "render": function (data) {
                            return "<a class='btn btn-success btn-sm' style='margin-left:5px' onclick=BuyProduct(" + data + ")><i class='fa fa-dollar'></i> Buy Product</a>";
                        }
                    }
                ],
                "language": {
                    "emptyTable": "no data found."
                }
            });

        }
        
    }
    
});

function ShowPopup(url) {
    var formDiv = $('<div/>');
    $.get(url)
        .done(function (response) {
            formDiv.html(response);
            popup = formDiv.dialog({
                autoOpen: true,
                resizeable: false,
                width: 600,
                height: 400,
                title: 'Add or Edit Data',
                close: function () {
                    popup.dialog('destroy').remove();
                }
            });
        });
}


function SubmitAddEdit(form) {
    //alert("nt");
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        console.log(data);
        $.ajax({
            type: 'POST',
            url: '/api/products',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.success) {
                    popup.dialog('close');
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                }
            }
        });
        
    }
    return false;
}

function DeleteProduct(id) {
    swal({
        title: "Are you sure want to Delete?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: '/api/products/' + id,
            success: function (data) {
                if (data.success) {
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                }
            }
        });
    });


}



function ShowMessage(msg) {
    toastr.success(msg);
}

