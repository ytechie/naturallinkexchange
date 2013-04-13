
function ConfirmLinkCount(articleTextId, linkCount, keyword1, keyword2)
{
	try
	{
	    var pattern1 = '(^|\\W)(' + keyword1 + ')(\\W|$)';
	    var pattern2 = '(^|\\W)(' + keyword2 + ')(\\W|$)'
		var articleText = document.getElementById(articleTextId);
		var reg1 = new RegExp(pattern1, ["i"]);
		var reg2 = new RegExp(pattern2, ["i"]);
		
		if(articleText == null)
		{
			alert('Could not find Article Textbox.');
			return false;
		}
		else
		{
			var text = articleText.value;
			
			//Check for at least one link.
			if(reg1.exec(text) == null && (linkCount < 2 || reg2.exec(text) == null))
			{
				var subscriptionChoices;
				alert('You have not placed at least on of your keywords in your link article.  This means this article does not link to you.  Please add at least one of your keywords to your article before Publishing.');
				return false;
			}
			
			return true;
		}
	}
	catch(ex)
	{
		alert(ex.message);
		return false;
	}
}

function ShowArticleCountMessage(articleCount, redirect)
{
	if(confirm('Your current subscription level only allows you to have ' + articleCount + ' articles per group.  If you wish to add more articles per group, please upgrade your subscription level.\n\nClick OK to go to the Payment Settings Control Panel to upgrade your subscription level, or click Cancel to stay on this page.'))
		window.location = redirect;
	return false;
}

function ShowArticleGroupCountMessage(groupCount, redirect)
{
	if(confirm('Your current subscription level only allows you to have ' + groupCount + ' article groups.  If you wish to add more article groups, please upgrade your subscription level.\n\nClick OK to go to the Payment Settings Control Panel to upgrade your subscription level, or click Cancel to stay on this page.'))
		window.location = redirect;
	return false;
}

function MakeEqual(chkAdvancedId, staticControlId, changeControlId)
{
    var chkAdvanced, staticControl, changeControl;
    
    chkAdvanced = document.getElementById(chkAdvancedId);
    
    if(chkAdvanced != null && !chkAdvanced.checked)
    {
        staticControl = document.getElementById(staticControlId);
        changeControl = document.getElementById(changeControlId);
        
        if(staticControl != null && changeControl != null)
        {
            if(staticControl.value != '')
                changeControl.value = staticControl.value;
            else
                staticControl.value = changeControl.value;
        }
    }
}