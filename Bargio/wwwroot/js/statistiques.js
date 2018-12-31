$(document).ready(function () {
    function refreshUi() {
        if ($("#radio-conso-soiree").is(':checked')) {
            $("#choix-date").show();
            $('#timepicker-conso').datetimepicker("destroy");
            $('#timepicker-conso').datetimepicker({
                format: 'DD/MM/YYYY',
                viewMode: "days"
            });
        } else if ($("#radio-conso-mois").is(':checked')) {
			$("#choix-date").show();
            $('#timepicker-conso').datetimepicker("destroy");
            $('#timepicker-conso').datetimepicker({
                format: 'MM/YYYY',
                viewMode: "months"
            });
		} else if ($("#radio-conso-annee").is(':checked')) {
			$("#choix-date").show();
            $('#timepicker-conso').datetimepicker("destroy");
            $('#timepicker-conso').datetimepicker({
                format: "YYYY",
                viewMode: "years"
            });
		} else if ($("#radio-conso-tout").is(':checked')) {
			$("#choix-date").hide();
            $("#table-consos-partiel").hide();
            $('#timepicker-conso').datetimepicker("destroy");
		}
    }
    $(".table-consos-partiel-datatable").DataTable({
        "scrollX": true,
        "searching": false,
        "lengthChange": false,
        "language": {
          "info": "_TOTAL_ bucquages au total",
          "paginate": {
            "previous": "<",
            "next": ">"
          }
        }
    });
    $(".table-consos-total-datatable").DataTable({
        "scrollX": true,
        "language": {
          "info": "_TOTAL_ bucquages au total",
          "paginate": {
            "previous": "<",
            "next": ">"
          },
          "sLengthMenu": "_MENU_ transactions par page",
          "sSearch": "Rechercher"
        }
    });
    $("#table-consos-partiel").hide();
    $("#choix-date").hide();
    $('#timepicker-conso').on("change.datetimepicker", function(e) {
        $("#table-consos-partiel").show();
    });
	$('input:radio').change(refreshUi);
    
    var ctx = document.getElementById("conso-chart");
    new Chart(ctx, {
        type: 'line',
        data: {
            datasets: [{ 
                data: [{
                    x: new Date("12/02/2018 05:35:00"),
                    y: 1.25
                }, {
                    x: new Date("12/03/2018 22:05:00"),
                    y: 1.60
                },  {
                    x: new Date("12/04/2018 22:05:00"),
                    y: 0
                },  {
                    x: new Date("12/05/2018 22:05:00"),
                    y: 0
                }, {
                    x: new Date("12/06/2018 22:05:02"),
                    y: 0.80
                }],
                borderColor: "forestgreen",
                fill: false
          }]
        },        
        options: {
            scales: {
                xAxes: [{
                    type: 'time',
                    time: {
                        unit: 'day'
                    }
                }],
                yAxes: [{
                    ticks: {
                        beginAtZero:true
                    },
                  scaleLabel: {
                    display: true,
                    labelString: 'Brousoufs dépensés (€)'
                  }
                }]
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
});