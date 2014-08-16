using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Livit.Techtest.UnitTests
{
	[TestFixture]
	[Category("Player Status Unit Tests")]
	internal class PlayerStatusTests 
	{
		[Test]
		[Category ("Save Load")]
		public void SaveAndLoadTest ()
		{
			PlayerStatusModel playerStatusModel = new PlayerStatusModel();
			playerStatusModel.DataPath = Application.dataPath + "/Editor/Tests/TestData/player_status.xml";

			// fill the player status model with some data
			playerStatusModel.Level = "2";
			playerStatusModel.Step = "1";


			List<string> items = new List<string>();
			items.Add("1");
			items.Add("1");
			items.Add("1");
			items.Add("2");
			items.Add("2");

			// add the list with the 5 items to the player status model
			playerStatusModel.Items = items;

			// save player status into an xml file
			playerStatusModel.Save();

			// clear all player data
			playerStatusModel.Clear();

			// assert that all players data is empty
			Assert.That(playerStatusModel.Level == "");
			Assert.That(playerStatusModel.Step == "");
			Assert.That(playerStatusModel.Items.Count == 0);

			// now load the saved data into the model
			playerStatusModel.Load();

			// Assert that the players data is now equal to the data that is saved 
			// into the XML file
			Assert.That(playerStatusModel.Level == "2");
			Assert.That(playerStatusModel.Step == "1");
			Assert.That(playerStatusModel.Items.Count == 5);
		}		
	}
}



