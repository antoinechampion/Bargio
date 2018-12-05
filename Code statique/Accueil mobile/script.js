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