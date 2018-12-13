using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CManager
{
	private List<CGameObject> mArray;

	public CManager()
	{
		mArray = new List<CGameObject>();
	}

	public void add(CGameObject aGameObject)
	{
		mArray.Add (aGameObject);
	}

	virtual public void update()
	{
		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			mArray[i].update();
		}

		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			if (mArray[i].isDead())
			{
				removeObjectWithIndex(i);
			}
		}
	}

	virtual public void render()
	{
		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			mArray[i].render();
		}
	}

	private void removeObjectWithIndex(int aIndex)
	{
		if (aIndex < mArray.Count) 
		{
			mArray[aIndex].destroy();
			mArray[aIndex] = null;
			mArray.RemoveAt(aIndex);
		}
	}

	virtual public void destroy()
	{
		clean ();
		mArray = null;
	}

	public void clean()
	{
		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			removeObjectWithIndex(i);
		}
	}

	public CGameObject collides(CGameObject aGameObject)
	{
		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			if (aGameObject.collides(mArray[i]))
			{
				return mArray[i];
			}
		}

		return null;
	}

	public List<CGameObject> collidesList(CGameObject aGameObject)
	{
		List<CGameObject> results = new List<CGameObject>();

		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			if (aGameObject.collides(mArray[i]))
			{
				results.Add(mArray[i]);
			}
		}

		return results;
	}

	public CGameObject collidesRect(CGameObject aGameObject)
	{
		for (int i = mArray.Count - 1; i >= 0; i --) 
		{
			if (aGameObject.collidesRect(mArray[i]))
			{
				return mArray[i];
			}
		}

		return null;
	}

	public int getLength()
	{
		return mArray.Count;
	}


}