using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class text_cs : MonoBehaviour
{
    public IEnumerator TextActive(TextMeshProUGUI text, string Data)
    {
        if (Data == null) { yield break; }
        text.text = Data;
        // 文字の表示数を0に(テキストが表示されなくなる)
        text.maxVisibleCharacters = 0;
        // テキストの文字数分ループ
        for (var i = 0; i < text.text.Length; i++)
        {   // 一文字ごとに0.2秒待機
            yield return new WaitForSeconds(0.1f);
            // 文字の表示数を増やしていく
            text.maxVisibleCharacters = i + 1;
        }
        yield return new WaitForSeconds(0.5f);
    }
}