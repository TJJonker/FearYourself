using System.Collections;
using UnityEngine;
using TMPro;

public class TypeWriter : MonoBehaviour
{
    public void WriteLine(string text, float letterInterval = .1f) => StartCoroutine(TypeWrite(text, letterInterval));

    private IEnumerator TypeWrite(string text, float letterInterval)
    {
        var index = 0;
        var line = "";
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();
        while(index < text.Length)
        {
            line += text[index];
            tmp.text = line;
            index += 1;
            yield return new WaitForSeconds(letterInterval);
        }
    }


}
