using UnityEngine;
using System.Collections;

public class CParticleManager : CManager
{
	private static CParticleManager mInst = null;

	public CParticleManager()
	{
		registerSingleton ();
	}

	public static CParticleManager inst()
	{
		return mInst;
	}

	private void registerSingleton()
	{
		if (mInst == null) 
		{
			mInst = this;
		}
		else 
		{
			throw new UnityException( "ERROR: Cannot create another instance of singleton class CParticleManager.");
		}
	}

	override public void update()
	{
		base.update ();
	}

	override public void render()
	{
		base.render ();
	}

	override public void destroy()
	{
		base.destroy ();
		mInst = null;
	}
}