package mono;

import java.io.*;
import java.lang.String;
import java.util.Locale;
import java.util.HashSet;
import java.util.zip.*;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.res.AssetManager;
import android.util.Log;
import mono.android.Runtime;

public class MonoPackageManager {

	static Object lock = new Object ();
	static boolean initialized;

	static android.content.Context Context;

	public static void LoadApplication (Context context, ApplicationInfo runtimePackage, String[] apks)
	{
		synchronized (lock) {
			if (context instanceof android.app.Application) {
				Context = context;
			}
			if (!initialized) {
				android.content.IntentFilter timezoneChangedFilter  = new android.content.IntentFilter (
						android.content.Intent.ACTION_TIMEZONE_CHANGED
				);
				context.registerReceiver (new mono.android.app.NotifyTimeZoneChanges (), timezoneChangedFilter);
				
				System.loadLibrary("monodroid");
				Locale locale       = Locale.getDefault ();
				String language     = locale.getLanguage () + "-" + locale.getCountry ();
				String filesDir     = context.getFilesDir ().getAbsolutePath ();
				String cacheDir     = context.getCacheDir ().getAbsolutePath ();
				String dataDir      = getNativeLibraryPath (context);
				ClassLoader loader  = context.getClassLoader ();

				Runtime.init (
						language,
						apks,
						getNativeLibraryPath (runtimePackage),
						new String[]{
							filesDir,
							cacheDir,
							dataDir,
						},
						loader,
						new java.io.File (
							android.os.Environment.getExternalStorageDirectory (),
							"Android/data/" + context.getPackageName () + "/files/.__override__").getAbsolutePath (),
						MonoPackageManager_Resources.Assemblies,
						context.getPackageName ());
				
				mono.android.app.ApplicationRegistration.registerApplications ();
				
				initialized = true;
			}
		}
	}

	public static void setContext (Context context)
	{
		// Ignore; vestigial
	}

	static String getNativeLibraryPath (Context context)
	{
	    return getNativeLibraryPath (context.getApplicationInfo ());
	}

	static String getNativeLibraryPath (ApplicationInfo ainfo)
	{
		if (android.os.Build.VERSION.SDK_INT >= 9)
			return ainfo.nativeLibraryDir;
		return ainfo.dataDir + "/lib";
	}

	public static String[] getAssemblies ()
	{
		return MonoPackageManager_Resources.Assemblies;
	}

	public static String[] getDependencies ()
	{
		return MonoPackageManager_Resources.Dependencies;
	}

	public static String getApiPackageName ()
	{
		return MonoPackageManager_Resources.ApiPackageName;
	}
}

class MonoPackageManager_Resources {
	public static final String[] Assemblies = new String[]{
		/* We need to ensure that "JominiEngineAndroid.dll" comes first in this list. */
		"JominiEngineAndroid.dll",
		"ClientDLL.dll",
		"Lidgren.Network.dll",
		"protobuf-net.dll",
		"System.Configuration.dll",
		"ProtoMessage.dll",
		"TestClientROry.exe",
		"hist_mmorpg.exe",
		"QuickGraph.dll",
		"RiakClient.dll",
		"Newtonsoft.Json.dll",
		"System.Web.dll",
		"System.Drawing.dll",
		"System.DirectoryServices.dll",
		"System.Web.RegularExpressions.dll",
		"System.Design.dll",
		"System.Windows.Forms.dll",
		"Accessibility.dll",
		"System.Deployment.dll",
		"System.Runtime.Serialization.Formatters.Soap.dll",
		"System.Data.OracleClient.dll",
		"System.Drawing.Design.dll",
		"System.Web.ApplicationServices.dll",
		"System.DirectoryServices.Protocols.dll",
		"System.Runtime.Caching.dll",
		"System.ServiceProcess.dll",
		"System.Configuration.Install.dll",
		"Microsoft.Build.Utilities.v4.0.dll",
		"Microsoft.Build.Framework.dll",
		"System.Xaml.dll",
		"Microsoft.Build.Tasks.v4.0.dll",
		"QuickGraph.Serialization.dll",
		"System.Threading.dll",
		"System.Runtime.dll",
		"System.Collections.dll",
		"System.Collections.Concurrent.dll",
		"System.Diagnostics.Debug.dll",
		"System.Reflection.dll",
		"System.Linq.dll",
		"System.Runtime.InteropServices.dll",
		"System.Runtime.Extensions.dll",
		"System.Reflection.Extensions.dll",
		"System.Resources.ResourceManager.dll",
		"System.IO.dll",
		"System.Xml.ReaderWriter.dll",
		"System.Text.RegularExpressions.dll",
		"Shim.dll",
		"System.Globalization.dll",
		"System.Xml.XDocument.dll",
		"System.Threading.Tasks.dll",
		"System.Runtime.Serialization.Primitives.dll",
		"System.Net.Requests.dll",
		"System.Net.Primitives.dll",
		"System.Threading.Tasks.Parallel.dll",
	};
	public static final String[] Dependencies = new String[]{
	};
	public static final String ApiPackageName = "Mono.Android.Platform.ApiLevel_23";
}
