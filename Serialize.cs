using UnityEngine;
using System.Collections;

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Serialize : MonoBehaviour {

	public int order = 6;

	private int height;
	private int width;
	private int start;
	private float end;

	private float[,] dangerMap;
	private float var;

	private ArrayList enemies;

	private ArrayList trace;
	private ArrayList filteredTrace_a;
	private ArrayList filteredTrace_b;
	private float stepsize;


	private ArrayList forecast_d;
	private float angle;

	private string filePrefix;
	private string dir;

	// Use this for initialization
	void Start () {
		filePrefix = GetComponent<Forecast>().filePrefix;
		dir = GetComponent<Forecast>().dir;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void serialize()
	{

		
		height = GetComponent<Background>().height;
		width = GetComponent<Background>().width;
		start = GetComponent<Background>().start;
		end = GetComponent<Background>().end;
		
		
		dangerMap = GetComponent<DangerMap>().getDangerMap();
		var = GetComponent<DangerMap>().var;
		
		enemies = GetComponent<Enemies>().getEnemy();
		
		trace = GetComponent<Player>().getTrace();
		filteredTrace_a = GetComponent<Player>().get_filteredTrace_a();
		filteredTrace_b = GetComponent<Player>().get_filteredTrace_b();
		stepsize = GetComponent<Player>().stepsize;
		
		forecast_d = GetComponent<Forecast>().get_forecast_d();
		angle = GetComponent<Forecast>().angle;

		SerialObject obj = new SerialObject(width, height, start, end,
		                                    dangerMap, var, enemies, trace,
		                                    filteredTrace_a, filteredTrace_b, stepsize,
		                                    forecast_d, angle);


		string file = dir + filePrefix + "_serial.xml";
		Stream stream = File.Open(file, FileMode.Create);

		BinaryFormatter formatter = new BinaryFormatter();
		
		formatter.Serialize(stream, obj);
		stream.Close();

	}

	public SerialObject unserialize()
	{
		SerialObject obj;

		string file = dir + filePrefix + "_serial.xml";


		Stream stream = File.Open(file, FileMode.Open);

		BinaryFormatter formatter = new BinaryFormatter();
		
		obj = (SerialObject)formatter.Deserialize(stream);
		stream.Close();
		print("unserialization complete");

		return obj;
	}


}

// A test object that needs to be serialized.
[Serializable()]		
public class SerialObject  {
	
	//Background
	public int width;
	public int height;
	public int start;
	public float end;



	//DangerMap
	public float[,] dangerMap;
	public float var;


	//Enemies
	public ArrayList enemies;
	
	//Player
	public ArrayList trace;
	public ArrayList filteredTrace_a;
	public ArrayList filteredTrace_b;
	public float stepsize;



	//Forecast
	public ArrayList forecast_d;
	public float angle;

	public SerialObject(int width, int height, int start, float end,
	                    float[,] dangerMap, float var, ArrayList enemies,
	                    ArrayList trace, ArrayList filteredTrace_a,
	                    ArrayList filteredTrace_b, float stepsize,
	                    ArrayList forecast_d, float angle) 
	{
		this.width = width;
		this.height = height;
		this.start = start;
		this.end = end;

		this.dangerMap = dangerMap;
		this.var = var;

		this.enemies = enemies;

		this.trace = trace;
		this.filteredTrace_a = filteredTrace_a;
		this.filteredTrace_b = filteredTrace_b;
		this.stepsize = stepsize;

		this.forecast_d = forecast_d;
		this.angle = angle;
	}
}
	
	


