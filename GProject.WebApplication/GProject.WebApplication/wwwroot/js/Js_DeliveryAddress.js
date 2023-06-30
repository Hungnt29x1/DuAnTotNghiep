$(document).ready(function () {
    var PlaceHolderElement = $('#layout');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        })
    })
})

function ChangePanel() {
    if ($("#changeform").text() == "Thêm mới") {
        $('#formdata').show();
        $("#changeform").text("Hủy thay đổi");
    } else if ($("#changeform").text() == "Hủy thay đổi") {
        $('#formdata').hide();
        $("#changeform").text("Thêm mới");
    }
}

function DataAddressCustomer() {
    $.ajax({
        url: "/Product/GetDataDeliveryAddressByCustomerId",
        type: "GET",
        success: function (result) {

            var renderData = "<option></option>";
            $.each(result, function (value) {
                renderData += '<option value="' + result[value].id + '">' + result[value].name + ' | ' + result[value].phoneNumber + ', ' + result[value].address + ', ' + result[value].wardName + ', ' + result[value].districtName + ', ' + result[value].provinceName + '</option>';
            });
            $("#selectDeliveryAddress").html(renderData)
        }
    })
}

function SaveDeliveryAddress() {
    if (!checkValidateVOucher()) {

    }
    else {
        swal("Bạn có muốn lưu thông tin địa chỉ này không?", {
            title: 'Thông Báo!',
            icon: 'info',
            buttons: ["Không, tôi cần xem lại", "Có, tôi đồng ý"],
        }).then(function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: "/DeliveryAddress/Save",
                    type: "POST",
                    data: JSON.stringify({
                        id: $("#txtId").val(),
                        provinceID: $("#selectProvince").val(),
                        provinceName: $("[id='selectProvince'] option:selected").text(),
                        districtID: $("#selectDistrict").val(),
                        districtName: $("[id='selectDistrict'] option:selected").text(),
                        wardCode: $("#selectWards").val(),
                        wardName: $("[id='selectWards'] option:selected").text(),
                        name: $("#ShippingFullName").val(),
                        address: $("#ShippingAddress").val(),
                        phoneNumber: $("#ShippingPhone").val(),
                    }),
                    contentType: "application/json; charset=utf-8;",
                    dataType: 'json',
                    success: function (result) {
                        if (result) {
                            swal("Thành công", "Lưu địa chỉ thành công", "success");
                            LoadDataTable();
                            ClearData();
                            DataAddressCustomer();
                            $('#formdata').hide();
                            $("#changeform").text("Thêm mới");
                        }
                        else {
                            swal("Không thành công", "Lưu địa chỉ thất bại, bạn cần kiểm tra lại thông tin", "error");
                        }
                    }
                })
            } else {
                swal("Đã hủy", "Bạn đã không lưu những thay đổi này!", "error");
            }
        });
    }

}

function DeleteDeliveryAddress(id) {

    swal("Bạn có muốn xóa thông tin địa chỉ này không?", {
        title: 'Thông Báo!',
        icon: 'info',
        buttons: ["Không, tôi cần xem lại", "Có, tôi đồng ý"],
    }).then(function (isConfirm) {
        if (isConfirm) {
            $.ajax({
                url: "/DeliveryAddress/Delete?id=" + id,
                type: "GET",
                success: function (result) {
                    if (result) {
                        swal("Thành công", "Xóa địa chỉ thành công", "success");
                        LoadDataTable();
                    }
                    else {
                        swal("Không thành công", "Xóa địa chỉ thất bại", "error");
                    }
                }
            })
        } else {
            swal("Đã hủy", "Bạn đã không lưu những thay đổi này!", "error");
        }
    });

}

function SearchProduct()
{
    var _key = $("#Keyword_Layout").val()
    $.ajax({
        url: "/Product/Index?Keyword=" + _key,
        type: "GET",
        success: function () {
            location.href = "/Product/Index"
        }
    })
}


function ClearData() {
    $("#txtId").val(0);
    $("#selectDistrict").val('');
    $("[id='selectDistrict'] option:selected").text('');
    $("#selectWards").val('');
    $("[id='selectWards'] option:selected").text('');
    $("#ShippingFullName").val('');
    $("#ShippingAddress").val('');
    $("#ShippingPhone").val('');
}


function LoadDataTable() {
    $.ajax({
        url: "/DeliveryAddress/GetDataDeliveryAddress",
        type: "GET",
        success: function (result) {
            var renderData = "";
            $.each(result, function (value) {
                renderData += '<tr>';
                renderData += '<td>' + result[value].name + ' | ' + result[value].phoneNumber + ', ' + result[value].address + ', ' + result[value].wardName + ', ' + result[value].districtName + ', ' + result[value].provinceName + '</td>';
                renderData += '<td><a class="btn btn-warning" onclick="ViewDetailData(' + result[value].id + ')" ><i class="fa-solid fa-pen-to-square"></i></a>' + ' | ' + '<a class="btn btn-danger" onclick="DeleteDeliveryAddress(' + result[value].id + ')"> <i class="fa-regular fa-trash-can"> </i></a></td>'
                renderData += '<tr>';
            });
            $("#tbody").html(renderData)
        }
    })
}


function LoadProvince() {
    $.ajax({
        url: "/DeliveryAddress/GetProvinces",
        type: "GET",
        success: function (result) {
            var renderData = "<option></option>";
            $.each(result, function (value) {
                renderData += '<option value="' + result[value].provinceID + '">' + result[value].provinceName + '</option>';
            });
            $("#selectProvince").html(renderData)
        }
    })
}


function ViewDetailData(id) {
    $.getJSON("/DeliveryAddress/Detail?id=" + id, function (data) {
        var renderData = "";
        var renderDataWard = "";

        $("#txtId").val(data.id);
        $("#selectProvince").val(data.provinceID);
        $("[id='selectProvince'] option:selected").text(data.provinceName);

        renderData += '<option value="' + data.districtID + '">' + data.districtName + '</option>';
        $("#selectDistrict").html(renderData)

        renderDataWard += '<option value="' + data.wardCode + '">' + data.wardName + '</option>';
        $("#selectWards").html(renderDataWard)

        $("#ShippingFullName").val(data.name);
        $("#ShippingAddress").val(data.address);
        $("#ShippingPhone").val(data.phoneNumber);


        $("#changeform").text("Thêm mới");
        ChangePanel()
    });
}

function LoadDistrict() {
    $("#selectProvince").change(function () {
        let _id = $("#selectProvince").val();
        $.ajax({
            url: "/DeliveryAddress/GetDistricts?id=" + _id,
            type: "GET",
            success: function (result) {
                var renderData = "<option></option>";
                $.each(result, function (value) {
                    renderData += '<option value="' + result[value].districtID + '">' + result[value].districtName + '</option>';
                });
                $("#selectDistrict").html(renderData)
            }
        })
    });
}

function LoadWard() {
    $("#selectDistrict").change(function () {
        let _id = $("#selectDistrict").val();
        $.ajax({
            url: "/DeliveryAddress/GetWards?id=" + _id,
            type: "GET",
            success: function (result) {
                var renderData = "<option></option>";
                $.each(result, function (value) {
                    renderData += '<option value="' + result[value].wardCode + '">' + result[value].wardName + '</option>';
                });
                $("#selectWards").html(renderData)
            }
        })
    });
}

function validatePhoneNumber(code) {
    var regex = /(84|0[3|5|7|8|9])+([0-9]{8})\b/;
    return regex.test(code);
}

function checkValidateVOucher() {
    if ($('#ShippingFullName').val() == '') {
        sweetAlert("Thông báo", "Thông tin tên người mua không được để trống", "error");
        return false;
    }
    if ($('#ShippingPhone').val() == '') {
        sweetAlert("Thông báo", "Thông tin số điện thoại không được để trống", "error");
        return false;
    }
    if ($('#selectProvince').val() == '') {
        sweetAlert("Thông báo", "Thông tin Tỉnh/Thành phố không được để trống", "error");
        return false;
    }
    if ($('#selectDistrict').val() == '') {
        sweetAlert("Thông báo", "Thông tin Quận/Huyện không được để trống", "error");
        return false;
    }
    if ($('#selectWards').val() == '') {
        sweetAlert("Thông báo", "Thông tin Phường/Xã không được để trống", "error");
        return false;
    }
    if ($('#ShippingAddress').val() == '') {
        sweetAlert("Thông báo", "Thông tin địa chỉ không được để trống", "error");
        return false;
    }    
    if (!validatePhoneNumber($('#ShippingPhone').val())) {
        sweetAlert("Thông báo", "Số điện thoại không đúng định dạng", "error");
        return false;
    }
    return true;
}

//function OrderAccomplishedAsync(id) {
//    $.ajax({
//        url: "/Order/OrderAccomplishedAsync?id=" + id,
//        type: "GET",
//        success: function (result) {
//            location.href = '/Product/ShowDetailMyCart'
//        }
//    })
//}