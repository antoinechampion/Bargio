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
        $("input-mdp-zifoys").val("");
        $("input-mdp-zifoys-confirmer").val("");
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
    refreshUi();

    // Gestion des events
    (function() {
        // On rafraichit l'interface quand un bouton radio a été cliqué
        $('input:radio').change(refreshUi);

        var imageHorsBabassePath = ""; 
        // On met la nouvelle image hors babasse en preview
        $("#input-file-hors-babasse").change(function(e){
            imageHorsBabassePath = URL.createObjectURL(event.target.files[0]);
            $(".bloc-case-rouge-test").css('background-image', "url(" + imageHorsBabassePath + ")");
        });

        // Quand le bouton rechargement manuel est cliqué
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

        // Quand le bouton de l'image hors babasse a été cliqué
        $("#button-image-hors-babasse").click(function(e) {
            if (imageHorsBabassePath !== "") {
                $(".bloc-case-rouge").css('background-image', "url(" + imageHorsBabassePath + ")");
                $("#modal-zifoys-validation").modal('show');
            }
        });

    })();
    
});
