using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	

	public int width;
	public int height;
	private int layer = 3;

	public int order = 1;

	private int horizontal;
	private int vertical;

	public Texture2D blueTexture;
	public Texture2D redTexture;
	public Texture2D blackTexture;
	public Texture2D greenTexture;
	public Texture2D yellowTexture;
	public Texture2D whiteTexture;
	public Texture2D greyTexture;
	public Texture2D magentaTexture;


	[Range(10, 50)] public int start = 20;
	[Range(0.5f, 0.9f)] public float end = 0.8f;
	private int final;

	// Use this for initialization
	void Awake() {
		createTexture(new Color(1.0f, 0, 1.0f), out magentaTexture);
		createTexture(Color.blue, out blueTexture);
		createTexture(Color.red, out redTexture);
		createTexture(Color.black, out blackTexture);
		createTexture(Color.green, out greenTexture);
		createTexture(Color.yellow, out yellowTexture);
		createTexture(Color.white, out whiteTexture);
		createTexture(Color.grey, out greyTexture);
	}

	void Start () {

		end = (int) (end * width);
		final = width - start;

		horizontal = GetComponent<Forecast>().horizontal;
		vertical = GetComponent<Forecast>().vertical;
	}

	// Update is called once per frame
	void Update () {
		
	}



	void createTexture(Color color, out Texture2D texture2)
	{
		texture2 = new Texture2D(1,1);
		texture2.SetPixel(0,0, color);
		texture2.Apply();
	}




	void OnGUI () {
		horizontal = GetComponent<Forecast>().horizontal;
		vertical = GetComponent<Forecast>().vertical;

		GUI.skin.box.normal.background = whiteTexture;
		GUI.skin.box.border.left = 0;
		GUI.skin.box.border.right = 0;
		GUI.skin.box.border.top = 0;
		GUI.skin.box.border.bottom = 0;

		GUI.depth = layer;
		GUI.Box(new Rect(horizontal, vertical, width, height), GUIContent.none);



		for (int y=0; y<height; y++)
		{
			GUI.skin.box.normal.background = redTexture;

			GUI.Box(new Rect(start + horizontal, vertical+y, 3, 3), GUIContent.none);
			GUI.Box(new Rect(end + horizontal, vertical+y, 3, 3), GUIContent.none);
			GUI.Box(new Rect(final + horizontal, vertical+y, 3, 3), GUIContent.none);
		}
	
	}

	public void reload(int start, float end, int width, int height)
	{
		this.start = start;
		this.width = width;
		this.height = height;
		Start();
		this.end = end;


	}
	

}
