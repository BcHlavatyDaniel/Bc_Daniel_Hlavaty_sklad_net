$(document).ready(function () {
    dirWelcome();
});

function  dirWelcome() {
    $.ajax({
        type: "POST",
        url: "ucty.php",
        data: {
            key: "checklogout"
        }, success: function (response) {
            window.location = 'index.html';
        }
    })
}