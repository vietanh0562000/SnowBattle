using UnityEngine;
using System.Collections;


public class Variations : MonoBehaviour 
{
    #region Class members
    [SerializeField] private GameObject[] variations;
    #endregion
	
    #region Class accsessors

	private int variation;
	public int Variation 
    {
		get { return variation; }
		set {
			variation = value;
			Toggle(variation);
		}
	}

	public int Count 
    {
        get { return (int)variations.Length; }
	}

    #endregion

    #region Class implementation

    private void Toggle(int v)
    {
        // Toggle additional models
        for (int i = 0; i < variations.Length; i++)
        {
            if (variations[i] != null)
                variations[i].SetActive(i == v);
        }
    }

    #endregion
}
