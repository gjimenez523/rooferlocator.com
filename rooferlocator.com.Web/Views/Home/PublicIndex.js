(function () {
    $(function () {
        var d2 = [];
        var d3 = [];
        var dataset = [];
        var options = {};
        var previousPoint = null, previousLabel = null;
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var m_names = new Array("Jan", "Feb", "Mar",
            "Apr", "May", "Jun", "Jul", "Aug", "Sep",
            "Oct", "Nov", "Dec")


        abp.ui.setBusy(abp.ajax({
            url: abp.appPath + 'Home/Reports',
            type: 'POST'
        }).done(function (data) {
            for (var count = 0; count < data.MemberVisits.length; count++) {
                d2.push([new Date(data.MemberVisits[count].VisitYear, data.MemberVisits[count].VisitMonth, data.MemberVisits[count].VisitDay), data.MemberVisits[count].ReturnVisits]);
                d3.push([new Date(data.MemberVisits[count].VisitYear, data.MemberVisits[count].VisitMonth, data.MemberVisits[count].VisitDay), data.MemberVisits[count].NewVisits]);
            }

            dataset = [
                {
                    label: "New Visits",
                    data: d2,
                    color: "#3d1617",
                    points: { fillColor: "#3d1617", show: true },
                    lines: { show: true }
                },
                {
                    label: "Return Visits",
                    data: d3,
                    color: "#578ebe",
                    points: { fillColor: "#578ebe", show: true },
                    lines: { show: true }
                }
            ];

            options = {
                series: {
                    shadowSize: 5,
                    border: "#ffffff"
                },
                xaxes: [
                {
                    mode: "time",
                    timeformat: "%d",
                    tickFormatter: function (val, axis) {
                        var result = new Date(val);
                        //return m_names[result.getDate()+1];
                        return m_names[result.getMonth() - 1] + ' ' + (result.getDate() + 1);
                    },
                    tickSize: [1, "day"],
                    color: "gray",
                    axisLabel: "Day",
                    axisLabelUseCanvas: false,
                    axisLabelFontSizePixels: 12,
                    axisLabelFontFamily: 'Verdana, Arial',
                    axisLabelPadding: 5
                }],
                yaxis: {
                    color: "gray",
                    tickDecimals: 2,
                    axisLabel: "Visits",
                    axisLabelUseCanvas: true,
                    axisLabelFontSizePixels: 12,
                    axisLabelFontFamily: 'Verdana, Arial',
                    axisLabelPadding: 5
                },
                legend: {
                    noColumns: 0,
                    labelFormatter: function (label, series) {
                        return "<font color=\"black\">" + label + "</font>";
                    },
                    backgroundColor: "#ffffff",
                    backgroundOpacity: 0.9,
                    labelBoxBorderColor: "#ffffff",
                    position: "nw"
                },
                grid: {
                    hoverable: true,
                    borderWidth: 1,
                    mouseActiveRadius: 50,
                    border: "#fffff0"
                }

            };


            $.fn.UseTooltip = function () {
                $(this).bind("plothover", function (event, pos, item) {
                    if (item) {
                        if ((previousLabel != item.series.label) || (previousPoint != item.dataIndex)) {
                            previousPoint = item.dataIndex;
                            previousLabel = item.series.label;
                            $("#tooltip").remove();

                            var x = item.datapoint[0];
                            var y = item.datapoint[1];
                            var date = new Date(x);
                            var color = item.series.color;

                            showTooltip(item.pageX, item.pageY, color,
                                        "<strong>" + item.series.label + "</strong><br>" +
                                        (m_names[date.getMonth() - 1]) + " " + date.getDate() +
                                        " : <strong>" + y + "</strong> (visits)");
                        }
                    } else {
                        $("#tooltip").remove();
                        previousPoint = null;
                    }
                });
            };

            function showTooltip(x, y, color, contents) {
                $('<div id="tooltip">' + contents + '</div>').css({
                    position: 'absolute',
                    display: 'none',
                    top: y - 40,
                    left: x - 120,
                    border: '2px solid ' + color,
                    padding: '3px',
                    'font-size': '9px',
                    'border-radius': '5px',
                    'background-color': '#fff',
                    'font-family': 'Verdana, Arial, Helvetica, Tahoma, sans-serif',
                    opacity: 0.9
                }).appendTo("body").fadeIn(200);
            }

            $.plot("#chart_2", dataset, options);
            $("#chart_2").UseTooltip();

        })
            );

        //var d2 = [[new Date(2016, 3, 11), 100], [new Date(2016, 3, 12), 80], [new Date(2016, 3, 13), 55], [new Date(2016, 3, 14), 90]];

        // A null signifies separate line segments
        //var d3 = [[new Date(2016, 3, 11), 12], [new Date(2016, 3, 12), 12], [new Date(2016, 3, 13), 10], [new Date(2016, 3, 14), 5]];




        //$.plot("#placeholder", [d1, d2, d3]);

        // Add the Flot version string to the footer
        //$("#footer").prepend("Flot " + $.plot.version + " &ndash; ");
    });
})();