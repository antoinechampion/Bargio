$(document).ready(function () {
    new Chart(document.getElementById("doughnut-chart").getContext('2d')
        , {
            type: 'doughnut',
            data: {
                labels: ["Bières", "Zabulles", "Biffuts", "Pain choc'ss"],
                datasets: [
                    {
                        label: "Historique",
                        backgroundColor: ["#234d20", "#36802d", "#77ab59", "#c9df8a"],
                        data: [2478, 5267, 734, 784]
                    }
                ]
            },

        });
});