using UnityEngine;
using System.Collections;

public class Stars : MonoBehaviour {

	public float speed = 200f;

	// Update is called once per frame
	void Update () {
	    MeshRenderer mr = GetComponent<MeshRenderer> ();

		Material material1 = mr.materials[0]; 
		Material material2 = mr.materials[1]; 

		Vector2 offset1 = material1.GetTextureOffset("_MainTex");
		Vector2 offset2 = material2.GetTextureOffset("_MainTex");

		material1.SetTextureOffset ("_MainTex", new Vector2 (
			offset1.x += Time.deltaTime/speed, 0
		));
		material2.SetTextureOffset ("_MainTex", new Vector2 (
			offset2.x -= Time.deltaTime/speed, 0
			));
	}
}
