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