using UnityEngine;
using System.Collections;

public class DangerMap : MonoBehaviour {
	[Range(0,1)] public float opacity = 0.5f;
	private int layer = 2;
	[Range(0,1)] public float contrast = 0.5f;
	public int order = 3;
	[Range(0,1)] public float var = 0.5f;

	private int width;
	private int height;
	private int horizontal;
	private int vertical;
	ArrayList enemies;
	private Texture2D texture;
	private int n_enemies;
	private float[,] dangerMap;

	void Awake()
	{
		width = GetComponent<Background>().width;
		height = GetComponent<Background>().height;
		dangerMap = new float[width, height];

	}

	void Start(){

		width = GetComponent<Background>().width;
		height = GetComponent<Background>().height;
		horizontal = GetComponent<Forecast>().horizontal;
		vertical = GetComponent<Forecast>().vertical;

		n_enemies = GetComponent<Enemies>().n_enemies;
		enemies = GetComponent<Enemies>().getEnemy();

		texture = new Texture2D(width, height);
		createTexture();
	}
	

	void createTexture()
	{
		float[] rgb = new float[3];
		for (int x=0; x<width; x++)
		{
			for (int y=0; y<height; y++)
			{

				float sum = 0;
				for (int n=0; n<enemies.Count; n++)
				{
					float d;
					dangerValue(((int[])enemies[n])[0], ((int[])enemies[n])[1], x, y, out d);
					sum += d;
				}

				dangerMap[x, y] = sum;


			}
		}

		for (int x=0; x<width; x++)
		{
			for (int y=0; y<height; y++)
			{
				d2rgb(dangerMap[x,y], rgb);
				texture.SetPixel(x, height-y-1, new Color(rgb[0],rgb[1],rgb[2],opacity));
			}
		}



		texture.Apply();
	}

	public float[,] getDangerMap()
	{
		return dangerMap;
	}


	private float max;
	void dangerValue(int enemy_x, int enemy_y, int x, int y, out float d)
	{
		float d_sqr = Mathf.Pow(enemy_x - x, 2) + Mathf.Pow(enemy_y - y, 2);
		d = Mathf.Exp(-d_sqr/(10000f * var));
		if (d > max)
		{
			max = d;
		}
	}

	void d2rgb(float d, float[] rgb)
	{
		float r, g, b;

		if (d > max)
		{
			r = 1.0f;
			b = 0f;
			g = 0f;
		}
		else if (d > max/2)
		{
			r = 1.0f;
			g = 1 - (d - max/2) / (max/2);
			b = 0f;
		}

		else
		{
			r = d / (max/2) ; 
			g = 1f;
			b = 0f;
		}
		rgb[0] = r;
		rgb[1] = g;
		rgb[2] = b;
	}

	void OnGUI () {	

		horizontal = GetComponent<Forecast>().horizontal;
		vertical = GetComponent<Forecast>().vertical;

		GUI.skin.box.normal.background = texture;
		GUI.depth = layer;
		GUI.Box(new Rect(horizontal, vertical, width, height), GUIContent.none);

		if (GUI.Button(new Rect(5, height + 5, 100, 65), "0. Generate \n Danger Map"))
		{
			createTexture();
		}
	}

	public void reload(float[,] dangerMap, float var)
	{
		this.dangerMap = dangerMap;
		this.var = var;
		Start();
	}
}
