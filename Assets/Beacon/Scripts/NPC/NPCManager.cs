using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class NPCManager : MonoBehaviour
{
	List<NPC> npcs = new List<NPC>(); 

	public void AddNPC(NPC npc) // 不支持扩展公用NPC RoleID
	{
		var n = npcs.Find((NPC obj) => obj._roleIdent == npc._roleIdent); 
		if (n != null)
		{
			RemoveNPC(n); 
		}
		npcs.Add(npc); 
	}

	public void RemoveNPC(NPC npc) // 此处可能有BUG，销毁的时候没有移除
	{
		npcs.Remove(npc); 
	}

	public void RemoveNPC(ERole roleIdent)
	{
		Debug.LogError("Remove NPC0: " + roleIdent); 
		var n = npcs.Find((NPC obj) => obj._roleIdent == roleIdent); 
		if (n != null)
		{
			Debug.LogError("Remove NPC1: " + roleIdent); 
			RemoveNPC(n);
		}
	}

	public T GetNPC<T>(ERole roleIdent) where T : class
	{
		return npcs.Find((NPC obj) => obj._roleIdent == roleIdent) as T; 
	}

	public bool Contains(ERole roleIdent)
	{
		return npcs.Find((NPC obj) => obj._roleIdent == roleIdent) != null; 
	}
}
