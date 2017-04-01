using Android.App;
using Android.Widget;
using Android.OS;

namespace JominiEngineAndroid
{
	[Activity(Label = "JominiEngineAndroid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
	    private PlayerOperations _playerOperations;
	    private TextTestClient _testClient;
	    private Button _northWButton;
	    private Button _northEButton;
	    private Button _westButton;
	    private Button _eastButton;
	    private Button _southEButton;
	    private Button _southWButton;
	    private Button _hireButton;
	    private Button _siegeButton;
	    private TextView _currentFiefTextView;
        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		    _playerOperations = new PlayerOperations();
            _testClient = new TextTestClient();
            _testClient.LogInAndConnect("helen", "potato");
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
		    _currentFiefTextView = FindViewById<TextView>(Resource.Id.current_fief);
            SetUpButtons();
        }

	    private void SetUpButtons()
	    {
            _northWButton = FindViewById<Button>(Resource.Id.north_west);
            _northEButton = FindViewById<Button>(Resource.Id.north_east);
            _westButton = FindViewById<Button>(Resource.Id.west);
            _eastButton = FindViewById<Button>(Resource.Id.east);
            _southWButton = FindViewById<Button>(Resource.Id.south_west);
            _southEButton = FindViewById<Button>(Resource.Id.south_east);
            _hireButton = FindViewById<Button>(Resource.Id.hire);
            _siegeButton = FindViewById<Button>(Resource.Id.siege);
            _northWButton.Click += delegate
            {
                _currentFiefTextView.Text = _playerOperations.Move(PlayerOperations.MoveDirections.Nw, _testClient).fiefID;
            };
            _northEButton.Click += delegate
            {
                _currentFiefTextView.Text = _playerOperations.Move(PlayerOperations.MoveDirections.Ne, _testClient).fiefID;
            };
            _eastButton.Click += delegate
            {
                _currentFiefTextView.Text = _playerOperations.Move(PlayerOperations.MoveDirections.E, _testClient).fiefID;
            };
            _westButton.Click += delegate
            {
                _currentFiefTextView.Text = _playerOperations.Move(PlayerOperations.MoveDirections.W, _testClient).fiefID;
            };
            _southWButton.Click += delegate
            {
                _currentFiefTextView.Text = _playerOperations.Move(PlayerOperations.MoveDirections.Sw, _testClient).fiefID;
            };
            _southEButton.Click += delegate
            {
                _currentFiefTextView.Text = _playerOperations.Move(PlayerOperations.MoveDirections.Se, _testClient).fiefID;
            };
	        _hireButton.Click += delegate
	        {
	            _playerOperations.HireTroops(20, _testClient);
	        };
	        _siegeButton.Click += delegate
	        {
	            _playerOperations.SiegeCurrentFief(_testClient);
	        };
	    }
    }
}

