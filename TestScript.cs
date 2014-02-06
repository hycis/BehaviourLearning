using UnityEngine;
using System.Collections;


using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

//It is common to create a class to contain all of your
//extension methods. This class must be static.
//public static class ExtensionMethods
//{
//	//Even though they are used like normal methods, extension
//	//methods must be declared static. Notice that the first
//	//parameter has the 'this' keyword followed by a Transform
//	//variable. This variable denotes which class the extension
//	//method becomes a part of.
//	public static void ResetTransformation(this Transform trans)
//	{
//		trans.position = Vector3.zero;
//		trans.localRotation = Quaternion.identity;
//		trans.localScale = new Vector3(1, 1, 1);
//	}
//}



public class TestScript : MonoBehaviour 
{
	void Start () {
		//Notice how you pass no parameter into this
		//extension method even though you had one in the
		//method declaration. The transform object that
		//this method is called from automatically gets
		//passed in as the first parameter
		print (2.828793e+00);

		
		//Creates a new TestSimpleObject object.
		TestSimpleObject obj = new TestSimpleObject();
		
		print("Before serialization the object contains: ");
		obj.Print();
		
		//Opens a file and serializes the object into it in binary format.
		Stream stream = File.Open("data.xml", FileMode.Create);
		BinaryFormatter formatter = new BinaryFormatter();
		
		//BinaryFormatter formatter = new BinaryFormatter();
		
		formatter.Serialize(stream, obj);
		stream.Close();
		
		//Empties obj.
		obj = null;
		
		//Opens file "data.xml" and deserializes the object from it.
		stream = File.Open("data.xml", FileMode.Open);
		formatter = new BinaryFormatter();
		
		//formatter = new BinaryFormatter();
		
		obj = (TestSimpleObject)formatter.Deserialize(stream);
		stream.Close();
		
		 print("After deserialization the object contains: ");
		obj.Print();
//		Console.WriteLine("you suck");
	}
}

// A test object that needs to be serialized.
[Serializable()]		
public class TestSimpleObject  {
	
	public int member1;
	public string member2;
	public string member3;
	public double member4;
	
	// A field that is not serialized.
	[NonSerialized()] public string member5; 
	
	public TestSimpleObject() {
		
		member1 = 11;
		member2 = "hello";
		member3 = "hello";
		member4 = 3.14159265;
		member5 = "hello world!";
	}
	
	
	public void Print() {
		
		 Debug.Log("member1 = '{0}'" + member1);
		 Debug.Log("member2 = '{0}'"+ member2);
		 Debug.Log("member3 = '{0}'"+ member3);
		 Debug.Log("member4 = '{0}'"+ member4);
		 Debug.Log("member5 = '{0}'"+ member5);
	}
}