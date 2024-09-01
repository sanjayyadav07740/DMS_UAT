(function($){ 
     $.fn.extend({  
         airport: function(array) {
			
			var self = $(this);
			var chars = ['A','B','C','D','E','F','G',' ','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','-'];
			var longest = 0;
			var items = items2 = array.length;

			function pad(A,B) { return A + new Array(B - A.length + 1).join(' '); }
			
			$(this).empty();
			
			while(items--)
				if(array[items].length > longest) longest = array[items].length;

			while(items2--)
				array[items2] = pad(array[items2],longest);
				
			spans = longest;
			while(spans--)
				$(this).prepend("<span class='C" + spans + "'></span>");
				
			
			function testChar(A,B,C,D){
				if(C >= array.length)
					setTimeout(function() { testChar(0,0,0,0); }, 1000);				
				else if(D >= longest)
					setTimeout(function() { testChar(0,0,C+1,0); }, 1000);
				else {
					$(self).find('.C'+A).html((chars[B]==" ")?"&nbsp;":chars[B]);
					setTimeout(function() {
						if(B > chars.length)
							testChar(A+1,0,C,D+1);
						else if(chars[B] != array[C].substring(D,D+1).toUpperCase())
							testChar(A,B+1,C,D);
						else
							testChar(A+1,0,C,D+1);
					}, 20);
				}
			}
			
			testChar(0,0,0,0);
        } 
    }); 
})(jQuery);