using UnityEngine;
using System.Collections;

public class SampleForm : QMRequisitionForms
{

    public SampleForm(IDBadge insertedBadge)
    {
        //insertedBadge.playerName = requester;
        requestersSection = insertedBadge.playerJob;
        //itemsToRequest.Add(SurgeryTable);
    }

	
	public override string ToString ()
	{
		return string.Format ("[SampleForm]");
	}
}

