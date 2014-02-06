using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	public int order = 4;

	public int x = 10;
	public int y = 10;
	public int len = 3;
	private int layer = 1;
	public bool traceOn;
	public bool markingOn;


	[Range(5,50)]public float stepsize = 10f;


	private Texture2D whiteTexture;
	private Texture2D blueTexture;
	private Texture2D yellowTexture;
	private Texture2D greenTexture;


	private int horizontal;
	private int vertical;
	private ArrayList trace;
	private ArrayList filteredTrace_a;
	private ArrayList filteredTrace_b;

	private float[,] dangerMap;
	private int width;
	private int height;
	private bool forecastOn;

	private int start;
	private float end;
	private int final;
	private string outputpath1;
	private string outputpath2;

	private string filePrefix;
	private string dir;


	// Use this for initialization
	void Awake() {
		trace = new ArrayList();
		filteredTrace_a = new ArrayList();
		filteredTrace_b = new ArrayList();

	
	}

	void Start () {

		horizontal = GetComponent<Forecast>().horizontal;
		vertical = GetComponent<Forecast>().vertical;
		width = GetComponent<Background>().width;
		start = GetComponent<Background>().start;
		end = GetComponent<Background>().end;
		final = width - start;
		height = GetComponent<Background>().height;
		dangerMap = GetComponent<DangerMap>().getDangerMap();

		filePrefix = GetComponent<Forecast>().filePrefix;
		dir = GetComponent<Forecast>().dir;

		outputpath1 = dir + filePrefix + "_filteredTrace_a.out";
		outputpath2 = dir + filePrefix + "_filteredTrace_b.out";

		whiteTexture = GetComponent<Background>().whiteTexture;
		blueTexture = GetComponent<Background>().blueTexture;
		yellowTexture = GetComponent<Background>().yellowTexture;
		greenTexture = GetComponent<Background>().greenTexture;


	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey(KeyCode.D))
		{
			x++;
			if (x > start & x < final)
			{
				trace.Add(new float[3]{x, y, dangerMap[x,y]});
			}
		}
		
		if (Input.GetKey(KeyCode.A))
		{
			x--;
			if (x > start & x < final)
			{
				trace.Add(new float[3]{x, y, dangerMap[x,y]});
			}
		}
		
		if (Input.GetKey(KeyCode.W))
		{
			y--;
			if (x > start & x < final)
			{
				trace.Add(new float[3]{x, y, dangerMap[x,y]});
			}

		}
		
		if (Input.GetKey(KeyCode.S))
		{
			y++;
			if (x > start & x < final)
			{
				trace.Add(new float[3]{x, y, dangerMap[x,y]});
			}
		}

	}




	void preprocessPath()
	{
		filteredTrace_a.Clear();
		filteredTrace_b.Clear();
		bool runout = false;

		float[] pointer = (float[])(trace[0]);

		float prevdist = 0;

		for (int i=1; i<trace.Count; i++)
		{
			float dist = Mathf.Sqrt(Mathf.Pow(pointer[0]-((float[])trace[i])[0],2) 
			                        + Mathf.Pow(pointer[1]-((float[])trace[i])[1], 2));

			if (dist > stepsize)
			{
				float delta1 = dist - stepsize;
				float delta2 = stepsize - prevdist;

				if (delta1 <= delta2)
				{
					if (((float[])trace[i])[0] < end)
					{
						filteredTrace_a.Add(trace[i]);
					}

					else
					{
						filteredTrace_b.Add(trace[i]);
					}
				
					pointer = (float[])(trace[i]);
				}

				else
				{
					if (((float[])trace[i-1])[0] < end)
					{
						filteredTrace_a.Add(trace[i-1]);
					}
					
					else
					{
						filteredTrace_b.Add(trace[i-1]);
					}

					pointer = (float[])(trace[i-1]);
				}
			}
			prevdist = dist;
		}
	}


	public ArrayList get_filteredTrace_a()
	{
		return filteredTrace_a;
	}

	public ArrayList get_filteredTrace_b()
	{
		return filteredTrace_b;
	}

	public ArrayList getTrace()
	{
		return trace;
	}


	void OnGUI () {
		
		horizontal = GetComponent<Forecast>().horizontal;
		vertical = GetComponent<Forecast>().vertical;
		GUI.skin.box.normal.background = blueTexture;
		GUI.skin.box.border.left = 0;
		GUI.skin.box.border.right = 0;
		GUI.skin.box.border.top = 0;
		GUI.skin.box.border.bottom = 0;

		GUI.depth = layer;

		GUI.Box(new Rect(x + horizontal, y + vertical, len, len), GUIContent.none); 


		if (GUI.Button(new Rect(110, height+5, 300, 30), "1. Generate and Save Keypoints"))
		{
			print("Start Generating and Saving Keypoints ...");

			preprocessPath();

			System.IO.StreamWriter file1 = new System.IO.StreamWriter(outputpath1);
			System.IO.StreamWriter file2 = new System.IO.StreamWriter(outputpath2);
			
			
			foreach(float[] ele in filteredTrace_a)
			{
				file1.WriteLine(ele[0]+":"+ele[1]+":"+ele[2]);
			}

			foreach(float[] ele in filteredTrace_b)
			{
				file2.WriteLine(ele[0]+":"+ele[1]+":"+ele[2]);
			}

			file1.Close();
			file2.Close();
			markingOn = true;
			
			print("Saving Done!");
		}



		if (traceOn)
		{
			foreach(float[] ele in trace)
			{
				GUI.Box(new Rect(ele[0] + horizontal, ele[1] + vertical, len, len), GUIContent.none); 
			}
		}
	

		if (markingOn)
		{

			preprocessPath();

			foreach(float[] ele in filteredTrace_a)
			{
				GUI.skin.box.normal.background = whiteTexture;

				GUI.Box(new Rect(ele[0] + horizontal, ele[1] + vertical, len, len), GUIContent.none); 
			}

			foreach(float[] ele in filteredTrace_b)
			{
				GUI.skin.box.normal.background = greenTexture;

				GUI.Box(new Rect(ele[0] + horizontal, ele[1] + vertical, len, len), GUIContent.none); 
			}

		}
		
	}

	public void reload(ArrayList trace, ArrayList filteredTrace_a, ArrayList filteredTrace_b, float stepsize)
	{
		Start();
		this.trace = trace;
		this.filteredTrace_a = filteredTrace_a;
		this.filteredTrace_b = filteredTrace_b;
		this.stepsize = stepsize;
		traceOn = true;
	}

}
