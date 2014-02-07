// Google Analytics
(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
})(window,document,'script','//www.google-analytics.com/analytics.js','ga');

ga('create', 'UA-47261481-1', 'billcacy.github.io');
ga('send', 'pageview');

$( document ).ready(function() {
  
  /**
  *
  *  Base64 encode / decode
  *  http://www.webtoolkit.info/
  *
  **/

  var Base64 = {

      // private property
      _keyStr : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",

      // public method for encoding
      encode : function (input) {
          var output = "";
          var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
          var i = 0;

          input = Base64._utf8_encode(input);

          while (i < input.length) {

              chr1 = input.charCodeAt(i++);
              chr2 = input.charCodeAt(i++);
              chr3 = input.charCodeAt(i++);

              enc1 = chr1 >> 2;
              enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
              enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
              enc4 = chr3 & 63;

              if (isNaN(chr2)) {
                  enc3 = enc4 = 64;
              } else if (isNaN(chr3)) {
                  enc4 = 64;
              }

              output = output +
              this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
              this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

          }

          return output;
      },

      // public method for decoding
      decode : function (input) {
          var output = "";
          var chr1, chr2, chr3;
          var enc1, enc2, enc3, enc4;
          var i = 0;

          input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

          while (i < input.length) {

              enc1 = this._keyStr.indexOf(input.charAt(i++));
              enc2 = this._keyStr.indexOf(input.charAt(i++));
              enc3 = this._keyStr.indexOf(input.charAt(i++));
              enc4 = this._keyStr.indexOf(input.charAt(i++));

              chr1 = (enc1 << 2) | (enc2 >> 4);
              chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
              chr3 = ((enc3 & 3) << 6) | enc4;

              output = output + String.fromCharCode(chr1);

              if (enc3 != 64) {
                  output = output + String.fromCharCode(chr2);
              }
              if (enc4 != 64) {
                  output = output + String.fromCharCode(chr3);
              }

          }

          output = Base64._utf8_decode(output);

          return output;

      },

      // private method for UTF-8 encoding
      _utf8_encode : function (string) {
          string = string.replace(/\r\n/g,"\n");
          var utftext = "";

          for (var n = 0; n < string.length; n++) {

              var c = string.charCodeAt(n);

              if (c < 128) {
                  utftext += String.fromCharCode(c);
              }
              else if((c > 127) && (c < 2048)) {
                  utftext += String.fromCharCode((c >> 6) | 192);
                  utftext += String.fromCharCode((c & 63) | 128);
              }
              else {
                  utftext += String.fromCharCode((c >> 12) | 224);
                  utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                  utftext += String.fromCharCode((c & 63) | 128);
              }

          }

          return utftext;
      },

      // private method for UTF-8 decoding
      _utf8_decode : function (utftext) {
          var string = "";
          var i = 0;
          var c = c1 = c2 = 0;

          while ( i < utftext.length ) {

              c = utftext.charCodeAt(i);

              if (c < 128) {
                  string += String.fromCharCode(c);
                  i++;
              }
              else if((c > 191) && (c < 224)) {
                  c2 = utftext.charCodeAt(i+1);
                  string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                  i += 2;
              }
              else {
                  c2 = utftext.charCodeAt(i+1);
                  c3 = utftext.charCodeAt(i+2);
                  string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                  i += 3;
              }

          }

          return string;
      }

  }
  
  if (isIE()) {
    $('#latestReleaseDiv').hide();
  }
  
  /*$.get("https://api.github.com/repos/billcacy/NPOD/commits?path=windows/Setup/NPODSetup/Release/NPODSetup.msi", function( data ) {
    var winLastMod = parseDate(data[0].commit.author.date.substring(0,10));
    winLastMod = $.format.date(winLastMod.toString(), 'M/d/yy');
    $('#lu-win').html(winLastMod);
  });
  
  $.get("https://api.github.com/repos/billcacy/NPOD/commits?path=mac/NPOD.zip", function( data ) {
    var macLastMod = parseDate(data[0].commit.author.date.substring(0,10));
    macLastMod = $.format.date(macLastMod.toString(), 'M/d/yy');
    $('#lu-mac').html(macLastMod);
  });
  */
  
  $.get("https://api.github.com/repos/billcacy/NPOD/contents/windows/bin/Release/NasaPicOfDay.exe.config", function( data ) {
    var winVersion = "";
    var xml = Base64.decode(data.content);  
    var xmlDoc = $.parseXML( $.trim(xml) );
    var $xml = $( xmlDoc );
    var winVersion = $xml.find( "setting[name='CurrentVersion']" ).children('value').text();
    $('#lu-win').html(winVersion);
  });

  $.get("https://api.github.com/repos/billcacy/NPOD/contents/mac/NPOD.app/Contents/Info.plist", function( data ) {
    var macVersion = "";
    var xml = Base64.decode(data.content);
    var xmlDoc = $.parseXML( $.trim(xml) );
    var $xml = $( xmlDoc );
    var $key = $xml.find( "key" );
    $key.each(function() {
      if($(this).text()=="CFBundleShortVersionString") {
        macVersion = $(this).next().text();
        return false;
      }
    });
    $('#lu-mac').html(macVersion);
  });
  
  var trackMacDownloads = "ga('send', 'event', 'Downloads', 'Mac');";
  var trackWinDownloads = "ga('send', 'event', 'Downloads', 'Windows');";
  
  if(navigator.platform.match(/^win/i)) {
    $('.download').append('<a href="https://github.com/BillCacy/NPOD/raw/master/windows/Setup/NPODSetup/Release/NPODSetup.msi" class="btn btn-primary" onClick="'+trackWinDownloads+'">Windows</a>&nbsp;&nbsp;&nbsp;&nbsp;or&nbsp;&nbsp;&nbsp;&nbsp;<a href="https://github.com/BillCacy/NPOD/raw/master/mac/NPOD.zip" class="btn btn-default" onClick="'+trackMacDownloads+'">Mac</a> <a href="mac-install.htm">Install instructions</a>');
  }
  else {
    $('.download').append('<a href="https://github.com/BillCacy/NPOD/raw/master/mac/NPOD.zip" class="btn btn-primary" onClick="'+trackMacDownloads+'">Mac</a> <a href="mac-install.htm">Install instructions</a>&nbsp;&nbsp;&nbsp;&nbsp;or&nbsp;&nbsp;&nbsp;&nbsp;<a href="https://github.com/BillCacy/NPOD/raw/master/windows/Setup/NPODSetup/Release/NPODSetup.msi" class="btn btn-default" onClick="'+trackWinDownloads+'">Windows</a>');
  }

  // parse a date in yyyy-mm-dd format
  parseDate = function(input) {
    var parts = input.match(/(\d+)/g);
    return new Date(parts[0], parts[1]-1, parts[2]); // months are 0-based
  }
  
  function getInternetExplorerVersion()
  // Returns the version of Internet Explorer or a -1
  // (indicating the use of another browser).
  {
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == 'Microsoft Internet Explorer')
    {
      var ua = navigator.userAgent;
      var re  = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
      if (re.exec(ua) != null)
        rv = parseFloat( RegExp.$1 );
    }
    return rv;
  }
  function isIE()
  {
    var isIE = false;
    var ver = getInternetExplorerVersion();

    if ( ver > -1 )
    {
      isIE = true;
    }
    return isIE;
  }
  
});