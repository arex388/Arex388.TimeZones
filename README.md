# Arex388.TimeZones

A small utility library to help with time zone discovery. It combines the [GeoTimeZone](https://github.com/mj1856/GeoTimeZone), [TimeZoneConverter](https://github.com/mj1856/TimeZoneConverter), [TimeZoneNames](https://github.com/mj1856/TimeZoneNames), and [NodaTime](https://github.com/nodatime/nodatime) NuGet packages to make it all work.

#### How to Use

There are four methods available:

- `GetTimeZones` - Returns a list of all time zones like the one below.
- `GetTimeZoneByCoordinate` - Returns the current time zone at the current point in time for a specific location coordinate.
- `GetTimeZoneByIanaId` - Returns a time zone for a specific IANA id.
- `GetTimeZonesByWindowsId` - Returns a list of time zones for a specific Windows id.

#### Extensions

As of v1.0.10, I've added two extension methods on `DateTime` and `DateTimeOffset` to shift an instance of either to a different time zone without changing the time. For example you're in Los Angeles, California but are setting a date time value for a user in Miami, Florida. If you're presented with a `datetime-local` picker in the browser, the server will interpret the value in its local time zone, but it's really for the other user's time zone. The `AtTimeZone` extension method shifts the time zone without changing the value.

#### Why?

I really needed to be able to get the time zone for a specific location coordinate. I was going to use the Google Maps Time Zone API, but at $5.00 per 1000 requests, it was going to become extremely expensive and fast. After a bit of prototyping I pieced together the same functionality for free using the NuGet packages listed above.

#### Time Zones

Here's an example of the full list of time zones returned by the `GetTimeZones` method. The offset varies depending on when you actually pull the list. I personally have a scheduled job using [Hangfire](https://github.com/HangfireIO/Hangfire) to run once a day and pull the list, then update my database with the "latest" offset.

| Abbreviation | IANA Id | Daylight Savings | Offset | Windows Id |
|--------------|---------|------------------|--------|------------|
|GMT|Africa/Abidjan|False|+00:00|Greenwich Standard Time|
|GMT|Africa/Accra|False|+00:00|Greenwich Standard Time|
|EAT|Africa/Addis_Ababa|False|+03:00|E. Africa Standard Time|
|CET|Africa/Algiers|False|+01:00|W. Central Africa Standard Time|
|EAT|Africa/Asmara|False|+03:00|E. Africa Standard Time|
|EAT|Africa/Asmera|False|+03:00|E. Africa Standard Time|
|GMT|Africa/Bamako|False|+00:00|Greenwich Standard Time|
|WAT|Africa/Bangui|False|+01:00|W. Central Africa Standard Time|
|GMT|Africa/Banjul|False|+00:00|Greenwich Standard Time|
|GMT|Africa/Bissau|False|+00:00|Greenwich Standard Time|
|CAT|Africa/Blantyre|False|+02:00|South Africa Standard Time|
|WAT|Africa/Brazzaville|False|+01:00|W. Central Africa Standard Time|
|CAT|Africa/Bujumbura|False|+02:00|South Africa Standard Time|
|EET|Africa/Cairo|False|+02:00|Egypt Standard Time|
|WEST|Africa/Casablanca|True|+01:00|Morocco Standard Time|
|CET|Africa/Ceuta|False|+01:00|Romance Standard Time|
|GMT|Africa/Conakry|False|+00:00|Greenwich Standard Time|
|GMT|Africa/Dakar|False|+00:00|Greenwich Standard Time|
|EAT|Africa/Dar_es_Salaam|False|+03:00|E. Africa Standard Time|
|EAT|Africa/Djibouti|False|+03:00|E. Africa Standard Time|
|WAT|Africa/Douala|False|+01:00|W. Central Africa Standard Time|
|WEST|Africa/El_Aaiun|True|+01:00|Morocco Standard Time|
|GMT|Africa/Freetown|False|+00:00|Greenwich Standard Time|
|CAT|Africa/Gaborone|False|+02:00|South Africa Standard Time|
|CAT|Africa/Harare|False|+02:00|South Africa Standard Time|
|SAST|Africa/Johannesburg|False|+02:00|South Africa Standard Time|
|EAT|Africa/Juba|False|+03:00|E. Africa Standard Time|
|EAT|Africa/Kampala|False|+03:00|E. Africa Standard Time|
|CAT|Africa/Khartoum|False|+02:00|Sudan Standard Time|
|CAT|Africa/Kigali|False|+02:00|South Africa Standard Time|
|WAT|Africa/Kinshasa|False|+01:00|W. Central Africa Standard Time|
|WAT|Africa/Lagos|False|+01:00|W. Central Africa Standard Time|
|WAT|Africa/Libreville|False|+01:00|W. Central Africa Standard Time|
|GMT|Africa/Lome|False|+00:00|Greenwich Standard Time|
|WAT|Africa/Luanda|False|+01:00|W. Central Africa Standard Time|
|CAT|Africa/Lubumbashi|False|+02:00|South Africa Standard Time|
|CAT|Africa/Lusaka|False|+02:00|South Africa Standard Time|
|WAT|Africa/Malabo|False|+01:00|W. Central Africa Standard Time|
|CAT|Africa/Maputo|False|+02:00|South Africa Standard Time|
|SAST|Africa/Maseru|False|+02:00|South Africa Standard Time|
|SAST|Africa/Mbabane|False|+02:00|South Africa Standard Time|
|EAT|Africa/Mogadishu|False|+03:00|E. Africa Standard Time|
|GMT|Africa/Monrovia|False|+00:00|Greenwich Standard Time|
|EAT|Africa/Nairobi|False|+03:00|E. Africa Standard Time|
|WAT|Africa/Ndjamena|False|+01:00|W. Central Africa Standard Time|
|WAT|Africa/Niamey|False|+01:00|W. Central Africa Standard Time|
|GMT|Africa/Nouakchott|False|+00:00|Greenwich Standard Time|
|GMT|Africa/Ouagadougou|False|+00:00|Greenwich Standard Time|
|WAT|Africa/Porto-Novo|False|+01:00|W. Central Africa Standard Time|
|GMT|Africa/Sao_Tome|False|+00:00|Sao Tome Standard Time|
|GMT|Africa/Timbuktu|False|+00:00|Greenwich Standard Time|
|EET|Africa/Tripoli|False|+02:00|Libya Standard Time|
|CET|Africa/Tunis|False|+01:00|W. Central Africa Standard Time|
|CAT|Africa/Windhoek|False|+02:00|Namibia Standard Time|
|HAST|America/Adak|False|-10:00|Aleutian Standard Time|
|AKST|America/Anchorage|False|-09:00|Alaskan Standard Time|
|AST|America/Anguilla|False|-04:00|SA Western Standard Time|
|AST|America/Antigua|False|-04:00|SA Western Standard Time|
|Brazil Standard Time|America/Araguaina|False|-03:00|Tocantins Standard Time|
|Argentina Standard Time|America/Argentina/Buenos_Aires|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/Catamarca|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/ComodRivadavia|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/Cordoba|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/Jujuy|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/La_Rioja|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/Mendoza|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/Rio_Gallegos|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/Salta|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/San_Juan|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/San_Luis|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/Tucuman|False|-03:00|Argentina Standard Time|
|Argentina Standard Time|America/Argentina/Ushuaia|False|-03:00|Argentina Standard Time|
|AST|America/Aruba|False|-04:00|SA Western Standard Time|
|Paraguay Daylight Time|America/Asuncion|True|-03:00|Paraguay Standard Time|
|EST|America/Atikokan|False|-05:00|SA Pacific Standard Time|
|HAST|America/Atka|False|-10:00|Aleutian Standard Time|
|Brazil Standard Time|America/Bahia|False|-03:00|Bahia Standard Time|
|CST|America/Bahia_Banderas|False|-06:00|Central Standard Time (Mexico)|
|AST|America/Barbados|False|-04:00|SA Western Standard Time|
|Brazil Standard Time|America/Belem|False|-03:00|SA Eastern Standard Time|
|CST|America/Belize|False|-06:00|Central America Standard Time|
|AST|America/Blanc-Sablon|False|-04:00|SA Western Standard Time|
|Brazil Standard Time|America/Boa_Vista|False|-04:00|SA Western Standard Time|
|Colombia Standard Time|America/Bogota|False|-05:00|SA Pacific Standard Time|
|MST|America/Boise|False|-07:00|Mountain Standard Time|
|Argentina Standard Time|America/Buenos_Aires|False|-03:00|Argentina Standard Time|
|MST|America/Cambridge_Bay|False|-07:00|Mountain Standard Time|
|Brazil Standard Time|America/Campo_Grande|False|-04:00|Central Brazilian Standard Time|
|EST|America/Cancun|False|-05:00|Eastern Standard Time (Mexico)|
|Venezuela Standard Time|America/Caracas|False|-04:00|Venezuela Standard Time|
|Argentina Standard Time|America/Catamarca|False|-03:00|Argentina Standard Time|
|French Guiana Standard Time|America/Cayenne|False|-03:00|SA Eastern Standard Time|
|EST|America/Cayman|False|-05:00|SA Pacific Standard Time|
|CST|America/Chicago|False|-06:00|Central Standard Time|
|Mexico Standard Time|America/Chihuahua|False|-07:00|Mountain Standard Time (Mexico)|
|EST|America/Coral_Harbour|False|-05:00|SA Pacific Standard Time|
|Argentina Standard Time|America/Cordoba|False|-03:00|Argentina Standard Time|
|CST|America/Costa_Rica|False|-06:00|Central America Standard Time|
|MST|America/Creston|False|-07:00|US Mountain Standard Time|
|Brazil Standard Time|America/Cuiaba|False|-04:00|Central Brazilian Standard Time|
|AST|America/Curacao|False|-04:00|SA Western Standard Time|
|GMT|America/Danmarkshavn|False|+00:00|UTC|
|PST|America/Dawson|False|-08:00|Pacific Standard Time|
|MST|America/Dawson_Creek|False|-07:00|US Mountain Standard Time|
|MST|America/Denver|False|-07:00|Mountain Standard Time|
|EST|America/Detroit|False|-05:00|Eastern Standard Time|
|AST|America/Dominica|False|-04:00|SA Western Standard Time|
|MST|America/Edmonton|False|-07:00|Mountain Standard Time|
|Brazil Standard Time|America/Eirunepe|False|-05:00|SA Pacific Standard Time|
|CST|America/El_Salvador|False|-06:00|Central America Standard Time|
|PST|America/Ensenada|False|-08:00|Pacific Standard Time (Mexico)|
|MST|America/Fort_Nelson|False|-07:00|US Mountain Standard Time|
|EST|America/Fort_Wayne|False|-05:00|US Eastern Standard Time|
|Brazil Standard Time|America/Fortaleza|False|-03:00|SA Eastern Standard Time|
|AST|America/Glace_Bay|False|-04:00|Atlantic Standard Time|
|Greenland Standard Time|America/Godthab|False|-03:00|Greenland Standard Time|
|AST|America/Goose_Bay|False|-04:00|Atlantic Standard Time|
|EST|America/Grand_Turk|False|-05:00|Turks And Caicos Standard Time|
|AST|America/Grenada|False|-04:00|SA Western Standard Time|
|AST|America/Guadeloupe|False|-04:00|SA Western Standard Time|
|CST|America/Guatemala|False|-06:00|Central America Standard Time|
|Ecuador Standard Time|America/Guayaquil|False|-05:00|SA Pacific Standard Time|
|GYT|America/Guyana|False|-04:00|SA Western Standard Time|
|AST|America/Halifax|False|-04:00|Atlantic Standard Time|
|Cuba Standard Time|America/Havana|False|-05:00|Cuba Standard Time|
|Mexico Standard Time|America/Hermosillo|False|-07:00|US Mountain Standard Time|
|EST|America/Indiana/Indianapolis|False|-05:00|US Eastern Standard Time|
|CST|America/Indiana/Knox|False|-06:00|Central Standard Time|
|EST|America/Indiana/Marengo|False|-05:00|US Eastern Standard Time|
|EST|America/Indiana/Petersburg|False|-05:00|Eastern Standard Time|
|CST|America/Indiana/Tell_City|False|-06:00|Central Standard Time|
|EST|America/Indiana/Vevay|False|-05:00|US Eastern Standard Time|
|EST|America/Indiana/Vincennes|False|-05:00|Eastern Standard Time|
|EST|America/Indiana/Winamac|False|-05:00|Eastern Standard Time|
|EST|America/Indianapolis|False|-05:00|US Eastern Standard Time|
|MST|America/Inuvik|False|-07:00|Mountain Standard Time|
|EST|America/Iqaluit|False|-05:00|Eastern Standard Time|
|EST|America/Jamaica|False|-05:00|SA Pacific Standard Time|
|Argentina Standard Time|America/Jujuy|False|-03:00|Argentina Standard Time|
|AKST|America/Juneau|False|-09:00|Alaskan Standard Time|
|EST|America/Kentucky/Louisville|False|-05:00|Eastern Standard Time|
|EST|America/Kentucky/Monticello|False|-05:00|Eastern Standard Time|
|CST|America/Knox_IN|False|-06:00|Central Standard Time|
|AST|America/Kralendijk|False|-04:00|SA Western Standard Time|
|Bolivia Standard Time|America/La_Paz|False|-04:00|SA Western Standard Time|
|Peru Standard Time|America/Lima|False|-05:00|SA Pacific Standard Time|
|PST|America/Los_Angeles|False|-08:00|Pacific Standard Time|
|EST|America/Louisville|False|-05:00|Eastern Standard Time|
|AST|America/Lower_Princes|False|-04:00|SA Western Standard Time|
|Brazil Standard Time|America/Maceio|False|-03:00|SA Eastern Standard Time|
|CST|America/Managua|False|-06:00|Central America Standard Time|
|Brazil Standard Time|America/Manaus|False|-04:00|SA Western Standard Time|
|AST|America/Marigot|False|-04:00|SA Western Standard Time|
|AST|America/Martinique|False|-04:00|SA Western Standard Time|
|CST|America/Matamoros|False|-06:00|Central Standard Time|
|Mexico Standard Time|America/Mazatlan|False|-07:00|Mountain Standard Time (Mexico)|
|Argentina Standard Time|America/Mendoza|False|-03:00|Argentina Standard Time|
|CST|America/Menominee|False|-06:00|Central Standard Time|
|CST|America/Merida|False|-06:00|Central Standard Time (Mexico)|
|AKST|America/Metlakatla|False|-09:00|Alaskan Standard Time|
|CST|America/Mexico_City|False|-06:00|Central Standard Time (Mexico)|
|St. Pierre & Miquelon Standard Time|America/Miquelon|False|-03:00|Saint Pierre Standard Time|
|AST|America/Moncton|False|-04:00|Atlantic Standard Time|
|CST|America/Monterrey|False|-06:00|Central Standard Time (Mexico)|
|Uruguay Standard Time|America/Montevideo|False|-03:00|Montevideo Standard Time|
|EST|America/Montreal|False|-05:00|Eastern Standard Time|
|AST|America/Montserrat|False|-04:00|SA Western Standard Time|
|EST|America/Nassau|False|-05:00|Eastern Standard Time|
|EST|America/New_York|False|-05:00|Eastern Standard Time|
|EST|America/Nipigon|False|-05:00|Eastern Standard Time|
|AKST|America/Nome|False|-09:00|Alaskan Standard Time|
|Brazil Standard Time|America/Noronha|False|-02:00|UTC-02|
|CST|America/North_Dakota/Beulah|False|-06:00|Central Standard Time|
|CST|America/North_Dakota/Center|False|-06:00|Central Standard Time|
|CST|America/North_Dakota/New_Salem|False|-06:00|Central Standard Time|
|MST|America/Ojinaga|False|-07:00|Mountain Standard Time|
|EST|America/Panama|False|-05:00|SA Pacific Standard Time|
|EST|America/Pangnirtung|False|-05:00|Eastern Standard Time|
|Suriname Standard Time|America/Paramaribo|False|-03:00|SA Eastern Standard Time|
|MST|America/Phoenix|False|-07:00|US Mountain Standard Time|
|EST|America/Port-au-Prince|False|-05:00|Haiti Standard Time|
|AST|America/Port_of_Spain|False|-04:00|SA Western Standard Time|
|Brazil Standard Time|America/Porto_Acre|False|-05:00|SA Pacific Standard Time|
|Brazil Standard Time|America/Porto_Velho|False|-04:00|SA Western Standard Time|
|AST|America/Puerto_Rico|False|-04:00|SA Western Standard Time|
|Chile Standard Time|America/Punta_Arenas|False|-03:00|Magallanes Standard Time|
|CST|America/Rainy_River|False|-06:00|Central Standard Time|
|CST|America/Rankin_Inlet|False|-06:00|Central Standard Time|
|Brazil Standard Time|America/Recife|False|-03:00|SA Eastern Standard Time|
|CST|America/Regina|False|-06:00|Canada Central Standard Time|
|CST|America/Resolute|False|-06:00|Central Standard Time|
|Brazil Standard Time|America/Rio_Branco|False|-05:00|SA Pacific Standard Time|
|Argentina Standard Time|America/Rosario|False|-03:00|Argentina Standard Time|
|Santa Isabel Standard Time|America/Santa_Isabel|False|-08:00|Pacific Standard Time (Mexico)|
|Brazil Standard Time|America/Santarem|False|-03:00|SA Eastern Standard Time|
|Chile Daylight Time|America/Santiago|True|-03:00|Pacific SA Standard Time|
|AST|America/Santo_Domingo|False|-04:00|SA Western Standard Time|
|Brazil Standard Time|America/Sao_Paulo|False|-03:00|E. South America Standard Time|
|Greenland Standard Time|America/Scoresbysund|False|-01:00|Azores Standard Time|
|MST|America/Shiprock|False|-07:00|Mountain Standard Time|
|AKST|America/Sitka|False|-09:00|Alaskan Standard Time|
|AST|America/St_Barthelemy|False|-04:00|SA Western Standard Time|
|NST|America/St_Johns|False|-03:30|Newfoundland Standard Time|
|AST|America/St_Kitts|False|-04:00|SA Western Standard Time|
|AST|America/St_Lucia|False|-04:00|SA Western Standard Time|
|AST|America/St_Thomas|False|-04:00|SA Western Standard Time|
|AST|America/St_Vincent|False|-04:00|SA Western Standard Time|
|CST|America/Swift_Current|False|-06:00|Canada Central Standard Time|
|CST|America/Tegucigalpa|False|-06:00|Central America Standard Time|
|AST|America/Thule|False|-04:00|Atlantic Standard Time|
|EST|America/Thunder_Bay|False|-05:00|Eastern Standard Time|
|PST|America/Tijuana|False|-08:00|Pacific Standard Time (Mexico)|
|EST|America/Toronto|False|-05:00|Eastern Standard Time|
|AST|America/Tortola|False|-04:00|SA Western Standard Time|
|PST|America/Vancouver|False|-08:00|Pacific Standard Time|
|AST|America/Virgin|False|-04:00|SA Western Standard Time|
|PST|America/Whitehorse|False|-08:00|Pacific Standard Time|
|CST|America/Winnipeg|False|-06:00|Central Standard Time|
|AKST|America/Yakutat|False|-09:00|Alaskan Standard Time|
|MST|America/Yellowknife|False|-07:00|Mountain Standard Time|
|AWST|Antarctica/Casey|False|+08:00|W. Australia Standard Time|
|Antarctica Standard Time|Antarctica/Davis|False|+07:00|SE Asia Standard Time|
|Antarctica Standard Time|Antarctica/DumontDUrville|False|+10:00|West Pacific Standard Time|
|Australia Standard Time|Antarctica/Macquarie|False|+11:00|Central Pacific Standard Time|
|Antarctica Standard Time|Antarctica/Mawson|False|+05:00|West Asia Standard Time|
|NZDT|Antarctica/McMurdo|True|+13:00|New Zealand Standard Time|
|Antarctica Standard Time|Antarctica/Palmer|False|-03:00|Magallanes Standard Time|
|Antarctica Standard Time|Antarctica/Rothera|False|-03:00|SA Eastern Standard Time|
|NZDT|Antarctica/South_Pole|True|+13:00|New Zealand Standard Time|
|Antarctica Standard Time|Antarctica/Syowa|False|+03:00|E. Africa Standard Time|
|GMT|Antarctica/Troll|False|+00:00||
|Antarctica Standard Time|Antarctica/Vostok|False|+06:00|Central Asia Standard Time|
|CET|Arctic/Longyearbyen|False|+01:00|W. Europe Standard Time|
|Yemen Standard Time|Asia/Aden|False|+03:00|Arab Standard Time|
|Kazakhstan Standard Time|Asia/Almaty|False|+06:00|Central Asia Standard Time|
|EET|Asia/Amman|False|+02:00|Jordan Standard Time|
|Anadyr Standard Time|Asia/Anadyr|False|+12:00|Russia Time Zone 11|
|Kazakhstan Standard Time|Asia/Aqtau|False|+05:00|West Asia Standard Time|
|Kazakhstan Standard Time|Asia/Aqtobe|False|+05:00|West Asia Standard Time|
|Turkmenistan Standard Time|Asia/Ashgabat|False|+05:00|West Asia Standard Time|
|Turkmenistan Standard Time|Asia/Ashkhabad|False|+05:00|West Asia Standard Time|
|Kazakhstan Standard Time|Asia/Atyrau|False|+05:00|West Asia Standard Time|
|Iraq Standard Time|Asia/Baghdad|False|+03:00|Arabic Standard Time|
|Bahrain Standard Time|Asia/Bahrain|False|+03:00|Arab Standard Time|
|Azerbaijan Standard Time|Asia/Baku|False|+04:00|Azerbaijan Standard Time|
|Thailand Standard Time|Asia/Bangkok|False|+07:00|SE Asia Standard Time|
|Barnaul Standard Time|Asia/Barnaul|False|+07:00|Altai Standard Time|
|EET|Asia/Beirut|False|+02:00|Middle East Standard Time|
|Kyrgyzstan Standard Time|Asia/Bishkek|False|+06:00|Central Asia Standard Time|
|Brunei Standard Time|Asia/Brunei|False|+08:00|Singapore Standard Time|
|IST|Asia/Calcutta|False|+05:30|India Standard Time|
|Chita Standard Time|Asia/Chita|False|+09:00|Transbaikal Standard Time|
|Mongolia Standard Time|Asia/Choibalsan|False|+08:00|Ulaanbaatar Standard Time|
|China Standard Time|Asia/Chongqing|False|+08:00|China Standard Time|
|China Standard Time|Asia/Chungking|False|+08:00|China Standard Time|
|IST|Asia/Colombo|False|+05:30|Sri Lanka Standard Time|
|Bangladesh Standard Time|Asia/Dacca|False|+06:00|Bangladesh Standard Time|
|EET|Asia/Damascus|False|+02:00|Syria Standard Time|
|Bangladesh Standard Time|Asia/Dhaka|False|+06:00|Bangladesh Standard Time|
|Timor-Leste Standard Time|Asia/Dili|False|+09:00|Tokyo Standard Time|
|GST|Asia/Dubai|False|+04:00|Arabian Standard Time|
|Tajikistan Standard Time|Asia/Dushanbe|False|+05:00|West Asia Standard Time|
|EET|Asia/Famagusta|False|+02:00|GTB Standard Time|
|EET|Asia/Gaza|False|+02:00|West Bank Standard Time|
|China Standard Time|Asia/Harbin|False|+08:00|China Standard Time|
|EET|Asia/Hebron|False|+02:00|West Bank Standard Time|
|Vietnam Standard Time|Asia/Ho_Chi_Minh|False|+07:00|SE Asia Standard Time|
|HKT|Asia/Hong_Kong|False|+08:00|China Standard Time|
|Mongolia Standard Time|Asia/Hovd|False|+07:00|W. Mongolia Standard Time|
|Irkutsk Standard Time|Asia/Irkutsk|False|+08:00|North Asia East Standard Time|
|Turkey Standard Time|Asia/Istanbul|False|+03:00|Turkey Standard Time|
|Indonesia Standard Time|Asia/Jakarta|False|+07:00|SE Asia Standard Time|
|Indonesia Standard Time|Asia/Jayapura|False|+09:00|Tokyo Standard Time|
|Israel Standard Time|Asia/Jerusalem|False|+02:00|Israel Standard Time|
|Afghanistan Standard Time|Asia/Kabul|False|+04:30|Afghanistan Standard Time|
|Kamchatka Standard Time|Asia/Kamchatka|False|+12:00|Russia Time Zone 11|
|Pakistan Standard Time|Asia/Karachi|False|+05:00|Pakistan Standard Time|
|China Standard Time|Asia/Kashgar|False|+06:00|Central Asia Standard Time|
|Nepal Standard Time|Asia/Kathmandu|False|+05:45|Nepal Standard Time|
|Nepal Standard Time|Asia/Katmandu|False|+05:45|Nepal Standard Time|
|Khandyga Standard Time|Asia/Khandyga|False|+09:00|Yakutsk Standard Time|
|IST|Asia/Kolkata|False|+05:30|India Standard Time|
|Krasnoyarsk Standard Time|Asia/Krasnoyarsk|False|+07:00|North Asia Standard Time|
|MYT|Asia/Kuala_Lumpur|False|+08:00|Singapore Standard Time|
|MYT|Asia/Kuching|False|+08:00|Singapore Standard Time|
|Kuwait Standard Time|Asia/Kuwait|False|+03:00|Arab Standard Time|
|Macao SAR China Standard Time|Asia/Macao|False|+08:00|China Standard Time|
|Macao SAR China Standard Time|Asia/Macau|False|+08:00|China Standard Time|
|Magadan Standard Time|Asia/Magadan|False|+11:00|Magadan Standard Time|
|Indonesia Standard Time|Asia/Makassar|False|+08:00|Singapore Standard Time|
|Philippines Standard Time|Asia/Manila|False|+08:00|Singapore Standard Time|
|GST|Asia/Muscat|False|+04:00|Arabian Standard Time|
|EET|Asia/Nicosia|False|+02:00|GTB Standard Time|
|Novokuznetsk Standard Time|Asia/Novokuznetsk|False|+07:00|North Asia Standard Time|
|Novosibirsk Standard Time|Asia/Novosibirsk|False|+07:00|N. Central Asia Standard Time|
|Omsk Standard Time|Asia/Omsk|False|+06:00|Omsk Standard Time|
|Kazakhstan Standard Time|Asia/Oral|False|+05:00|West Asia Standard Time|
|Cambodia Standard Time|Asia/Phnom_Penh|False|+07:00|SE Asia Standard Time|
|Indonesia Standard Time|Asia/Pontianak|False|+07:00|SE Asia Standard Time|
|North Korea Standard Time|Asia/Pyongyang|False|+09:00|North Korea Standard Time|
|Qatar Standard Time|Asia/Qatar|False|+03:00|Arab Standard Time|
|Kazakhstan Standard Time|Asia/Qostanay|False|+06:00|Central Asia Standard Time|
|Kazakhstan Standard Time|Asia/Qyzylorda|False|+05:00|Qyzylorda Standard Time|
|Myanmar (Burma) Standard Time|Asia/Rangoon|False|+06:30|Myanmar Standard Time|
|Saudi Arabia Standard Time|Asia/Riyadh|False|+03:00|Arab Standard Time|
|Vietnam Standard Time|Asia/Saigon|False|+07:00|SE Asia Standard Time|
|Sakhalin Standard Time|Asia/Sakhalin|False|+11:00|Sakhalin Standard Time|
|Uzbekistan Standard Time|Asia/Samarkand|False|+05:00|West Asia Standard Time|
|South Korea Standard Time|Asia/Seoul|False|+09:00|Korea Standard Time|
|China Standard Time|Asia/Shanghai|False|+08:00|China Standard Time|
|SGT|Asia/Singapore|False|+08:00|Singapore Standard Time|
|Srednekolymsk Standard Time|Asia/Srednekolymsk|False|+11:00|Russia Time Zone 10|
|Taiwan Standard Time|Asia/Taipei|False|+08:00|Taipei Standard Time|
|Uzbekistan Standard Time|Asia/Tashkent|False|+05:00|West Asia Standard Time|
|Georgia Standard Time|Asia/Tbilisi|False|+04:00|Georgian Standard Time|
|Iran Standard Time|Asia/Tehran|False|+03:30|Iran Standard Time|
|Israel Standard Time|Asia/Tel_Aviv|False|+02:00|Israel Standard Time|
|Bhutan Standard Time|Asia/Thimbu|False|+06:00|Bangladesh Standard Time|
|Bhutan Standard Time|Asia/Thimphu|False|+06:00|Bangladesh Standard Time|
|Japan Standard Time|Asia/Tokyo|False|+09:00|Tokyo Standard Time|
|Tomsk Standard Time|Asia/Tomsk|False|+07:00|Tomsk Standard Time|
|Indonesia Standard Time|Asia/Ujung_Pandang|False|+08:00|Singapore Standard Time|
|Mongolia Standard Time|Asia/Ulaanbaatar|False|+08:00|Ulaanbaatar Standard Time|
|Mongolia Standard Time|Asia/Ulan_Bator|False|+08:00|Ulaanbaatar Standard Time|
|China Standard Time|Asia/Urumqi|False|+06:00|Central Asia Standard Time|
|Ust-Nera Standard Time|Asia/Ust-Nera|False|+10:00|Vladivostok Standard Time|
|Laos Standard Time|Asia/Vientiane|False|+07:00|SE Asia Standard Time|
|Vladivostok Standard Time|Asia/Vladivostok|False|+10:00|Vladivostok Standard Time|
|Yakutsk Standard Time|Asia/Yakutsk|False|+09:00|Yakutsk Standard Time|
|Myanmar (Burma) Standard Time|Asia/Yangon|False|+06:30|Myanmar Standard Time|
|Yekaterinburg Standard Time|Asia/Yekaterinburg|False|+05:00|Ekaterinburg Standard Time|
|Armenia Standard Time|Asia/Yerevan|False|+04:00|Caucasus Standard Time|
|Portugal Standard Time|Atlantic/Azores|False|-01:00|Azores Standard Time|
|AST|Atlantic/Bermuda|False|-04:00|Atlantic Standard Time|
|WET|Atlantic/Canary|False|+00:00|GMT Standard Time|
|Cape Verde Standard Time|Atlantic/Cape_Verde|False|-01:00|Cape Verde Standard Time|
|WET|Atlantic/Faeroe|False|+00:00|GMT Standard Time|
|WET|Atlantic/Faroe|False|+00:00|GMT Standard Time|
|CET|Atlantic/Jan_Mayen|False|+01:00|W. Europe Standard Time|
|WET|Atlantic/Madeira|False|+00:00|GMT Standard Time|
|GMT|Atlantic/Reykjavik|False|+00:00|Greenwich Standard Time|
|South Georgia & South Sandwich Islands Standard Time|Atlantic/South_Georgia|False|-02:00|UTC-02|
|GMT|Atlantic/St_Helena|False|+00:00|Greenwich Standard Time|
|Falkland Islands Standard Time|Atlantic/Stanley|False|-03:00|SA Eastern Standard Time|
|AEDT|Australia/ACT|True|+11:00|AUS Eastern Standard Time|
|ACDT|Australia/Adelaide|True|+10:30|Cen. Australia Standard Time|
|AEST|Australia/Brisbane|False|+10:00|E. Australia Standard Time|
|ACDT|Australia/Broken_Hill|True|+10:30|Cen. Australia Standard Time|
|AEDT|Australia/Canberra|True|+11:00|AUS Eastern Standard Time|
|AEDT|Australia/Currie|True|+11:00|Tasmania Standard Time|
|ACST|Australia/Darwin|False|+09:30|AUS Central Standard Time|
|ACWST|Australia/Eucla|False|+08:45|Aus Central W. Standard Time|
|AEDT|Australia/Hobart|True|+11:00|Tasmania Standard Time|
|LHDT|Australia/LHI|True|+11:00|Lord Howe Standard Time|
|AEST|Australia/Lindeman|False|+10:00|E. Australia Standard Time|
|LHDT|Australia/Lord_Howe|True|+11:00|Lord Howe Standard Time|
|AEDT|Australia/Melbourne|True|+11:00|AUS Eastern Standard Time|
|AEDT|Australia/NSW|True|+11:00|AUS Eastern Standard Time|
|ACST|Australia/North|False|+09:30|AUS Central Standard Time|
|AWST|Australia/Perth|False|+08:00|W. Australia Standard Time|
|AEST|Australia/Queensland|False|+10:00|E. Australia Standard Time|
|ACDT|Australia/South|True|+10:30|Cen. Australia Standard Time|
|AEDT|Australia/Sydney|True|+11:00|AUS Eastern Standard Time|
|AEDT|Australia/Tasmania|True|+11:00|Tasmania Standard Time|
|AEDT|Australia/Victoria|True|+11:00|AUS Eastern Standard Time|
|AWST|Australia/West|False|+08:00|W. Australia Standard Time|
|ACDT|Australia/Yancowinna|True|+10:30|Cen. Australia Standard Time|
|Brazil Standard Time|Brazil/Acre|False|-05:00|SA Pacific Standard Time|
|Brazil Standard Time|Brazil/DeNoronha|False|-02:00|UTC-02|
|Brazil Standard Time|Brazil/East|False|-03:00|E. South America Standard Time|
|Brazil Standard Time|Brazil/West|False|-04:00|SA Western Standard Time|
|CET|CET|False|+01:00|Romance Standard Time|
|CST|CST6CDT|False|-06:00|Central Standard Time|
|AST|Canada/Atlantic|False|-04:00|Atlantic Standard Time|
|CST|Canada/Central|False|-06:00|Central Standard Time|
|EST|Canada/Eastern|False|-05:00|Eastern Standard Time|
|MST|Canada/Mountain|False|-07:00|Mountain Standard Time|
|NST|Canada/Newfoundland|False|-03:30|Newfoundland Standard Time|
|PST|Canada/Pacific|False|-08:00|Pacific Standard Time|
|CST|Canada/Saskatchewan|False|-06:00|Canada Central Standard Time|
|PST|Canada/Yukon|False|-08:00|Pacific Standard Time|
|Chile Daylight Time|Chile/Continental|True|-03:00|Pacific SA Standard Time|
|Chile Daylight Time|Chile/EasterIsland|True|-05:00|Easter Island Standard Time|
|Cuba Standard Time|Cuba|False|-05:00|Cuba Standard Time|
|EET|EET|False|+02:00|GTB Standard Time|
|UTC Standard Time-5|EST|False|-05:00|SA Pacific Standard Time|
|EST|EST5EDT|False|-05:00|Eastern Standard Time|
|EET|Egypt|False|+02:00|Egypt Standard Time|
|GMT|Eire|False|+00:00|GMT Standard Time|
|UTC|Etc/GMT|False|+00:00|UTC|
|UTC|Etc/GMT+0|False|+00:00|UTC|
|UTC Standard Time-1|Etc/GMT+1|False|-01:00|Cape Verde Standard Time|
|UTC Standard Time-10|Etc/GMT+10|False|-10:00|Hawaiian Standard Time|
|UTC Standard Time-11|Etc/GMT+11|False|-11:00|UTC-11|
|UTC Standard Time-12|Etc/GMT+12|False|-12:00|Dateline Standard Time|
|UTC Standard Time-2|Etc/GMT+2|False|-02:00|UTC-02|
|UTC Standard Time-3|Etc/GMT+3|False|-03:00|SA Eastern Standard Time|
|UTC Standard Time-4|Etc/GMT+4|False|-04:00|SA Western Standard Time|
|UTC Standard Time-5|Etc/GMT+5|False|-05:00|SA Pacific Standard Time|
|UTC Standard Time-6|Etc/GMT+6|False|-06:00|Central America Standard Time|
|UTC Standard Time-7|Etc/GMT+7|False|-07:00|US Mountain Standard Time|
|UTC Standard Time-8|Etc/GMT+8|False|-08:00|UTC-08|
|UTC Standard Time-9|Etc/GMT+9|False|-09:00|UTC-09|
|UTC|Etc/GMT-0|False|+00:00|UTC|
|UTC Standard Time+1|Etc/GMT-1|False|+01:00|W. Central Africa Standard Time|
|UTC Standard Time+10|Etc/GMT-10|False|+10:00|West Pacific Standard Time|
|UTC Standard Time+11|Etc/GMT-11|False|+11:00|Central Pacific Standard Time|
|UTC Standard Time+12|Etc/GMT-12|False|+12:00|UTC+12|
|UTC Standard Time+13|Etc/GMT-13|False|+13:00|UTC+13|
|UTC Standard Time+14|Etc/GMT-14|False|+14:00|Line Islands Standard Time|
|UTC Standard Time+2|Etc/GMT-2|False|+02:00|South Africa Standard Time|
|UTC Standard Time+3|Etc/GMT-3|False|+03:00|E. Africa Standard Time|
|UTC Standard Time+4|Etc/GMT-4|False|+04:00|Arabian Standard Time|
|UTC Standard Time+5|Etc/GMT-5|False|+05:00|West Asia Standard Time|
|UTC Standard Time+6|Etc/GMT-6|False|+06:00|Central Asia Standard Time|
|UTC Standard Time+7|Etc/GMT-7|False|+07:00|SE Asia Standard Time|
|UTC Standard Time+8|Etc/GMT-8|False|+08:00|Singapore Standard Time|
|UTC Standard Time+9|Etc/GMT-9|False|+09:00|Tokyo Standard Time|
|UTC|Etc/GMT0|False|+00:00|UTC|
|UTC|Etc/Greenwich|False|+00:00|UTC|
|UTC Standard Time|Etc/UCT|False|+00:00|UTC|
|UTC Standard Time|Etc/UTC|False|+00:00|UTC|
|UTC Standard Time|Etc/Universal|False|+00:00|UTC|
|UTC Standard Time|Etc/Zulu|False|+00:00|UTC|
|CET|Europe/Amsterdam|False|+01:00|W. Europe Standard Time|
|CET|Europe/Andorra|False|+01:00|W. Europe Standard Time|
|Astrakhan Standard Time|Europe/Astrakhan|False|+04:00|Astrakhan Standard Time|
|EET|Europe/Athens|False|+02:00|GTB Standard Time|
|GMT|Europe/Belfast|False|+00:00|GMT Standard Time|
|CET|Europe/Belgrade|False|+01:00|Central Europe Standard Time|
|CET|Europe/Berlin|False|+01:00|W. Europe Standard Time|
|CET|Europe/Bratislava|False|+01:00|Central Europe Standard Time|
|CET|Europe/Brussels|False|+01:00|Romance Standard Time|
|EET|Europe/Bucharest|False|+02:00|GTB Standard Time|
|CET|Europe/Budapest|False|+01:00|Central Europe Standard Time|
|CET|Europe/Busingen|False|+01:00|W. Europe Standard Time|
|EET|Europe/Chisinau|False|+02:00|E. Europe Standard Time|
|CET|Europe/Copenhagen|False|+01:00|Romance Standard Time|
|GMT|Europe/Dublin|False|+00:00|GMT Standard Time|
|CET|Europe/Gibraltar|False|+01:00|W. Europe Standard Time|
|GMT|Europe/Guernsey|False|+00:00|GMT Standard Time|
|EET|Europe/Helsinki|False|+02:00|FLE Standard Time|
|GMT|Europe/Isle_of_Man|False|+00:00|GMT Standard Time|
|Turkey Standard Time|Europe/Istanbul|False|+03:00|Turkey Standard Time|
|GMT|Europe/Jersey|False|+00:00|GMT Standard Time|
|Kaliningrad Standard Time|Europe/Kaliningrad|False|+02:00|Kaliningrad Standard Time|
|EET|Europe/Kiev|False|+02:00|FLE Standard Time|
|Kirov Standard Time|Europe/Kirov|False|+03:00|Russian Standard Time|
|WET|Europe/Lisbon|False|+00:00|GMT Standard Time|
|CET|Europe/Ljubljana|False|+01:00|Central Europe Standard Time|
|GMT|Europe/London|False|+00:00|GMT Standard Time|
|CET|Europe/Luxembourg|False|+01:00|W. Europe Standard Time|
|CET|Europe/Madrid|False|+01:00|Romance Standard Time|
|CET|Europe/Malta|False|+01:00|W. Europe Standard Time|
|EET|Europe/Mariehamn|False|+02:00|FLE Standard Time|
|Belarus Standard Time|Europe/Minsk|False|+03:00|Belarus Standard Time|
|CET|Europe/Monaco|False|+01:00|W. Europe Standard Time|
|Moscow Standard Time|Europe/Moscow|False|+03:00|Russian Standard Time|
|EET|Europe/Nicosia|False|+02:00|GTB Standard Time|
|CET|Europe/Oslo|False|+01:00|W. Europe Standard Time|
|CET|Europe/Paris|False|+01:00|Romance Standard Time|
|CET|Europe/Podgorica|False|+01:00|Central Europe Standard Time|
|CET|Europe/Prague|False|+01:00|Central Europe Standard Time|
|EET|Europe/Riga|False|+02:00|FLE Standard Time|
|CET|Europe/Rome|False|+01:00|W. Europe Standard Time|
|Samara Standard Time|Europe/Samara|False|+04:00|Russia Time Zone 3|
|CET|Europe/San_Marino|False|+01:00|W. Europe Standard Time|
|CET|Europe/Sarajevo|False|+01:00|Central Europe Standard Time|
|Saratov Standard Time|Europe/Saratov|False|+04:00|Saratov Standard Time|
|Simferopol Standard Time|Europe/Simferopol|False|+03:00|Russian Standard Time|
|CET|Europe/Skopje|False|+01:00|Central Europe Standard Time|
|EET|Europe/Sofia|False|+02:00|FLE Standard Time|
|CET|Europe/Stockholm|False|+01:00|W. Europe Standard Time|
|EET|Europe/Tallinn|False|+02:00|FLE Standard Time|
|CET|Europe/Tirane|False|+01:00|Central Europe Standard Time|
|EET|Europe/Tiraspol|False|+02:00|E. Europe Standard Time|
|Ulyanovsk Standard Time|Europe/Ulyanovsk|False|+04:00|Astrakhan Standard Time|
|EET|Europe/Uzhgorod|False|+02:00|FLE Standard Time|
|CET|Europe/Vaduz|False|+01:00|W. Europe Standard Time|
|CET|Europe/Vatican|False|+01:00|W. Europe Standard Time|
|CET|Europe/Vienna|False|+01:00|W. Europe Standard Time|
|EET|Europe/Vilnius|False|+02:00|FLE Standard Time|
|Volgograd Standard Time|Europe/Volgograd|False|+04:00|Volgograd Standard Time|
|CET|Europe/Warsaw|False|+01:00|Central European Standard Time|
|CET|Europe/Zagreb|False|+01:00|Central Europe Standard Time|
|EET|Europe/Zaporozhye|False|+02:00|FLE Standard Time|
|CET|Europe/Zurich|False|+01:00|W. Europe Standard Time|
|GMT|GB|False|+00:00|GMT Standard Time|
|GMT|GB-Eire|False|+00:00|GMT Standard Time|
|UTC|GMT|False|+00:00|UTC|
|UTC|GMT+0|False|+00:00|UTC|
|UTC|GMT-0|False|+00:00|UTC|
|UTC|GMT0|False|+00:00|UTC|
|UTC|Greenwich|False|+00:00|UTC|
|UTC Standard Time-10|HST|False|-10:00|Hawaiian Standard Time|
|HKT|Hongkong|False|+08:00|China Standard Time|
|GMT|Iceland|False|+00:00|Greenwich Standard Time|
|EAT|Indian/Antananarivo|False|+03:00|E. Africa Standard Time|
|British Indian Ocean Territory Standard Time|Indian/Chagos|False|+06:00|Central Asia Standard Time|
|Christmas Island Standard Time|Indian/Christmas|False|+07:00|SE Asia Standard Time|
|Cocos (Keeling) Islands Standard Time|Indian/Cocos|False|+06:30|Myanmar Standard Time|
|EAT|Indian/Comoro|False|+03:00|E. Africa Standard Time|
|French Southern Territories Standard Time|Indian/Kerguelen|False|+05:00|West Asia Standard Time|
|Seychelles Standard Time|Indian/Mahe|False|+04:00|Mauritius Standard Time|
|Maldives Standard Time|Indian/Maldives|False|+05:00|West Asia Standard Time|
|Mauritius Standard Time|Indian/Mauritius|False|+04:00|Mauritius Standard Time|
|EAT|Indian/Mayotte|False|+03:00|E. Africa Standard Time|
|Réunion Standard Time|Indian/Reunion|False|+04:00|Mauritius Standard Time|
|Iran Standard Time|Iran|False|+03:30|Iran Standard Time|
|Israel Standard Time|Israel|False|+02:00|Israel Standard Time|
|EST|Jamaica|False|-05:00|SA Pacific Standard Time|
|Japan Standard Time|Japan|False|+09:00|Tokyo Standard Time|
|Marshall Islands Standard Time|Kwajalein|False|+12:00|UTC+12|
|EET|Libya|False|+02:00|Libya Standard Time|
|CET|MET|False|+01:00|W. Europe Standard Time|
|UTC Standard Time-7|MST|False|-07:00|US Mountain Standard Time|
|MST|MST7MDT|False|-07:00|Mountain Standard Time|
|PST|Mexico/BajaNorte|False|-08:00|Pacific Standard Time (Mexico)|
|Mexico Standard Time|Mexico/BajaSur|False|-07:00|Mountain Standard Time (Mexico)|
|CST|Mexico/General|False|-06:00|Central Standard Time (Mexico)|
|NZDT|NZ|True|+13:00|New Zealand Standard Time|
|CHADT|NZ-CHAT|True|+13:45|Chatham Islands Standard Time|
|MST|Navajo|False|-07:00|Mountain Standard Time|
|China Standard Time|PRC|False|+08:00|China Standard Time|
|PST|PST8PDT|False|-08:00|Pacific Standard Time|
|Samoa Daylight Time|Pacific/Apia|True|+14:00|Samoa Standard Time|
|NZDT|Pacific/Auckland|True|+13:00|New Zealand Standard Time|
|Papua New Guinea Standard Time|Pacific/Bougainville|False|+11:00|Bougainville Standard Time|
|CHADT|Pacific/Chatham|True|+13:45|Chatham Islands Standard Time|
|Micronesia Standard Time|Pacific/Chuuk|False|+10:00|West Pacific Standard Time|
|Chile Daylight Time|Pacific/Easter|True|-05:00|Easter Island Standard Time|
|Vanuatu Standard Time|Pacific/Efate|False|+11:00|Central Pacific Standard Time|
|Kiribati Standard Time|Pacific/Enderbury|False|+13:00|UTC+13|
|Tokelau Standard Time|Pacific/Fakaofo|False|+13:00|UTC+13|
|Fiji Daylight Time|Pacific/Fiji|True|+13:00|Fiji Standard Time|
|Tuvalu Standard Time|Pacific/Funafuti|False|+12:00|UTC+12|
|Ecuador Standard Time|Pacific/Galapagos|False|-06:00|Central America Standard Time|
|French Polynesia Standard Time|Pacific/Gambier|False|-09:00|UTC-09|
|Solomon Islands Standard Time|Pacific/Guadalcanal|False|+11:00|Central Pacific Standard Time|
|ChST|Pacific/Guam|False|+10:00|West Pacific Standard Time|
|HST|Pacific/Honolulu|False|-10:00|Hawaiian Standard Time|
|HAST|Pacific/Johnston|False|-10:00|Hawaiian Standard Time|
|Kiribati Standard Time|Pacific/Kiritimati|False|+14:00|Line Islands Standard Time|
|Micronesia Standard Time|Pacific/Kosrae|False|+11:00|Central Pacific Standard Time|
|Marshall Islands Standard Time|Pacific/Kwajalein|False|+12:00|UTC+12|
|Marshall Islands Standard Time|Pacific/Majuro|False|+12:00|UTC+12|
|French Polynesia Standard Time|Pacific/Marquesas|False|-09:30|Marquesas Standard Time|
|U.S. Outlying Islands Standard Time|Pacific/Midway|False|-11:00|UTC-11|
|Nauru Standard Time|Pacific/Nauru|False|+12:00|UTC+12|
|Niue Standard Time|Pacific/Niue|False|-11:00|UTC-11|
|Norfolk Island Daylight Time|Pacific/Norfolk|True|+12:00|Norfolk Standard Time|
|New Caledonia Standard Time|Pacific/Noumea|False|+11:00|Central Pacific Standard Time|
|American Samoa Standard Time|Pacific/Pago_Pago|False|-11:00|UTC-11|
|Palau Standard Time|Pacific/Palau|False|+09:00|Tokyo Standard Time|
|Pitcairn Islands Standard Time|Pacific/Pitcairn|False|-08:00|UTC-08|
|Micronesia Standard Time|Pacific/Pohnpei|False|+11:00|Central Pacific Standard Time|
|Micronesia Standard Time|Pacific/Ponape|False|+11:00|Central Pacific Standard Time|
|Papua New Guinea Standard Time|Pacific/Port_Moresby|False|+10:00|West Pacific Standard Time|
|Cook Islands Standard Time|Pacific/Rarotonga|False|-10:00|Hawaiian Standard Time|
|ChST|Pacific/Saipan|False|+10:00|West Pacific Standard Time|
|American Samoa Standard Time|Pacific/Samoa|False|-11:00|UTC-11|
|French Polynesia Standard Time|Pacific/Tahiti|False|-10:00|Hawaiian Standard Time|
|Kiribati Standard Time|Pacific/Tarawa|False|+12:00|UTC+12|
|Tonga Standard Time|Pacific/Tongatapu|False|+13:00|Tonga Standard Time|
|Micronesia Standard Time|Pacific/Truk|False|+10:00|West Pacific Standard Time|
|U.S. Outlying Islands Standard Time|Pacific/Wake|False|+12:00|UTC+12|
|Wallis & Futuna Standard Time|Pacific/Wallis|False|+12:00|UTC+12|
|Micronesia Standard Time|Pacific/Yap|False|+10:00|West Pacific Standard Time|
|CET|Poland|False|+01:00|Central European Standard Time|
|WET|Portugal|False|+00:00|GMT Standard Time|
|Taiwan Standard Time|ROC|False|+08:00|Taipei Standard Time|
|South Korea Standard Time|ROK|False|+09:00|Korea Standard Time|
|SGT|Singapore|False|+08:00|Singapore Standard Time|
|Turkey Standard Time|Turkey|False|+03:00|Turkey Standard Time|
|UTC Standard Time|UCT|False|+00:00|UTC|
|AKST|US/Alaska|False|-09:00|Alaskan Standard Time|
|HAST|US/Aleutian|False|-10:00|Aleutian Standard Time|
|MST|US/Arizona|False|-07:00|US Mountain Standard Time|
|CST|US/Central|False|-06:00|Central Standard Time|
|EST|US/East-Indiana|False|-05:00|US Eastern Standard Time|
|EST|US/Eastern|False|-05:00|Eastern Standard Time|
|HST|US/Hawaii|False|-10:00|Hawaiian Standard Time|
|CST|US/Indiana-Starke|False|-06:00|Central Standard Time|
|EST|US/Michigan|False|-05:00|Eastern Standard Time|
|MST|US/Mountain|False|-07:00|Mountain Standard Time|
|PST|US/Pacific|False|-08:00|Pacific Standard Time|
|PST|US/Pacific-New|False|-08:00|Pacific Standard Time|
|American Samoa Standard Time|US/Samoa|False|-11:00|UTC-11|
|UTC Standard Time|UTC|False|+00:00|UTC|
|UTC Standard Time|Universal|False|+00:00|UTC|
|Moscow Standard Time|W-SU|False|+03:00|Russian Standard Time|
|WET|WET|False|+00:00|GMT Standard Time|
|UTC Standard Time|Zulu|False|+00:00|UTC|
