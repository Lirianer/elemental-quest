using System.Collections.Generic;
using System.IO;
using UnityEngine;

using AudioClipMap = System.Collections.Generic.Dictionary<string, UnityEngine.AudioClip>;
using AudioSourceMap = System.Collections.Generic.Dictionary<CGameObject, CAudioSource>;

public class CAudioManager
{
	private float mTestTimer = 0.0f;
	private CAudioSource mListenerSourceFX;
	private CAudioSource mListenerSourceMusic;

	private AudioClipMap mAudioClips;
	private CAudioListener mAudioListener;
	private AudioSourceMap mAudioSources;

    private static CAudioManager _instance;
	public static CAudioManager Inst
    {
        get
        {
            if (_instance == null)
                _instance = new CAudioManager();
            return _instance;
        }
    }
	static CAudioManager() { }

	private CAudioManager() 
	{
		loadAudioClips();
		mAudioSources = new AudioSourceMap();
	}

    /// <summary>
	/// Sets the Cameras Audio Listener and Audio Sources for Music and SFX.
	/// </summary>
	/// <param name="aObject"></param>
	public void setAudioListener(CGameObject aObject)
    {
        if (mAudioListener == null)
            mAudioListener = new CAudioListener(aObject);

        mListenerSourceFX = new CAudioSource(aObject);
        mListenerSourceFX.setAs2DSource();

        mListenerSourceMusic = new CAudioSource(aObject);
        mListenerSourceMusic.setAs2DSource();
        mListenerSourceMusic.component.loop = true;
    }

    private CAudioSource getAudioSource(CGameObject aKey)
	{
        //@Fernando: Added null check in case the audiosource wasn't registered. -Mauri
        if (!mAudioSources.ContainsKey(aKey))
            return null;
		return mAudioSources[aKey];
	}

	private void addAudioSource(CGameObject aKey, CAudioSource aProduct)
	{
		mAudioSources.Add(aKey, aProduct);
	}

	private bool containsAudioSource(CGameObject aKey)
	{
		return mAudioSources.ContainsKey(aKey);
	}

	private AudioClip getAudioClip(string aKey)
	{
		return mAudioClips[aKey];
	}

	private void addAudioClip(string aKey, AudioClip aProduct)
	{
		mAudioClips.Add(aKey, aProduct);
	}

	private bool containsAudioClip(string aKey)
	{
		return mAudioClips.ContainsKey(aKey);
	}



	/// <summary>
	/// Loads all Audio files in "Audio" directory.
	/// </summary>
	public void loadAudioClips()
	{
        //Debug.Log("LOADING AUDIOS");
		mAudioClips = new AudioClipMap();

		// Levanta todos los archivos de sonidos. AudioClip es la representacion de Unity de un archivo de sonido.
		AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

		foreach (AudioClip clip in clips)
			addAudioClip(clip.name, clip);
	}

	/// <summary>
	/// Adds an Audio Source to the list managed by AudioManager.
	/// </summary>
	/// <param name="aObject"></param>
	public void addAudioSource(CGameObject aObject)
	{
		CAudioSource source = new CAudioSource(aObject);
		source.setAs3DSource();
		addAudioSource(aObject, source);
	}

	

	/// <summary>
	/// Updates all managed Audio Sources.
	/// </summary>
	public void update()
	{
		foreach (KeyValuePair<CGameObject, CAudioSource> source in mAudioSources)
		{
			source.Value.update();
		}
        // TODO: FIX PORQUE DA ERROR SI NO TIENE PUESTO EL OBJETO.
        // //CAudioManager.Inst.setAudioListener(mPlayer); en LEVEL STATE.
        //mAudioListener.update();
    }

    public void destroy()
	{
	}

	public void playMusic(string aKey)
	{
		AudioClip music = getAudioClip(aKey);
		mListenerSourceMusic.component.clip = music;
		mListenerSourceMusic.component.Play();
	}

	// TODO: PARA DISPARAR SONIDO:
	//CAudioManager.Inst.playFX("coin");

	/// <summary>
	/// Used for 2D SFX, plays at the camera AudioSource
	/// </summary>
	/// <param name="aKey"></param>
	public void playFX(string aKey)
	{
		AudioClip fx = getAudioClip(aKey);
		mListenerSourceFX.component.PlayOneShot(fx);
	}

	/// <summary>
	/// Used for 3D SFX, play at the objects AudioSource
	/// </summary>
	/// <param name="aObject"></param>
	/// <param name="aSoundName"></param>
	public void playFXAt(CGameObject aObject, string aSoundName)
	{
		AudioClip clip = getAudioClip(aSoundName);
		AudioSource source = getAudioSource(aObject).component;
		source.PlayOneShot(clip);
	}
	/// <summary>
	/// Used for 3D sounds. Plays a random sound at a desired Objects location.
	/// Example, PlayRandomRange(object, "foot_pasto_", 0, 11);
	/// Assumes files to be named: sound_name_number
	/// </summary>
	/// <param name="aObject">Object to play at</param>
	/// <param name="aSoundName">Prefix of sound name</param>
	/// <param name="aStart">First file starting number</param>
	/// <param name="aEnd">Last file starting number</param>
	public void playRandomRangeAt(CGameObject aObject, string aSoundName, int aStart, int aEnd)
	{
		mTestTimer += Time.deltaTime;
        //@Fernando: Added null check in case the audiosource wasn't registered. -Mauri
        CAudioSource csource = getAudioSource(aObject);
        if (csource == null)
            return;
		AudioSource source = csource.component;
		AudioClip clip = getAudioClip(aSoundName + CMath.randomIntBetween(aStart, aEnd));
		if (mTestTimer + CMath.randomFloatBetween(-0.1f, 0.1f) > 0.3f)
		{
            // Apply random volume.
            float vol = CMath.randomFloatBetween(0.7f, 1.0f);
            // Apply random pitch.
            source.pitch = CMath.randomFloatBetween(0.75f, 1.25f);

			source.PlayOneShot(clip, vol);
			mTestTimer = 0.0f;
		}
	}
}


