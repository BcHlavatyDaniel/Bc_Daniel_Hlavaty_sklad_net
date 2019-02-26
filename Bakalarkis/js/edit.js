$(document).ready(function () {
    getAllRows(0,20);
});

var table;

function chceZmenit(variab) {
    if (variab == "Zmenit") {
        return 1;
    } else {
        return 0;
    }
}

function editrow(num) {

    var fname, sname, bskladom, type, stat, adate, ldate;


    if (chceZmenit($('#menochoose').val()) == 1) {
        fname = $('#firstname').val();
    }

    if (chceZmenit($('#priezviskochoose').val()) == 1) {
        sname = $('#secondname').val();
    }

    if (chceZmenit($('#typechoose').val()) == 1) {
        var check = $("input#boxftype");

        if(check.is(":checked")) {
            type = 1;
        }

        check = $("input#boxstype");
        if(check.is(":checked")) {
            type = 2;
        }

        check = $("input#boxttype");
        if(check.is(":checked")) {
            type = 3;
        }
    }

    if (chceZmenit($('#skladomchoose').val()) == 1) {

        var check = $("input#boxskladom");

        if (check.is(":checked")) {
            bskladom = 1;
        } else {
            bskladom = 0;
        }
    }

    if (chceZmenit($('#statchoose').val()) == 1) {
        stat = $('[name="statnum"]').val();
    }

    if (chceZmenit($('#adatechoose').val()) == 1) {
        adate = $('[name="datearrival"]').val();
    }

    if (chceZmenit($('#ldatechoose').val()) == 1) {
        ldate = $('[name="dateleave"]').val();
    }

    $.ajax({
        type: "POST",
        url: "admin.php",
        data: {fname: fname,
            sname: sname,
            type: type,
            stat: stat,
            bskladom: bskladom,
            adate: adate,
            ldate: ldate,
            number: num,
            key: "editvalues"},
        success: function (response) {
            if (response == "done") {
                window.location.reload(true); //TO DO, this is retarded ofc
            } else {
                window.location.reload(true);
            }
        }
    });
}

function getAllRows(start, limit) {
    $.ajax({
        type: "POST",
        url: "admin.php",
        data: {
            key: "getallchange",
            start: start,
            limit: limit
        }, success: function (response) {
            if (response != "done") {
                $('tbody').append(response);
                start += limit;
                getAllRows(start, limit);
            } else {
                table = $(".table").DataTable({
                    "bFilter": false,
                    "paging": false,
                    "sDom": 't'
                });
            }

        }
    });
}