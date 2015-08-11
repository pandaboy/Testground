using UnityEngine;
using System.Collections;

public class LimitAttribute : PropertyAttribute {
    public float max;
    public float min;

	// Use this for initialization
	public LimitAttribute (float min, float max) {
        this.min = min;
        this.max = max;
	}
}
