// Google Analytics
(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
})(window,document,'script','//www.google-analytics.com/analytics.js','ga');

ga('create', 'UA-47261481-1', 'billcacy.github.io');
ga('send', 'pageview');

$( document ).ready(function() {

  if (isIE()) {
    $('#latestReleaseDiv').hide();
  }
  
  $.get("https://api.github.com/repos/billcacy/NPOD/commits?path=windows/Setup/NPODSetup/Release/NPODSetup.msi", function( data ) {
    var winLastMod = parseDate(data[0].commit.author.date.substring(0,10));
    winLastMod = $.format.date(winLastMod.toString(), 'M/d/yy');
    $('#lu-win').html(winLastMod);
  });

  $.get("https://api.github.com/repos/billcacy/NPOD/commits?path=mac/NPOD.zip", function( data ) {
    var macLastMod = parseDate(data[0].commit.author.date.substring(0,10));
    macLastMod = $.format.date(macLastMod.toString(), 'M/d/yy');
    $('#lu-mac').html(macLastMod);
  });
  
  if(navigator.platform.match(/^win/i)) {
    $('.download').append('<a href="https://github.com/BillCacy/NPOD/raw/master/windows/Setup/NPODSetup/Release/NPODSetup.msi" class="btn btn-primary">Windows</a>&nbsp;&nbsp;&nbsp;&nbsp;or&nbsp;&nbsp;&nbsp;&nbsp;<a href="https://github.com/BillCacy/NPOD/raw/master/mac/NPOD.zip" class="btn btn-default">Mac</a> <a href="mac-install.htm">Install instructions</a>');
  }
  else {
    $('.download').append('<a href="https://github.com/BillCacy/NPOD/raw/master/mac/NPOD.zip" class="btn btn-primary">Mac</a> <a href="mac-install.htm">Install instructions</a>&nbsp;&nbsp;&nbsp;&nbsp;or&nbsp;&nbsp;&nbsp;&nbsp;<a href="https://github.com/BillCacy/NPOD/raw/master/windows/Setup/NPODSetup/Release/NPODSetup.msi" class="btn btn-default">Windows</a>');
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