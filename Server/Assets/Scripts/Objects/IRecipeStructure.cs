using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IRecipeStructure
{
    List<Item> ProducesMats();
    List<Item> RequiredMats();
}
