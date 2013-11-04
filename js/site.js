$( document ).ready(function() {
  
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
});