$(document).ready(function () {
    var listeProduits = [];
    var listeConsos = [];

    for (var produit in compteurConsos) {
        if (compteurConsos.hasOwnProperty(produit)) {
            listeProduits.push(produit);
            listeConsos.push(compteurConsos[produit]);
        }
    }

    var colors = ["#246421", "#2f832c", "#3ba537", "#46c041", "#71ce6d", "#8ed88b", "#aae2a8", "#c7ecc5", "d6f1d4"];
    var ratio = Math.ceil(colors.length / listeProduits.length);
    var sampledColors = colors.filter(function(value, index, arr) {
        return index % ratio === 0;
    });

    if (listeProduits.length !== 0) {
        new Chart(document.getElementById("doughnut-chart").getContext('2d'),
            {
                type: 'doughnut',
                data: {
                    labels: listeProduits,
                    datasets: [
                        {
                            label: "Historique",
                            backgroundColor: sampledColors,
                            data: listeConsos
                        }
                    ]
                },

            });
    } else {
        $("#doughnut-chart-message").removeClass("d-none");
        $("#doughnut-chart").addClass("d-none");
    }
            
});