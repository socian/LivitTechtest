using System;
using System.Net;
using System.Net.Sockets;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityTest.IntegrationTests
{
	public class PlatformRunner
	{
		public static BuildTarget defaultBuildTarget
		{
			get
			{
				var target = EditorPrefs.GetString ("ITR-platformRunnerBuildTarget");
				BuildTarget buildTarget;
				try
				{
					buildTarget = (BuildTarget) Enum.Parse (typeof (BuildTarget), target);
				}
				catch
				{
					return GetDefaultBuildTarget ();
				}
				return buildTarget;
			}
			set { EditorPrefs.SetString ("ITR-platformRunnerBuildTarget", value.ToString ()); }
		}

		[MenuItem ("Unity Test Tools/Platform Runner/Run current scene %#&r")]
		public static void BuildAndRunCurrentScene ()
		{
			Debug.Log ("Building and running current test for " + defaultBuildTarget);
			BuildAndRunInPlayer (new PlatformRunnerConfiguration (defaultBuildTarget));
		}

		[MenuItem ("Unity Test Tools/Platform Runner/Run on platform %#r")]
		public static void RunInPlayer ()
		{
			var w = EditorWindow.GetWindow (typeof (PlatformRunnerSettingsWindow));
			w.Show ();
		}

		public static void BuildAndRunInPlayer (PlatformRunnerConfiguration configuration)
		{
			NetworkResultsReceiver.StopReceiver ();

			var settings = new PlayerSettingConfigurator (false);

			if (configuration.sendResultsOverNetwork)
			{
				try
				{
					var l = new TcpListener (IPAddress.Any, configuration.port);
					l.Start();
					configuration.port = ((IPEndPoint) l.Server.LocalEndPoint).Port;
					l.Stop ();
				}
				catch (SocketException e)
				{
					Debug.LogException (e);
					EditorApplication.Exit (Batch.RETURN_CODE_RUN_ERROR);
				}
			}

			if (InternalEditorUtility.inBatchMode)
				settings.AddConfigurationFile(TestRunnerConfigurator.batchRunFileMarker, "");

			if (configuration.sendResultsOverNetwork)
				settings.AddConfigurationFile (TestRunnerConfigurator.integrationTestsNetwork,
					string.Join ("\n", configuration.GetConnectionIPs ()));

			settings.ChangeSettingsForIntegrationTests ();

			AssetDatabase.Refresh ();

			var result = BuildPipeline.BuildPlayer (configuration.scenes,
										configuration.GetTempPath ( ),
										configuration.buildTarget,
										BuildOptions.AutoRunPlayer | BuildOptions.Development);

			settings.RevertSettingsChanges ();
			settings.RemoveAllConfigurationFiles ();

			AssetDatabase.Refresh();

			if (!string.IsNullOrEmpty (result))
			{
				if (InternalEditorUtility.inBatchMode)
					EditorApplication.Exit (Batch.RETURN_CODE_RUN_ERROR);
				return;
			}

			if (configuration.sendResultsOverNetwork)
				NetworkResultsReceiver.StartReceiver (configuration);
			else
				EditorApplication.Exit (Batch.RETURN_CODE_TESTS_OK);
		}

		private static BuildTarget GetDefaultBuildTarget ()
		{
			switch (EditorUserBuildSettings.selectedBuildTargetGroup)
			{
				case BuildTargetGroup.Android:
					return BuildTarget.Android;
				case BuildTargetGroup.WebPlayer:
					return BuildTarget.WebPlayer;
				case BuildTargetGroup.Standalone:
				default:
				{
					switch (Application.platform)
					{
						case RuntimePlatform.WindowsPlayer:
							return BuildTarget.StandaloneWindows;
						case RuntimePlatform.OSXPlayer:
							return BuildTarget.StandaloneOSXIntel;
						case RuntimePlatform.LinuxPlayer:
							return BuildTarget.StandaloneLinux;
					}
					return BuildTarget.WebPlayer;
				}
			}
		}
	}
}
