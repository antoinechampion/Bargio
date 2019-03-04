$(document).ready(function() {
    var chart = null;

    // unité: days, hours
    function updateChart(listeTransactions, unite) {
        const data = [];

        if (listeTransactions.length === 0) {
            chart.data.datasets[0].data = [];
            chart.update();
            return;
        }

        var next = moment(listeTransactions[0][0]).add(1, unite);
        if (unite === "days")
            next.set("hour", 0);
        next.set("minute", 0);

        var montant = 0;
        for (let i = 0; i < listeTransactions.length; i++) {
            const bucquage = listeTransactions[i];
            if (bucquage[2] < 0)
                continue;
            if (moment(bucquage[0]).isAfter(next)) {
                data.push({ x: moment(next).subtract(1, unite), y: -montant });
                next = moment(bucquage[0]).add(1, unite);
                if (unite === "days")
                    next.set("hour", 0);
                next.set("minute", 0);
                montant = bucquage[2];
            } else {
                montant += bucquage[2];
            }
        }
        data.push({ x: moment(next).subtract(1, unite), y: -montant });
        chart.data.datasets[0].data = data;
        chart.update();
    }

    function updateMontant(data) {
        var montant = 0;
        for (let i = 0; i < data.length; i++) {
            if (data[i][2] < 0) {
                montant -= data[i][2];
            }
        }
        $("#montant-total").text(montant.toFixed(2).replace(".", ","));
    }

    function refreshUi() {
        if ($("#radio-conso-soiree").is(":checked")) {
            $("#choix-date").show();
            $("#timepicker-conso").datetimepicker("destroy");
            $("#timepicker-conso").datetimepicker({
                format: "DD/MMM/YYYY",
                viewMode: "days"
            });
        } else if ($("#radio-conso-mois").is(":checked")) {
            $("#choix-date").show();
            $("#timepicker-conso").datetimepicker("destroy");
            $("#timepicker-conso").datetimepicker({
                format: "MM/YYYY",
                viewMode: "months"
            });
        } else if ($("#radio-conso-annee").is(":checked")) {
            $("#choix-date").show();
            $("#timepicker-conso").datetimepicker("destroy");
            $("#timepicker-conso").datetimepicker({
                format: "YYYY",
                viewMode: "years"
            });
        } else if ($("#radio-conso-tout").is(":checked")) {
            $("#choix-date").hide();
            $("#timepicker-conso").datetimepicker("destroy");
            const table = $(".table-consos-total-datatable").DataTable();
            table.clear();
            table.rows.add(transactions);
            table.draw();
            updateChart(transactions, "month");
            updateMontant(transactions);
        }
    }

    // Initialisation des tables et du graph
    (() => {
        var table = $(".table-consos-total-datatable").DataTable({
            "scrollX": true,
            "language": {
                "info": "_TOTAL_ bucquages au total",
                "paginate": {
                    "previous": "<",
                    "next": ">"
                },
                "sLengthMenu": "_MENU_ transactions par page",
                "sSearch": "Rechercher"
            },
            "columnDefs": [
                {
                    "targets": 0,
                    "render": $.fn.dataTable.render.moment("DD/MM/YYYY HH:mm")
                }
            ]
        });
        table.clear();
        $("#choix-date").hide();

        var ctx = document.getElementById("conso-chart");
        moment.locale("fr");
        chart = new Chart(ctx,
            {
                type: "line",
                data: {
                    datasets: [
                        {
                            data: [],
                            borderColor: "forestgreen",
                            fill: "origin"
                        }
                    ]
                },
                options: {
                    scales: {
                        xAxes: [
                            {
                                type: "time",
                                time: {
                                    displayFormats: {
                                        'millisecond': "DD/MM/YYYY HH:mm",
                                        'second': "HH:mm",
                                        'minute': "HH:mm",
                                        'hour': "HH:mm",
                                        'day': "DD/MM",
                                        'week': "DD/MM",
                                        'month': "MM/YYYY",
                                        'quarter': "MM/YYYY",
                                        'year': "MM/YYYY"
                                    }
                                }
                            }
                        ],
                        yAxes: [
                            {
                                ticks: {
                                    beginAtZero: true
                                },
                                scaleLabel: {
                                    display: true,
                                    labelString: "Broüsoüfs dépensés (€)"
                                }
                            }
                        ]
                    },
                    elements: {
                        line: {
                            tension: 0 // disables bezier curves
                        }
                    },
                    legend: {
                        display: false
                    }
                }
            });
    })();

    // On rentre les données dans l'historique complet
    (() => {
        transactions = $.map(transactions,
            function(val, i) {
                return [[moment(val.date, "DD/MM/YYYY HH:mm").toDate(), val.commentaire, val.montant]];
            }
        );

        var table = $(".table-consos-total-datatable").DataTable();
        table.clear();
        table.rows.add(transactions);
        table.draw();
    })();

    // Gestion des event (plage de temps consommation)
    (() => {
        $("#timepicker-conso").on("change.datetimepicker",
            function(e) {
                var chosenDate = $("#timepicker-conso").datetimepicker("viewDate");

                var timescale = "years";
                if ($("#radio-conso-mois").is(":checked")) {
                    timescale = "months";
                } else if ($("#radio-conso-soiree").is(":checked")) {
                    timescale = "days";
                    chosenDate.set("hour", 15);
                }

                const data = $.grep(transactions,
                    function(val) {
                        return chosenDate.isBefore(val[0]) && moment(chosenDate).add(1, timescale).isAfter(val[0]);
                    }
                );

                const table = $(".table-consos-total-datatable").DataTable();
                table.clear();
                table.rows.add(data);
                table.draw();

                var unite = "month";
                if (timescale === "months") {
                    unite = "day";
                } else if (timescale === "days") {
                    unite = "hour";
                }
                updateChart(data, unite);

                updateMontant(data);
            });

        $("input:radio").change(refreshUi);
        refreshUi();
    })();

});