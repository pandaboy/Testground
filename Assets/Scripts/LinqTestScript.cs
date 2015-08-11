using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LinqTestScript : MonoBehaviour {
	
	List<int> integers = new List<int>();
	List<int> moreIntegers = new List<int>();

    [Limit(0.0f, 24.0f)]
    public float someFloat = 0.0f;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < 5; i++) {
			integers.Add(i + 2);
			moreIntegers.Add(i + 1);
		}
		
		IEnumerable<int> both = integers.Intersect(moreIntegers);
		
		Debug.Log("Looking for similarities " + both.Count());
		Debug.Log(both.ToString());
		foreach (int item in both)
		{
			Debug.Log(item);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
