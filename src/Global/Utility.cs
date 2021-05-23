namespace Global
{
  using Godot;
  using System.Collections.Generic;
  using System;
  using System.Reflection;

  public class Utility
  {
	  public static float Abs(float number)
	  {
		if(number < 0)
		{
	  	return number * -1f;
		}
		return number;
	  }
  
	  public static float Clamp(float number, float min, float max)
	  {
		if(number < min)
		{
	  	return min;
		}
		else if(number > max)
		{
	  	return max;
		}
  
		return number;
	  }
	

	  public static object Raycast(Vector3 start, Vector3 end, World world)
	  {
  		if(world == null)
  	  {
  	  	  return null;
  		}
  		PhysicsDirectSpaceState spaceState = world.DirectSpaceState as PhysicsDirectSpaceState;
  		var result = spaceState.IntersectRay(start, end);
  		
  		if(!result.Contains("collider"))
  	  {
  	  	  return null;
  		}
  	
  		object collider = result["collider"];
  	
  		return collider;
	  }

		// Use this to find methods for classes when the docs won't
		public static void ShowMethods(Type type){
		  foreach (var method in type.GetMethods()){
			string ret = "" + method.ReturnType +"," + method.Name;
			foreach( var parameter in method.GetParameters()){
			  ret += ", " + parameter.ParameterType + " " + parameter.Name; 
			}
			GD.Print(ret);
			System.Threading.Thread.Sleep(100);
		  }
		}
		  // Use this to find variables for classes
	  public static void ShowProperties(Type type){
		foreach(PropertyInfo prop in type.GetProperties()){
		  GD.Print(prop.Name + " : " + prop.PropertyType );
		}
	  }
  }
}
