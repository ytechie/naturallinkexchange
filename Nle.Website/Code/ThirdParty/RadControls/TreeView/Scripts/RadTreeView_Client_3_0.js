//////////////////////////////////////////////
///    RadTreeView 3.0 (c)telerik 2004    ///
/////////////////////////////////////////////
var RadTreeView_KeyboardHooked = false;
var RadTreeView_DragAndDropHooked = false;
var RadTreeView_Active = null;
var RadTreeView_DragActive = null;

/////////////////////////////
///    RadTreeNode       ///
////////////////////////////
function RadTreeNode() 
{
	this.Parent = null;
	this.TreeView = null;
	this.Nodes = new Array();
	this.ID = null;
	this.SignImage = null;
	this.SignImageExpanded = null;
	this.Image = 0;
	this.ImageExpanded = 0;
	this.Action = null;
	this.Index = 0;
	
	this.Text = null;
	this.Value = null;
	this.Category = null;
	
	this.NodeCss = null;
	this.NodeCssOver = null;
	this.NodeCssSelect = null;
	
	this.ContextMenuName = null;
	
	this.Enabled = true;
	this.Expanded = false;
	this.Checked = false;
	this.Selected = false;
}

RadTreeNode.prototype.Next = function()
{
	var nodeCollection = (this.Parent != null) ? this.Parent.Nodes : this.TreeView.Nodes;	
	return (this.Index >= nodeCollection.length) ? null : nodeCollection[this.Index + 1];	
}

RadTreeNode.prototype.Prev = function()
{
	var nodeCollection = (this.Parent != null) ? this.Parent.Nodes : this.TreeView.Nodes;
	return (this.Index <= 0) ? null : nodeCollection[this.Index - 1];
}

RadTreeNode.prototype.NextVisible = function()
{
	if (this.Expanded)
		return this.Nodes[0];
	if (this.Next() != null)
		return this.Next();	
	
	var currentNode = this;
	while (currentNode.Parent != null)
	{
		if (currentNode.Parent.Next() != null);
			return currentNode.Parent.Next();
		currentNode = currentNode.Parent;
	}
	
	return null;
}

RadTreeNode.prototype.PrevVisible = function()
{	
	if (this.Prev() != null)
		return this.Prev();
	if (this.Parent != null)
		return this.Parent;
	
	return null;
}

RadTreeNode.prototype.Toggle = function()
{
	if (!this.Nodes.length)
		return;
	if (!this.TreeView.FireEvent(this.TreeView.BeforeClientToggle, this))
		return;
	
	(this.Expanded) ? this.Collapse() : this.Expand();	
	
	if (!this.TreeView.FireEvent(this.TreeView.AfterClientToggle, this));
}

RadTreeNode.prototype.CollapseNonParentNodes = function()
{
    for (i=0;i<this.TreeView.AllNodes.length;i++)
	{
	   if (!this.IsParent(this.TreeView.AllNodes[i]))
	   {
			this.TreeView.AllNodes[i].CollapseNoEffect();
	   }
	} 
}

RadTreeNode.prototype.Expand = function()
{	
    if (this.TreeView.SingleExpandPath)		
		this.CollapseNonParentNodes();
	
    var childGroup = document.getElementById("G" + this.ID); 
   
	childGroup.style.overflow = "hidden";
	childGroup.style.height = "1px";  
    childGroup.style.display = "block";	    
    childGroup.firstChild.style.position = "relative";
    
	window.setTimeout( "rtvNodeExpand(1,'" + childGroup.id + "');", 20);
    
	this.ImageOn();
	this.SignOn();
	this.Expanded = true;
	this.TreeView.UpdateExpandedState();
}

RadTreeNode.prototype.CollapseNoEffect = function()
{	
	if (this.Nodes.length > 0)
	{
		var childGroup = document.getElementById("G" + this.ID); 		
		childGroup.style.display = "none";
		
		this.ImageOff();
		this.SignOff();
		this.Expanded = false;
		this.TreeView.UpdateExpandedState();
	}
}

RadTreeNode.prototype.Collapse = function()
{	
	if (this.Nodes.length > 0)
	{				
		var childGroup = document.getElementById("G" + this.ID); 
		childGroup.style.overflow = "hidden";
		childGroup.style.display = "block";	    
		childGroup.firstChild.style.position = "relative"	
		
		var s = "rtvNodeCollapse(" + childGroup.scrollHeight + ",'" + childGroup.id + "');";				
		window.setTimeout( "rtvNodeCollapse(" + childGroup.scrollHeight + ",'" + childGroup.id + "');", 20);
		
		this.ImageOff();
		this.SignOff();
		this.Expanded = false;
		this.TreeView.UpdateExpandedState();
	}
}

RadTreeNode.prototype.Highlight = function(e)
{
	if (!this.Enabled) 
		return;	
	if (!this.TreeView.FireEvent(this.TreeView.BeforeClientHighlight, this))
		return;	
	
	if (this.TreeView.MultipleSelect && e.ctrlKey)
	{
		if (this.Selected)
		{
			this.TextElement().className = this.NodeCss;
			this.Selected = false;
			this.TreeView.UpdateSelectedState();			
			this.TreeView.FireEvent(this.TreeView.AfterClientHighlight, this);
			return;				
		}
	}
	else
	{
		this.TreeView.UnSelectAllNodes();
	}
	
	this.TextElement().className = this.NodeCssSelect;
	this.TreeView.SelectNode(this);
	this.TreeView.FireEvent(this.TreeView.AfterClientHighlight, this);
}

RadTreeNode.prototype.ExecuteAction = function()
{	
	if (!this.TreeView.FireEvent(this.TreeView.BeforeClientClick, this))
		return;

	if (this.Action != null)
	{		
		if (this.Action.substr(0,12) == "__doPostBack" && this.TreeView.CausesValidation)
		{			
			if (typeof(Page_ClientValidate) != 'function' ||  Page_ClientValidate())
			{
				eval(this.Action);	
			}
		}
		else
		{	
			eval(this.Action);
		}
	}	
}

RadTreeNode.prototype.Select = function(e)
{	
	if (!this.Enabled) return;
	this.Highlight(e);
	this.TreeView.LastHighlighted = this;
	if (!this.TreeView.CancelAction)
		this.ExecuteAction();	
}

RadTreeNode.prototype.UnSelect = function()
{
	this.TextElement().className = this.NodeCss;
	this.Selected = false;
}

RadTreeNode.prototype.Hover = function()
{	
	if (!this.Enabled) return;
	this.TreeView.LastHighlighted = this;
	if (!this.Selected)	
		this.TextElement().className = this.NodeCssOver;	
}

RadTreeNode.prototype.UnHover = function()
{
	if (!this.Enabled) return;
	this.TreeView.LastHighlighted = null;
	if (!this.Selected)	
		this.TextElement().className = this.NodeCss;	
}

RadTreeNode.prototype.CheckBoxClick = function(e)
{	
	if (!this.TreeView.FireEvent(this.TreeView.BeforeClientCheck, this))
		return;	
	
	this.Checked = !this.Checked;
	this.TreeView.UpdateCheckedState();
	
	this.TreeView.FireEvent(this.TreeView.AfterClientCheck, this);
}

RadTreeNode.prototype.Check = function()
{
	if (this.CheckElement() != null)
	{		
		this.CheckElement().checked = true;
		this.Checked = true;
		this.TreeView.UpdateCheckedState();
	}	
}

RadTreeNode.prototype.UnCheck = function()
{
	if (this.CheckElement() != null)
	{
		this.CheckElement().checked = false;
		this.Checked = false;
		this.TreeView.UpdateCheckedState();
	}	
}

RadTreeNode.prototype.IsSet = function(a) 
{
   return (a != null && a != "");
}

RadTreeNode.prototype.ImageOn = function() 
{	
	var imageElement = document.getElementById(this.ID + "i");
	if (this.ImageExpanded != 0)	
		imageElement.src = this.ImageExpanded; 	
}

RadTreeNode.prototype.ImageOff = function() 
{ 
	var imageElement = document.getElementById(this.ID + "i");
	if (this.Image != 0) 
		imageElement.src = this.Image; 
}

RadTreeNode.prototype.SignOn = function() 
{ 
	var signElement = document.getElementById(this.ID + "c");
	if (this.IsSet(this.SignImageExpanded)) 
		signElement.src = this.SignImageExpanded; 
}

RadTreeNode.prototype.SignOff = function()
{ 
	var signElement = document.getElementById(this.ID + "c");
	if (this.IsSet(this.SignImage)) 
		signElement.src = this.SignImage; 
}

RadTreeNode.prototype.TextElement = function()
{
	return document.getElementById(this.ID).getElementsByTagName("span")[0];
}

RadTreeNode.prototype.CheckElement = function()
{
	return document.getElementById(this.ID).getElementsByTagName("input")[0];
}

RadTreeNode.prototype.IsParent = function(node)
{
   var parent = this.Parent
   while (parent != null)
   {
		if (node == parent)
			return true;
			
		parent = parent.Parent;		
   }
   
   return false;
}
   
   
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///    RadTreeView Defs
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   
function RadTreeView(uniqueId, id, firstNodeId, _nodeCss, _nodeCssOver, _nodeCssSelect, _multipleSelect, _ie, _serverId, _dragAndDrop)
{
	this.ID = _serverId;
	this.UniqueId = uniqueId;
	this.Container = id;
	this.Nodes = new Array();
	this.AllNodes = new Array();
	this.NodeCss = _nodeCss;
	this.NodeCssOver = _nodeCssOver;
	this.NodeCssSelect = _nodeCssSelect;	
	this.MultipleSelect = _multipleSelect;
	this.IE = _ie;
	this.SelectedNode = null;
	
	this.DragAndDrop = _dragAndDrop;
	this.DragMode = false;
	this.DragSource = null;	
	this.DragClone = null;
	this.LastHighlighted = null;
	this.MouseInside = false;
	this.HtmlElementID = "";
	
	// Client-side events
	this.BeforeClientClick = null;
	this.BeforeClientHighlight = null;
	this.AfterClientHighlight = null;
	this.BeforeClientDrop = null;
	this.BeforeClientToggle = null;
	this.AfterClientToggle = null;
	this.BeforeClientContextClick = null;
	this.AfterClientContextClick = null;
	this.BeforeClientCheck = null;	
	this.AfterClientCheck = null;
	this.AfterClientMove = null;
		
	this.CausesValidation = true;
	this.ContextMenuVisible = false;
	this.ContextMenuName = null;
	this.SingleExpandPath = false;
	
	var ImageList = new Array();
	this.PreloadImages(ImageList);
	this.LoadTree(ImageList);
	
	var images = document.getElementById(this.Container).getElementsByTagName("IMG");
	var i;
	for (i=0; i<images.length; i++)
	{
		var index = images[i].getAttribute("r");
		if (index != null && index != "")
			images[i].src = ImageList[index].src;
		(document.all) ? images[i].align = "absmiddle" : images[i].valign = "absmiddle";
	}	
	
	if (!RadTreeView_KeyboardHooked)	
	{		
		RadTreeView_KeyboardHooked = true;
		if (this.IE)
			document.attachEvent('onkeydown', this.KeyDown);
		else
			document.addEventListener('keydown', this.KeyDown, false);
	}

	if (!RadTreeView_DragAndDropHooked)
	{		
		RadTreeView_DragAndDropHooked = true;
		if (this.IE)
		{
		  if (this.DragAndDrop)
			  document.attachEvent('onmousemove', rtvMouseMove);
		  document.attachEvent('onmouseup', rtvMouseUp);		  
		}
		else
		{
		  if (this.DragAndDrop)
				document.addEventListener('mousemove', rtvMouseMove, false);
		  document.addEventListener('mouseup', rtvMouseUp, false);
		}
	}	
}

RadTreeView.prototype.PreloadImages = function(images)
{
	var imageData = eval(this.ID + "ImageData"); 
	var i;
	for (i=0; i<imageData.length; i++)
	{
		var image = new Image();
		image.src = imageData[i];
		images[i] = image;
	}
}

RadTreeView.prototype.FindNode = function(node)
{
	var i;
	for (i=0;i<this.AllNodes.length;i++)
	{
		if (this.AllNodes[i].ID == node)		
			return this.AllNodes[i];		
	}
	return null;
}

RadTreeView.prototype.LoadTree = function(imageList)
{
	var cd = eval(this.ID + "ClientData"); 
	var i;	
	var parent = null;
	
	for (i=0; i<cd.length; i++)
	{
		var rtNode = new RadTreeNode();
		rtNode.ID = cd[i][0];
		rtNode.TreeView = this;
		var parentIndex = cd[i][17];
		if (parentIndex >= 0)
			rtNode.Parent = this.AllNodes[parentIndex-1];
		rtNode.NodeCss = this.NodeCss;	
		rtNode.NodeCssOver = this.NodeCssOver;
		rtNode.NodeCssSelect = this.NodeCssSelect;		
		rtNode.Text = cd[i][1];
		rtNode.Value = cd[i][2];
		rtNode.Category = cd[i][3];
		rtNode.SignImage = imageList[cd[i][4]].src;
		rtNode.SignImageExpanded = imageList[cd[i][5]].src;		
		if (cd[i][6] > 0)
			rtNode.Image = imageList[cd[i][6]].src;
		if (cd[i][7] > 0)
			rtNode.ImageExpanded = imageList[cd[i][7]].src;
		rtNode.Selected = cd[i][8];
		if (rtNode.Selected)
			this.SelectedNode = rtNode;
		rtNode.Checked = cd[i][9];
		rtNode.Enabled = cd[i][10];
		rtNode.Expanded = cd[i][11];
		rtNode.Action = cd[i][12];
		if (this.IsSet(cd[i][13])) rtNode.NodeCss = cd[i][13];
		if (this.IsSet(cd[i][14])) rtNode.ContextMenuName = cd[i][14];		
		this.AllNodes[this.AllNodes.length] = rtNode;		
		
		if (rtNode.Parent != null)
			rtNode.Parent.Nodes[rtNode.Parent.Nodes.length] = rtNode;		
		else		
			this.Nodes[this.Nodes.length] = rtNode;	
		
		//if (cd[i][15])
		//	parent = rtNode;			
		rtNode.Index = cd[i][16];
	}
}


RadTreeView.prototype.Toggle = function(node) {  this.FindNode(node).Toggle(); }
RadTreeView.prototype.Select = function(node, e) { this.FindNode(node).Select(e); }
RadTreeView.prototype.Hover = function(node) { this.FindNode(node).Hover(); }
RadTreeView.prototype.UnHover = function(node) { this.FindNode(node).UnHover(); }
RadTreeView.prototype.CheckBoxClick = function(node, e) { this.FindNode(node).CheckBoxClick(e); }
RadTreeView.prototype.Highlight = function(node, e) { this.FindNode(node).Highlight(e); }


RadTreeView.prototype.SelectNode = function(node)
{
	this.SelectedNode = node;
	node.Selected = true;
	this.UpdateSelectedState();
}

RadTreeView.prototype.GetSelectedNodes = function()
{
	var i;
	var selNodes = new Array();
	for (i=0; i<this.AllNodes.length; i++)
	{
		if (this.AllNodes[i].Selected)		
			selNodes[selNodes.length] = this.AllNodes[i];		
	}		
	return selNodes;
}

RadTreeView.prototype.UnSelectAllNodes = function(node)
{
	var i;
	for (i=0; i<this.AllNodes.length;i++)
	{
		if (this.AllNodes[i].Selected)		
			this.AllNodes[i].UnSelect();		
	}
}

RadTreeView.prototype.KeyDown = function(e) 
{	
	try	{ var dummy = tlrkTreeViews.length;	} catch (exception) { return; };
	
	var LastActiveRadTreeView = RadTreeView_Active;
	
	if (LastActiveRadTreeView != null && LastActiveRadTreeView.SelectedNode != null)
	{		
		if(e.keyCode == 107 || e.keyCode == 109 || e.keyCode == 37 || e.keyCode == 39)
			LastActiveRadTreeView.SelectedNode.Toggle();

		if(e.keyCode == 40 && LastActiveRadTreeView.SelectedNode.NextVisible() != null) // down
			LastActiveRadTreeView.SelectedNode.NextVisible().Highlight(e);

		if(e.keyCode == 38 && LastActiveRadTreeView.SelectedNode.PrevVisible() != null) // up
			LastActiveRadTreeView.SelectedNode.PrevVisible().Highlight(e);

		if(e.keyCode == 13) // Enter
			LastActiveRadTreeView.SelectedNode.ExecuteAction();				
		
		if(e.keyCode == 32) // Space
			LastActiveRadTreeView.SelectedNode.CheckBoxClick();	
	}
}


RadTreeView.prototype.UpdateExpandedState = function() 
{
	var expandedState = "";	
	for (var i=0; i<this.AllNodes.length; i++)
	{
		var isExpanded = (this.AllNodes[i].Expanded) ? "1" : "0";
		expandedState += isExpanded;
	}
	document.getElementById(this.UniqueId + "_expanded").value = expandedState;
}

RadTreeView.prototype.UpdateCheckedState = function() 
{
	var checkedState = "";
	for (var i=0; i<this.AllNodes.length; i++)
	{
		var isChecked = (this.AllNodes[i].Checked) ? "1" : "0";
		checkedState += isChecked;
	}
	document.getElementById(this.UniqueId + "_checked").value = checkedState;
}

RadTreeView.prototype.UpdateSelectedState = function() 
{
	var selectedState = "";	
	for (var i=0; i<this.AllNodes.length; i++)
	{
		var isSelected = (this.AllNodes[i].Selected) ? "1" : "0";
		selectedState += isSelected;
	}
	document.getElementById(this.UniqueId + "_selected").value = selectedState;	
}


RadTreeView.prototype.Scroll = function(e)
{	
	document.getElementById(this.UniqueId + "_scroll").value = document.getElementById(this.Container).scrollTop;	
}

RadTreeView.prototype.ContextMenuClick = function(e,p1,p2,p3) 
{
	if (!this.FireEvent(this.BeforeClientContextClick, this.LastHighlighted, p1))
		return;	
	
	if (p2)
	{
		var postBack = "__doPostBack('" + this.UniqueId + "','" + "@CC:" + this.SelectedNode.ID + "#" + p1 + "#" + p3 + "')";		
		eval(postBack);
	}
		
	instance = this;
	window.setTimeout("instance.HideContextMenu()", 10);
	this.HideContextMenu();	
}

RadTreeView.prototype.ContextMenu = function(e) 
{	
	var src = (this.IE) ? e.srcElement : e.target;
	if (this.LastHighlighted != null && this.LastHighlighted.ContextMenuName != null)
	{
		if (!this.ContextMenuVisible)
		{			
			try
			{
			    if (!this.FindNode(src.parentNode.id).Selected)
				    this.Highlight(src.parentNode.id, e);
			
			    if (this.IE)
				    this.ShowContextMenu(this.LastHighlighted.ContextMenuName, e.x, e.y);
				else
					this.ShowContextMenu(this.LastHighlighted.ContextMenuName, e.clientX, e.clientY);
			}
			catch (e)
			{
			}
		}		
	}
	
	this.IE ? e.returnValue = false : e.preventDefault();	
}

RadTreeView.prototype.ShowContextMenu = function(name, x, y)
{
	var menu = document.getElementById("rtvcm" + this.ID + name);
	menu.style.left = x + document.body.scrollLeft + "px";
	menu.style.top = y + document.body.scrollTop + "px";	
	menu.style.display = "block";	
	this.ContextMenuVisible = true;
	this.ContextMenuName = name;
}

RadTreeView.prototype.HideContextMenu = function()
{	
	var menu = document.getElementById("rtvcm" + this.ID + this.ContextMenuName);
	if (menu)
		menu.style.display = "none";
	
	this.ContextMenuVisible = false;
}

RadTreeView.prototype.ClickDispatcher = function(e) 
{
	var src = (this.IE) ? e.srcElement : e.target;	
	if (src.tagName == "SPAN" || (src.tagName == "IMG" && ( src.getAttribute("rtimg") != null) && (src.getAttribute("rtimg") != "") ))
	{
		this.Select(src.parentNode.id, e);
	}
	if (src.tagName == "IMG" && src.getAttribute("rtsign") != null)
	{
		var myNode = this.FindNode(src.parentNode.id);
		this.Toggle(src.parentNode.id);
	}
	if (src.tagName == "INPUT")
	{
		if (src.type == "checkbox")
		{			
			this.CheckBoxClick(src.parentNode.id, e);
		}
	}
	if (src.nodeName == "#text")
	{
		this.Select(src.parentNode.parentNode.id, e);
	}
}

RadTreeView.prototype.DoubleClickDispatcher = function(e) 
{
	var src = (this.IE) ? e.srcElement : e.target;
	if (src.tagName == "SPAN")
	{
		this.Toggle(src.parentNode.id);
	}
}

RadTreeView.prototype.MouseOverDispatcher = function(e) 
{	
	var src = (this.IE) ? e.srcElement : e.target;
	
	if (src.tagName == "SPAN")
	{	
		this.Hover(src.parentNode.id);
	}
	else
	{
		var nodeName = src.nodeName;
		if (nodeName == "#text")
			this.Hover(src.parentNode.parentNode.id);
	}
}

RadTreeView.prototype.MouseOutDispatcher = function(e) 
{	
	var src = (this.IE) ? e.srcElement : e.target;	
	if (src.tagName == "SPAN")
	{
		this.UnHover(src.parentNode.id);
	}
	else
	{
		var nodeName = src.nodeName;
		if (nodeName == "#text")
			this.UnHover(src.parentNode.parentNode.id);	
	}	
}

RadTreeView.prototype.MouseDown = function(e) 
{	
	if (this.LastHighlighted != null && this.DragAndDrop)
	{		
		if (e.button == 2) return;
		this.DragSource = this.LastHighlighted;
		this.DragClone = this.LastHighlighted.TextElement().cloneNode(true);
		this.DragClone.className = null;
		RadTreeView_DragActive = this;		
		
		var res = "";		
		if (this.MultipleSelect)
		{
			for (var i=0; i<this.AllNodes.length; i++)
			{
				if (this.AllNodes[i].Selected)
				{
					if (this.IE)
					{
						var img = this.AllNodes[i].TextElement().previousSibling;
						if (img.getAttribute("rtimg"))
							res += this.AllNodes[i].TextElement().previousSibling.outerHTML;
					}
					res += this.AllNodes[i].TextElement().innerHTML + "<BR>";
				}
			}
		}
		
		if (res == "")
		{
			var img = this.LastHighlighted.TextElement().previousSibling;
			if (img.getAttribute("rtimg") == "T")
			{
				res += "<img valign='absmiddle' src=" + img.src + ">";
			}
			res += this.LastHighlighted.TextElement().innerHTML;
		}
		
		this.DragClone.innerHTML = res;
		this.DragClone.style.color = "gray";
		this.DragClone.style.position = "absolute";
		this.DragClone.style.visibility = "hidden";
		document.body.appendChild(this.DragClone);
	}
}

RadTreeView.prototype.FireEvent = function(handler, a, b, c) 
{
	if (!handler)
		return true;
	
	RadTreeViewGlobalFirstParam = a;
	RadTreeViewGlobalSecondParam = b;
	RadTreeViewGlobalThirdParam = c;
	
	var s = handler;
	s = s + "(RadTreeViewGlobalFirstParam";
	s = s + ",RadTreeViewGlobalSecondParam";
	s = s + ",RadTreeViewGlobalThirdParam";
	s = s + ");";
			
	return eval(s);	
}


RadTreeView.prototype.IsSet = function(a) 
{
   return (a != null && a != "");
}

function rtvMouseMove(e)
{	
	var i;
	try
	{
		for (i=0; i<tlrkTreeViews.length; i++)	
			if (tlrkTreeViews[i].ContextMenuVisible)
				return;
	}
	catch (e) {}
	
	if (RadTreeView_DragActive != null && RadTreeView_DragActive.DragClone != null)
	{
		var newx, newy;
		if (RadTreeView_DragActive.IE)
		{
			newx = e.x + document.body.scrollLeft + 8;
			newy = e.y + document.body.scrollTop + 4;
		}
		else
		{
			newx = e.clientX + document.body.scrollLeft + 8;
			newy = e.clientY + document.body.scrollTop + 4;
		}
		
		RadTreeView_DragActive.DragClone.style.zIndex = 999;		
		RadTreeView_DragActive.DragClone.style.top = newy;
		RadTreeView_DragActive.DragClone.style.left = newx;
		RadTreeView_DragActive.DragClone.style.visibility = "visible";
		
		RadTreeView_DragActive.FireEvent(RadTreeView_DragActive.AfterClientMove, e);
	}
}

function rtvMouseUp(e) 
{	
	if (RadTreeView_Active == null)
		return;

	var i;	
	for (i=0; i<tlrkTreeViews.length; i++)
	{
		if (tlrkTreeViews[i].ContextMenuVisible)
		{
			RadTreeView_ContextMenuToBeHidden = tlrkTreeViews[i];
			window.setTimeout("RadTreeView_ContextMenuToBeHidden.HideContextMenu();", 10);		
			return;			
		}
	}
	
	if (RadTreeView_DragActive == null || RadTreeView_DragActive.DragClone == null)
		return;	
	
	document.body.removeChild(RadTreeView_DragActive.DragClone);
	RadTreeView_DragActive.DragClone = null;
	
	var sourceNode = RadTreeView_DragActive.DragSource;
	var destNode = RadTreeView_Active.LastHighlighted;
	var destTree = RadTreeView_Active;
	
	if (sourceNode == destNode)	return;
	
	if (!RadTreeView_DragActive.FireEvent(RadTreeView_DragActive.BeforeClientDrop, sourceNode, destNode, e))
		return;
	
	var sourceString = RadTreeView_DragActive.ID + ":" + sourceNode.ID + ":";
	var destString = "";
	if (destNode == null)	
		destString = "null" + ":" + RadTreeView_DragActive.HtmlElementID;
	else	
		destString = destTree.ID + ":" + destNode.ID;
	
	var postBack = "__doPostBack('" + RadTreeView_DragActive.UniqueId + "','" + "@ND:" + sourceString + destString + "')";	
	eval(postBack);
			
	RadTreeView_DragActive = null;
}

function rtvNodeExpand(a, id)
{
	var scrollDiv = document.getElementById(id);
	var heightLimit = scrollDiv.scrollHeight;	
	var step = (heightLimit - a) / 3;
	var height = a + step;
	
	if (height > heightLimit - 1)
	{		
		scrollDiv.style.height = "";		
	    scrollDiv.firstChild.style.position = "";    
	}	
	else
	{
		scrollDiv.style.height = height + "px";	
		window.setTimeout("rtvNodeExpand(" + height + "," + "'" + id + "');", 5);	
	}	
}

function rtvNodeCollapse(a, id)
{
	var scrollDiv = document.getElementById(id);
	var heightLimit = scrollDiv.scrollHeight;
	var step = (heightLimit - Math.abs(heightLimit - a)) / 3;
	var height = a - step;	
	
	if (height <= 1)
	{		
		scrollDiv.style.height = "";		
	    scrollDiv.style.display = "none";
	    scrollDiv.firstChild.style.position = "";    
	}	
	else
	{
		scrollDiv.style.height = height + "px";	
		window.setTimeout("rtvNodeCollapse(" + height + "," + "'" + id + "');", 5);	
	}	
}

function rtvDispatcher(t,w,e,p1,p2,p3)
{	
	try	{ var dummy = tlrkTreeViews.length;	} catch (exception) { return; };		
	if (tlrkTreeViews)
	{
		var i;
		var targetTree = null;
		for (i=0; i<tlrkTreeViews.length; i++)
		{
			if (tlrkTreeViews[i].ID == t)
			{
				targetTree = tlrkTreeViews[i];
				break;
			}
		}
		
		
		if (targetTree.ContextMenuVisible && w != "mclick" && w != "cclick") return;
		RadTreeView_Active = targetTree;
						
		switch (w)
		{
			case "mover"	 : targetTree.MouseOverDispatcher(e); break;
			case "mout"		 : targetTree.MouseOutDispatcher(e); break;
			case "mclick"	 : targetTree.ClickDispatcher(e); break;
			case "mdclick"	 : targetTree.DoubleClickDispatcher(e); break;
			case "mdown"	 : targetTree.MouseDown(e); break;
			case "mup"		 : targetTree.MouseUp(e); break;
			case "context"   : targetTree.ContextMenu(e); break;
			case "cclick"	 : targetTree.ContextMenuClick(e,p1,p2,p3); break;
			case "scroll"	 : targetTree.Scroll(e); break;
		}
	}
}