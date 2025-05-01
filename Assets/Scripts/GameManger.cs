using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Toggle[] Allcheak;
    public string Sencename;

    private Coroutine nextSceneCoroutine;

    private void Update()
    {
        if (AllTogglesOn() && nextSceneCoroutine == null)
        {
            // Start the coroutine to load the next scene
            nextSceneCoroutine = StartCoroutine(goNextSc());
        }
    }

    public void Cheak()
    {
        for (int i = 0; i < Allcheak.Length; i++)
        {
            if (Allcheak[i].isOn && nextSceneCoroutine == null)
            {
                // Start the coroutine to load the next scene
                nextSceneCoroutine = StartCoroutine(goNextSc());
            }
        }
    }

    bool AllTogglesOn()
    {
        // Check if all toggles are on
        foreach (Toggle toggle in Allcheak)
        {
            if (!toggle.isOn)
            {
                // If any toggle is not on, return false
                return false;
            }
        }

        // If all toggles are on, return true
        return true;
    }

    IEnumerator goNextSc()
    {
        // Wait for 10 seconds
        yield return new WaitForSeconds(10f);

        // Load the next scene
        SceneManager.LoadScene(Sencename);
    }
    public void ResetScene()
    {
        SceneManager.LoadScene("Vrmainsence");
    }
}