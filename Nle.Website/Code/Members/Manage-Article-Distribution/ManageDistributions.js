function displayTotal(totalBoxId, textBoxIds)
{
    var totalBox;
    var textboxes;
    var currBox;
    var total;
    total = 0;
    
    totalBox = document.getElementById(totalBoxId);
    if(totalBox == null)
    {
        alert('Can\'t find Total text box.');
        return;
    }
    
    textboxes = textBoxIds.split(";")
    for ( x in textboxes )
    {
        if(textboxes[x] != '')
        {
            currBox = document.getElementById(textboxes[x]);
            if(currBox == null)
            {
                alert('Can\'t find ' + textboxes[x] + '.');
                return;
            }
            if(currBox.value.length > 0)
                total += parseFloat(currBox.value);
        }
    }
    
    totalBox.innerHTML = total;
    if(total == 100)
        totalBox.style["color"] = "Green";
    else
        totalBox.style["color"] = "Red";
}

