using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : PanelBase
{

    [SerializeField] private List<string> listOfPhrases = new List<string>();
    [SerializeField] private Text textSpot;

    private static int lastChoosedIndex;
 
    #region Class members
    [SerializeField]
    private GameObject confettiRoot;
    [SerializeField]
    private Camera confettiCamera;

    [SerializeField]
    private ParticleSystem[] particleSystems;
    #endregion

    #region MonoBehaviour events

    override protected void Awake()
    {
        base.Awake();
        SetParticleSystemsEnabled(false);
    }

    private void OnEnable()
    {
        int index = Random.Range(0, listOfPhrases.Count);
        while (index == lastChoosedIndex)
        {
            index = Random.Range(0, listOfPhrases.Count);
        }
        textSpot.text = listOfPhrases[index];
    }

    #endregion

    #region Super class overrides
    public override void UpdateInfo()
    {
        SetParticleSystemsEnabled(true);
        StartCoroutine(EnableParticleSystemsWithDelay(false, 2.5f));
        Hide(3);
        UIManager.Instance.gameOverPanel.Show(3.5f);
        UIManager.Instance.titlePanel.Show(3.5f);
    }
    #endregion

    #region Class implementation
    public void SetParticleSystemsEnabled(bool state)
    {
        if (state)
        {
            foreach (ParticleSystem ps in particleSystems)
                ps.Play();
        }
        else
        {
            foreach (ParticleSystem ps in particleSystems)
                ps.Stop();
        }
    }

    private IEnumerator EnableParticleSystemsWithDelay(bool state, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SetParticleSystemsEnabled(state);
    }
    #endregion
}
