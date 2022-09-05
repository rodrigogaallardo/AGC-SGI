//$(document).ready(function () {
//    var brw = new browser();
//    alert('fullName: ' + brw.fullName + '\n' + 'name: ' + brw.name + '\n' + 'fullVersion: ' + brw.fullVersion + '\n' + 'version: ' + brw.version + '\n' + 'platform: ' + brw.platform + '\n' + 'mobile: ' + brw.mobile + '\n' + 'resolution: ' + brw.width + 'x' + brw.height);

//});

function browser() {
    // ---- public properties -----
    this.fullName = 'unknow'; // getName(false);
    this.name = 'unknow'; // getName(true);
    this.code = 'unknow'; // getCodeName(this.name);
    this.fullVersion = 'unknow'; // getVersion(this.name);
    this.version = 'unknow'; // getBasicVersion(this.fullVersion);
    this.mobile = false; // isMobile(navigator.userAgent);
    this.width = screen.width;
    this.height = screen.height;
    this.platform = 'unknow'; //getPlatform(navigator.userAgent);

    // ------- init -------    
    this.init = function () { //operative system, is an auxiliary var, for special-cases
        //the first var is the string that will be found in userAgent. the Second var is the common name
        // IMPORTANT NOTE: define new navigators BEFORE firefox, chrome and safari
        var navs = [
            { name: 'Opera Mobi', fullName: 'Opera Mobile', pre: 'Version/', cond1: 'Opera Mobi', cond2: 'Opera Mobi' },
            { name: 'Opera Mini', fullName: 'Opera Mini', pre: 'Version/', cond1: 'Opera Mini', cond2: 'Opera Mini' },
            { name: 'Opera', fullName: 'Opera', pre: 'Version/', cond1: 'Opera', cond2: 'Opera' },
            { name: 'MSIE', fullName: 'Microsoft Internet Explorer', pre: 'MSIE ', cond1: 'MSIE', cond2: 'MSIE' },
            { name: 'MSIE', fullName: 'Microsoft Internet Explorer', pre: 'rv:', cond1: 'Trident', cond2: 'rv:11' },
            { name: 'BlackBerry', fullName: 'BlackBerry Navigator', pre: '/', cond1: 'BlackBerry', cond2: 'BlackBerry' },
            { name: 'BrowserNG', fullName: 'Nokia Navigator', pre: 'BrowserNG/', cond1: 'BrowserNG', cond2: 'BrowserNG' },
            { name: 'Midori', fullName: 'Midori', pre: 'Midori/', cond1: 'Midori', cond2: 'Midori' },
            { name: 'Kazehakase', fullName: 'Kazehakase', pre: 'Kazehakase/', cond1: 'Kazehakase', cond2: 'Kazehakase' },
            { name: 'Chromium', fullName: 'Chromium', pre: 'Chromium/', cond1: 'Chromium', cond2: 'Chromium' },
            { name: 'Flock', fullName: 'Flock', pre: 'Flock/', cond1: 'Flock', cond2: 'Flock' },
            { name: 'Galeon', fullName: 'Galeon', pre: 'Galeon/', cond1: 'Galeon', cond2: 'Galeon' },
            { name: 'RockMelt', fullName: 'RockMelt', pre: 'RockMelt/', cond1: 'RockMelt', cond2: 'RockMelt' },
            { name: 'Fennec', fullName: 'Fennec', pre: 'Fennec/', cond1: 'Fennec', cond2: 'Fennec' },
            { name: 'Konqueror', fullName: 'Konqueror', pre: 'Konqueror/', cond1: 'Konqueror', cond2: 'Konqueror' },
            { name: 'Arora', fullName: 'Arora', pre: 'Arora/', cond1: 'Arora', cond2: 'Arora' },
            { name: 'Swiftfox', fullName: 'Swiftfox', pre: 'Firefox/', cond1: 'Swiftfox', cond2: 'Swiftfox' },
            { name: 'Maxthon', fullName: 'Maxthon', pre: 'Maxthon/', cond1: 'Maxthon', cond2: 'Maxthon' },
            // { name:'', fullName:'', pre:'' } //add new broswers
            // { name:'', fullName:'', pre:'' }
            { name: 'Firefox', fullName: 'Mozilla Firefox', pre: 'Firefox/', cond1: 'Firefox', cond2: 'Firefox' },
            { name: 'Chrome', fullName: 'Google Chrome', pre: 'Chrome/', cond1: 'Chrome', cond2: 'Chrome' },
            { name: 'Safari', fullName: 'Apple Safari', pre: 'Version/', cond1: 'Safari', cond2: 'Safari' }
        ];

        var agent = navigator.userAgent, pre;

        //string IE 11.0
        //agent = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";
        ////set names

        for (i in navs) {
            if (agent.indexOf(navs[i].cond1) > -1 && agent.indexOf(navs[i].cond2) > -1) {
                pre = navs[i].pre;
                this.name = navs[i].name.toLowerCase(); //the code name is always lowercase
                this.fullName = navs[i].fullName;
                //if (this.name == 'msie') this.name = 'msie';
                if (this.name == 'opera mobi') this.name = 'opera';
                if (this.name == 'opera mini') this.name = 'opera';
                break; //when found it, stops reading
            }
        }//for


        //set version
        if ((idx = agent.indexOf(pre)) > -1) {
            this.fullVersion = '';
            this.version = '';
            var nDots = 0;
            var len = agent.length;
            var indexVersion = idx + pre.length;
            for (j = indexVersion; j < len; j++) {
                var n = agent.charCodeAt(j);
                if ((n >= 48 && n <= 57) || n == 46) { //looking for numbers and dots
                    if (n == 46) nDots++;
                    if (nDots < 2) this.version += agent.charAt(j);
                    this.fullVersion += agent.charAt(j);
                } else j = len; //finish sub-cycle
            }//for
            this.version = parseInt(this.version);
        }

        // set Mobile
        var mobiles = ['mobi', 'mobile', 'mini', 'iphone', 'ipod', 'ipad', 'android', 'blackberry'];
        for (var i in mobiles) {
            if (agent.indexOf(mobiles[i]) > -1) this.mobile = true;
        }
        if (this.width < 700 || this.height < 600) this.mobile = true;

        // set Platform        
        var plat = navigator.platform;
        if (plat == 'Win32' || plat == 'Win64') this.platform = 'Windows';
        if (agent.indexOf('NT 5.1') != -1) this.platform = 'Windows XP';
        if (agent.indexOf('NT 6') != -1) this.platform = 'Windows Vista';
        if (agent.indexOf('NT 6.1') != -1) this.platform = 'Windows 7';
        if (agent.indexOf('Mac') != -1) this.platform = 'Macintosh';
        if (agent.indexOf('Linux') != -1) this.platform = 'Linux';
        if (agent.indexOf('iPhone') != -1) this.platform = 'iOS iPhone';
        if (agent.indexOf('iPod') != -1) this.platform = 'iOS iPod';
        if (agent.indexOf('iPad') != -1) this.platform = 'iOS iPad';
        if (agent.indexOf('Android') != -1) this.platform = 'Android';

        if (this.name != 'unknow') {
            this.code = this.name + '';
            if (this.name == 'opera') this.code = 'op';
            if (this.name == 'firefox') this.code = 'ff';
            if (this.name == 'chrome') this.code = 'ch';
            if (this.name == 'safari') this.code = 'sf';
            if (this.name == 'iexplorer') this.code = 'ie';
            if (this.name == 'maxthon') this.code = 'mx';
        }

        //manual filter, when is so hard to define the navigator type
        if (this.name == 'safari' && this.platform == 'Linux') {
            this.name = 'unknow';
            this.fullName = 'unknow';
            this.code = 'unknow';
        }

    };//function


    //function esIE() {
    //    return esIEObsoleto() || esIE11();
    //};//function

    //function esIEObsoleto() {
    //    return !!(navigator.userAgent.match("/MSIE/"));
    //};//function

    //function esIE11(agent) {
    //    return !!(
    //        agent.match("/Trident/") && agent.match("/rv:11/")
    //    );
    //};//function

    this.init();

}//Browser class

