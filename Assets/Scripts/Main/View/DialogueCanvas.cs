using System.Collections;
using System.Text.RegularExpressions;
using Main.Mono.Lore;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Main.View
{
    public class DialogueCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private Color _highlightedPassageColour;
        [SerializeField] private GameObject _waitForInputIndicator;

        private int _currentLineIndex;
        private string[] _currentLines;
        private bool _informCutscenePlayerOnFinish;

        private Coroutine _revealCoroutine;

        private Vector2 _upAndDown;
        private bool _waitingForProceed;

        public static DialogueCanvas Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "Dialogue Canvas singleton already exists!");
            Instance = this;
        }


        public void TryProceed()
        {
            if (!_waitingForProceed) return;
            _waitingForProceed = false;
            ShowNextLine();
        }

        public void StartDialogue(string[] dialogueLines, bool informCutscenePlayer = false)
        {
            _container.gameObject.SetActive(true);
            _currentLineIndex = -1;
            _currentLines = dialogueLines;
            _waitingForProceed = false;
            _informCutscenePlayerOnFinish = informCutscenePlayer;

            ShowNextLine();
        }

        private void ShowNextLine()
        {
            if (HasNextLine())
            {
                _currentLineIndex++;

                var text = _currentLines[_currentLineIndex];
                var amountOfHighlightedPassages = 0;

                const string highlightedPassagePattern = @"\[(.*?)\]";

                var replacedText = Regex.Replace(text, highlightedPassagePattern, match =>
                {
                    var matchValue = match.Groups[1].Value;
                    amountOfHighlightedPassages++;
                    return $"<color=#{ColorUtility.ToHtmlStringRGB(_highlightedPassageColour)}>{matchValue}</color>";
                });

                IndicateAway();

                _dialogueText.text = replacedText;
                _revealCoroutine = StartCoroutine(RevealCharacters(
                    text.Length - amountOfHighlightedPassages * 2));
            }
            else
            {
                HandleCompletedDialogue();
            }
        }

        private void IndicateIn()
        {
            LeanTween.cancel(_waitForInputIndicator);
            _waitForInputIndicator.transform.localScale = Vector2.zero;
            _upAndDown.x = 0.9f;
            _upAndDown.y = 0.9f;
            LeanTween
                .scale(_waitForInputIndicator, Vector2.one, 0.4f)
                .setEaseInOutBounce()
                .setIgnoreTimeScale(true)
                .setOnComplete(() =>
                {
                    LeanTween
                        .scale(_waitForInputIndicator, _upAndDown, 0.9f)
                        .setEaseInOutQuad()
                        .setLoopPingPong()
                        .setIgnoreTimeScale(true);
                });
        }

        private IEnumerator RevealCharacters(int textPartLength)
        {
            var characterCounter = 0;

            while (true)
            {
                var visibleCounter = characterCounter % (textPartLength + 1);
                _dialogueText.maxVisibleCharacters = visibleCounter;

                if (visibleCounter >= textPartLength)
                {
                    _waitingForProceed = true;
                    IndicateIn();

                    yield break;
                }

                characterCounter++;

                yield return new WaitForSeconds(0.03f);
            }
        }

        private void IndicateAway()
        {
            LeanTween.cancel(_waitForInputIndicator);
            _waitForInputIndicator.transform.localScale = Vector2.one;
            LeanTween
                .scale(_waitForInputIndicator, Vector2.zero, 0.4f)
                .setEaseOutCirc()
                .setIgnoreTimeScale(true);
        }

        public void HandleCompletedDialogue()
        {
            StopCoroutine(_revealCoroutine);

            _currentLineIndex = -1;
            _currentLines = null;
            _waitingForProceed = false;

            if (_informCutscenePlayerOnFinish) CutsceneManager.Instance.ProceedWithNextSegment();

            _informCutscenePlayerOnFinish = false;
            _container.gameObject.SetActive(false);
        }

        private bool HasNextLine() => _currentLineIndex < _currentLines.Length - 1;
    }
}