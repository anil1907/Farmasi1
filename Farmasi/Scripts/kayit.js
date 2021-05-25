var btnSaveClick = function () {
    $('#btnSave').on('click', function () {
        debugger;

        var name = $("#name").val().trim();
        var phone = $("#phone").val().trim();
        var tc = $("#tc").val().trim();
        var il = $("#il").val().trim();
        var ilce = $("#ilce").val().trim();
        var adress = $("#adress").val().trim();

        var gun = $("#days").val();
        var ay = $("#months").val();
        var yil = $("#years").val();

        if (gun == "" & ay == "" & yil == "")
            return swal("Başarısız", "Doğum tarihiniz boş geçilemez", "error");

        if (yil > 2000)
            return swal("Başarısız", "Doğum tarihiniz geçersiz", "error");

        var dTarih = gun + "/" + ay + "/" + yil;

        if (tc.length != 11) return swal("Başarısız", "TC Kimlik Numarası boş geçilemez", "error");

        if (name == "") return swal("Başarısız", "Adınız boş geçilemez", "error");
        if (phone == "") return swal("Başarısız", "Telefon numaranız boş geçilemez", "error");
        if (il == "") return swal("Başarısız", "İliniz boş geçilemez", "error");


        var jsonObject =
        {
            "name": name,
            "phone": phone,
            "tc": tc,
            "il": il,
            "ilce": ilce,
            "adress": adress,
            "dogumTarih" : dTarih
        };


        ajaxGet(jsonObject).done(function (data) {
            if (data == 1)
                return swal("Başarılı", "Kaydınız alınmıştır", "success")

            return swal("Başarısız", "Kayıt başarısız.", "error")
        });

    });
};

var renderCheckbx = function () {

    for (i = new Date().getFullYear(); i > 1900; i--) {
        $('#years').append($('<option />').val(i).html(i));
    }

    for (i = 1; i < 13; i++) {
        $('#months').append($('<option />').val(i).html(i));
    }

    for (i = 1; i < 32; i++) {
        $('#days').append($('<option />').val(i).html(i));
    }
}


function ajaxGet(data) {
    return $.ajax({
        url: "/Home/GetData?data=" + JSON.stringify(data),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        type: "GET"
    });
}


var init = function () {
    btnSaveClick();
    renderCheckbx();
};

$(document).ready(function () {
    init();
});