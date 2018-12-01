$( document ).ready(function() {
	// DEBUG
    $("#ajouter").click(function(){
        // On ajoute une transaction random
        // product id, montant, utilisateur, commentaire
        var textArray = [
            [1,-1.25,"1Test217"],
            [2,-2.50,"1Test217"],
            [3,-3.75,"2Test217"],
            [4,-5.00,"1Test217"]
        ];

        var randomNumber = Math.floor(Math.random()*textArray.length);
        var commentaire = "Id produit " + textArray[randomNumber][0]
            + ", Prix = " + textArray[randomNumber][1]
            + "€, sur " + textArray[randomNumber][2] + "\n";

        $("#historique").val($("#historique").val() + commentaire);
		db.UserData.update(textArray[randomNumber][2], 
								{Solde: user.Solde + textArray[randomNumber][1]});
		db.HistoriqueTransactions.add({UserName: textArray[randomNumber][2], 
			Montant: textArray[randomNumber][1], IdProduit: textArray[randomNumber][2]});
    });
	$("#force_request").click(function() {
		foysApiGetUpdates();
		foysApiPostUpdates();
	});
	$("#clear_history").click(function() {
		db.HistoriqueTransactions.clear();
		$("#historique").val("");
	});
	$("#clear_userdata").click(function() {
		db.UserData.clear();
	});
	// END DEBUG
	
	// Créée les tables de bdd locales :
	// - UserData : sera chargée depuis le serveur,
	// contient chaque PG, son solde et son statut (hors babasse...)
	// - HistoriqueTransactions : table à synchroniser, contient tout l'historique
	// des transactions, bucquages comme rechargement
    var db = new Dexie("db");
    db.version(1).stores({
        UserData: 'UserName,HorsFoys,Surnom,Solde,'
            + 'FoysApiHasPassword,FoysApiPasswordHash,'
            + 'FoysApiPasswordSalt',
        HistoriqueTransactions: '++,UserName,Date,Montant,IdProduit,Commentaire'
	});
    db.open().catch (function (err) {
        console.error('Failed to open db: ' + (err.stack || err));
    });
	// DEBUG
	db.HistoriqueTransactions.clear().then(function() {
		db.HistoriqueTransactions.add({UserName: "1Test217", Montant: 1});
		db.HistoriqueTransactions.add({UserName: "1Test217", Montant: 3});
	});
	// END DEBUG
	
	function dateTimeNow() {
		return dateFormat(new Date(), "dd-mm-yyyy HH:MM:ss");
	}
	
    function foysApiGet() {
		// Si il n'y a pas de resynchro à faire, on recharge la liste des utilisateurs
		db.HistoriqueTransactions.count().then( function (count) {
			if (count === 0) {
				console.log(dateTimeNow() + ": Historique vide: pas de resynchro à faire.");
				db.UserData.clear().then(function () {
					$.ajax({
						type: 'GET',
						url: '/Api/Foys',
						success: function (response) {
							JSON.parse(response).forEach(function(user) {
								console.log("Ajout de " + user.UserName);
								db.UserData.add(user);
							});
							$("#chargement").hide();
							$("#fin_chargement").show();
						}
					});
				});
			} else {				
				$("#chargement").hide();
				$("#fin_chargement").show();
			}
		});
    }
	
	function foysApiGetUpdates() {
		$.ajax({
			type: 'GET',
			url: '/Api/Foys/' + dateTimeNow(),
			success: function (response) {
				// Pour chaque utilisateur modifié, on met à jour son solde
				// et son status, et on re-applique tout son historique
				// de transaction local
				var arr = JSON.parse(response);
				if (arr.length === 0) {
					console.log(dateTimeNow() + ": Pas de nouvelles modifications côté serveur");
					return;
				}
				arr.forEach(function(user) {
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
						).then(function() {
							console.log("\t-> Modifs solde local : " + modifSoldeLocal);
							console.log("\t-> Nouveau solde : " + (user.Solde + modifSoldeLocal));
							db.UserData.update(user.UserName, 
								{Solde: user.Solde + modifSoldeLocal, HorsFoys: user.HorsFoys});
						});
				});
			}
		});
	}
	
	function foysApiPostUpdates() {
		// On créée le corps de la requête POST
		var fdata = new FormData();
		db.HistoriqueTransactions.toArray().then(function (arr) {
			if (arr.length === 0) {
				console.log(dateTimeNow() + ": Pas de nouvelles modifications côté client à POST");
				
				return;
			}
			var json = JSON.stringify(arr);
			fdata.append("json", json);
			console.log(json);
			$.ajax({
				type: 'POST',
				url: '/Api/Foys/history',
				data: fdata,
				contentType: false,
				processData: false,
				success: function (response) {
					db.HistoriqueTransactions.clear();
					// DEBUG
					$("#historique").val("");
					// END DEBUG
					
				}
			});
		});      
	}

	// Chargement
    foysApiGet();

    var timer = new Timer();
    timer.start({countdown: true, startValues: {seconds: 30}});
    timer.addEventListener('secondsUpdated', function (e) {
        $('#timer').html(timer.getTimeValues().toString());
    });
    timer.addEventListener('targetAchieved', function (e) {
		foysApiGetUpdates();
		foysApiPostUpdates();
		timer.reset();
    });

});