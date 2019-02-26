$(document).ready(function () {
    getAllRows(0,20);
});

function getAllRows(start, limit) {
    $.ajax({
        type:"POST",
        url:"admin.php",
        data:{key:"getallstat",
            start: start,
            limit: limit},
        success: function (response) {
            if(response != "done") {
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