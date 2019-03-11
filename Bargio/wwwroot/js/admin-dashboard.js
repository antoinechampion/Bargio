$(document).ready(function() {
    const admin = (function() {
        var admin = {
            derniersBucquages: function() {
                $("#table-derniers-bucquages").DataTable({
                    "ajax": "/api/admin/derniersbucquages",
                    "bDestroy": true,
                    "order": []
                });
            },

            modifierCompte: function() {
                console.log("go");
                $("#card-modif-utilisateur").hide();
                var table = $("#table-modifier-compte").DataTable({
                    "ajax": "/api/admin/modifiercompte",
                    "bDestroy": true,
                    "drawCallback": function() {
                        const api = this.api();
                        $("#total-utilisateurs").text(api.column(1, { filter: "applied" }).data().sum().toFixed(2));
                    }
                });
                $("#table-modifier-compte").on("click",
                    "tbody tr",
                    function() {
                        const userName = table.row(this).data()[0];
                        admin.recupererInfosCompte(userName);
                    });
            },

            bucquageManip: function() {
                var selectedUserData = null; // [username, bucquages, solde]
                var historique = [];
                var table = null;
                var currentRow = null;

                $("#manip-proms-panel").hide();
                $("#bucquage-patience").hide();

                $("#a-manip-proms-confirmer").on("click", function() {                  
                    $("#manip-proms-panel").show();
                    var proms = $("#manip-proms").val();
                    table = $("#table-manip-proms").DataTable({
                        "ajax": `/api/admin/chargerproms/${proms}`,
                        "bDestroy": true,
                        "select": true,
                        "order": []
                    });

                    $("#table-manip-proms").on("click",
                        "tbody tr",
                        function() {
                            currentRow = this;
                            selectedUserData = table.row(this).data();
                        });
                });

                $(document).on("keydown", function(e) {
                    if (!$("#manip").hasClass("active") || selectedUserData === null)
                        return;
                    if (e.keyCode === 13) {
                        selectedUserData[1] = "" + (parseInt(selectedUserData[1]) + 1);
                        table.row(currentRow).data(selectedUserData);
                        historique.push({
                            UserName: selectedUserData[0],
                            Montant: Math.round(parseFloat($("#manip-prix").val().replace(",", ".")) * 100) / 100,
                            Manip: $("#manip-nom").val()
                        });
                    } else if (e.keyCode === 8 && historique.length !== 0) {
                        var dernier = historique.pop();
                        var row = table
                            .rows(function(idx, data, node) {
                                return data[0] === dernier.UserName;
                            });
                        var newData = row.data()[0];
                        newData[1] = "" + (parseInt(newData[1]) - 1);
                        console.log(newData);
                        table.row(row).data(newData);
                    }
                });
                
                $("#a-manip-confirmer").on("click", function() {
                    $('#a-manip-confirmer').click(function(e) {
                        e.preventDefault();
                    });
                    $('#a-manip-confirmer').addClass("disabled");
                    $("#bucquage-patience").show();

                    const json = JSON.stringify({ "Historique": historique });
                    const fdata = new FormData();
                    fdata.append("json", json);
                    $.ajax({
                        type: "POST",
                        url: "/api/admin/bucquermanip",
                        cache: false,
                        data: fdata,
                        contentType: false,
                        processData: false,
                        success: function(response) {
                            $("#modal-succes").modal("show");
                        },
                        error: function(xhr, error) {
                            $("#modal-texte-erreur").text(`(${xhr.status}) ${xhr.responseText}`);
                            $("#modal-erreur").modal("show");
                        }
                    });
                });
            },

            recupererInfosCompte: function(userName) {
                $.ajax({
                    type: "GET",
                    url: `/api/admin/recupererinfocompte/${userName}`,
                    success: function(response) {
                        $("#card-modif-utilisateur").show();

                        $("#utilisateur-a-modifier").text(userName);
                        response = JSON.parse(response);
                        $("#mode-archi").prop("checked", response.ModeArchi);
                        $("#hors-foys").prop("checked", response.HorsFoys);
                        $("#prenom").val(response.Prenom);
                        $("#nom").val(response.Nom);
                        $("#surnom").val(response.Surnom);
                        $("#nums").val(response.Nums);
                        $("#tbk").val(response.TBK);
                        $("#proms").val(response.Proms);
                    },
                    error: function(xhr, error) {
                        $("#modal-texte-erreur").text(`(${xhr.status}) ${xhr.responseText}`);
                        $("#modal-erreur").modal("show");
                    }
                });

                $("#a-bucquage-manuel").click(function() {
                    const data = {
                        UserName: userName,
                        Montant: $("#bucquage-montant").val(),
                        Commentaire: $("#bucquage-raison").val()
                    };
                    const json = JSON.stringify(data);
                    const fdata = new FormData();
                    fdata.append("json", json);
                    $.ajax({
                        type: "POST",
                        url: "/api/admin/bucquagemanuel",
                        cache: false,
                        data: fdata,
                        contentType: false,
                        processData: false,
                        success: function(response) {
                            $("#modal-succes").modal("show");
                        },
                        error: function(xhr, error) {
                            $("#modal-texte-erreur").text(`(${xhr.status}) ${xhr.responseText}`);
                            $("#modal-erreur").modal("show");
                        }
                    });
                });

                $("#a-confirmer").click(function() {
                    const data = {
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
                    const json = JSON.stringify(data);
                    console.log(json);
                    const fdata = new FormData();
                    fdata.append("json", json);
                    $.ajax({
                        type: "POST",
                        url: "/api/admin/modifiercompte",
                        cache: false,
                        data: fdata,
                        contentType: false,
                        processData: false,
                        success: function(response) {
                            $("#modal-succes").modal("show");
                        },
                        error: function(xhr, error) {
                            $("#modal-texte-erreur").text(`(${xhr.status}) ${xhr.responseText}`);
                            $("#modal-erreur").modal("show");
                        }
                    });
                });

                $("#a-supprimer-mdp").click(function() {
                    const data = {
                        UserName: userName
                    };
                    const json = JSON.stringify(data);
                    console.log(json);
                    const fdata = new FormData();
                    fdata.append("json", json);
                    $.ajax({
                        type: "POST",
                        url: "/api/admin/supprimermdp",
                        cache: false,
                        data: fdata,
                        contentType: false,
                        processData: false,
                        success: function(response) {
                            $("#modal-succes").modal("show");
                        },
                        error: function(xhr, error) {
                            $("#modal-texte-erreur").text(`(${xhr.status}) ${xhr.responseText}`);
                            $("#modal-erreur").modal("show");
                        }
                    });
                });
            },

            historique: function() {
                $("#timepicker-debut").datetimepicker({
                    format: "DD-MM-YYYY HH:mm"
                });
                $("#timepicker-fin").datetimepicker({
                    format: "DD-MM-YYYY HH:mm"
                });

                $("#a-historique").click(function() {
                    const debut = $("#timepicker-debut").datetimepicker("viewDate").toISOString();
                    const fin = $("#timepicker-fin").datetimepicker("viewDate").toISOString();
                    $("#table-historique").DataTable({
                        "ajax": `/api/admin/historique/${debut}/${fin}`,
                        "bDestroy": true,
                        "drawCallback": function() {
                            const api = this.api();
                            $("#total-historique").text(api.column(3, { filter: "applied" }).data().sum().toFixed(2));
                        }
                    });
                });
            },

            modeArchiPost: function(exclusive, remove) {
                const data = {
                    proms: $("#liste-proms-mode-archi").val(),
                    exclusive: exclusive,
                    remove: remove
                };
                const json = JSON.stringify(data);
                const fdata = new FormData();
                fdata.append("json", json);
                $.ajax({
                    type: "POST",
                    url: "/api/admin/modearchi",
                    cache: false,
                    data: fdata,
                    contentType: false,
                    processData: false,
                    success: function(response) {
                        $("#modal-succes").modal("show");
                    },
                    error: function(xhr, error) {
                        $("#modal-texte-erreur").text(`(${xhr.status}) ${xhr.responseText}`);
                        $("#modal-erreur").modal("show");
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
                $.ajaxSetup({ cache: false });
                admin.derniersBucquages();
                admin.modifierCompte();
                admin.bucquageManip();
                admin.historique();
                admin.modeArchi();
                $("#modal-succes").on("hidden.bs.modal",
                    function(e) {
                        window.location.reload(true);
                    });
            }
        };
        return admin;
    })();

    admin.init();

});