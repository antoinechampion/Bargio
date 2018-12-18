/// <reference path="./babasse.js" />

$(document).ready(function () {
    $('#timepicker-hors-babasse-quotidienne').datetimepicker({
        format: 'HH:mm',
        defaultDate: new Date('1900-01-01T00:00:00')
    });
    $('#timepicker-hors-babasse-hebdomadaire').datetimepicker({
        format: 'HH:mm',
        defaultDate: new Date('1900-01-01T00:00:00')
    });
	
    function uiGetAjax() {
        $.ajax({
            type: 'GET',
            url: '/Api/Foys/zifoysparams',
            cache: false,
            success: function (response) {
                if (response === null) {
                    console.log("Impossible de récupérer les paramètres zifoy'ss");
                }
                zifoysParams = JSON.parse(response);
                
                $("#checkbox-hors-babasse-auto").prop('checked', zifoysParams.MiseHorsBabasseAutoActivee);
                $("#input-seuil-hors-babasse-auto").val(zifoysParams.MiseHorsBabasseSeuil.toFixed(2));
                $("#radio-instantanee").prop('checked', zifoysParams.MiseHorsBabasseInstantanee);
                $("#radio-periodique").prop('checked', !zifoysParams.MiseHorsBabasseInstantanee);
                $("#radio-quotidienne").prop('checked', zifoysParams.MiseHorsBabasseQuotidienne);
                $("#radio-hebdomadaire").prop('checked', !zifoysParams.MiseHorsBabasseQuotidienne);
                $("#timepicker-hors-babasse-quotidienne").datetimepicker("date", zifoysParams.MiseHorsBabasseQuotidienneHeure);

                if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("lundi"))
                    $("#checkbox-hors-babasse-lundi").button("toggle");
                if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("mardi"))
                    $("#checkbox-hors-babasse-mardi").button("toggle");
                if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("mercredi"))
                    $("#checkbox-hors-babasse-mercredi").button("toggle");
                if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("jeudi"))
                    $("#checkbox-hors-babasse-jeudi").button("toggle");
                if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("vendredi"))
                    $("#checkbox-hors-babasse-vendredi").button("toggle");
                if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("samedi"))
                    $("#checkbox-hors-babasse-samedi").button("toggle");
                if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("dimanche"))
                    $("#checkbox-hors-babasse-dimanche").button("toggle");

                refreshUi();
            },
            error: function(xhr, error) {
                console.log("Impossible de récupérer les paramètres zifoy'ss");
			},
			timeout: 30000
		});
    }
    function uiPostAjax() {
        var fdata = new FormData();
        var json = JSON.stringify(zifoysParams);
        fdata.append("json", json);
        $.ajax({
            type: 'POST',
            url: '/Api/Foys/zifoysparams',
            cache: false,
            data: fdata,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response === null) {
                    console.log("Impossible de récupérer les paramètres zifoy'ss");
                }
                zifoysParams = JSON.parse(response);
            },
            error: function(xhr, error) {
                console.log("Impossible de récupérer les paramètres zifoy'ss");
            },
            timeout: 30000
        });
    }

    function clearUi() {
        $("#input-mdp-zifoys").val("");
        $("#input-mdp-zifoys-confirmer").val("");
        $("#input-username-rechargement-manuel").val("");
        $("#input-montant-rechargement-manuel").val("0.00");
        $("#input-commentaire-rechargement-manuel").val("bucquage manuel par zifoy'ss");
        $("#validation-rechargement-manuel").text("");
    }

    function refreshUi() {
        if ($("#radio-periodique").is(':checked')) {
            $("#hors-babasse-periodique").show();
			
            $("#timepicker-hors-babasse-quotidienne").hide();
            $("#jours-hors-babasse-hebdomadaire").hide();
            $("#timepicker-hors-babasse-hebdomadaire").hide();
			
            if ($("#radio-quotidienne").is(':checked')) {
                $("#timepicker-hors-babasse-quotidienne").show();
            } else if ($("#radio-hebdomadaire").is(':checked')) {
                $("#jours-hors-babasse-hebdomadaire").show();
                $("#timepicker-hors-babasse-hebdomadaire").show();
            }
        } else {
            $("#hors-babasse-periodique").hide();
        }
    }
    
    clearUi();
    uiGetAjax();
    refreshUi();

    // On relie chaque input à son bouton
    (() => {
        $("input-username-rechargement-manuel,#input-montant-rechargement-manuel,#input-commentaire-rechargement-manuel")
            .each(function() {
                $(this).keypress(function(e) {
                    if (e.keyCode === 13)
                        $("#button-rechargement-manuel").click();
                });
            });
    
        $('#input-seuil-hors-babasse-manuelle').keypress(function(e){
            if(e.keyCode===13)
                $('#button-hors-babasse-manuelle').click();
        });

        $('#input-remettre-en-babasse').keypress(function(e){
            if(e.keyCode===13)
                $('#button-remettre-en-babasse').click();
        });

        $('#input-mdp-zifoys-confirmer').keypress(function(e){
            if(e.keyCode===13)
                $('#button-mdp-zifoys').click();
        });

        $('#input-username-reset-mdp').keypress(function(e){
            if(e.keyCode===13)
                $('#button-reset-mdp').click();
        });
        
    })();

    // Gestion des events
    (() => {
        // On rafraichit l'interface quand un bouton radio a été cliqué
        $('input:radio').change(refreshUi);

        var imageHorsBabassePath = ""; 
        // On met la nouvelle image hors babasse en preview
        $("#input-file-hors-babasse").change(function(e){
            imageHorsBabassePath = URL.createObjectURL(event.target.files[0]);
            $(".bloc-case-rouge-test").css('background-image', "url(" + imageHorsBabassePath + ")");
        });

        // Rechargement manuel
        $("#button-rechargement-manuel").click(async function() {
            var userName = $("#input-username-rechargement-manuel").val();
            var montant = parseFloat($("#input-montant-rechargement-manuel").val());
            var commentaire = $("#input-commentaire-rechargement-manuel").val();

            var user = await db.UserData.get({ UserName: userName });
            // Si l'utilisateur n'existe pas
            if (typeof user === "undefined") {
                $("#validation-rechargement-manuel").text("L'utilisateur " + userName + " n'existe pas.");
                return;
            }

            var transaction = {
                UserName: userName,
                Date: dateTimeNow(),
                Montant: montant,
                Commentaire: commentaire
            };

            db.UserData.get({ UserName: transaction.UserName },
                user => {
                    db.UserData.update(transaction.UserName,
                        { Solde: Math.round((user.Solde + transaction.Montant)*100)/100 });
            }).then(function() {
                db.HistoriqueTransactions.add(transaction).then(clearUi);
                $("#modal-zifoys-validation").modal('show');
            });
        });

        // Hors babasse manuelle
        $("#button-hors-babasse-manuelle").click(function() {
            seuilHorsBabasse = parseFloat($("#input-seuil-hors-babasse-manuelle").val().replace(",", "."));
            if (seuilHorsBabasse === undefined) {
                return;
            }
            db.transaction('rw', db.UserData, db.HorsBabasse, () => {
                db.UserData.each(function(user) {
                    if (user.Solde < seuilHorsBabasse) {
                        db.HorsBabasse.put({ UserName: user.UserName }).catch(function(err) {
                            console.error(err.stack || err);
                        });
                    }
                }).then(function() {
                    $("#modal-zifoys-validation").modal('show');
                });
            });
        });
        
        // Remettre en babasse un compte
        $("#button-remettre-en-babasse").click(async function(e) {
            var userName = $("#input-remettre-en-babasse").val();
            var user = db.HorsBabasse.where("UserName").equals(userName).delete().then(function () {
                $("#modal-zifoys-validation").modal('show');
                console.log(dateTimeNow() + ": " + userName + " a été remis en babasse manuellement.");
            });
        });
        
        // Hors babasse auto
        $("#button-hors-babasse-auto").click(function() {
            zifoysParams.MiseHorsBabasseAutoActivee = $("#checkbox-hors-babasse-auto").is(":checked");
            zifoysParams.MiseHorsBabasseSeuil = parseFloat($("#input-seuil-hors-babasse-auto").val().replace(",", ".")) || 0.0;
            zifoysParams.MiseHorsBabasseInstantanee = $("#radio-instantanee").is(":checked") || false;
            zifoysParams.MiseHorsBabasseQuotidienne = $("#radio-quotidienne").is(":checked") || false;
            zifoysParams.MiseHorsBabasseQuotidienneHeure = 
                $("#timepicker-hors-babasse-quotidienne").datetimepicker("viewDate").format("HH:mm");
            zifoysParams.MiseHorsBabasseHebdomadaireJours = ""
                + ($("#checkbox-hors-babasse-lundi > input").is(":checked") ? "lundi," : "")
                + ($("#checkbox-hors-babasse-mardi > input").is(":checked") ? "mardi," : "")
                + ($("#checkbox-hors-babasse-mercredi > input").is(":checked") ? "mercredi," : "")
                + ($("#checkbox-hors-babasse-jeudi > input").is(":checked") ? "jeudi," : "")
                + ($("#checkbox-hors-babasse-vendredi > input").is(":checked") ? "vendredi," : "")
                + ($("#checkbox-hors-babasse-samedi > input").is(":checked") ? "samedi," : "")
                + ($("#checkbox-hors-babasse-dimanche > input").is(":checked") ? "dimanche," : "");
            if (zifoysParams.MiseHorsBabasseHebdomadaireJours.length !== 0) {
                zifoysParams.MiseHorsBabasseHebdomadaireJours = 
                    zifoysParams.MiseHorsBabasseHebdomadaireJours.slice(0, -1);
            }
            zifoysParams.MiseHorsBabasseHebdomadaireHeure = 
                $("#timepicker-hors-babasse-hebdomadaire").datetimepicker("viewDate").format("HH:mm");

            uiPostAjax();
            $("#modal-zifoys-validation").modal('show');
        });

        // Quand le bouton de l'image hors babasse a été cliqué
        $("#button-image-hors-babasse").click(function(e) {
            if (imageHorsBabassePath !== "") {
                $(".bloc-case-rouge").css('background-image', "url(" + imageHorsBabassePath + ")");
                $("#modal-zifoys-validation").modal('show');
            }
        });

    })();
    
});
