﻿using UnityEngine;
using System.Collections;

public static class InputExtensions
{
	public static int LimitToRange(
		this int value, int inclusiveMinimum, int inclusiveMaximum)
	{
		if (value < inclusiveMinimum) { return inclusiveMinimum; }
		if (value > inclusiveMaximum) { return inclusiveMaximum; }
		return value;
	}

	public static float LimitToRange(
		this float value, float inclusiveMinimum, float inclusiveMaximum)
	{
		if (value < inclusiveMinimum) { return inclusiveMinimum; }
		if (value > inclusiveMaximum) { return inclusiveMaximum; }
		return value;
	}
}
