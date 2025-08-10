#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utils;
#if NCITE_SPINE
using Framework.Spine;
#endif


namespace Framework.Lua
{
	/// <summary>
	/// OtimizedLuaGameObject.
	/// Defines accessors for cached versions of the most commonly used components in the Lua integration framework.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public class OptimizedLuaGameObject : OptimizedGameObject 
	{
		
		/// <summary>
		/// Gets the cached LuaMonoBehaviourBinder.
		/// </summary>
		/// <value>The cached LuaMonoBehaviourBinder.</value>
		protected LuaMonoBehaviourBinder _cachedLuaMonoBinder;
		virtual public LuaMonoBehaviourBinder cachedLuaMonoBinder
		{
			get 
			{
				// Store this component's reference for the first time
				if (_cachedLuaMonoBinder == null)
					_cachedLuaMonoBinder = GetComponentInChildren<LuaMonoBehaviourBinder>();

				return _cachedLuaMonoBinder;
			}
		}

		/// <summary>
		/// Gets the cached LuaFSMStateBinder.
		/// </summary>
		/// <value>The cached LuaFSMStateBinder.</value>
		protected LuaFSMStateBinder _cachedLuaFSMStateBinder;
		virtual public LuaFSMStateBinder cachedLuaFSMStateBinder
		{
			get 
			{
				// Store this component's reference for the first time
				if (_cachedLuaFSMStateBinder == null)
					_cachedLuaFSMStateBinder = GetComponentInChildren<LuaFSMStateBinder>();

				return _cachedLuaFSMStateBinder;
			}
		}

		/// <summary>
		/// Duplicates a game object.
		/// </summary>
		/// <returns>The game object.</returns>
		/// <param name="go">Go.</param>
		/// <param name="parent">Parent.</param>
		public GameObject DuplicateGameObject (GameObject go, Transform parent)
		{
			GameObject result = Instantiate (go, parent);
			return result;
		}

		/// <summary>
		/// Plaies the audio source.
		/// </summary>
		public void PlayAudioSource ()
		{
			cachedAudioSource.Play ();
		}

		/// <summary>
		/// Stops the audio source.
		/// </summary>
		public void StopAudioSource ()
		{
			cachedAudioSource.Stop ();
		}

		/// <summary>
		/// Plays an audio clip.
		/// </summary>
		/// <param name="audioClip">Audio clip.</param>
		public void PlayAudioClip (AudioClip audioClip)
		{
			cachedAudioSource.clip = audioClip;
			cachedAudioSource.Play ();
		}

		/// <summary>
		/// Plaies the one shot.
		/// </summary>
		/// <param name="audioClip">Audio clip.</param>
		public void PlayOneShot (AudioClip audioClip)
		{
			cachedAudioSource.PlayOneShot (audioClip);
		}

		#if NCITE_SPINE
		/// <summary>
		/// Gets the cached spine animation player.
		/// </summary>
		/// <value>The cached spine animation player.</value>
		protected SpineAnimationPlayer _cachedSpineAnimationPlayer;
		virtual public SpineAnimationPlayer cachedSpineAnimationPlayer
		{
			get 
			{
				// Store this component's reference for the first time
				if (_cachedSpineAnimationPlayer == null)
					_cachedSpineAnimationPlayer = GetComponentInChildren<SpineAnimationPlayer>();

				return _cachedSpineAnimationPlayer;
			}
		}
		#endif
	}
}
#endif
