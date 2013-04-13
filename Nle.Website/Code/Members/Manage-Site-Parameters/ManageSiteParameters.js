
function Validate(minLinksPerCycleClientId, minLinksPerCycle, maxLinksPerCycleClientId, maxLinksPerCycle, minFeedsPerPageClientId, minFeedsPerPage, maxFeedsPerPageClientId, maxFeedsPerPage, maxFrequencyClientId, maxFrequency)
{
	var txtId1 = document.getElementById(minLinksPerCycleClientId);
	var txtId2 = document.getElementById(maxLinksPerCycleClientId);
	
	var msg1 = "Your minimum links per cycle must be greater than or equal to " + minLinksPerCycle + " and less than or equal to your maximum links per cycle.";
	var msg2 = "Your maximum links per cycle must be greater than or equal to your minimum links per cycle and less than or equal to " + maxLinksPerCycle + ".";
	if(!AssertIsTrue(txtId1.value >= minLinksPerCycle && txtId1.value <= maxLinksPerCycle && txtId1.value <= txtId2.value, msg1)) return false;
	if(!AssertIsTrue(txtId2.value >= txtId1.value && txtId2.value >= minLinksPerCycle && txtId2.value <= maxLinksPerCycle, msg2)) return false;
	
	
	var txtId3 = document.getElementById(minFeedsPerPageClientId);
	var txtId4 = document.getElementById(maxFeedsPerPageClientId);
	
	msg1 = "Your minimum feeds per page must be greater than or equal to " + minFeedsPerPage + " and less than or equal to your maximum feeds per page.";
	msg2 = "Your maximum feeds per page must be greater than or equal to your minimum feeds per page and less than or equal to " + maxFeedsPerPage + ".";
	if(!AssertIsTrue(txtId3.value >= minFeedsPerPage && txtId3.value <= maxFeedsPerPage && txtId3.value <= txtId4.value, msg1)) return false;
	if(!AssertIsTrue(txtId4.value >= txtId3.value && txtId4.value >= minFeedsPerPage && txtId4.value <= maxFeedsPerPage, msg2)) return false;
	
	var txtId5 = document.getElementById(maxFrequencyClientId);
	
	msg1 = "Your maximum frequency to add links must be greater than 0 and less than " + maxFrequency + ".";
	if(!AssertIsTrue(txtId5.value > 0 && txtId5.value <= maxFrequency, msg1)) return false;
}

function AssertIsTrue(evaluation, message)
{
	if(!evaluation)
		alert(message);
	return evaluation;
}