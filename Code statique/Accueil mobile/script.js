$(document).ready(function () {
	
	/* Set the width of the side navigation to 250px */
	function openNav() {
		document.getElementById("mySidenav").style.width = "250px";
	}

	/* Set the width of the side navigation to 0 */
	function closeNav() {
		document.getElementById("mySidenav").style.width = "0";
	}

	function ShowHideDiv() {
		"use strict";
		if (document.getElementById('MDPoptionnel').checked) {
			document.getElementById('txtMDPopt').style.display = 'none';
			
		} else {
			document.getElementById('txtMDPopt').style.display = 'block';
			
		}
	}

	new Chart(document.getElementById("doughnut-chart").getContext('2d')
	, {
		type: 'doughnut',
		data: {
			labels: ["Africa", "Asia", "Europe", "Latin America", "North America"],
			datasets: [
				{
					label: "Population (millions)",
					backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9" , "#c45850"],
					data: [2478, 5267, 734, 784, 433]
				}
			]
		},
		options: {
			title: {
				display: true,
				text: 'Predicted world population (millions) in 2050'
			}
		}
	});
});