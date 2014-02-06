using UnityEngine;
using System.Collections;

public class Forecast : MonoBehaviour {

	// Use this for initialization

	public int horizontal;
	public int vertical;

	public int order = 5;
	[Range(10, 180)] public float angle = 135f;

	public string filePrefix = "test";
	public string dir = "/Users/Hycis/Dropbox/CodingProjects/Thesis_Project/";

	private int width;
	private int height;

	private string forecastpath;

	private ArrayList forecast_d;
	private ArrayList forecast_trace;
	private bool forecastOn;

	private ArrayList filteredTrace_a;
	private ArrayList filteredTrace_b;
	private float stepsize;


	private float[,] dangerMap;

	private Texture2D whiteTexture;
	private Texture2D greyTexture;
	private Texture2D magentaTexture;
	private int len;

	void Awake()
	{
		forecast_trace = new ArrayList();
		forecastpath = dir + filePrefix + "_forecast.in";
		len = 3;
	}


	void Start () {

		greyTexture = GetComponent<Background>().greyTexture;
		magentaTexture = GetComponent<Background>().magentaTexture;
		width = GetComponent<Background>().width;
		height = GetComponent<Background>().height;
		dangerMap = GetComponent<DangerMap>().getDangerMap();
		stepsize = GetComponent<Player>().stepsize;

		filteredTrace_a = GetComponent<Player>().get_filteredTrace_a();
		filteredTrace_b = GetComponent<Player>().get_filteredTrace_b();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			horizontal+=3;
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			horizontal-=3;
		}

		if (Input.GetKey(KeyCode.UpArrow))
		{
			vertical-=3;
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			vertical+=3;
		}

	}


	Vector2 pt1 = new Vector2();
	Vector2 pt2 = new Vector2();
	int[] pivot = new int[2];
	void OnGUI () {
		if (GUI.Button(new Rect(110, height + 40, 300, 30), "2. Load and Plot Forecast Data (after MATLAB)"))
		{
			Start();
			loadForecastData();
			forecastOn = true;
		}

		if (forecastOn)
		{

			GUI.skin.box.normal.background = magentaTexture;


			pivot[0] = ((int)((float[])filteredTrace_a[filteredTrace_a.Count-1])[0]);
			pivot[1] = ((int)((float[])filteredTrace_a[filteredTrace_a.Count-1])[1]);


			foreach(float d in forecast_d)
			{
				int[] next_p = nextPosition(pivot[0], pivot[1], d);

				if (next_p[0] >= width-1 | next_p[1] >= height-1 | next_p[1] <= 1)
					break;

				
				GUI.Box(new Rect(next_p[0] + horizontal, next_p[1] + vertical, len, len), GUIContent.none);
				pt1.Set(pivot[0], pivot[1]);
				pt2.Set(next_p[0], next_p[1]);
				Drawing.DrawLine(pt1, pt2, Color.cyan);
				pivot = next_p;	
			}
		}


		if (GUI.Button(new Rect(415, height + 5, 100, 30), "3. Save Model"))
		{
			GetComponent<Serialize>().serialize();
			print("serialization complete");
		}

		if (GUI.Button(new Rect(415, height + 40, 100, 30), "Load Model"))
		{
			SerialObject obj = GetComponent<Serialize>().unserialize();

			GetComponent<Background>().reload(obj.start, obj.end, obj.width, obj.height);
			GetComponent<Enemies>().reload(obj.enemies);
			GetComponent<DangerMap>().reload(obj.dangerMap, obj.var);
			GetComponent<Player>().reload(obj.trace, obj.filteredTrace_a, obj.filteredTrace_b, obj.stepsize);
			reload(obj.forecast_d, obj.angle);


			print ("Finished Loading Model");
		}
	}

	public ArrayList get_forecast_d()
	{
		return forecast_d;
	}

	public ArrayList get_forecast_trace()
	{
		return forecast_trace;
	}

	void loadForecastData()
	{
		forecast_d = new ArrayList();
		System.IO.StreamReader file = new System.IO.StreamReader(forecastpath);
		
		string line = file.ReadLine();

		forecast_d.Clear();
		while (line != null)
		{
			forecast_d.Add(float.Parse(line));
			line = file.ReadLine();
		}
	}

	int[] nextPosition(int x, int y, float next_d)
	{
		stepsize = GetComponent<Player>().stepsize;
		float delta = 1f / stepsize;
		float startTheta = angle / 2f / 180f * Mathf.PI;
		float endTheta = - startTheta;
		
		float min_diff = 100;
		int[] next_pos = null;
		for (float theta = startTheta; theta > endTheta; theta -= delta)
		{
			int x_step = (int)Mathf.Round(stepsize * Mathf.Cos(theta));
			int y_step = (int)Mathf.Round(stepsize * Mathf.Sin(theta));
			
			int x_pos = x + x_step;
			int y_pos = y + y_step;
			
			if (x_pos >= width)
			{
				x_pos = width-1;
			}
			
			if (y_pos < 0)
			{
				y_pos = 0;
			}
			
			else if (y_pos >= height)
			{
				y_pos = height - 1;
			}

			float d = dangerMap[x_pos, y_pos];	
			float diff = Mathf.Abs(d - next_d);

			if (diff < min_diff)
			{
				min_diff = diff;
				next_pos = new int[2]{x_pos, y_pos};
			}
		}
		return next_pos;
	}

	public void reload(ArrayList forecast_d, float angle)
	{
		Start();
		forecastOn = true;
		this.forecast_d = forecast_d;
		this.angle = angle;
	}


}
