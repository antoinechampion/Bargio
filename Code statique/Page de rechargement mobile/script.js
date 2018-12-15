function ShowHideDiv() {
    "use strict";
    if (document.getElementById('MDPoptionnel').checked) {
        document.getElementById('txtMDPopt').style.display = 'none';
        
    } else {
        document.getElementById('txtMDPopt').style.display = 'block';
        
    }
}