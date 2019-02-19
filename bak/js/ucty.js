$(document).ready(function () {
    dirWelcome();
});

function dirWelcome() {
    $.ajax({
        type: "POST",
        url: "ucty.php",
        data: {
            key: "check"
        }, success: function (response) {
            if (response == "locationwelcomeadmin") {
                window.location = 'welcomeAdmin.html';
            } else if (response == "locationwelcome") {
                window.location = 'welcome.html';
            }
        }
    })
}

function logIn() {

    var uname = $('[name="username"]');
    var pass = $('[name="password"]');

    $.ajax({
        type: "POST",
        url: "ucty.php",
        data: {
            key: "login",
            username: uname.val(),
            password: pass.val()
        }, success: function (response) {
            if (response == "locationwelcome") {
                window.location = 'welcome.html';
            } else if (response == "locationwelcomeadmin") {
                window.location = 'welcomeAdmin.html';
            } else if (response == "Zadaj username" || response == "Zle meno"){
                wrongSmthn(0);
            } else if (response == "Zadaj heslo" || response == "Zle heslo") {
                wrongSmthn(1);
            }
        }
    });
}

function  signIn() {
    var lname = $('[name="username"]');
    var pass = $('[name="password"]');
    var confpass = $('[name="confirm_password"]');

    $.ajax({
        type: "POST",
        url: "ucty.php",
        data: {
            key: "signin",
            username: lname.val(),
            password: pass.val(),
            confirm_password : confpass.val()
        }, success: function (response) {
            if (response == "done") {
            } else if (response == "locationlogin") {
                window.location = 'index.html';
            }  else if (response == "Zadaj username" || response == "Tvoj username uz existuje" ) {
                wrongSmthn(0);
            } else if (response == "Zadaj password" || response == "password musi mat aspon 6 znakov") {
                wrongSmthn(1);
            } else if (response == "Potvrd heslo") {
                wrongSmthn(2)
            } else if (response == "Hesla sa nerovnaju") {
                wrongSmthn(3);
            }
        }
    });
}

function wrongSmthn(key) {
    if (key === 0) {
        $("[name='username']").css('border', '1px solid red');
        $("[name='labeluzivatel']").text("Zle meno");
    } else if (key === 1) {
        $("[name='password']").css('border', '1px solid red');
        $("[name='labelheslo']").text("Zle heslo");
        $("[name='username']").css('border', '');
        $("[name='labeluzivatel']").text("Uzivatelske meno");
    } else if (key === 2) {
        $("[name='confirm_password']").css('border', '1px solid red');
        $("[name='username']").css('border', '');
        $("[name='labeluzivatel']").text("Uzivatelske meno");
        $("[name='password']").css('border', '');
        $("[name='labelheslo']").text("Heslo");
    } else if (key === 3) {
        $("[name='confirm_password']").css('border', '1px solid red');
        $("[name='labelconfheslo']").text("Hesla sa nerovnaju");
        $("[name='password']").css('border', '1px solid red');
        $("[name='labelheslo']").text("Hesla sa nerovnaju");
    }
}