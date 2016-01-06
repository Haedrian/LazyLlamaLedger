
$(document).ready(function () {
    $('#txtDateFrom').pickadate(
    {
        selectMonths: true,
        selectYears: true,
        format: 2,
        format: 'yyyy-mm'
    });

    $('#txtDateTo').pickadate(
  {
      selectMonths: true,
      selectYears: true,
      format: 2,
      format: 'yyyy-mm',
  });

});


function getAndCreateExpenseTable() {
    //Do we have a date from and a date to?
    if ($("#txtDateFrom").val().length == 0 || $("#txtDateTo").val().length == 0) {
        Materialize.toast('Start Date and End Date are required', 2000);
        return;
    }

    $.get("http://localhost:7744/api/report/ExpenseTable?StartDate=" + $("#txtDateFrom").val() + "-01" + "&EndDate=" + $("#txtDateTo").val() + "-01", function (data) {
        var headerHtml = "<tr>";
        //Start with the headings

        headerHtml += "<td>Category</td>";

        for (var i = 0; i < data.Headers.length; i++) {
            headerHtml += "<td style='text-align:right'>" + data.Headers[i] + "</td>";
        }

        headerHtml += "</tr>";

        var bodyHtml = "";

        //Now do the rows
        for (var i = 0; i < data.Rows.length; i++) {
            bodyHtml += "<tr>";

            bodyHtml += "<td style='font-weight:bold'>" + data.Rows[i].CategoryName + "</td>";

            //And the values
            for (var j = 0; j < data.Rows[i].Values.length; j++) {
                bodyHtml += "<td style='text-align:right'>" + data.Rows[i].Values[j] + "</td>";
            }

            bodyHtml += "</td>";
        }

        //Done :) Let's create the table

        $("#tblExpenseReport thead").html(headerHtml);
        $("#tblExpenseReport tbody").html(bodyHtml);

        $("#divActualReport").css("display", "block");

    });

    $.get("http://localhost:7744/api/report/IncomeTable?StartDate=" + $("#txtDateFrom").val() + "-01" + "&EndDate=" + $("#txtDateTo").val() + "-01", function (data) {
        var headerHtml = "<tr>";
        //Start with the headings

        headerHtml += "<td>Category</td>";

        for (var i = 0; i < data.Headers.length; i++) {
            headerHtml += "<td style='text-align:right'>" + data.Headers[i] + "</td>";
        }

        headerHtml += "</tr>";

        var bodyHtml = "";

        //Now do the rows
        for (var i = 0; i < data.Rows.length; i++) {
            bodyHtml += "<tr>";

            bodyHtml += "<td style='font-weight:bold'>" + data.Rows[i].CategoryName + "</td>";

            //And the values
            for (var j = 0; j < data.Rows[i].Values.length; j++) {
                bodyHtml += "<td style='text-align:right'>" + data.Rows[i].Values[j] + "</td>";
            }

            bodyHtml += "</td>";
        }

        //Done :) Let's create the table

        $("#tblIncomeReport thead").html(headerHtml);
        $("#tblIncomeReport tbody").html(bodyHtml);

    });

    //And the bar chart

    $("#divReportBar").html("");

    $.get("http://localhost:7744/api/report/Totals?StartDate=" + $("#txtDateFrom").val() + "-01" + "&EndDate=" + $("#txtDateTo").val() + "-01", function (data) {
        //Load the bar chart
        //$.jqplot('divReportBar', [data.s2, data.s1],
        //    {
        //        grid: { background: '#FFFFFF', borderWidth: 0, shadow: 0 },
        //        animate: !$.jqplot.use_excanvas,
        //        seriesDefaults:
        //            {
        //                renderer: $.jqplot.BarRenderer,
        //                pointLabels: { show: true }
        //            },
        //        axes: {
        //            xaxis:
        //                {
        //                    renderer: $.jqplot.CategoryAxisRenderer,
        //                    ticks: data.ticks
        //                }
        //        },
        //        highlighter: { show: false }
        //    });

        $.jqplot('divReportBar', [data.s2,data.s1, data.s3], {
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: {
                    fillToZero: true
                },
                pointLabels: {
                    show: true,
                    stackedValue: true
                }
            },
            series: [{},
            {},
                     {
                         disableStack: true,//otherwise it wil be added to values of previous series
                         renderer: $.jqplot.LineRenderer,
                         lineWidth: 2,
                         pointLabels: {
                             show: false
                         },
                         markerOptions: {
                             size: 5
                         }
                     }],
            axesDefaults: {
                tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                tickOptions: {
                    angle: 30
                }
            },
            axes: {
                xaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    ticks:data.ticks
                },
                yaxis: {
                    autoscale: true
                }
            }
        });

    });
}