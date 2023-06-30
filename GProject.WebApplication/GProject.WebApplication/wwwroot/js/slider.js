function viewInstruction() {
    $('.instruc').slick('slickGoTo', 0);
    $('#instruction').modal('show');
    $('.slick-next').click()
}

function caiKySo() {
    $('.sweet-alert').css('z-index', 99998);
    $('#caidatKySo').css('z-index', 99999);
    $('.installsw').slick('slickGoTo', 0);
    $('#caidatKySo').modal('show');
    $('.slick-next').click()
}

function caiTienIchSo() {
    $('.sweet-alert').css('z-index', 99998);
    $('#caidatTienIch').css('z-index', 99999);
    $('.installuf').slick('slickGoTo', 0);
    $('#caidatTienIch').modal('show');
    $('.slick-next').click()
}

function Alert(txtalert) {
    swal({
        type: "error",
        title: "Thông báo",
        text: txtalert,
        html: true
    })
}

function checkExtensitonAddOn() {
    var content = $('#extension-cert').val();
    if (content === '') {
        let txtalert = `Vui lòng thực hiện cài đặt<a onclick='caiTienIchSo()'href='javascript:void(0)'>Tiệních kýsố</a></br><span style='font-style:italic;font-size:13px'>Lưu ý:Máy tính phải được cắm chứng thưsốtrước khi phát hành hóađơn!</span>`;
        Alert(txtalert);
        return false
    } else return true
}
$(function () {
    $('#instruction').on('hidden.bs.modal', function (e) {
        $("body").addClass("modal-open")
    });
    $('[data-toggle="tooltip"]').tooltip();
    $(".instruc").slick({
        dots: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        initialSlide: -1
    });
    $(".instruc").on("afterChange", function (event, slick, currentSlide) {
        $("#instruc_print").text(parseInt(currentSlide + 1) + '/' + slick.slideCount)
    });
    $(".instruc_install").slick({
        dots: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        initialSlide: -1
    });
    $(".installsw").on("afterChange", function (event, slick, currentSlide) {
        $("#sw-counter").text(parseInt(currentSlide + 1) + '/' + slick.slideCount)
    });
    $(".installuf").on("afterChange", function (event, slick, currentSlide) {
        $("#uf-counter").text(parseInt(currentSlide + 1) + '/' + slick.slideCount)
    })
});

function popupNotifyPublish(result, message) {
    if (result === 'user_cancelled') Alert("Đã hủy ký hóa đơn");
    else if (result === 'no_implementation' || message === 'no_implementation') {
        let txtalert = "Vui lòng thực hiện cài đặt<a href='javascript:void(0)'onclick='caiKySo()'> Phần mềm ký số</a> và <a onclick='caiTienIchSo()' href='javascript:void(0)'>Tiện ích ký số</a></br><span style='font-style:italic;font-size:13px'>Lưu ý: Máy tính phải được cắm chứng thư số trước khi phát hành hóa đơn!<br/>Hệ thống chỉ hỗ trợ trên trình duyệt Chrome và Cốc Cốc</span>";
        Alert(txtalert)
    } else Alert("Có lỗi trong quá trình xử lý, vui lòng thực hiện lại")
}

function popupNotifySign(result, message) {
    if (result === 'user_cancelled') showDialog("Đã hủy ký hóa đơn");
    else if (result === 'no_implementation' || message === 'no_implementation') {
        let txtalert = "Vui lòng thực hiện cài đặt<a href='javascript:void(0)'onclick='caiKySo()'> Phần mềm ký số</a> và <a onclick='caiTienIchSo()' href='javascript:void(0)'>Tiện ích ký số</a></br><span style='font-style:italic;font-size:13px'>Lưu ý: Máy tính phải được cắm chứng thư số trước khi phát hành hóa đơn!<br/>Hệ thống chỉ hỗ trợ trên trình duyệt Chrome và Cốc Cốc</span>";
        showDialog(txtalert)
    } else showDialog("Có lỗi trong quá trình xử lý, vui lòng thực hiện lại.")
}

function showDialog(message) {
    var millisecondsToWait = 100;
    setTimeout(function () {
        swal({
            type: "error",
            title: "Thông báo",
            text: message,
            html: true
        })
    }, millisecondsToWait)
}