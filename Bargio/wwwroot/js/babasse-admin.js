﻿$(document).ready(function() {
    const timerCallbackZifoys = new Timer();
    var horsBabasseTimer = null;
    var initialGet = true;
    later.date.localTime();

    snowStorm.snowColor = "#bbddbb";
    snowStorm.flakesMaxActive = 30;
    snowStorm.flakesMax = 40;
    snowStorm.snowStick = false;
    snowStorm.useTwinkleEffect = false;
    snowStorm.useMeltEffect = false;
    snowStorm.autoStart = false;
    snowStorm.animationInterval = 50;
    snowStorm.vMaxX = 5;
    snowStorm.vMaxY = 5;

    function runAt(daysOfWeek, hourOfDay, minuteOfHour, callback) {
        var cronStr;
        if (daysOfWeek === null) {
            cronStr = minuteOfHour + " " + hourOfDay + " * * *";
        } else {
            let cronDays = daysOfWeek.toLowerCase();
            cronDays = cronDays.replace("lundi", "MON");
            cronDays = cronDays.replace("mardi", "TUE");
            cronDays = cronDays.replace("mercredi", "WED");
            cronDays = cronDays.replace("jeudi", "THU");
            cronDays = cronDays.replace("vendredi", "FRI");
            cronDays = cronDays.replace("samedi", "SAT");
            cronDays = cronDays.replace("dimanche", "SUN");
            cronStr = minuteOfHour + " " + hourOfDay + " * * " + cronDays;
        }
        const sched = later.parse.cron(cronStr);
        const occurrences = later.schedule(sched).next(1, new Date());
        bargio.log(`--> Prochaine mise hors babasse auto: ${occurrences}`);
        bargio.log(`\t\t (CRON string: ${cronStr})`);
        if (horsBabasseTimer !== null) {
            horsBabasseTimer.clear();
        }
        horsBabasseTimer = later.setInterval(callback, sched);
    }

    function miseHorsBabasseAuto() {
        const callback = function() {
            db.transaction("rw",
                db.UserData,
                db.HorsBabasse,
                () => {
                    db.UserData.each(function(user) {
                        if (user.Solde < zifoysParams.MiseHorsBabasseSeuil) {
                            db.HorsBabasse.put({ UserName: user.UserName }).catch(function(err) {
                                bargio.log(err.stack || err);
                            });
                        }
                    }).then(function() {
                        bargio.log("Mise hors babasse auto effectuée.");
                    });
                });
        };
        var temps = null;
        if (zifoysParams.MiseHorsBabasseAutoActivee && zifoysParams.MiseHorsBabasseQuotidienne) {
            temps = zifoysParams.MiseHorsBabasseQuotidienneHeure.split(":");
            runAt(null,
                parseInt(temps[0]),
                parseInt(temps[1]),
                callback);
        } else if (zifoysParams.MiseHorsBabasseAutoActivee) {
            temps = zifoysParams.MiseHorsBabasseHebdomadaireHeure.split(":");
            runAt(zifoysParams.MiseHorsBabasseHebdomadaireJours,
                parseInt(temps[0]),
                parseInt(temps[1]),
                callback);
        } else if (horsBabasseTimer !== null) {
            horsBabasseTimer.clear();
        }
    }

    $("#timepicker-hors-babasse-quotidienne").datetimepicker({
        format: "HH:mm",
        defaultDate: new Date("1900-01-01T00:00:00")
    });
    $("#timepicker-hors-babasse-hebdomadaire").datetimepicker({
        format: "HH:mm",
        defaultDate: new Date("1900-01-01T00:00:00")
    });

    function uiGetAjax() {
        $.ajax({
            type: "GET",
            url: "/Api/Foys/zifoysparams",
            cache: false,
            success: function(response) {
                if (response === null) {
                    bargio.log("Impossible de récupérer les paramètres zifoy'ss");
                }
                var newZifoysParams = JSON.parse(response);
                if (initialGet ||
                    newZifoysParams.MiseHorsBabasseAutoActivee !== zifoysParams.MiseHorsBabasseAutoActivee ||
                    newZifoysParams.MiseHorsBabasseHebdomadaireHeure !==
                    zifoysParams.MiseHorsBabasseHebdomadaireHeure ||
                    newZifoysParams.MiseHorsBabasseHebdomadaireJours !==
                    zifoysParams.MiseHorsBabasseHebdomadaireJours ||
                    newZifoysParams.MiseHorsBabasseQuotidienne !== zifoysParams.MiseHorsBabasseQuotidienne ||
                    newZifoysParams.MiseHorsBabasseQuotidienneHeure !== zifoysParams.MiseHorsBabasseQuotidienneHeure) {
                    initialGet = false;
                }
                zifoysParams = newZifoysParams;
                miseHorsBabasseAuto();

                $("#checkbox-hors-babasse-auto").prop("checked", zifoysParams.MiseHorsBabasseAutoActivee);
                $("#input-seuil-hors-babasse-auto").val(zifoysParams.MiseHorsBabasseSeuil.toFixed(2));
                $("#radio-instantanee").prop("checked", zifoysParams.MiseHorsBabasseInstantanee);
                $("#radio-periodique").prop("checked", !zifoysParams.MiseHorsBabasseInstantanee);
                $("#radio-quotidienne").prop("checked", zifoysParams.MiseHorsBabasseQuotidienne);
                $("#radio-hebdomadaire").prop("checked", !zifoysParams.MiseHorsBabasseQuotidienne);
                $("#timepicker-hors-babasse-quotidienne")
                    .datetimepicker("date", zifoysParams.MiseHorsBabasseQuotidienneHeure);
                $("#checkbox-snow").prop("checked", zifoysParams.Snow);
                $("#textarea-mot-des-zifoys").val(zifoysParams.MotDesZifoys);
                $("#textarea-actualites").val(zifoysParams.Actualites);

                $("#p-mot-des-zifoys").text(zifoysParams.MotDesZifoys);
                $("#p-actualites").text(zifoysParams.Actualites);

                $("#timepicker-hors-babasse-hebdomadaire")
                    .datetimepicker("date", zifoysParams.MiseHorsBabasseHebdomadaireHeure);
                if (zifoysParams.MiseHorsBabasseHebdomadaireJours !== null) {
                    if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("lundi"))
                        $("#checkbox-hors-babasse-lundi").addClass("active");
                    if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("mardi"))
                        $("#checkbox-hors-babasse-mardi").addClass("active");
                    if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("mercredi"))
                        $("#checkbox-hors-babasse-mercredi").addClass("active");
                    if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("jeudi"))
                        $("#checkbox-hors-babasse-jeudi").addClass("active");
                    if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("vendredi"))
                        $("#checkbox-hors-babasse-vendredi").addClass("active");
                    if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("samedi"))
                        $("#checkbox-hors-babasse-samedi").addClass("active");
                    if (zifoysParams.MiseHorsBabasseHebdomadaireJours.includes("dimanche"))
                        $("#checkbox-hors-babasse-dimanche").addClass("active");
                }

                if (zifoysParams.Snow) {
                    snowStorm.show();
                    snowStorm.resume();
                } else {
                    snowStorm.stop();
                    snowStorm.freeze();
                }

                refreshUi();
            },
            error: function(xhr, error) {
                bargio.log("Impossible de récupérer les paramètres zifoy'ss");
            },
            timeout: 30000
        });
    }

    timerCallbackZifoys.start({ countdown: true, startValues: { seconds: 30 } });
    timerCallbackZifoys.addEventListener("targetAchieved",
        function(e) {
            if (!desynchronisation) {
                uiGetAjax();
            }
        });

    function uiPostAjax() {
        const fdata = new FormData();
        const json = JSON.stringify(zifoysParams);
        fdata.append("json", json);
        $.ajax({
            type: "POST",
            url: "/Api/Foys/zifoysparams",
            cache: false,
            data: fdata,
            contentType: false,
            processData: false,
            success: function(response) {
            },
            error: function(xhr, error) {
                bargio.log(`Impossible d'envoyer les paramètres zifoy'ss: ${error}`);
            },
            timeout: 30000
        });
    }

    function clearUi() {
        bargio.log("mabite");
        $("#input-mdp-zifoys").val("");
        $("#input-mdp-zifoys-confirmer").val("");
        $("#input-username-rechargement-manuel").val("");
        $("#input-montant-rechargement-manuel").val("0.00");
        $("#input-commentaire-rechargement-manuel").val("bucquage manuel par zifoy'ss");
        $("#validation-rechargement-manuel").text("");
        $("#input-mettre-hors-babasse").val("");
        $("#input-remettre-en-babasse").val("");
        $("#input-mettre-hors-foys").val("");
        $("#input-remettre-en-foys").val("");
        $("#input-username-reset-mdp").val("");
    }

    function refreshUi() {
        if ($("#radio-periodique").is(":checked")) {
            $("#hors-babasse-periodique").show();

            $("#timepicker-hors-babasse-quotidienne").hide();
            $("#jours-hors-babasse-hebdomadaire").hide();
            $("#timepicker-hors-babasse-hebdomadaire").hide();

            if ($("#radio-quotidienne").is(":checked")) {
                $("#timepicker-hors-babasse-quotidienne").show();
            } else if ($("#radio-hebdomadaire").is(":checked")) {
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
        $(
                "#input-username-rechargement-manuel,#input-montant-rechargement-manuel,#input-commentaire-rechargement-manuel")
            .each(function() {
                $(this).keypress(function(e) {
                    if (e.keyCode === 13)
                        $("#button-rechargement-manuel").click();
                });
            });

        $("#input-seuil-hors-babasse-manuelle").keypress(function(e) {
            if (e.keyCode === 13)
                $("#button-hors-babasse-manuelle").click();
        });

        $("#input-remettre-en-babasse").keypress(function(e) {
            if (e.keyCode === 13)
                $("#button-remettre-en-babasse").click();
        });

        $("#input-mettre-hors-babasse").keypress(function(e) {
            if (e.keyCode === 13)
                $("#button-mettre-hors-babasse").click();
        });

        $("#input-remettre-en-foys").keypress(function(e) {
            if (e.keyCode === 13)
                $("#button-remettre-en-foys").click();
        });

        $("#input-mettre-hors-foys").keypress(function(e) {
            if (e.keyCode === 13)
                $("#button-mettre-hors-foys").click();
        });

        $("#input-mdp-zifoys-confirmer").keypress(function(e) {
            if (e.keyCode === 13)
                $("#button-mdp-zifoys").click();
        });

        $("#input-username-reset-mdp").keypress(function(e) {
            if (e.keyCode === 13)
                $("#button-reset-mdp").click();
        });
    })();

    // Gestion des events
    (() => {
        // On rafraichit l'interface quand un bouton radio a été cliqué
        $("input:radio").change(refreshUi);

        var imageHorsBabassePath = "";
        // On met la nouvelle image hors babasse en preview
        $("#input-file-hors-babasse").change(function(e) {
            imageHorsBabassePath = URL.createObjectURL(e.target.files[0]);
            $(".bloc-case-rouge-test").css("background-image", `url(${imageHorsBabassePath})`);
        });

        // Rechargement manuel
        $("#button-rechargement-manuel").click(async function() {
            const userName = $("#input-username-rechargement-manuel").val();
            const montant = parseFloat($("#input-montant-rechargement-manuel").val().replace(",", "."));
            if (isNaN(montant) || montant === undefined) {
                $("#modal-zifoys-erreur").modal("show");
                return;
            }
            const commentaire = $("#input-commentaire-rechargement-manuel").val();

            const user = await db.UserData.get({ UserName: userName });
            // Si l'utilisateur n'existe pas
            if (typeof user === "undefined") {
                $("#validation-rechargement-manuel").text(`L'utilisateur ${userName} n'existe pas.`);
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
                        { Solde: Math.round((user.Solde + transaction.Montant) * 100) / 100 });
                }).then(function() {
                db.HistoriqueTransactions.add(transaction).then(function() {
                    bargio.log(dateTimeNow() +
                        ": " +
                        transaction.Montant +
                        " par " +
                        transaction.UserName +
                        " (" +
                        transaction.Commentaire +
                        ")");
                    clearUi();
                });
                $("#modal-zifoys-validation").modal("show");
            });
        });

        // Hors babasse manuelle
        $("#button-hors-babasse-manuelle").click(function() {
            seuilHorsBabasse = parseFloat($("#input-seuil-hors-babasse-manuelle").val().replace(",", "."));
            if (isNaN(seuilHorsBabasse) || seuilHorsBabasse === undefined) {
                $("#modal-zifoys-erreur").modal("show");
                return;
            }
            db.transaction("rw",
                db.UserData,
                db.HorsBabasse,
                () => {
                    db.UserData.each(function(user) {
                        if (user.Solde < seuilHorsBabasse) {
                            db.HorsBabasse.put({ UserName: user.UserName }).catch(function(err) {
                                bargio.log(err.stack || err);
                            });
                        }
                    }).then(function() {
                        bargio.log(dateTimeNow() + ": Hors babasse manuelle (seuil=" + seuilHorsBabasse + ")");
                        $("#modal-zifoys-validation").modal("show");
                    });
                });
        });

        // Remettre en babasse un compte
        $("#button-remettre-en-babasse").click(async function(e) {
            var userName = $("#input-remettre-en-babasse").val();
            db.HorsBabasse.where("UserName").equals(userName).delete().catch(function(err) {
                bargio.log(err.stack || err);
            }).then(function() {
                $("#modal-zifoys-validation").modal("show");
                bargio.log(dateTimeNow() + ": " + userName + " a été remis en babasse manuellement.");
            });
        });

        // Mettre hors babasse un compte
        $("#button-mettre-hors-babasse").click(async function(e) {
            var userName = $("#input-mettre-hors-babasse").val();
            db.HorsBabasse.put({ UserName: userName }).catch(function(err) {
                bargio.log(err.stack || err);
            }).then(function(e) {
                bargio.log(dateTimeNow() + userName + " a été mis hors babasse manuellement");
                $("#modal-zifoys-validation").modal("show");
            });
        });

        // Remettre en foys un compte
        $("#button-remettre-en-foys").click(async function(e) {
            var userName = $("#input-remettre-en-foys").val();
            db.UserData.update(userName,
                { HorsFoys: false }
            ).catch(function(err) {
                bargio.log(err.stack || err);
            }).then(function() {
                const fdata = new FormData();
                const json = JSON.stringify({ UserName: userName, HorsFoys: false });
                fdata.append("json", json);
                $.ajax({
                    type: "POST",
                    url: "/Api/Foys/sethorsfoys",
                    cache: false,
                    data: fdata,
                    contentType: false,
                    processData: false,
                    success: function(response) {
                        bargio.log(dateTimeNow() + ": " + userName + " a été remis en foy's.");
                    },
                    error: function(xhr, error) {
                        bargio.log(dateTimeNow() +
                            ": La remise en foy's de " +
                            userName +
                            " n'a pas pu être synchronisée avec le serveur.");
                    },
                    timeout: 30000
                });
                $("#modal-zifoys-validation").modal("show");
            });
        });

        // Mettre hors foys un compte
        $("#button-mettre-hors-foys").click(async function(e) {
            var userName = $("#input-mettre-hors-foys").val();
            db.UserData.update(userName,
                { HorsFoys: true }
            ).catch(function(err) {
                bargio.log(err.stack || err);
            }).then(function() {
                const fdata = new FormData();
                const json = JSON.stringify({ UserName: userName, HorsFoys: true });
                fdata.append("json", json);
                $.ajax({
                    type: "POST",
                    url: "/Api/Foys/sethorsfoys",
                    cache: false,
                    data: fdata,
                    contentType: false,
                    processData: false,
                    success: function(response) {
                        bargio.log(dateTimeNow() + ": " + userName + " a été mis hors foy's.");
                    },
                    error: function(xhr, error) {
                        bargio.log(dateTimeNow() +
                            ": La mise hors foy's de " +
                            userName +
                            " n'a pas pu être synchronisée avec le serveur.");
                    },
                    timeout: 30000
                });
                $("#modal-zifoys-validation").modal("show");
            });
        });

        // Hors babasse auto
        $("#button-hors-babasse-auto").click(function() {
            zifoysParams.MiseHorsBabasseAutoActivee = $("#checkbox-hors-babasse-auto").is(":checked");
            zifoysParams.MiseHorsBabasseSeuil =
                parseFloat($("#input-seuil-hors-babasse-auto").val().replace(",", "."));
            if (isNaN(zifoysParams.MiseHorsBabasseSeuil) || zifoysParams.MiseHorsBabasseSeuil === undefined) {
                zifoysParams.MiseHorsBabasseSeuil = 0;
                $("#modal-zifoys-erreur").modal("show");
                return;
            }
            zifoysParams.MiseHorsBabasseInstantanee = $("#radio-instantanee").is(":checked") || false;
            zifoysParams.MiseHorsBabasseQuotidienne = $("#radio-quotidienne").is(":checked") || false;
            zifoysParams.MiseHorsBabasseQuotidienneHeure =
                $("#timepicker-hors-babasse-quotidienne").datetimepicker("viewDate").format("HH:mm");
            zifoysParams.MiseHorsBabasseHebdomadaireJours = "" +
                ($("#checkbox-hors-babasse-lundi > input").is(":checked") ? "lundi," : "") +
                ($("#checkbox-hors-babasse-mardi > input").is(":checked") ? "mardi," : "") +
                ($("#checkbox-hors-babasse-mercredi > input").is(":checked") ? "mercredi," : "") +
                ($("#checkbox-hors-babasse-jeudi > input").is(":checked") ? "jeudi," : "") +
                ($("#checkbox-hors-babasse-vendredi > input").is(":checked") ? "vendredi," : "") +
                ($("#checkbox-hors-babasse-samedi > input").is(":checked") ? "samedi," : "") +
                ($("#checkbox-hors-babasse-dimanche > input").is(":checked") ? "dimanche," : "");
            if (zifoysParams.MiseHorsBabasseHebdomadaireJours.length !== 0) {
                zifoysParams.MiseHorsBabasseHebdomadaireJours =
                    zifoysParams.MiseHorsBabasseHebdomadaireJours.slice(0, -1);
            }
            zifoysParams.MiseHorsBabasseHebdomadaireHeure =
                $("#timepicker-hors-babasse-hebdomadaire").datetimepicker("viewDate").format("HH:mm");

            miseHorsBabasseAuto();

            uiPostAjax();
            $("#modal-zifoys-validation").modal("show");
        });

        // Quand le bouton de l'image hors babasse a été cliqué
        $("#button-image-hors-babasse").click(function(e) {
            if (imageHorsBabassePath !== "") {
                $(".bloc-case-rouge").css("background-image", `url(${imageHorsBabassePath})`);
                $("#modal-zifoys-validation").modal("show");
            }
        });

        // Supprimer le mdp d'un utilisateur
        $("#button-reset-mdp").click(function(e) {
            console.log("ok");
            const data = {
                UserName: $("#input-username-reset-mdp").val()
            };
            const json = JSON.stringify(data);
            console.log(json);
            const fdata = new FormData();
            fdata.append("json", json);
            $.ajax({
                type: "POST",
                url: "/api/foys/supprimermdp",
                cache: false,
                data: fdata,
                contentType: false,
                processData: false,
                success: function(response) {
                    $("#modal-zifoys-validation").modal("show");
                }
            });
        });

        // Activer la neige
        $("#button-snow").click(function(e) {
            zifoysParams.Snow = $("#checkbox-snow").is(":checked");

            if (zifoysParams.Snow) {
                snowStorm.show();
                snowStorm.resume();
            } else {
                snowStorm.stop();
                snowStorm.freeze();
            }


            uiPostAjax();
            $("#modal-zifoys-validation").modal("show");
        });

        // Changer mdp
        $("#button-mdp-zifoys").click(function(e) {
            const newPwd = $("#input-mdp-zifoys").val();
            const newPwdConfirm = $("#input-mdp-zifoys-confirmer").val();

            if (newPwd !== newPwdConfirm) {
                $("#modal-zifoys-mdp-validation").modal("show");
                return;
            }

            zifoysParams.MotDePasseZifoys = newPwd;

            uiPostAjax();
            bargio.log(dateTimeNow() + ": Changement du mdp zifoy's");
            $("#modal-zifoys-validation").modal("show");
        });

        // Mot des zifoys / actualités
        $("#button-mots-des-zifoys-actualites").click(function(e) {
            zifoysParams.MotDesZifoys = $("#textarea-mot-des-zifoys").val();
            zifoysParams.Actualites = $("#textarea-actualites").val();

            $("#p-mot-des-zifoys").text(zifoysParams.MotDesZifoys);
            $("#p-actualites").text(zifoysParams.Actualites);

            uiPostAjax();
            $("#modal-zifoys-validation").modal("show");
        });

        // DL les logs
        $("#button-telecharger-logs").click(function(e) {
            bargio.downloadLogs();
        });

        // On clear l'interface à chaque fermeture de la modal
        $("#modal-zifoys").on("hidden.bs.modal", function () {
            clearUi();
        });

    })();

});