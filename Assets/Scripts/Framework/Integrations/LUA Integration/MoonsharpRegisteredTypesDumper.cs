#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Serialization;
using MoonSharp.Interpreter.Interop;
using Framework.AIModels;
using Framework.Pooling;
using Framework.Utils;
using Framework.Lua;
using Framework.Gameplay;

public class MoonsharpRegisteredTypesDumper : MonoBehaviour 
{

	#region MonoBehaviour overrides
	private void Update ()
	{
		if (Input.GetKeyDown (KeyCode.D)) 
		{
			Debug.Log ("Dimping Moonsharp registered types");
			//FixUnityTypes ();
			//FixCustomTypes ();

			FilterMonoRunEditMode ();


			Table dump = UserData.GetDescriptionOfRegisteredTypes (true);
			File.WriteAllText ("Assets/Hardwiring/testdump.lua", dump.Serialize ());
		}
	}

	private void FilterMonoRunEditMode ()
	{
		// Unregister run in edit mode 
		List<System.Type> registeredTypes = new List<System.Type>(UserData.GetRegisteredTypes (true));

		foreach (System.Type t in registeredTypes) 
		{
			var descr = UserData.RegisterType (t) as StandardUserDataDescriptor; //new StandardUserDataDescriptor (t, InteropAccessMode.Default);

			if (descr != null) 
			{
				if (descr.HasMember ("runInEditMode")) descr.RemoveMember ("runInEditMode");
				if (descr.HasMember ("get_runInEditMode")) descr.RemoveMember ("get_runInEditMode");
				if (descr.HasMember ("set_runInEditMode")) descr.RemoveMember ("set_runInEditMode");

				if (descr.HasMember ("ParentMaskStateChanged")) descr.RemoveMember("ParentMaskStateChanged");
				if (descr.HasMember ("OnRebuildRequested")) descr.RemoveMember("OnRebuildRequested");

				if (descr.HasMember ("op_Implicit")) descr.RemoveMember("op_Implicit");
				if (descr.HasMember ("PlayAnimation")) descr.RemoveMember("PlayAnimation");
				if (descr.HasMember ("StopAnimation")) descr.RemoveMember("StopAnimation");
				if (descr.HasMember ("SampleAnimation")) descr.RemoveMember("SampleAnimation");

				if (descr.HasMember ("IsJoystickPreconfigured")) descr.RemoveMember("IsJoystickPreconfigured");

			}
		}
	}
	#endregion
}
#endif