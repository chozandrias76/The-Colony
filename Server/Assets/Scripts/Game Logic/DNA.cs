using UnityEngine;
using System.Collections;

public class DNA : MonoBehaviour
{
	string[] uniqueID =
	{
		"000",
		"000",
		"000",
		"000",
		"000",
		"000"
	};

	public string[] structureDNA =
	{
		"000",
		"000",
		"000",
		"000",
		"000",
		"000",
		"000",
		"000",
		"000",
		"000",
		"000",
		"000",
		"000",
		"000"
	};

	int[] mutationAndDiseaseBlocks = new int[System.Enum.GetNames(typeof(BaseDisease.diseases)).Length + System.Enum.GetNames(typeof(Attributes.mutations)).Length];
	public void GenerateMutationAndDiseaseBlocks()
	{
		int maxValue = 15*15*15;
		for(int i = 0; i < mutationAndDiseaseBlocks.Length; i++)
		{
			mutationAndDiseaseBlocks[i] = (i/mutationAndDiseaseBlocks.Length)*maxValue;
		}
        
	}
	
	public static string ToHex(int integer)
	{
		return string.Format("{0:X}", integer);
	}
	
	public static int FromHex(string stringValue)
	{
		return System.Int32.Parse(stringValue,System.Globalization.NumberStyles.HexNumber);
	}
	
	public void IrradiateBlock(int blockid, int intensity, int duration)
	{
		//int[] possibleDiseasesForBlock = FindDiseasesInBlock(blockid);
		int blockValueForBlockID = FromHex(structureDNA[blockid]);
		
		float spreadAmmount = GaussianValueSpreadCheck(intensity, duration);
		//System.Random random = new System.Random();
        if ((blockValueForBlockID * (15 * 15 * 15) - (15 * 15 * 15) * 0.2f) < spreadAmmount * (15 * 15 * 15) && spreadAmmount * (15 * 15 * 15) > (blockValueForBlockID * (15 * 15 * 15) + (15 * 15 * 15) * 0.2f))
		{
			//structureDNA[blockid] = ToHex((int)(mutationAndDiseaseBlocks[random.Next(0,mutationAndDiseaseBlocks.Length)]));
		}
		else
		{
			//structureDNA[blockid] = ToHex((int)(spreadAmmount*(float)mutationAndDiseaseBlocks[random.Next(0,mutationAndDiseaseBlocks.Length)]));
		}
		
		//TODO: Randomize chance of hitting disease or mutation based on some function based on intensity and duration
	}

	public float GaussianValueSpreadCheck(int trueSpread, int allSpread)
	{
		//allSpread will be the area of the gaussian function based on the intensity for radiation

		//trueSpread will be the area of the gaussian function based on the duration.
		//This will determine the likelihood of landing true
		System.Random random = new System.Random();
		bool firstTrue = random.Next(0, allSpread) <= trueSpread ? true : false;
		if(firstTrue)
		{
			return random.Next(0, trueSpread);
		}
		else
		{
			return random.Next(0, allSpread);
		}
		
	}

	public bool CheckForMutations(out BaseDisease disease)
	{
        disease = new BaseDisease();
        return true;
	}

	public int[] FindDiseasesInBlock(int blockid)
	{
		int numberOfPossibleDiseasesInBlock = 0;
		for(int i = 0; i < mutationAndDiseaseBlocks.Length; i++)
		{
			if(mutationAndDiseaseBlocks[i] == blockid)
			{
				numberOfPossibleDiseasesInBlock++;
			}
		}
		int[] validDiseasesForBlock = new int[numberOfPossibleDiseasesInBlock];
		int indexer = 0;
		for(int i = 0; i < mutationAndDiseaseBlocks.Length; i++)
		{
			if(mutationAndDiseaseBlocks[i] == blockid)
			{
				validDiseasesForBlock[indexer] = i;
			}
		}
		return validDiseasesForBlock;
	}

	public void CreateRandomStructureDNA()
	{
		System.Random random = new System.Random();
		for(int i = 0; i < structureDNA.Length; i++)
		{
			structureDNA[i] = ToHex(random.Next(0,(15*15*15)));
		}
		BaseDisease disease = new BaseDisease();
		if(CheckForMutations(out disease))
		{

		}
	}

	public void CreateRaceDNA()
	{
        //int raceTypes = System.Enum.GetNames(typeof(Attributes.races)).Length;
	}

	public void CreateHairColorDNA()
	{
		Color32 hairColor = GetComponent<PlayerCharacter>().playerAttributes.ourPhysicals.hairColor;
		string redResultant = ToHex(hairColor.r);
		string greenResultant = ToHex(hairColor.g);
		string blueResultant = ToHex(hairColor.b);
		uniqueID[0] = redResultant + greenResultant + blueResultant;
	}

	public void CreateFaceHairColorDNA()
	{
		Color32 hairColor = GetComponent<PlayerCharacter>().playerAttributes.ourPhysicals.facialHairColor;
		string redResultant = ToHex(hairColor.r);
		string greenResultant = ToHex(hairColor.g);
		string blueResultant = ToHex(hairColor.b);
		uniqueID[1] = redResultant + greenResultant + blueResultant;
	}

	public void CreateSkinColorDNA()
	{
		Color32 skinColor = GetComponent<PlayerCharacter>().playerAttributes.ourPhysicals.skinTone;
		string redResultant = ToHex(skinColor.r);
		string greenResultant = ToHex(skinColor.g);
		string blueResultant = ToHex(skinColor.b);
		uniqueID[2] = redResultant + greenResultant + blueResultant;
	}

	public void CreateEyeColorDNA()
	{
		Color32 eyeColor = GetComponent<PlayerCharacter>().playerAttributes.ourPhysicals.eyeColor;
		string redResultant = ToHex(eyeColor.r);
		string greenResultant = ToHex(eyeColor.g);
		string blueResultant = ToHex(eyeColor.b);
		uniqueID[3] = redResultant + greenResultant + blueResultant;
	}

	public void CreateFacialHairDNA()
	{
		int differentHairTypes = System.Enum.GetNames(typeof(CharacterPhysicalAttributes.FacialHair)).Length;
		int currentHairType = GetComponent<PlayerCharacter>().playerAttributes.ourPhysicals.facialHair;
		try
		{
			uniqueID[4] = ToHex(currentHairType/differentHairTypes*(15*15*15));
		}
		catch
		{
			//Devide by zero exception
		}
	}

	public void CreateHairDNA()
	{
		int differentHairTypes = System.Enum.GetNames(typeof(CharacterPhysicalAttributes.HairType)).Length;
		int currentHairType = GetComponent<PlayerCharacter>().playerAttributes.ourPhysicals.hair;
		try
		{
			uniqueID[5] = ToHex(currentHairType/differentHairTypes*(15*15*15));
		}
		catch
		{
			//Devide by zero exception
		}
	}

	
	// Use this for initialization
	void Start ()
	{

	}

	public DNA()
	{
		System.Random random = new System.Random();
		for(int i = 0; i < mutationAndDiseaseBlocks.Length; i++)
		{
			mutationAndDiseaseBlocks[i] = random.Next(0, structureDNA.Length - 1);
		}
		CreateRandomStructureDNA();

		CreateRaceDNA();
		CreateHairColorDNA();
		CreateFaceHairColorDNA();
		CreateSkinColorDNA();
		CreateEyeColorDNA();
		CreateFacialHairDNA();
		CreateHairDNA();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

