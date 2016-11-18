///////////////////////////////////////////////////////////////////////////////////////////////
//
//  NAMING CONVENTION FOR CONTROLS
//
///////////////////////////////////////////////////////////////////////////////////////////////
//              //                  //            //        //            
// Control      // Validation Req.  // Mandatory  // Prefix // Function   
//              //                  //            //        //            
///////////////////////////////////////////////////////////////////////////////////////////////
// Mail         // Yes              // Yes        // emly   // emailvalidate_m()
// Mail         // Yes              // No         // emln   // emailvalidate_nm()
// Numeric      // Yes              // Yes        // nmry   // numericvalidate_m()
// Numeric      // Yes              // No         // nmrn   // numericvalidate_nm()
// Amount       // Yes              // Yes        // amty   // isAmount_m()
// Amount       // Yes              // No         // amtn   // isAmount_nm()
// Date         // No               // Yes        // daty   // chkempty()                    
// Date         // No               // No         // datn   // NONE                    
// Text         // No               // Yes        // txty   // chkempty()                    
// Text         // No               // No         // txtn   // NONE                    
// Alphanumeric // Yes              // Yes        // alny   // alphanumericvalidate_m()    
// Alphanumeric // Yes              // No         // alnn   // alphanumericvalidate_nm()   
//              //                  //            //        //                
///////////////////////////////////////////////////////////////////////////////////////////////
//In ListBox attribute concatenate ListBox message with lst$ before the Message
     
///////////////////////////////////////////////////////////////////////////////////////////////

var msgstring;
var arr_attr;
var strFld;
var strDdl;
///////////////////////////////////////////////////////////////////////////////////////////////
//Main Validate Function
//
function chkvalidate()
{
msgstring="";

	for (x=0;x<document.forms[0].length;x++) 
    {

    // TextBox Validation
        if(document.forms[0].elements[x].type == "text")
        {
            strFld = document.forms[0].elements[x];

            //Control Level Validation
            if(strFld.getAttribute("ctrlvldt") != null)
            {
                arr_attr = strFld.getAttribute("ctrlvldt").split(",");
                if(arr_attr[0] == "emly")
                {
                    emailvalidate_m(strFld,arr_attr[1]);
                }
                
                else if(arr_attr[0] == "emln")
                {
                    emailvalidate_nm(strFld,arr_attr[1]);
                }
                
                else if(arr_attr[0] == "alny")
                {
                    alphanumericvalidate_m(strFld,arr_attr[1]);
                }
                
                else if(arr_attr[0] == "txty")
                {
                    chkempty(strFld,arr_attr[1]);
                }
                else if(arr_attr[0] == "amty")
                {
                    isAmount_m(strFld,arr_attr[1]);
                }
                else if(arr_attr[0] == "nmry")
                {
                    numericvalidate_m(strFld,arr_attr[1]);
                }
                else if(arr_attr[0] == "date")
                {
                    validateDate(strFld,arr_attr[1]);
                }                
                
                else if(arr_attr[0] == "daty")
                {
                   chkempty(strFld,arr_attr[1]);
                }
                if(arr_attr[2] != null)
                {
                
                }
            }
            
            //Functional Level Validation
            
        }
        
      // DropDownList / Generic List / ListBox Validation
        if(document.forms[0].elements[x].type == "select-one")
        {
            strDdl = document.forms[0].elements[x];

        // Generic List Validation
            if(strDdl.id.indexOf("Generic") > 0)
            {
                if(strDdl.getAttribute("ctrlvldt").length > 0)
                {
                    if(strDdl.options[strDdl.selectedIndex].text.indexOf("lease select") > 0)
                    {
                        chkselect(strDdl.getAttribute("ctrlvldt"));
                    }
                }
            }
            else
            {
                if(strDdl.getAttribute("ctrlvldt") != null)
                {
        // ListBox Validation
                    if(strDdl.getAttribute("ctrlvldt").indexOf("st$") > 0)
                    {
                        if(strDdl.options.length == 0)
                        {
                            selectList(strDdl.getAttribute("ctrlvldt"));
                        }
                    }
                    else
                    {
        // DropDownList Validation                    
                        if(strDdl.options[strDdl.selectedIndex].text.indexOf("extBox") > 0)
                        {   
                            chkselect(strDdl.getAttribute("ctrlvldt"));
                        }
                    }
                }
            }
        }
        
        
        //CheckBox Validation        
        if(document.forms[0].elements[x].type == "checkbox")
        {
        }
        
        //Radio Button Validation        
        if(document.forms[0].elements[x].type == "radio")
        {
        }
        
        
    }
    if(msgstring.length > 0)
    {
        alert(msgstring);
        return false;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////
// STANDARD FUNCTIONS
///////////////////////////////////////////////////////////////////////////////////////////////
//Function for Email Validation - Mandatory
//

function emailvalidate_m(_strFld,attr_msg)
{
emailStr=_strFld.value;
var emailPat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
var matchArray = emailStr.match(emailPat);

		if (matchArray == null) {
            //alert(_strFld.id + ' - InValid Email Entry in ');
            msgstring = msgstring + '\n' + attr_msg + ' - InValid Email Entry';
            return false;
 		} 
}

//Function for Email Validation - Non Mandatory
//

function emailvalidate_nm(_strFld,attr_msg)
{
emailStr=_strFld.value;

var emailPat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
var matchArray = emailStr.match(emailPat);

if (emailStr.length > 0)
{
		if (matchArray == null) {
            //alert(_strFld.id + ' - InValid Email Entry in ');
            msgstring = msgstring + '\n' + attr_msg + ' - InValid Email Entry';
            return false;
 		} 
}
}

///////////////////////////////////////////////////////////////////////////////////////////////
//Function for Number Validation - Mandatory
//
function numericvalidate_m(_strFld,attr_msg)
{
	var o = _strFld.value;
	if (isInteger(o))
	{
	return  true;
	}
	else
	{
	//alert(_strFld.id + ' - Plz. Enter Number [0-9] Only in ' + _strFld.id);
	msgstring = msgstring + '\n' + attr_msg + ' - Plz. Enter Number [0-9] Only';
	return false;
	}
}

//Function for Number Validation - Non Mandatory
//
function numericvalidate_nm(_strFld,attr_msg)
{
	var o = _strFld.value;
    if (o.length > 0)
    {
	    if (isInteger(o))
	    {
	    return  true;
	    }
	    else
	    {
	    //alert(_strFld.id + ' - Plz. Enter Number [0-9] Only in ' + _strFld.id);
	    msgstring = msgstring + '\n' + attr_msg + ' - Plz. Enter Number [0-9] Only';
	    return false;
	    }
    }	
}

function isInteger (s)
{
var i;
if (isEmpty(s))
if (isInteger.arguments.length == 1) return 0;
else return (isInteger.arguments[1] == true);
for (i = 0; i < s.length; i++)
{
var c = s.charAt(i);
if (!isDigit(c)) return false;
}
return true;
}


function isEmpty(s)
{
return ((s == null) || (s.length == 0))
}

function isDigit (c)
{
return ((c >= "0") && (c <= "9"))
}

///////////////////////////////////////////////////////////////////////////////////////////////
//Function for AlphaNumeric Validation - Mandatory
//
function alphanumericvalidate_m(_strFld,attr_msg)
{
    if(_strFld.value.length == 0)
    {
         //alert(_strFld.id + ' - Plz. Enter AlphaNumeric[0-9], [a-z] & [A-Z] Only in ' + _strFld.id);
         msgstring = msgstring + '\n' + attr_msg + ' - Plz. Enter AlphaNumeric[0-9], [a-z] & [A-Z] Only';
	     return false;
    }

	numaric = _strFld.value;
	for(var j=0; j<numaric.length; j++)
	{
	  var alphaa = numaric.charAt(j);
	  var hh = alphaa.charCodeAt(0);
	  if((hh > 47 && hh<59) || (hh > 64 && hh<91) || (hh > 96 && hh<123))
	  {
	  
	  }
	  else	
	  {
         //alert(_strFld.id + ' - Plz. Enter AlphaNumeric[0-9], [a-z] & [A-Z] Only in ' + _strFld.id);
         msgstring = msgstring + '\n' + attr_msg + ' - Plz. Enter AlphaNumeric[0-9], [a-z] & [A-Z] Only';
		 return false;
	  }
	}
    return true;
}


//Function for AlphaNumeric Validation - Non Mandatory
//

function alphanumericvalidate_nm(_strFld,attr_msg)
{
	numaric = _strFld.value;
	for(var j=0; j<numaric.length; j++)
	{
	  var alphaa = numaric.charAt(j);
	  var hh = alphaa.charCodeAt(0);
	  if((hh > 47 && hh<59) || (hh > 64 && hh<91) || (hh > 96 && hh<123))
	  {
	  
	  }
	  else	
	  {
         //alert(_strFld.id + ' - Plz. Enter AlphaNumeric[0-9], [a-z] & [A-Z] Only in ' + _strFld.id);
         msgstring = msgstring + '\n' + attr_msg + ' - Plz. Enter AlphaNumeric[0-9], [a-z] & [A-Z] Only';
		 return false;
	  }
	}
    return true;
}
///////////////////////////////////////////////////////////////////////////////////////////////
//Function for Amount Validation - Mandatory
//

function isAmount_m(_strFld,attr_msg)	
{

   var regNoDecNum = /^\d{1,10}$/;
   var regDecNum = /^\d{1,8}\.{1}\d{1,2}$/;

    value= _strFld.value;

	if(value.search(regNoDecNum) != -1)	
	{
		return true;
	}
	else if(value.search(regDecNum) != -1)	
	{
		return true;
	}
	msgstring = msgstring + '\n' + attr_msg + ' - Plz. Enter Valid Amount';
	return false;
}


//Function for Amount Validation - Non Mandatory
//

function isAmount_nm(_strFld,attr_msg)	
{

   var regNoDecNum = /^\d{1,10}$/;
   var regDecNum = /^\d{1,8}\.{1}\d{1,2}$/;

    value= _strFld.value;
    if (value.length > 0)
    {
	    if(value.search(regNoDecNum) != -1)	
	    {
		    return true;
	    }
	    else if(value.search(regDecNum) != -1)	
	    {
		    return true;
	    }
	    msgstring = msgstring + '\n' + attr_msg + ' - Plz. Enter Valid Amount';
	    return false;
    }	
}
///////////////////////////////////////////////////////////////////////////////////////////////
//Function for Entry Validation - Mandatory
//
function chkempty(_strFld,attr_msg)
{

	var re = /\s*((\S+\s*)*)/;
	
    if(_strFld.value.replace(re, "$1").length == 0)
    {
         //alert(_strFld.id + ' - Plz. Enter AlphaNumeric[0-9], [a-z] & [A-Z] Only in ' + _strFld.id);
         msgstring = msgstring + '\n' + attr_msg + ' - Plz. Enter the Value';
	     return false;
    }
    return true;
}

function validateDate(_strFld,attr_msg) 
{
    var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
//    var errorMessage = attr_msg + ' - Please enter valid date as month, day, and four digit year.\nYou may use a slash, hyphen or period to separate the values.\nThe date must be a real date. 30/2/2000 would not be accepted.\nFormay dd/mm/yyyy.';
   
     value= _strFld.value;
     
    if ((value.match(RegExPattern)) && (value!='')) 
    {
//        alert('Date is OK'); 
    } 
    else 
    {
      msgstring = msgstring + '\n' + attr_msg + ' - Please enter valid date as month, day, and four digit year.\nYou may use a slash, hyphen or period to separate the values.\nThe date must be a real date. 30/2/2000 would not be accepted.\nFormay dd/mm/yyyy.';
//        alert(errorMessage);
//        _strFld.focus();
    } 
}

///////////////////////////////////////////////////////////////////////////////////////////////
//Function for Dropdown Validation - Mandatory
//
function chkselect(attr_msg)
{
         msgstring = msgstring + '\n' + attr_msg + ' - Plz. Select the Value';
	     return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////
//Function for ListBox Validation - Mandatory
//
function selectList(attr_msg)
{
         msgstring = msgstring + '\n' + attr_msg.replace("lst$","") + ' - List should not be blank';
	     return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////
// CUSTOME FUNCTIONS
///////////////////////////////////////////////////////////////////////////////////////////////
//Function for IC Validation
//

