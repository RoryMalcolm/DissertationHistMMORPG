using System;
using Gtk;
public class LogInWindow
{
	static Table layout;
	static Entry password; 
	static Entry username;
	static Button okayButton;
	static Window loginWin;
	public LogInWindow ()
	{
		loginWin = new Window ("Login");
		layout = new Table (3, 2, false);
		password = new Entry ();
		username = new Entry ();
		okayButton = new Button ("Okay");
		layout.Attach (new Label ("Username: "), 0, 1, 0, 1);
		layout.Attach (username, 1, 2,0,1);
		layout.Attach (new Label ("Password: "), 0, 1, 1, 2);
		layout.Attach (password, 1, 2,1,2);
		layout.Attach (okayButton, 0, 2, 2, 3);
		okayButton.Clicked += LogInClick;
		loginWin.Add (layout);
		loginWin.ShowAll ();
	}

	public void LogInClick(object obj, EventArgs args){
        
		GtkHelloWorld.LoggedIn (username.Text, password.Text, obj, args);
		loginWin.Destroy ();
	}
}