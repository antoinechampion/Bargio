var superLocal = (function () {
	"use strict";
	var superLocal = {
		settings : {
			dbPrefix:		'db',
			dataFieldName:	'data',
			separator:		'_',
			saveType:		'relaxed'
		},
		// made a function so it's "private"
		prefixRegex : function() {
			return new RegExp(this.settings.dbPrefix,'g');
		},
		// made a function so it's "private"
		createdRegex : function() {
			return new RegExp('created','g');
		},
		// made a function so it's "private"
		modifiedRegex : function() {
			return new RegExp('modified','g');
		},
		// made a function so it's "private"
		datafieldRegex : function() {
			return new RegExp(this.settings.dataFieldName,'g');
		},
		capable : function() {
			var testData = 'ls';
			try {
				localStorage.setItem(testData, testData);
				localStorage.removeItem(testData);
				return true;
			} catch(e) {
				return false;
			}
		},
		findNewest : function() {
			var timestamp = 0;
			var newestRecordID = 0;
			var localKeys = Object.keys(localStorage);
			var totalKeys = localKeys.length;
			for ( var i=0; i<totalKeys; i++ ) {
				if (this.prefixRegex().test(localKeys[i]) && this.modifiedRegex().test(localKeys[i])) {
					var currentKey = localKeys[i].replace(this.settings.dbPrefix + this.settings.separator,'').replace(this.settings.separator + 'modified','');
					var thisModified = localStorage[this.settings.dbPrefix + this.settings.separator + currentKey + this.settings.separator + 'modified'];
					if ( thisModified > timestamp) {
						newestRecordID = currentKey;
						// makes 'timestamp' we're comparing against the current one, for the next comparison to run against
						timestamp = thisModified;
					}
				}
			}
			return newestRecordID;
		},
		findOldest : function() {
			var timestamp = 0;
			var oldestRecordID = 0;
			var localKeys = Object.keys(localStorage);
			var totalKeys = localKeys.length;
			for ( var i=0; i<totalKeys; i++ ) {
				if (this.prefixRegex().test(localKeys[i]) && this.modifiedRegex().test(localKeys[i])) {
					var currentKey = localKeys[i].replace(this.settings.dbPrefix + this.settings.separator,'').replace(this.settings.separator + 'modified','');
					var thisModified = localStorage[this.settings.dbPrefix + this.settings.separator + currentKey + this.settings.separator + 'modified'];
					// have to add '|| 0' because every timestamp will be larger than 0 and nothing will satisfy the if statement!
					if ( thisModified < timestamp || timestamp === 0) {
						oldestRecordID = currentKey;
						// makes 'timestamp' we're comparing against the current one, for the next comparison to run against
						timestamp = thisModified;
					}
				}
			}
			return oldestRecordID;
		},
		findSmallest : function() {
			var dataSize = 0;
			var smallestRecordID = 0;
			var localKeys = Object.keys(localStorage);
			var totalKeys = localKeys.length;
			for ( var i=0; i<totalKeys; i++ ) {
				if (this.prefixRegex().test(localKeys[i]) && this.datafieldRegex().test(localKeys[i])) {
					var currentKey = localKeys[i].replace(this.settings.dbPrefix + this.settings.separator,'').replace(this.settings.separator + 'modified','');
					var thisData = localStorage[this.settings.dbPrefix + this.settings.separator + currentKey + this.settings.separator + this.settings.dataFieldName].length;
					// have to add '|| 0' because every data size will be larger than 0 and nothing will satisfy the if statement!
					if ( thisData < dataSize || dataSize === 0) {
						smallestRecordID = currentKey;
						// makes 'dataSize' we're comparing against the current one, for the next comparison to run against
						dataSize = thisData;
					}
				}
			}
			return smallestRecordID;
		},
		findBiggest : function() {
			var dataSize = 0;
			var biggestRecordID = 0;
			var localKeys = Object.keys(localStorage);
			var totalKeys = localKeys.length;
			for ( var i=0; i<totalKeys; i++ ) {
				if (this.prefixRegex().test(localKeys[i]) && this.datafieldRegex().test(localKeys[i])) {
					var currentKey = localKeys[i].replace(this.settings.dbPrefix + this.settings.separator,'').replace(this.settings.separator + 'modified','');
					var thisData = localStorage[this.settings.dbPrefix + this.settings.separator + currentKey + this.settings.separator + this.settings.dataFieldName].length;
					if ( thisData > dataSize ) {
						biggestRecordID = currentKey;
						// makes 'dataSize' we're comparing against the current one, for the next comparison to run against
						dataSize = thisData;
					}
				}
			}
			return biggestRecordID;
		},
		fetch : function(param) {
			var data = {};
			var uID;
			if (param === 'newest') {
				uID = this.findNewest();
			} else if (param === 'oldest') {
				uID = this.findOldest();
			} else if (param === 'smallest') {
				uID = this.findSmallest();
			} else if (param === 'biggest') {
				uID = this.findBiggest();
			} else if ( isNaN(param) ) {
				uID = param;
			} else {
				return false;
			}
			var recordPrefix = this.settings.dbPrefix + this.settings.separator + uID + this.settings.separator;
			data.uID = uID;
			data[this.settings.dataFieldName] = localStorage[recordPrefix + this.settings.dataFieldName];
			data.created = localStorage[recordPrefix + 'created'];
			data.modified = localStorage[recordPrefix + 'modified'];
			return data;
		},
		remove : function(uID) {
			var recordPrefix = this.settings.dbPrefix + this.settings.separator + uID + this.settings.separator;
			localStorage.removeItem(recordPrefix + this.settings.dataFieldName);
			localStorage.removeItem(recordPrefix + 'created');
			localStorage.removeItem(recordPrefix + 'modified');
		},
		clearAll : function() {
			localStorage.clear();
		},
		listAllCreated : function() {
			var localKeys = Object.keys(localStorage);
			var totalKeys = localKeys.length;
			var allKeys = {};
			for ( var i=0; i<totalKeys; i++ ) {
				if (this.prefixRegex().test(localKeys[i]) && this.createdRegex().test(localKeys[i])) {
					var currentKey = localKeys[i].replace(this.settings.dbPrefix + this.settings.separator,'').replace(this.settings.separator + 'created','');
					allKeys[currentKey] = localStorage[this.settings.dbPrefix + this.settings.separator + currentKey + this.settings.separator + 'created'];
				}
			}
			return allKeys;
		},
		listAllModified : function() {
			var localKeys = Object.keys(localStorage);
			var totalKeys = localKeys.length;
			var allKeys = {};
			for ( var i=0; i<totalKeys; i++ ) {
				if (this.prefixRegex().test(localKeys[i]) && this.modifiedRegex().test(localKeys[i])) {
					var currentKey = localKeys[i].replace(this.settings.dbPrefix + this.settings.separator,'').replace(this.settings.separator + 'modified','');
					allKeys[currentKey] = localStorage[this.settings.dbPrefix + this.settings.separator + currentKey + this.settings.separator + 'modified'];
				}
			}
			return allKeys;
		},
		save : function(uID,data,customTime) {
			if ( this.capable() === true) {
				var timestamp;
				var recordPrefix = this.settings.dbPrefix + this.settings.separator + uID + this.settings.separator;
				// sets timestamp, and sets it to customTime if it's been provided
				if (typeof(customTime)!=='undefined') {
					timestamp = customTime;
				} else {
					timestamp = Math.floor(new Date().getTime()/1000);
				}
				try {
					// attempt to save the record
					localStorage.setItem(recordPrefix + this.settings.dataFieldName, data);
					localStorage.setItem(recordPrefix + 'modified', timestamp);
					// if there's currently no 'created' record for the supplied ID, we know that this is a record, so we'll make the associated 'created' record
					if ( typeof(localStorage[ recordPrefix + 'created' ])==='undefined' ) {
						localStorage.setItem(recordPrefix + 'created', timestamp);
					}
					return true;
				} catch(e) {
					// remove any partially failed data (example, 'modified' might have saved, but 'data' failed)
					this.remove(uID);
					if (this.settings.saveType === 'greedy') {
						// if the save fails, it's likely because localStorage is full
						// we get around this by deleting the oldest record to free space...
						this.remove( this.findOldest() );
						// ...then try the save again!
						this.save(uID,data,customTime);
					}
					return false;
				}
			}
		}
	};
	return superLocal;
}());
