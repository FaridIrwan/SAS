				//TRIM FUNCTION
				
				function trim(s) {
					while (s.substring(0,1) == ' ') {
						s = s.substring(1,s.length);
					}
					while (s.substring(s.length-1,s.length) == ' ') {
						s = s.substring(0,s.length-1);
					}
					return s;
				}	

				//----------------

				//EMAIL VALIDATION

				function isValidEmail(str) {
					return (str.indexOf(".") > 2) && (str.indexOf("@") > 0);
				}		
								
				//----------------
								
											
				//NUMBER VALIDATION
					
				function checkValue()	{
								
					if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13)  && (event.keyCode != 46) )
						{
							alert("Enter Only Digits");
							event.keyCode=0;
						}
					}
					
				//----------------
								
				//STATUS MESSAGE
								
				function showStatus(msg)	{
								
					alert(msg);
								
					}
					
				//----------------


				//DELETE CONFIRMATION

				function ConfirmDelete(sName) {
					
				var sMsg = 'Are you sure you want to delete "' + sName + '"?';
				return (confirm(sMsg));
			
				}
				
				//----------------	


				//NEW WINDOW
				
				function openNewWin(fpath) {
				
				window.open(fpath);

				}

				//----------------	
				
				
				//REDIRECTIN URL
				
				function redirectUrl(sName)	{
				
				if (sName == 'A')
					window.location='default.aspx';
				else	
					window.location='companieslist.aspx';
				}

				//----------------

				
		function isAlphabet()	{
								
	if (((event.keyCode < 97) || (event.keyCode > 122)) && ((event.keyCode < 65) || (event.keyCode > 90)) && (event.keyCode != 32))  
						{
							event.keyCode=0;
						}

				}
					




