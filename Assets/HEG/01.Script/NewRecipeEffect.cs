using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class NewRecipeEffect : MonoBehaviour
{
    public GameObject effect;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;

    public void StartScaleEeffect()
    {
        StartCoroutine(PlayNewRecipeEffect());
    }
    
    public void StartWaveEffect()
    {
        subtitleText.text = "새로운 레시피 획득!";
        StartCoroutine(AnimateWave(subtitleText));
    }

    IEnumerator PlayNewRecipeEffect()
    {
        AudioManager.Instance.PlaySFX("Fanfarate");
        // 1. 타이틀 팝 효과
        effect.SetActive(true);
        titleText.text = "새로운 레시피 획득!";
        titleText.transform.localScale = Vector3.zero;
        titleText.transform.DOScale(1.2f, 0.8f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);

        yield return new WaitForSeconds(2f);

        effect.SetActive(false);

        // 2. 서브 텍스트 wave 효과
        //subtitleText.text = recipeName;

    }

    IEnumerator AnimateWave(TextMeshProUGUI tmpText, float duration = 3f)
    {
        AudioManager.Instance.PlaySFX("Fanfarate");

        float waveSpeed = 5f;
        float waveHeight = 12f;
        float elapsed = 0f;

        effect.SetActive(true);

        TMP_TextInfo textInfo = tmpText.textInfo;

        while (elapsed < duration)
        {
            tmpText.ForceMeshUpdate();  
            textInfo = tmpText.textInfo;
            elapsed += Time.deltaTime;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible) continue;

                var charInfo = textInfo.characterInfo[i];
                int matIndex = charInfo.materialReferenceIndex;
                int vertIndex = charInfo.vertexIndex;

                Vector3[] verts = textInfo.meshInfo[matIndex].vertices;

                float wave = Mathf.Sin(Time.time * waveSpeed + i * 0.3f) * waveHeight;

                for (int j = 0; j < 4; j++)
                {
                    verts[vertIndex + j] = textInfo.meshInfo[matIndex].vertices[vertIndex + j];
                    verts[vertIndex + j].y += wave;
                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
        tmpText.enabled = false;
        effect.SetActive(false);
    }
}
