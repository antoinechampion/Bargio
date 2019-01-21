$(document).ready(function () {
    var admin = (function () {
        var admin = {
            init: () => {
                var table = $("#table-derniers-bucquages").DataTable( {
                    ajax:  "/api/admin/derniersbucquages"
                });
                
                $.ajax({
                    type: 'GET',
                    url: '/api/admin/derniersbucquages',
                    success: function(response) {
                        console.log(response);
                    }
                });

            },
            ajaxDerniersBucquages: function() {

            }
        };
        return admin;
    })();

    admin.init();

});
