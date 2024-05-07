using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetReadyTextBehaviour : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(HideText());
    }

    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
