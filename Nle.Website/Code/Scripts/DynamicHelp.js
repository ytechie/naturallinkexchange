var errorShown = false;

function ShowDynamicHelp(dynamicHelpId, HelpId, left, right, top, bottom, width, height)
{
	try
	{
		var dh = document.getElementById(dynamicHelpId + "_Container");				
		var title = document.getElementById(dynamicHelpId + "_Title" + HelpId);
		var text = document.getElementById(dynamicHelpId + "_Text" + HelpId);
		var h1 = document.getElementById(dynamicHelpId + "_Title");
		var divContent = document.getElementById(dynamicHelpId + "_Text");
	
		if(dh == null || title == null || text == null || h1 == null || divContent == null)
		{
			if(!errorShown) 
			{
				alert('Could not display Dynamic Help.');
				errorShown = true;
			}
			return false;
		}
		
		h1.innerHTML = title.value;
		
		/// When giving an Html Encoded value to a hidden input element's value attribute,
		/// the '&'s get replaced with '&amp;'s.  This needs to be undone before Html
		/// Decoding.
		var encodedHtml = text.value.replace('&amp;', '&');
		
		divContent.innerHTML = HtmlDecode(encodedHtml);
		
		dh.style.display = "";
		
		dh.style.left = left == null ? "" : left;
		dh.style.right = right == null ? "" : right;
		dh.style.top = top == null ? "" : top;
		dh.style.bottom = bottom == null ? "" : bottom;
		
		dh.style.width = width == null ? "" : width;
		dh.style.height = height == null ? "" : height;
	}
	catch(ex)
	{
		if(!errorShown) 
		{	
			alert('Could not properly display dynamic help.\n\n' + ex.message);
			errorShown = true;
		}
		return false;
	}
	
	return true;
}

function HideDynamicHelp(dynamicHelpId)
{
	try
	{
		var dh = document.getElementById(dynamicHelpId + "_Container");
		var h1 = document.getElementById(dynamicHelpId + "_Title");
		var divContent = document.getElementById(dynamicHelpId + "_Text");
		
		if(dh == null || h1 == null || divContent == null)
		{
			if(!errorShown) 
			{
				alert('Could not hide Dynamic Help.');
				errorShown = true;
			}
			return false;
		}
		
		h1.innerHTML = "";
		divContent.innerHTML = "";
		
		dh.style.display = "none";
	}
	catch(ex)
	{
		if(!errorShown) 
		{	
			alert('Could not properly hide dynamic help.\n\n' + ex.message);
			errorShown = true;
		}
		return false;
	}
	
	return true;
}
