$(document).ready(function () {
    var admin = (function () {
        var admin = {
            init: () => {
                this.derniersBucquages();
                this.modifierCompte();
            },

            derniersBucquages: function() {
                $("#table-derniers-bucquages").DataTable( {
                    "ajax":  "/api/admin/derniersbucquages"
                });
            },

            modifierCompte: function() {
                $("#table-modifier-compte").DataTable( {
                    "ajax":  "/api/admin/modifiercompte"
                });
                $('#table-modifier-compte').on('click', 'tbody tr', function() {
                    var userName = table.row(this).data().Bucque;
                    $("#utilisateur-a-modifier").text(userName);
                    this.recupererInfosCompte(userName);
                })
            },

            recupererInfosCompte: function(userName) {
                $.ajax({
                    type: 'GET',
                    url: '/api/admin/recupererinfocompte/' + userName,
                    success: function(response) {
                        response = JSON.parse(response);
                        $("#mode-archi").prop('checked', response.ModeArchi);
                        $("#hors-foys").prop('checked', response.HorsFoys);
                        $("#prenom").val(response.Prenom);
                        $("#nom").val(response.Nom);
                        $("#surnom").val(response.Surnom);
                        $("#tbk").val(response.TBK);
                        $("#proms").val(response.Proms);
                    }
                });
            }
        };
        return admin;
    })();

    admin.init();

});
