$(document).ready(function () {
	function test(val) {
		console.log("Go! " + val);
	}
	
	function runAt(daysOfWeek, hourOfDay, callback) {
		var cronStr = "";
		if (daysOfWeek === null) {
			cronStr = "0 " + hourOfDay + " * * *";
		} else {
			cronDays = daysOfWeek;
			cronDays = cronDays.replace("Lundi", "MON");
			cronDays = cronDays.replace("Mardi", "TUE");
			cronDays = cronDays.replace("Mercredi", "WED");
			cronDays = cronDays.replace("Jeudi", "THU");
			cronDays = cronDays.replace("Vendredi", "FRI");
			cronDays = cronDays.replace("Samedi", "SAT");
			cronDays = cronDays.replace("Dimanche", "SUN");
			cronStr = "0 " + hourOfDay + " * * " + cronDays;
		}
		console.log(cronStr);
		var sched = later.parse.cron(cronStr);
		t = later.setInterval(callback, sched);
	}
	
	runAt("Lundi,Mardi,Mercredi,Jeudi,Vendredi,Samedi,Dimanche", 0, test);
	
	var t = null;
	
	later.date.localTime();
	var sched = later.parse.cron('0 0 * * MON,SAT,SUN');
	var sched = later.parse.cron('0 0 * * *');
	t = later.setInterval(test, sched);
	
	var startdate = new Date(2015, 0, 1);

    var occurrences = later.schedule(sched).next(20, startdate);
    for (var i = 0; i < 20; i++) {
        //console.log(occurrences[i]);
        //alert(curDate);         
        //textDates += moment(curDate).format("MM/DD/YY") + " <br /> ";
        //moment library is used for date formatting only 
    }

});