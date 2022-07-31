using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeWriterUI : MonoBehaviour
{	TMP_Text _tmpProText;
	string writer;
    private AudioSource audioSource;
    public AudioClip[] audioClips;
    private Coroutine audioCoroutine = null;
    public bool isOver = false;

	[SerializeField] float delayBeforeStart = 0f;
	public float timeBtwChars = 0.1f;
	[SerializeField] string leadingChar = "";
	[SerializeField] bool leadingCharBeforeDelay = false;
    // Start is called before the first frame update
    void Start()
    {
        _tmpProText = GetComponent<TMP_Text>();
        audioSource = GetComponent<AudioSource>();
        if (_tmpProText != null)
		{
			writer = _tmpProText.text;
			_tmpProText.text = "";

			StartCoroutine("TypeWriterTMP");
            audioCoroutine = StartCoroutine(PlaySound());
		}
    }

	IEnumerator TypeWriterTMP()
    {
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
			if (_tmpProText.text.Length > 0)
			{
				_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
			}
			_tmpProText.text += c;
			_tmpProText.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (leadingChar != "")
		{
			_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
		}
        StopCoroutine(audioCoroutine);
        audioSource.Stop();

        isOver = true;

	}

    IEnumerator PlaySound()
    {
        while(true)
        {
            for (int index = 0 ; index < audioClips.Length ; index++)
            {
                audioSource.PlayOneShot(audioClips[index]);
                yield return new WaitForSeconds(audioClips[index].length);
            }
        }

    }
}
