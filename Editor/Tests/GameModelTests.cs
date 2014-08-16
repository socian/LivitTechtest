using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Livit.Techtest.UnitTests
{

	[TestFixture]
	[Category("Game Model Tests")]
	internal class GameModelTests 
	{

		GameModel LoadGameModel()
		{
			GameModel gameModel = new GameModel();
			gameModel.DataPath = Application.dataPath + "/Editor/Tests/TestData/gamemodel_test_data.xml";
			gameModel.Load();
			return gameModel;
		}

		[Test]
		[Category ("Available Items")]
		public void AvailableItemsTest () 
		{
			GameModel gameModel = LoadGameModel();
		
			// We should have 5 available items
			Assert.That(gameModel.Data.ItemList.Length == 5);
		}

		[Test]
		[Category ("Available Levels")]
		public void AvailableLevelsTest()
		{
			GameModel gameModel = LoadGameModel();

			// we should have only 1 level
			Assert.That(gameModel.Data.LevelList.Length == 1);
		}

		[Test]
		[Category ("Available Steps")]
		public void AvailableStepsTest()
		{
			GameModel gameModel = LoadGameModel();
			
			// get the first level data 
			Level level = gameModel.Data.LevelList[0];
			Assert.That(level != null);

			// we should have 3 steps in the first level
			Assert.That(level.StepList.Length == 3);
		}

		[Test]
		[Category ("Available Items & Quests")]
		public void AvailableItemsAndQuestsTest()
		{
			GameModel gameModel = LoadGameModel();
			
			// get the first level data 
			Level level = gameModel.Data.LevelList[0];
			Assert.That(level != null && level.Id == "LEVEL_1");

			// get the first step of the first level
			Step step = level.StepList[0];
			Assert.That (step != null && step.Id == "STEP_1");

			// Assert that the number of items and quests in 
			// the first step are correct
			Assert.That(step.ItemList.Length == 3);
			Assert.That (step.QuestList.Length == 1);
		}
	}
}