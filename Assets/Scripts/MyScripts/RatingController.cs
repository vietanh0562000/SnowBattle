using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaperPlaneTools;

public class RatingController : MonoBehaviour {

    public static RatingController instance;
    public int ratingFreq;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RatingBtnClicked()
    {
        RateBox.Instance.ForceShow();
    }

    public void ShowRatingDialog()
    {
        Debug.Log("Showing Rating Dialog..");
        RatingCounting.counter++;
        if (RatingCounting.counter >= ratingFreq)
        {
            RateBox.Instance.Show();
            RatingCounting.counter = 0;
        }
    }
}

public static class RatingCounting{
    public static int counter;
}
