using System;
using Gtk;

public class SiegeResultWindow
	{
		Table SiegeLayout;
		Label SiegeResult;
		Label SiegeLabel;
		Label SiegeLabelOutput;

	public SiegeResultWindow (string sieger, string siegee)
		{
			uint tableRows = 5;
			SiegeLayout = new Table (4, 2, false);
			SiegeResult = new Label ("Siege Result");
			SiegeLabel = new Label ("Siege:");
			SiegeLabelOutput = new Label (sieger + " -> " + siegee);
			SiegeLayout.Attach (SiegeResult, 0, 2, 0, 1);
			SiegeLayout.Attach (SiegeLabel, 0, 1, 1, 2);
			SiegeLayout.Attach (SiegeLabelOutput, 1, 2, 1, 2);
		}

	public Table getSiegeLayout(){
		return SiegeLayout;
	}

	public void DestroySiege(){
		SiegeLayout.Destroy ();
	}
}