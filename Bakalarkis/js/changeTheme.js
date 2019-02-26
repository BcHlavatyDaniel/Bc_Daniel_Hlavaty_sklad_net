$(document).ready(function () {
    theme = 1;
    changeTheme();
});

var theme;

function changeTheme() {
    if (theme === 0) {
        document.body.style.backgroundImage = "url('other/druhatema.jpg')";
        $("#changetable").attr('class','table table-dark table-hover');
        $("#changenavbar").attr('class','navbar fixed-top navbar-expand-md navbar-light bg-light');
        theme = 1;
    } else {
        document.body.style.backgroundImage = "url('other/lightblue.jpg')";
        $("#changetable").attr('class','table table-hover');
        $("#changenavbar").attr('class','navbar fixed-top navbar-expand-md navbar-dark bg-dark')
        theme = 0;
    }
}