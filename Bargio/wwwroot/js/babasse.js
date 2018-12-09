"use strict";
$( document ).ready(function() {
	// variables globales
	var interfaceAccueil = true;
	var db = new Dexie("db");
	var derniereSynchro = null;
	var derniereSynchroAsDatetime = null;
	var desynchronisation = false;
	var timerCallback = new Timer();
	var timerCheckDesync = new Timer();
	var desyncTimeout = 60; // seconds

	// Passe de l'interface de bucquage à l'interface d'accueil
	function setInterfaceAccueil() {
		$("#undefined-user-alert").hide();
		$("#ui-userdata").hide();
		$("#ui-proms").slideDown(200);
		$("#ui-tarifs").hide();
		$("#ui-nums").show();
		$("#ui-solde").hide();
		$("#ui-actualites").show();
		$("#ui-historique").hide();
		$("#ui-motzifoys").show();
		$("#inputNumss").focus();
		$("#inputNumss").val("");
		$("#input-modal-proms").val("");
		interfaceAccueil = true;
	}
	
	// Passe de l'interface d'accueil à l'interface de bucquage
	function setInterfaceBucquage() {
		$("#ui-proms").hide();
		$("#ui-tarifs").show();
		$("#ui-nums").hide();
		$("#ui-userdata").slideDown(200);
		$("#ui-solde").show();
		$("#ui-actualites").hide();
		$("#ui-historique").show();
		$("#ui-motzifoys").hide();
		interfaceAccueil = false;
	}

	// Créée les tables de bdd locales :
	// - UserData : sera chargée depuis le serveur,
	// contient chaque PG, son solde et son statut (hors babasse...)
	// - HistoriqueTransactions : table à synchroniser, contient tout l'historique
	// des transactions, bucquages comme rechargement
	(function() {
		db.version(1).stores({
			UserData: 'UserName,HorsFoys,Surnom,Solde,'
				+ 'FoysApiHasPassword,FoysApiPasswordHash,'
				+ 'FoysApiPasswordSalt',
			HistoriqueTransactions: '++,UserName,Date,Montant,IdProduit,Commentaire'
		});
		db.open().catch (function (err) {
			console.error('Failed to open db: ' + (err.stack || err));
		});
	})();

	// Retourne la date et l'heure au format dd-mm-yyyy HH:MM:ss
	function dateTimeNow() {
		return dateFormat(new Date(), "dd-mm-yyyy HH:MM:ss");
	}
	
	// Retourne une chaîne de caractère correspondant
	// à la touche passée en paramètre
    function keycodeToShortcut(keycode)
	{
		var raccourci = "";
		if (keycode < 112 || keycode > 123) return null;
		else return "F" + (keycode - 111);
	}
	
	// Mises à jour asynchrones
    (function () {	
		function foysApiGet() {
			// Si il n'y a pas de resynchro à faire, on recharge la liste des utilisateurs
			db.HistoriqueTransactions.count().then( function (count) {

				if (count === 0) {
					console.log(dateTimeNow() + ": Historique vide: pas de resynchro à faire.");
					db.UserData.clear().then(function () {
						$.ajax({
							type: 'GET',
							url: '/Api/Foys',
							cache: false,
							success: function (response) {
								JSON.parse(response).forEach(function(user) {
									console.log("Ajout de " + user.UserName);
									db.UserData.add(user);
								});
								$("#ui-chargement").slideUp(200);
								setInterfaceAccueil();
							}
						});
					});
				} else {
					db.HistoriqueTransactions
						.each(
							function(transaction) {
								var commentaire = "Id produit " +
									transaction.IdProduit +
									", Prix = " +
									transaction.Montant +
									"€, sur " +
									transaction.UserName +
									"\n";

								$("#historique").val($("#historique").val() + commentaire);
							}
						);
				}
				derniereSynchro = dateTimeNow();
				derniereSynchroAsDatetime = new Date();
				$("#ui-chargement").slideUp(200);
				setInterfaceAccueil();
			});
		}

		function foysApiPostUpdates() {
			// On créée le corps de la requête POST
			var fdata = new FormData();
			db.HistoriqueTransactions.toArray().then(function (arr) {
				if (arr.length === 0) {
					console.log(dateTimeNow() + ": Pas de nouvelles modifications côté client à POST");
					timerCallback.reset();
					return;
				}
				var json = JSON.stringify(arr);
				fdata.append("json", json);
				console.log(json);
				$.ajax({
					type: 'POST',
					url: '/Api/Foys/history',
					cache: false,
					data: fdata,
					contentType: false,
					processData: false,
					success: function (response) {
						db.HistoriqueTransactions.clear();
						// DEBUG
						$("#historique").val("");
						// END DEBUG
						timerCallback.reset();
					}
				});
			});      
		}

		function foysApiGetUpdates() {
        $.ajax({
            type: 'GET',
            url: '/Api/Foys/' + derniereSynchro,
			cache: false,
            success: function (response) {
                // Pour chaque utilisateur modifié, on met à jour son solde
                // et son status, et on re-applique tout son historique
                // de transaction local
                var arr = JSON.parse(response);
                if (arr.length === 0) {
                    console.log(dateTimeNow() + ": Pas de nouvelles modifications côté serveur");
                } else {
                    arr.forEach(function (user) {
                        console.log("Modifications serveur sur l'utilisateur " + user.UserName);
                        var modifSoldeLocal = 0;
                        db.HistoriqueTransactions
                            .where("UserName")
                            .equals(user.UserName)
                            .each(
                                function (transaction) {
                                    console.log("\t -> Transaction : " + transaction.Montant + "€");
                                    modifSoldeLocal += transaction.Montant;
                                }
                            ).then(function () {
                                console.log("\t-> Modifs solde local : " + modifSoldeLocal);
                                console.log("\t-> Nouveau solde : " + (user.Solde + modifSoldeLocal));
                                db.UserData.update(user.UserName,
                                    { Solde: user.Solde + modifSoldeLocal, HorsFoys: user.HorsFoys });
                            });
                    });
                }
                derniereSynchro = dateTimeNow();
				derniereSynchroAsDatetime = new Date();
                foysApiPostUpdates();
            },
			error: function(xhr, error){
				console.log(dateTimeNow() + ": Impossible de synchroniser\n\t-> Erreur: " 
					+ error + "\n\t-> Dernière synchro réussie: " + derniereSynchro);
				timer.reset();
			},
			timeout: 3000
		});
	}

		foysApiGet();

		timerCallback.start({countdown: true, startValues: {seconds: 30}});
		timerCallback.addEventListener("targetAchieved", function (e) {
			foysApiGetUpdates();
		});
		timerCheckDesync.start({countdown: true, startValues: {seconds: 60}});
		timerCheckDesync.addEventListener("targetAchieved", function (e) {
			if (derniereSynchroAsDatetime === null)
				return;

			var desync = new Date();
			desync.setSeconds(desync.getSeconds() + desyncTimeout);
			desynchronisation = derniereSynchroAsDatetime > desync;

            if (desynchronisation) {
				$("#logo-footer").attr("src", "/images/title_logo_red106x20.png");
			} else {
				$("#logo-footer").attr("src", "/images/title_logo_106x20.png");
			}
		});
	}());
	
	// Fonction de gestion des inputs pour l'accueil
	function onKeydownCallbackAccueil(e) {
		// On retrouve l'identifiant du PG à travers le DOM
		e = e || e.which;
		var keyPressed = keycodeToShortcut(e.keyCode);
		if (keyPressed === null)
			return;
		
		async function changerInterface(proms) {
			proms = proms.toLowerCase();
			var username = $("#inputNumss").val() + proms;
			// On vérifie si il existe bien dans la BDD
			var user = await db.UserData.get({ UserName: username });
			if (typeof user === "undefined") {
				$("#undefined-user-alert").slideDown(200); 
				window.setTimeout(function() {
					$("#undefined-user-alert").slideUp(200); 
				}, 2000);
				return;
			} else {
				$("#username").text(user.UserName);
				$("#surnom").text(user.Surnom);

				// On récupère son historique
				// var hist = await db.HistoriqueTransactions.get({ UserName: username });
				// table-historique-consos
                if (!desynchronisation) {
					;
				} else {
					;
				}
			}

			// Hors foy'ss ?
			// Mode archi ?
			setInterfaceBucquage();
		}
		
		e.preventDefault();
		if (keyPressed === "F1") {
			$("#modal-autre-proms").on("hidden.bs.modal",
				function() {
					changerInterface($("#input-modal-proms").val());
				}
			);

			$("#modal-autre-proms").modal("show");
			$('#modal-autre-proms').on('keyup keypress', function(e) {
				var keyCode = e.keyCode || e.which;
				if (keyCode === 13) {
					$("#button-modal-proms").click();
					e.preventDefault();
				}
			});
			setTimeout(function() {
				$("#input-modal-proms").focus();
			}, 500);
		} else {
			var proms = $(".raccourci-proms").filter(function() {
				return $(this).text() === keyPressed;
			}).next().text();
			console.log(proms);
			changerInterface(proms);
		}
	}
	
	// Fonction de gestion des inputs pour la page de bucquage
	function onKeydownCallbackBucquage(e) {
		e = e || e.which;
		if (e.keyCode === 27) { // ESC
			setInterfaceAccueil();
		}
	}

	// Callback pour le changement d'interface
    $(document).on("keydown", function(e) {
		if (interfaceAccueil)
			onKeydownCallbackAccueil(e);
		else
			onKeydownCallbackBucquage(e);
	});

});