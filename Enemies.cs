using UnityEngine;
using System.Collections;

public class Enemies : MonoBehaviour {

	public int n_enemies;
	public int seed = 0;
	public int len = 2;
	public bool randomOn = false;
	public int order = 2;

	private int layer = 1;

	private Texture2D blackTexture;
	private int width;
	private int height;
	private ArrayList enemies;
	private int horizontal;
	private int vertical;
	private int start;
	private int final;

	void Awake()
	{
		enemies = new ArrayList();
	}

	// Use this for initialization
	void Start () {
		blackTexture = GetComponent<Background>().blackTexture;
		width = GetComponent<Background>().width;
		height = GetComponent<Background>().height;
		start = GetComponent<Background>().start;
		final = width - start;

		if (randomOn)
		{
			createRandEnemies();
		}
	}

	void Update() {

		if (!randomOn)
		{
			if (Input.GetMouseButtonUp(0))
			{
				int cur_x = ((int)Input.mousePosition.x);
				int cur_y = ((int)Input.mousePosition.y);

				if (cur_y > Screen.height-vertical-height & cur_y < Screen.height-vertical 
				    & cur_x > horizontal & cur_x < horizontal+width)
				{
					enemies.Add(new int[2] {cur_x-horizontal, Screen.height-cur_y-vertical});
				}
			}
		}
	}


	void createRandEnemies()
	{
		Random.seed = seed;
		for (int enemy_num=0; enemy_num<n_enemies; enemy_num++)
		{
			int x = Random.Range(start, final);
			int y = Random.Range(0, height);
			enemies.Add(new int[2]{x,y});
		}
	}

	
	public ArrayList getEnemy()
	{
		return enemies;
	}

	
	void OnGUI () {

		horizontal = GetComponent<Forecast>().horizontal;
		vertical = GetComponent<Forecast>().vertical;
		GUI.skin.box.normal.background = blackTexture;
		GUI.skin.box.border.left = 0;
		GUI.skin.box.border.right = 0;
		GUI.skin.box.border.top = 0;
		GUI.skin.box.border.bottom = 0;


		GUI.depth = layer;

		for (int n=0; n<enemies.Count; n++)
		{
			GUI.Box(new Rect(((int[])enemies[n])[0] + horizontal, 
			                  ((int[])enemies[n])[1] + vertical, len, len), GUIContent.none);
		}
	

	}

	public void reload(ArrayList enemies)
	{
		this.enemies = enemies;
		Start();
	}
}
