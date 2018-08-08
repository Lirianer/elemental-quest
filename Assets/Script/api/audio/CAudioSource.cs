using System;
using UnityEngine;

public class CAudioSource : CAudioComponent
{
	private AudioSource mAudioSource;

	public AudioSource component
	{
		get{return mAudioSource;}
	}

	public CAudioSource(CGameObject aGameObject) : base(aGameObject)
	{
		gameObject.name = "Audio Source: " + aGameObject.getName();

		mAudioSource = gameObject.AddComponent<AudioSource>();
	}

	public void setAs2DSource()
	{
		mAudioSource.dopplerLevel = 0.0f;
		//mAudioSource.minDistance = CCamera.Inst.zoom;
		//mAudioSource.maxDistance = CCamera.Inst.maxZoom;
		mAudioSource.rolloffMode = AudioRolloffMode.Linear;
		mAudioSource.spatialBlend = 0.0f;
		mAudioSource.loop = false;
	}

	public void setAs3DSource()
	{
		mAudioSource.dopplerLevel = 0.01f;
		//mAudioSource.minDistance = CCamera.Inst.zoom;
		//mAudioSource.maxDistance = CCamera.Inst.maxZoom;
		mAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
		mAudioSource.spatialBlend = 1.0f;
		mAudioSource.loop = false;
	}

	override public void update()
	{
		base.update();
		//mAudioSource.minDistance = Mathf.InverseLerp(CCamera.Inst.minZoom, CCamera.Inst.maxZoom, CCamera.Inst.zoom) * 200.0f;
	}

	override public void destroy()
	{
		base.destroy();
	}

}

