using UnityEngine;
using TMPro;

namespace Void2610.TypingGame
{
    public class TypingGameView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI typedText;
        [SerializeField] private TextMeshProUGUI remainingText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Color correctColor = new Color(0.2f, 0.8f, 0.2f);

        public void SetTypedText(string text)
        {
            typedText.text = text;
            typedText.color = correctColor;
        }

        public void SetRemainingText(string text) => remainingText.text = text;

        public void SetStatus(string text) => statusText.text = text;
    }
}
