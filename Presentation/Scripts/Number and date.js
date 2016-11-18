// JScript File
var date_seperator = '/';
var decimal_seperator = ',';
var thousand_seperator = '.';
	var K_Validate = {
		Version: '1.0.0',
		Load: function(){
			var children = document.getElementsByTagName('input');
			for (var i = 0; i < children.length; i++) {
				var classNames = children[i].className.split(' ');
					
					if (children[i].className.match(new RegExp("(^|\\s)validatenum(\\s|$)"))) {
						this.addInListener(children[i],'keypress',K_Validate.checkNum);
						
						if (children[i].className.match(new RegExp("(^|\\s)thousand(\\s|$)"))) {
							this.addInListener(children[i],'focus',K_Validate.normalFormat);
							this.addInListener(children[i],'blur',K_Validate.formatThousand);
						}
					
					}
					if (children[i].className.match(new RegExp("(^|\\s)validatedate(\\s|$)"))) {
							this.addInListener(children[i],'keypress',K_Validate.checkDate);
							this.addInListener(children[i],'blur',K_Validate.isDate);
							children[i].maxlength=10;
						}					
		
			}
		},
		addInListener : function (a,b,c){
			a["on"+b]=c
		},
		
		isNumeric: function(ch) {
			if (!(ch >= '0' && ch <= '9')) {
				return false;
			}
		return true;
		},
		
		elo : function (e){
			if (!e) var e = window.event;
			var eid = (window.event) ? e.srcElement : e.target;
			if(!eid) return false;
			return eid;
		},
	
		checkNum: function (e,not_dec,not_minus) {
			if(!e) e = window.event;
			if(window.event){
				keynum = e.keyCode
			}else if(e.which){
				keynum = e.which
			}	
			if (K_Validate.elo(e).className.match(new RegExp("(^|\\s)nominus(\\s|$)"))) not_minus = true;
			if (K_Validate.elo(e).className.match(new RegExp("(^|\\s)nodecimal(\\s|$)"))) not_dec = true;
			if (!not_minus) not_minus = false;
			if (!not_dec) not_dec = false;
			var obj = (window.event) ? e.srcElement: e.target;

			var keychar;
			if (keynum == null) return true;
			goods = '0123456789';
			decimal_used = (obj.value.indexOf(decimal_seperator) != -1) ? true:false;
			minus_used = (obj.value.indexOf('-') != -1) ? true:false;

			if(!not_dec && !decimal_used) goods = goods+decimal_seperator;
			keychar = String.fromCharCode(keynum);
			keychar = keychar.toLowerCase();
			goods = goods.toLowerCase();
			var curr_pos = K_Validate.insertAtCursor(obj);
			if(!not_minus && curr_pos<1 && !minus_used) goods = goods+'-';

			if (goods.indexOf(keychar) != -1)
				return true;

			if ( keynum==null || keynum==0 || keynum==8 || keynum==9 || keynum==13 || keynum==27 )
				return true;

			return false;
		},
		
		insertAtCursor : function (obj) {
			if (document.selection) {
			sel=document.selection;
				if(sel)
				{
					r2=sel.createRange();
					rng=obj.createTextRange();
					rng.setEndPoint("EndToStart", r2);
					i=rng.text.length;
				}
				return i;
			}else if (obj.selectionStart || obj.selectionStart == '0') {
				return obj.selectionStart;
			}
		},
		
		
		
		formatThousand : function (e) {
			if(!e) e = window.event;
			obj = K_Validate.elo(e);
			formattedNbr="";
			if (obj.value.match(new RegExp(/-/))){
				var minus=true;
				obj.value = obj.value.replace("-","");
			}
			var temp_val	=	obj.value.split(decimal_seperator);
			len = temp_val[0].length;
			for (i=len-1, k=1 ; i>=0; i = i-1,k++ )  {
				if (((k % 3)	== 0) && (k != len))
					formattedNbr = thousand_seperator + temp_val[0].charAt(i) + formattedNbr;
				else
					formattedNbr = temp_val[0].charAt(i) + formattedNbr;
			}
			formattedNbr = (minus)?'-'+formattedNbr:formattedNbr;
			if(temp_val[1]){
				obj.value = formattedNbr+decimal_seperator+temp_val[1];
			}else{
				obj.value = formattedNbr;
			}
		},

		normalFormat : function(e)	{
			if(!e) e = window.event;
			obj = K_Validate.elo(e);
			tempText= new String();
			text = obj.value;
			if (text=="") { 
				return false;
			}
			len = text.length;
			for(i=0; i < len; ++i) {
				ch = text.charAt(i);
				if ((ch >= '0' && ch <= '9') || ch==decimal_seperator || ch=='-') {
					tempText+=ch;
				}
			}
			obj.value=tempText;
			return true;
		},
		
		checkDate : function (e){
			if(!e) e = window.event;
			var obj = (window.event) ? e.srcElement: e.target;
			if(window.event) // IE
			{
				keynum = e.keyCode
			}else if(e.which) {
				keynum = e.which
			}
			goods = '0123456789';
			keychar = String.fromCharCode(keynum);
			keychar = keychar.toLowerCase();
			var curr_pos = K_Validate.insertAtCursor(obj);
			if (goods.indexOf(keychar) != -1){
				if(curr_pos==2 && obj.value.charAt(2)!=date_seperator && obj.value.length<3){
					obj.value = obj.value+date_seperator;
					curr_pos++;
				}
				if(curr_pos==5 && obj.value.charAt(4)!=date_seperator && obj.value.length<6){
					obj.value = obj.value+date_seperator;
					curr_pos++;
				}
			return true;

			}
			if(keychar==date_seperator && (curr_pos==5 || curr_pos==2))
				return true;

			if ( keynum==null || keynum==0 || keynum==8 || keynum==9 || keynum==13 || keynum==27 )
				return true;

			return false;
		},
		
		getDateFromFormat : function (val,format){
			val=val+"";
			format=format+"";
			var i_val=0;
			var i_format=0;
			var c="";
			var token="";
			var token2="";
			var x,y;
			var now=new Date();
			var year=now.getYear();
			var month=now.getMonth()+1;
			var date=1;
			var hh=now.getHours();
			var mm=now.getMinutes();
			var ss=now.getSeconds();
			var ampm="";
			while(i_format < format.length){
				c=format.charAt(i_format);
				token="";
				while((format.charAt(i_format)==c) &&(i_format < format.length)){
					token += format.charAt(i_format++);
				}
				if(token=="yyyy" || token=="yy" || token=="y"){
					if(token=="yyyy"){x=4;y=4;}
					if(token=="yy"){x=2;y=2;}
					if(token=="y"){x=2;y=4;}
					year=K_Validate._getInt(val,i_val,x,y);
					if(year==null){return 0;}
					i_val += year.length;
					if(year.length==2){
						if(year > 70){
							year=1900+(year-0);
						}else{
							year=2000+(year-0);
						}
					}
				}else if(token=="MMM"||token=="NNN"){
					month=0;
					for(var i=0;i<MONTH_NAMES.length;i++){
						var month_name=MONTH_NAMES[i];
						if(val.substring(i_val,i_val+month_name.length).toLowerCase()==month_name.toLowerCase()){
							if(token=="MMM"||(token=="NNN"&&i>11)){
								month=i+1;
								if(month>12){month -= 12;}
								i_val += month_name.length;
								break;
							}
						}
					}
				
					if((month < 1)||(month>12)){return 0;}
				}else if(token=="EE"||token=="E"){
					for(var i=0;i<DAY_NAMES.length;i++){
						var day_name=DAY_NAMES[i];
						if(val.substring(i_val,i_val+day_name.length).toLowerCase()==day_name.toLowerCase()){
							i_val += day_name.length;
							break;
						}
					}
				}else if(token=="MM"||token=="M"){
					month=K_Validate._getInt(val,i_val,token.length,2);
					if(month==null||(month<1)||(month>12)){return 0;}
					i_val+=month.length;
				}else if(token=="dd"||token=="d"){
					date=K_Validate._getInt(val,i_val,token.length,2);
					if(date==null||(date<1)||(date>31)){return 0;}
					i_val+=date.length;
				}else if(token=="hh"||token=="h"){
					hh=K_Validate._getInt(val,i_val,token.length,2);
					if(hh==null||(hh<1)||(hh>12)){return 0;}
					i_val+=hh.length;
				}else if(token=="HH"||token=="H"){
					hh=K_Validate._getInt(val,i_val,token.length,2);
					if(hh==null||(hh<0)||(hh>23)){return 0;}
					i_val+=hh.length;
				}else if(token=="KK"||token=="K"){
					hh=K_Validate._getInt(val,i_val,token.length,2);
					if(hh==null||(hh<0)||(hh>11)){return 0;}
					i_val+=hh.length;
				}else if(token=="kk"||token=="k"){
					hh=K_Validate._getInt(val,i_val,token.length,2);
					if(hh==null||(hh<1)||(hh>24)){return 0;}
					i_val+=hh.length;hh--;
				}else if(token=="mm"||token=="m"){
					mm=K_Validate._getInt(val,i_val,token.length,2);
					if(mm==null||(mm<0)||(mm>59)){return 0;}
					i_val+=mm.length;
				}else if(token=="ss"||token=="s"){
					ss=K_Validate._getInt(val,i_val,token.length,2);
					if(ss==null||(ss<0)||(ss>59)){return 0;}
					i_val+=ss.length;
				}else if(token=="a"){
					if(val.substring(i_val,i_val+2).toLowerCase()=="am"){
						ampm="AM";
					}else if(val.substring(i_val,i_val+2).toLowerCase()=="pm"){
						ampm="PM";
					}else{return 0;}
					i_val+=2;
				}else{
					if(val.substring(i_val,i_val+token.length)!=token){
						return 0;
					}else{
						i_val+=token.length;
					}
				}
			}
			if(i_val != val.length){return 0;}
			if(month==2){
				if( ((year%4==0)&&(year%100 != 0) ) ||(year%400==0) ){
					if(date > 29){return 0;}
				}else{
					if(date > 28){return 0;}
				}
			}
			if((month==4)||(month==6)||(month==9)||(month==11)){
				if(date > 30){return 0;}
			}
			if(hh<12 && ampm=="PM"){
				hh=hh-0+12;
			}else if(hh>11 && ampm=="AM"){
				hh-=12;
			}
			var newdate=new Date(year,month-1,date,hh,mm,ss);
			return newdate.getTime();
		},
		
		
	isDate : function (e){
		if(!e) e = window.event;
		obj = K_Validate.elo(e);
		var val = obj.value;
		var date=K_Validate.getDateFromFormat(val,"dd-MM-y");
		if(date==0){
			obj.style.color="#ff0000";
		}else{
			obj.style.color="#000000";
		}
	},
	
	
	_isInteger : function (val){
		var digits="1234567890";
		for(var i=0;i < val.length;i++){if(digits.indexOf(val.charAt(i))==-1){return false;}}return true;
	},



	_getInt : function (str,i,minlength,maxlength){
	for(var x=maxlength;x>=minlength;x--){
		var token=str.substring(i,i+x);
			if(token.length < minlength){
				return null;
				}
			if(K_Validate._isInteger(token)){
				return token;
				}
		}
	return null;
	}


	}


