using UnityEngine;
using System.Collections;

public static class PositionUtil
{
	public static Pos ToPos(this Vector3 pos)
	{
		return new Pos((int)(pos.x - 0.5f), (int)(pos.y - 0.5f)); // 实际位置到Pos偏移0.5f
	}

	public static Vector3 ToVector3(this Pos pos)
	{
		return new Vector3(pos._x + 0.5f, pos._y + 0.5f); // 实际位置到Pos偏移0.5f
	}
}
