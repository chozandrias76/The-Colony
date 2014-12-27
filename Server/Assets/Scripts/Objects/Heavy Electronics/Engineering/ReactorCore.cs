using System;
using UnityEngine;

public class ReactorCore : HeavyElectronic
{
	public ReactorCore()
	{
		name = "Reactor Core";
	}
	
	private float reactorCharge = 0f;
	private float reactorMaxCharge = 10f;
	
	enum ReactorStage
	{
		Stage1 = 1,
		Stage2,
		Stage3,
		Stage4,
		Unknown
	}
	int currentStage = 0;
	int[] stageOutput =
	{
		12656,
		50625,
		113906,
		202500,
		316406,
	};
	
	public string status
	{
		get
		{
			return String.Format("Current stage {0} outputting {1}", currentStage, stageOutput[currentStage]);
		}
		set
		{
		}
	}
	public float ReactorCharge
	{
		get
		{
			return reactorCharge;
		}
		set
		{
			if(value > reactorCharge)
			{
				reactorCharge = value;
			}
		}
	}
	
	void Update()
	{
		//TODO: Detect being hit with a charge
		if(reactorCharge >= reactorMaxCharge && !online)
		{
			online = true;
		}
		
	}
}


