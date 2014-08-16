using UnityEngine;
using System.Collections;

public class LevelVM 
{
	
	Level level;
	
	public LevelVM(Level l)
	{
		level = l;
	}

	public Level Level
	{
		get 
		{
			return level;
		}
	}

	public StepVM GetStepVM(int index)
	{
		Step step = level.StepList[index];
		return new StepVM(step);
	}

	public StepVM GetFirstStepVM()
	{
		Step step = level.StepList[0];
		return new StepVM(step);
	}

	public StepVM GetStepVM(string id)
	{
		for(int i=0; i<level.StepList.Length; i++)
		{
			Step step = level.StepList[i];
			if(step.Id == id) return new StepVM(step);
		}
		return null;
	}
}



