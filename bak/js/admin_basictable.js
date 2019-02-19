$(document).ready(function () {;
    welcomeorlogin();
    getRows(0,10);
});

function  welcomeorlogin() {
    $.ajax({
        type: "POST",
        url: "admin.php",
        data: {
            key:"check"
        }, success: function (response) {
            if(response == "locationlogin"){
                window.location = 'index.html';
            } else if (response == "locationwelcome") {
                window.location = 'welcome.html';
            }
        }
    });
}

function getRows(start,limit) {
    $.ajax({
        type: "POST",
        url: "admin.php",
        data: {key: "getall",
            start: start,
            limit: limit
        },  success: function (response) {
            if (response != "done") {
                $('tbody').append(response);
                start += limit;
                getRows(start, limit);
            }
        }
    });
}


function findpressed(num) {
    if (num === 1) {

        var fname = $('[name="firstname"]');
        var sname = $('[name="secondname"]');

        $.ajax({
            type: "POST",
            url: "admin.php",
            data: {
                key: "find",
                num: num,
                firstname: fname.val(),
                secondname: sname.val()
            }, success: function (response) {
                if (response != "missing") {
                    $('tbody').empty();
                    $('tbody').append(response);
                }
            }
        });

    } else if (num == 2) {
        var typef;
        var typet;
        var types;

        var check;
        check = $('[name="boxftype]');
        if (check.is(":checked")) {
            typef = 1;
        } else {
            typef = 0;
        }
        check = $('[name="boxstype"]');
        if (check.is(":checked")) {
            types = 1;
        } else {
            types = 0;
        }
        check = $('[name="boxttype"]');
        if (check.is(":checked")) {
            typet = 1;
        } else {
            typet = 0;
        }

        $.ajax({
            type: "POST",
            url: "admin.php",
            data: {
                key: "find",
                num: num,
                typef: typef,
                typet: typet,
                types: types
            }, success: function (response) {
                if (response != "missing") {
                    $('tbody').empty();
                    $('tbody').append(response);
                }
            }
        });

    } else if (num == 3) {
        var boxskladom;
        var check;
        check = $('[name="boxskladom"]');

        if (check.is(":checked")) {
            boxskladom = 1;
        } else {
            boxskladom = 0;
        }

        $.ajax({
            type: "POST",
            url: "admin.php",
            data: {
                key: "find",
                num: num,
                boxskladom: boxskladom
            }, success: function (response) {
                $('tbody').empty();
                $('tbody').append(response);
            }
        });

    } else if (num == 4) {
        var adate = $('[name="datearrival"]');
        var ldate = $('[name="dateleave"]');

        $.ajax({
            type: "POST",
            url: "admin.php",
            data: {
                key: "find",
                num: num,
                adate: adate.val(),
                ldate: ldate.val()
            }, success: function (response) {
                if (response != "missing") {
                    $('tbody').empty();
                    $('tbody').append(response);
                }
            }
        });
    }
}