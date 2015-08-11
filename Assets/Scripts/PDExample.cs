using UnityEngine;
using System.Collections;

public enum IngredientUnit { Spoon, Cup, Bowl, Piece };

[System.Serializable]
public class Ingredient : System.Object
{
    public string name;
    public int amount = 1;
    public IngredientUnit unit;
}

public class PDExample : MonoBehaviour
{
    public Ingredient potionResult;
    public Ingredient[] potionIngredients;
	
	// Update is called once per frame
	void Update () {
	
	}
}
