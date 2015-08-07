//using UnityEngine;
using System;
using System.Collections;
using System.Reflection;


internal class Probe <T> {
	
	T instance;
	System.Type type = typeof(T);
	
	public Probe(T instance)
	{
		this.instance = instance;
	}

	public Object getField(string fieldName)
	{
		return type.GetField (fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue (instance);
	}
	
	public MethodInfo getMethod (string methodName)
	{
		return type.GetMethod (methodName, BindingFlags.NonPublic| BindingFlags.Public | BindingFlags.Instance);
	}
}
