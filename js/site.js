$( document ).ready(function() {
  if(navigator.platform.match(/^win/i)) {
    $('.download').append('<a href="https://github.com/BillCacy/NPOD/raw/master/windows/Setup/NPODSetup/Release/NPODSetup.msi" class="btn btn-primary">Windows</a>&nbsp;&nbsp;&nbsp;&nbsp;or&nbsp;&nbsp;&nbsp;&nbsp;<a href="https://github.com/BillCacy/NPOD/raw/master/mac/NPOD.zip" class="btn btn-default">Mac</a> <a href="mac-install.htm">Install instructions</a>');
  }
  else {
    $('.download').append('<a href="https://github.com/BillCacy/NPOD/raw/master/mac/NPOD.zip" class="btn btn-primary">Mac</a> <a href="mac-install.htm">Install instructions</a>&nbsp;&nbsp;&nbsp;&nbsp;or&nbsp;&nbsp;&nbsp;&nbsp;<a href="https://github.com/BillCacy/NPOD/raw/master/windows/Setup/NPODSetup/Release/NPODSetup.msi" class="btn btn-default">Windows</a>');
  }
});