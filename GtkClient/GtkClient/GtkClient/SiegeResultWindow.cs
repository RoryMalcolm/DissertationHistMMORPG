using System;
using Gtk;
using hist_mmorpg;

public class SiegeResultWindow
	{
		Table SiegeLayout;
		Label SiegeResult;
		Label SiegeLabel;
		Label SiegeLabelOutput;

	public SiegeResultWindow (string sieger, string siegee, string victor)
		{
			uint tableRows = 5;
			SiegeLayout = new Table (6, 2, false);
			SiegeResult = new Label ("Siege Result");
			SiegeLabel = new Label ("Siege:");
			SiegeLabelOutput = new Label (sieger + " -> " + siegee);
			SiegeLayout.Attach (SiegeResult, 0, 2, 0, 1);
			SiegeLayout.Attach (SiegeLabel, 0, 1, 1, 2);
			SiegeLayout.Attach (SiegeLabelOutput, 1, 2, 1, 2);
            SiegeLayout.Attach(new Label("Victor: "), 0, 1, 2, 3);
            SiegeLayout.Attach(new Label(victor), 1, 2, 2, 3);

    }

    public Table getSiegeLayout(){
		return SiegeLayout;
	}

	public void DestroySiege(){
		SiegeLayout.Destroy ();
	}
}