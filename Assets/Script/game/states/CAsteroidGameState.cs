using UnityEngine;
using System.Collections;

public class CLevelState : CGameState
{
	private CBackground mBackground;
	private CPlayer mPlayer;

	private CEnemyManager mEnemyManager;
	private CBulletManager mBulletManager;
	private CParticleManager mParticleManger;

	public CLevelState()
	{
	}

	override public void init()
	{
		base.init ();

		mBackground = new CBackground();
		mBackground.setXY (0, 0);

		mPlayer = new CPlayer ();
		mPlayer.setXY (CGameConstants.SCREEN_WIDTH / 2, CGameConstants.SCREEN_HEIGHT / 2);
		mPlayer.setRotation (45);

		CGame.inst ().setPlayer (mPlayer);

		mEnemyManager = new CEnemyManager ();
		mBulletManager = new CBulletManager ();
		mParticleManger = new CParticleManager ();

		createAsteroids ();

		CEnemyShip e = new CEnemyShip ();
		e.setXY (200, 200);
		CEnemyManager.inst ().add (e);
	}

	override public void update()
	{
		base.update (); 

		mBackground.update ();
		mPlayer.update ();

		mEnemyManager.update ();
		mBulletManager.update ();
		mParticleManger.update ();

		if (CEnemyManager.inst ().getLength () == 0) 
		{
			Debug.Log ("WIN");
		}
	}

	override public void render()
	{
		base.render ();

		mBackground.render ();
		mPlayer.render ();
		mEnemyManager.render ();
		mBulletManager.render ();
		mParticleManger.render ();
	}

	override public void destroy()
	{
		base.destroy ();

		mBackground.destroy ();
		mBackground = null;

		mPlayer.destroy ();
		mPlayer = null;

		mEnemyManager.destroy ();
		mEnemyManager = null;

		mBulletManager.destroy ();
		mBulletManager = null;

		mParticleManger.destroy ();
		mParticleManger = null;
	}

	private void createAsteroids()
	{
		CAsteroid asteroid;

		for (int i = 0; i < 10; i++) 
		{
			asteroid = new CAsteroid (CAsteroid.TYPE_BIG, CMath.randomIntBetween(1, 3));
			asteroid.setXY (CMath.randomIntBetween(0, CGameConstants.SCREEN_WIDTH), CMath.randomIntBetween(0, CGameConstants.SCREEN_HEIGHT));
			asteroid.setVelX (CMath.randomFloatBetween(-500, 500));
			asteroid.setVelY (CMath.randomFloatBetween(-500, 500));
			CEnemyManager.inst ().add (asteroid);
		}
	}
}
