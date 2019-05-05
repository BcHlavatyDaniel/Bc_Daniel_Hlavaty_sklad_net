using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using System.Net;
using MySql.Data.MySqlClient;
using DatabaseProj;

namespace WebApplication3.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            return View("Table",context.GetPrehladTable());
        }

        private void ftp_download(string name)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("test", "test");
            client.DownloadFile("ftp://dokelu.kst.fri.uniza.sk/" + name, "wwwroot/resources/" + name);
        }

        [HttpPost]
        public ActionResult LoadImage(string datat)
        {
            int id = Convert.ToInt32(datat);
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            string name = context.GetImageName(id);
            string path = System.IO.Path.GetFullPath("wwwroot/resources/" + name);

            if (!System.IO.File.Exists(path))
            {
                if (!System.IO.Directory.Exists("wwwroot/resources"))
                {
                    System.IO.Directory.CreateDirectory("wwwroot/resources");
                }
                if (!(name == null) && !(name == ""))
                    ftp_download(name);
                else return new JsonResult("No image");
            }

            JsonResult jresult = new JsonResult("resources/" + name);
            return jresult;
        }
    }
    /*
     * 
     *         <div id="content">
            <table id="datatable" class="table table-hover table-bordered">
                <thead>
                    <tr>
                        <th>id zakaznik</th>
                        <th>Prve meno</th>
                        <th>Druhe meno</th>
                        <th>id tovar</th>
                        <th>Nazov</th>
                        <th>Cena</th>
                        <th>Velkost</th>
                        <th>stav</th>
                        <th>foto</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var item in Model)
                    {
                        <tr id="@("but"+item.id_item)">
                            <td>
                                @Html.DisplayFor(modelItem => item.f_name)
                            </td>
                            <td>
                                @Html.DisplayFor(modelItem => item.s_name)
                            </td>
                            <td>
                                @Html.DisplayFor(modelItem => item.item_name)
                            </td>
                            <td>
                                @Html.DisplayFor(modelItem => item.price)
                            </td>
                            <td>
                                @Html.DisplayFor(modelItem => item.size)
                            </td>
                            <td>
                                @Html.DisplayFor(modelItem => item.state)
                            </td>
                            <td>
                                <input type="button" id="@("in"+item.id_item)" onclick="openclose(@item.id_item)" value="Otvor" />
                            </td>
                        </tr>
                    }
                </tbody>
            </table>
     * 
     * 
     *<script>
    var table;
    $(document).ready(function () {
        table = $("#datatable").DataTable({
            info: false,
            paging: false,
            "sDom": 'ltipr',
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                switch (aData[7]) {
                    case 'Predane karta':
                        $(nRow).css('background-color', 'limeGreen')
                        break;
                    case 'Predane hotovost':
                        $(nRow).css('background-color', 'lightyellow')
                        break;
                    case 'Vratene':
                        $(nRow).css('background-color', 'lightgrey')
                        break;
                    case 'Zaplatene karta':
                        $(nRow).css('background-color', 'green')
                        break;
                    case 'Zaplatene hotovost':
                        $(nRow).css('background-color', 'yellow')
                        break;
                }
            }
        });
    });

    $("#filter").keyup(function () {
        table.search(this.value).draw();
    });    

    function format(path) {
        var image = "<img src='" + path + "'/>" //tu vytvori novy riadok v tabulke, z nacitanou fotkou
        return "<table style=margin-left:auto;margin-right:auto>" + "<tr>" +
            "<td>Foto:</td>" +
            "<td>" + image + "</td>" +
            "</tr>" +
            "</table>";
    }

    function noImage() {
        return "<table style=margin-left:auto;margin-right:auto>" + "<tr>" +
            "<td>Foto:</td>" +
            "<td>" + "bez obrazku" + "</td>" +
            "</tr>" +
            "</table>";
    }

    function openclose(num) {
        $.ajax({
            type: "POST",
            url: '@Url.Action("LoadImage","Table")', //tu vola prislusny kontroler
            dataType: 'json',
            data: {datat: num},
            success: function (response) {
                tr = $("#but" + num);
                row = table.row(tr);

                if (row.child.isShown()) {
                    $("#in" + num).val('Otvor');
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    $("#in" + num).val('Zatvor');
                    if (response == "No image") row.child(noImage).show();
                    else row.child(format(response)).show();
                    tr.addClass('shown');
                }
            },
        });
    }

</script>
     *
     */
}