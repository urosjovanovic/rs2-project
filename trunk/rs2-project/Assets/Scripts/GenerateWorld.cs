using UnityEngine;
using System.Collections;
using System.IO;

public class GenerateWorld : MonoBehaviour {
	// Use this for initialization
	void Start () {
		int size = 32;
		FileInfo level = new FileInfo("maze32.txt");
		StreamReader sr = level.OpenText();
		float start = Time.realtimeSinceStartup;
		string line = "";
		
		GameObject Walls = new GameObject();
		Walls.gameObject.name = "Walls";
		GameObject Floor = new GameObject();
		Floor.gameObject.name = "Floor";
		
		Vector3 center = new Vector3(size/2,0,size/2);
		
		Walls.transform.position = center;
		Floor.transform.position = center;
		
		int i = 0;
		while((line=sr.ReadLine()) != null)
		{
			for(int j=0; j<line.Length; j++)
			{
				GameObject kocka = GameObject.CreatePrimitive(PrimitiveType.Cube);
				kocka.isStatic = true;
				
				char c = line[j];
				
				Vector3 pos;
				if(c=='0')
				{
					pos = new Vector3(j,-1,size-i);
					kocka.gameObject.name = pos.ToString();
					kocka.transform.position = pos;
					kocka.gameObject.tag = "Floor";
					kocka.gameObject.AddComponent<PathColor>();
					kocka.transform.parent = Floor.transform;
				}
				else
				{
					pos = new Vector3(j,0,size-i);
					kocka.gameObject.name = pos.ToString();
					kocka.transform.position = pos;
					kocka.gameObject.tag = "Wall";
					kocka.transform.parent = Walls.transform;
				}
			}
			
			i++;
		}
		float end = Time.realtimeSinceStartup;
		Debug.Log ("World generated in: " + (end - start));
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
