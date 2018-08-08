using System;
using UnityEngine;

public class CAudioListener : CAudioComponent
{
	private AudioListener mAudioListener;
	public AudioListener component
	{
		get { return mAudioListener; }
	}

	public CAudioListener(CGameObject aGameObject) : base(aGameObject)
	{
		gameObject.name = "Audio Listener " + aGameObject.getName();
		mAudioListener = gameObject.AddComponent<AudioListener>();
	}

	override public void update()
	{
		base.update();

	}

	override public void destroy()
	{
		base.destroy();
	}

}
