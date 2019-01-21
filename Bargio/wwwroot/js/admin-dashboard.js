$(document).ready(function () {
    var admin = (function () {
        var admin = {
            derniersBucquages: function() {
                $("#table-derniers-bucquages").DataTable( {
                    "ajax":  "/api/admin/derniersbucquages"
                });
            },

            modifierCompte: function() {
                $("#card-modif-utilisateur").hide();
                var table = $("#table-modifier-compte").DataTable( {
                    "ajax":  "/api/admin/modifiercompte"
                });
                $('#table-modifier-compte').on('click',
                    'tbody tr',
                    function() {
                        var userName = table.row(this).data()[0];
                        admin.recupererInfosCompte(userName);
                    });
            },

            recupererInfosCompte: function(userName) {
                $.ajax({
                    type: 'GET',
                    url: '/api/admin/recupererinfocompte/' + userName,
                    success: function(response) {
                        $("#card-modif-utilisateur").show();

                        $("#utilisateur-a-modifier").text(userName);
                        response = JSON.parse(response);
                        $("#mode-archi").prop('checked', response.ModeArchi);
                        $("#hors-foys").prop('checked', response.HorsFoys);
                        $("#prenom").val(response.Prenom);
                        $("#nom").val(response.Nom);
                        $("#surnom").val(response.Surnom);
                        $("#nums").val(response.Nums);
                        $("#tbk").val(response.TBK);
                        $("#proms").val(response.Proms);
                    },
                    error: function(xhr, error) {
                        $("#modal-texte-erreur").text("(" + xhr.status + ") " + xhr.responseText);
                        $("#modal-erreur").modal('show');
                    }
                });

                $("#a-bucquage-manuel").click(function() {
                    var data = {
                        UserName: userName,
                        Montant: $("#bucquage-montant").val(),
                        Commentaire: $("#bucquage-raison").val()
                    };
                    var json = JSON.stringify(data);
                    var fdata = new FormData();
                    fdata.append("json", json);
                    $.ajax({
                        type: 'POST',
                        url: '/api/admin/bucquagemanuel',
                        cache: false,
                        data: fdata,
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            $("#modal-succes").modal('show');
                        },
                        error: function(xhr, error) {
                            $("#modal-texte-erreur").text("(" + xhr.status + ") " + xhr.responseText);
                            $("#modal-erreur").modal('show');
                        }
                    });
                });

                $("#a-confirmer").click(function() {
                    console.log("go");
                    var data = {
                        UserName: userName,
                        Prenom: $("#prenom").val(),
                        Nom: $("#nom").val(),
                        Surnom: $("#surnom").val(),
                        Nums: $("#nums").val(),
                        TBK: $("#tbk").val(),
                        Proms: $("#proms").val(),
                        ModeArchi: $("#mode-archi").is(":checked") || false,
                        HorsFoys: $("#hors-foys").is(":checked") || false
                    };
                    var json = JSON.stringify(data);
                    console.log(json);
                    var fdata = new FormData();
                    fdata.append("json", json);
                    $.ajax({
                        type: 'POST',
                        url: '/api/admin/modifiercompte',
                        cache: false,
                        data: fdata,
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            $("#modal-succes").modal('show');
                        },
                        error: function(xhr, error) {
                            $("#modal-texte-erreur").text("(" + xhr.status + ") " + xhr.responseText);
                            $("#modal-erreur").modal('show');
                        }
                    });
                });

                $("#a-supprimer-mdp").click(function() {
                    var data = {
                        UserName: userName
                    };
                    var json = JSON.stringify(data);
                    console.log(json);
                    var fdata = new FormData();
                    fdata.append("json", json);
                    $.ajax({
                        type: 'POST',
                        url: '/api/admin/supprimermdp',
                        cache: false,
                        data: fdata,
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            $("#modal-succes").modal('show');
                        },
                        error: function(xhr, error) {
                            $("#modal-texte-erreur").text("(" + xhr.status + ") " + xhr.responseText);
                            $("#modal-erreur").modal('show');
                        }
                    });
                });
            },

            historique: function () {
                $('#timepicker-debut').datetimepicker({
                    format: 'DD-MM-YYYY HH:mm'
                });
                $('#timepicker-fin').datetimepicker({
                    format: 'DD-MM-YYYY HH:mm'
                });

                $("#a-historique").click(function() {
                    var debut = $("#timepicker-debut").datetimepicker("viewDate").toISOString();
                    var fin = $("#timepicker-fin").datetimepicker("viewDate").toISOString();
                    $("#table-historique").DataTable( {
                        "ajax":  "/api/admin/historique/" + debut + "/" + fin,
                        "bDestroy": true
                    });
                });
            },

            modeArchiPost: function(exclusive, remove) {
                var data = {
                    proms: $("#liste-proms-mode-archi").val(),
                    exclusive: exclusive,
                    remove: remove
                };
                var json = JSON.stringify(data);
                var fdata = new FormData();
                fdata.append("json", json);
                $.ajax({
                    type: 'POST',
                    url: '/api/admin/modearchi',
                    cache: false,
                    data: fdata,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        $("#modal-succes").modal('show');
                    },
                    error: function(xhr, error) {
                        $("#modal-texte-erreur").text("(" + xhr.status + ") " + xhr.responseText);
                        $("#modal-erreur").modal('show');
                    }
                });
            },

            modeArchi: function() {
                $("#a-mode-archi").click(function() {
                    admin.modeArchiPost(false, false);
                });
                $("#a-mode-archi-exclusif").click(function() {
                    admin.modeArchiPost(true, false);
                });
                $("#a-mode-archi-remove").click(function() {
                    admin.modeArchiPost(false, true);
                });
                $("#a-mode-archi-remove-exclusif").click(function() {
                    admin.modeArchiPost(true, true);
                });
            },

            init: () => {
                admin.derniersBucquages();
                admin.modifierCompte();
                admin.historique();
                admin.modeArchi();
                $('#modal-succes').on('hidden.bs.modal',
                    function(e) {
                        window.location.reload(true);
                    });
            }
        };
        return admin;
    })();

    admin.init();

});
