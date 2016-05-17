//We have to create 8 tags beforehand namely piece0, piece1, piece2 and so on upto piece7 as there are 8 pieces for each texture 
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour 
{
	static public int noOfSteps = 0;//For calculating no of steps taken to solve the pieces,used in other script also 

	public Texture2D tex;  // Texture one to cut up, assigned from outside
	public Texture2D tex1;  // Texture two to cut up, assigned from outside
	public Material mat;   // Material to use for texture one, assigned from outside
	public Material mat1;   // Material to use for texture two, assigned from outside
	public Text steps;     // Used for displaying the text in the textbox assigned from outside

	private int cols = 1;   // Number of columns
	private int rows = 8;  // Number of rows
	private GameObject[] piece = new GameObject[8]; //pieces of texture one in an array: piece[]
	private GameObject[] piece1 = new GameObject[8]; //pieces of texture two in an array: piece1[]
	private string getTag; // To get the tag of a gameobject
	private string getTag1; //To get the tag of a gameobject
	private GameObject[] sameTag = new GameObject[2]; //Used for storing the gameobjects with same tag,used during initial randomization
	private GameObject[] sameTagOriginal = new GameObject[2]; //Used for storing the gameobjects with same tag, used for swapping pieces on mouse click
	private GameObject[] sameTagTarget = new GameObject[2];  //Used for storing the gameobjects with same tag, used for swapping pieces on mouse click
	//private Vector3[] startingPositions = new Vector3[8]; 
	private float[] y = new float[8]; //for storing starting y axis positions of each pieces of any texture (we have used position of texture one)
	private int clickCount =1; // used when we have to swap the pieces after two mouse clicks ie, source and destination
	private GameObject originalTile; //used for storing gameobject on click one
	private GameObject targetTile; //used for storing gameobject on click two
	private float clickOneY;//y axis value of gameobject on click one
	private float clickTwoY;//y axis value of gameobject on click two
	private int yClickOneTwo = 0; //no of gameobjects whose y axis value is b/w click one and click two
	private float[] newSwapY = new float[7];//maximum of 7 pieces can be between two clicks, array to store those value
	private Shader shader;//for putting transparent shader on click one on gameobject
	// boolean variables used
	private bool gameOver = false;
	private bool flag = false;
	//other variable used
	int howManyChanged=0; 
	int allMatched = 0;
	
	void Start()
	{
		mat.mainTexture = tex;  //texure for a material
		mat1.mainTexture = tex1;//texure for a material

		BuildPieces(-2.5f,mat,piece); //cutting the images into pieces
		BuildPieces(2.5f,mat1,piece1);//cutting the images into pieces

		noOfSteps = 0; 

		SetStartingPositions (); //calling the function which sets starting positions of pieces 

	    Label: Shuffle (); // calling the function which randomizes the positions of pieces initially

		//checking that the new positions of pieces are not the same as was initially
		for (int i=0; i<8; i++) 
		{

				if((piece1[i].transform.position.y != y[i] && piece[i].transform.position.y != y[i]) && piece1[i].transform.position.y == piece[i].transform.position.y) 
				{
					howManyChanged = howManyChanged + 1;
				}

		}

		//if no randomization of pieces after check then re-shuffle
		if (howManyChanged != 8) 
		{
			howManyChanged = 0;
			goto Label;
		}
	}



	// Update is called once per frame
	void Update () 
	{
		//get the gameobject on mouseclick
		if (Input.GetMouseButtonDown(0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);

			if (hit) 
			{
				switch(clickCount)
				{ 					
					case 1: 
					originalTile = hit.collider.gameObject;			
					getTag = originalTile.tag;
					sameTagOriginal = GameObject.FindGameObjectsWithTag(getTag);

					//for shading
					shader = Shader.Find( "Transparent/Diffuse" );
					sameTagOriginal[0].GetComponent<Renderer> ().material.shader = shader;
					sameTagOriginal[1].GetComponent<Renderer> ().material.shader = shader;

					clickCount = 2;
					clickOneY = originalTile.transform.position.y;
					break;

					case 2: 
					targetTile = hit.collider.gameObject;
					getTag1 = targetTile.tag;

					sameTagTarget = GameObject.FindGameObjectsWithTag(getTag1);		
					shader = Shader.Find( "Unlit/Texture" );
					sameTagOriginal[0].GetComponent<Renderer> ().material.shader = shader;
					sameTagOriginal[1].GetComponent<Renderer> ().material.shader = shader;

					clickCount = 1;

					noOfSteps = noOfSteps + 1;
					clickTwoY = targetTile.transform.position.y;

					//for less than equal
					if (clickOneY <= clickTwoY){
					for (int i=0; i<8; i++) 
					{
						if(y[i] > clickOneY && y[i] <= clickTwoY)
						{
							newSwapY[yClickOneTwo] = y[i];
							print (newSwapY[yClickOneTwo]);
							yClickOneTwo ++;
							print (yClickOneTwo);
					
						}
					}
						FunctionUsedInClickTwo(); //fuction used from below
					}

					//now for greater

					if (clickOneY > clickTwoY){
						for (int i=7; i>=0; i--) 
						{
							if(y[i] < clickOneY && y[i] >= clickTwoY)
							{
								newSwapY[yClickOneTwo] = y[i];
								print (newSwapY[yClickOneTwo]);
								yClickOneTwo ++;
								print (yClickOneTwo);
								
							}
						}
						FunctionUsedInClickTwo();
					}
					yClickOneTwo = 0;
				
					break;
				}				

			}

		}   
		steps.text = noOfSteps.ToString();
		AllMatched ();

		if(gameOver == true)
		{
			Application.LoadLevel("GameOver");
		}

	}

	//function to store starting positions of each pieces in an array 
	void SetStartingPositions()
	{
		for (int i = 0; i < 8; i++)
		{
			y[i] = piece[i].transform.position.y;

		}
	}
	
	// function for the randomization of positions of pieces
	private void Shuffle()
	{
		for (int j = 0; j<8; j++)
		{
			//if (piece[j] == null) continue;	
			
			int randomJ = Random.Range(0,8);
			//swap them
			sameTag = GameObject.FindGameObjectsWithTag("piece" + j);

			Swap(sameTag[1],piece1[randomJ]);

			Swap(sameTag[0],piece[randomJ]);

		}
	}
	
	//function to swap two gameobject positions
	void  Swap(GameObject org, GameObject tar)
	{
		Vector3 temp =  new Vector3(org.transform.position.x, org.transform.position.y, org.transform.position.z);
		Vector3 temp1 =  new Vector3(tar.transform.position.x, tar.transform.position.y, tar.transform.position.z);
		Vector3 temp2 =  new Vector3(org.transform.position.x, org.transform.position.y, org.transform.position.z);

		temp.y = tar.transform.position.y;
		org.transform.position= temp;

		temp1.y = temp2.y;

		tar.transform.position = temp1;
	}


	private void AllMatched()
	{
		//checking that the new positions of pieces are not the same as was initially
		for (int i=0; i<8; i++) 
		{			
			if((y[i] == piece[i].transform.position.y && y[i] == piece1[i].transform.position.y) && piece[i].transform.position.y == piece1[i].transform.position.y) 
			{
				allMatched = allMatched + 1;
			}			
		}

		if (allMatched == 8) 
		{
			gameOver = true;
		}

		allMatched = 0;
	}

	//function used for splitting the images
	void BuildPieces(float offsetX, Material materials, GameObject[] partsOfImages) 
	{		
		Vector3 offset = Vector3.zero;
		offset.x = offsetX;
		offset.z = 2f;
		offset.y = -2.35f;
		float startX = offset.x;
		float uvWidth = 1.0f / cols;
		float uvHeight = 1.0f / rows;	
		
		for (int i = 0; i < rows; i++) 
		{
			for (int j = 0; j < cols; j++) 
			{
				partsOfImages[i] = GameObject.CreatePrimitive (PrimitiveType.Quad);
				partsOfImages[i].gameObject.tag = "piece" +i;
				Transform t = partsOfImages[i].transform;
				t.position = offset;
				t.localScale = new Vector3(4.7f, 0.8f, 1f);
				partsOfImages[i].GetComponent<Renderer>().material = materials;


				Mesh mesh = partsOfImages[i].GetComponent<MeshFilter>().mesh;
				Vector2[] uvs = mesh.uv;
				uvs[0] = new Vector2(j * uvWidth, i * uvHeight);
				uvs[3] = new Vector2(j * uvWidth, (i + 1) * uvHeight);
				uvs[1] = new Vector2((j + 1) * uvWidth, (i + 1) * uvHeight);
				uvs[2] = new Vector2((j + 1) * uvWidth, i * uvHeight);
				mesh.uv = uvs;
				offset.x += 1.0f;
			}
			offset.y += 0.8f;
			offset.x = startX;
		}
		for (int i = 0; i < rows; i++) 
		{
			DestroyImmediate (partsOfImages[i].GetComponent("MeshCollider"));
			partsOfImages[i].AddComponent<BoxCollider2D>();

		}

	}

	private void FunctionUsedInClickTwo()
	{
		//this part sets y axis value of gameobject on click one to y axis value of gameobject on click two
		Vector3 oneMoreTemp =  new Vector3(sameTagOriginal[0].transform.position.x, sameTagOriginal[0].transform.position.y, sameTagOriginal[0].transform.position.z);
		oneMoreTemp.y = clickTwoY;
		Vector3 oneMoreTemp3 =  new Vector3(sameTagOriginal[1].transform.position.x, sameTagOriginal[1].transform.position.y, sameTagOriginal[1].transform.position.z);
		oneMoreTemp3.y = clickTwoY;
		sameTagOriginal[0].transform.position = oneMoreTemp;
		sameTagOriginal[1].transform.position = oneMoreTemp3;
		
		//this part sets y axis value of the gameobject next to the gameobject on click one but in b/w the clicks
		//then sets y axis value of the gameobject next to the gameobject obtained above and so on
		//flag is used so that for one newSwapY no two or more setting of y axis values
		//actually this part is bit clumpsy to make you understand without visual aid
		for (int i=0; i<yClickOneTwo; i++)
		{
			for (int j=0; j<8; j++)
			{
				//sameTagOriginal[0] != piece[j] means after first setting of y axis above same gameobject should not be set ie, the gameobject on click one
				if((piece[j].transform.position.y == newSwapY[i] && sameTagOriginal[0] != piece[j])&&(piece1[j].transform.position.y == newSwapY[i] && sameTagOriginal[1] != piece1[j]))
				{
					Vector3 oneMoreTemp1 =  new Vector3(piece[j].transform.position.x, piece[j].transform.position.y, piece[j].transform.position.z);
					Vector3 oneMoreTemp2 =  new Vector3(piece1[j].transform.position.x, piece1[j].transform.position.y, piece1[j].transform.position.z);
					oneMoreTemp1.y = clickOneY;
					oneMoreTemp2.y = clickOneY;
					clickOneY = piece[j].transform.position.y;
					piece[j].transform.position = oneMoreTemp1;
					piece1[j].transform.position = oneMoreTemp2;
					flag = true;
					
				}
				
				if(flag == true) {
					break;
				}
				
			}
			flag = false;
		}
	}

}