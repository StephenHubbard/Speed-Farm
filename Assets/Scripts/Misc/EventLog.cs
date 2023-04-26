using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventLog : MonoBehaviour
{
    [SerializeField] private float _textFadeTime = 3f;
    
    private TMP_Text _text;

    private void Awake() {

        _text = GetComponent<TMP_Text>();
    }

    public void UpdateEventText(string text) {
        _text.text = text;
        StartCoroutine(SlowFadeRoutine());
    }

    private IEnumerator SlowFadeRoutine()
    {
        yield return new WaitForSeconds(2f);

        float elapsedTime = 0;
        float startValue = 1;

        while (elapsedTime < _textFadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime / _textFadeTime);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, newAlpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
