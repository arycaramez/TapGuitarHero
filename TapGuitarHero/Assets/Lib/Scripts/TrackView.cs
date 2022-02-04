using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Beats
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    [RequireComponent(typeof(ContentSizeFitter))]
    [RequireComponent(typeof(RectTransform))]
    public class TrackView : MonoBehaviour
    {
        public enum Trigger { Missed, Right, Wrong}
        //[SerializeField] private Track _track;

        [SerializeField] private RectTransform _left;
        [SerializeField] private RectTransform _right;
        [SerializeField] private RectTransform _up;
        [SerializeField] private RectTransform _down;

        [SerializeField] private RectTransform _empty;

        RectTransform _rTransform;
        List<Image> _beatViews = new List<Image>();

        Vector2 _position;
        float _beatViewSize;
        float _spacing;
        
        public float position {
            get { return _position.y; }
            set {
                if (value != _position.y) {
                    _position.y = value;
                    _rTransform.anchoredPosition = _position;
                }
            }
        }

        public void Init(Track track) {
            if (track == null) return;
            _rTransform = (RectTransform) transform;
            _position = _rTransform.anchoredPosition;

            _beatViewSize = _empty.rect.height;
            _spacing = GetComponent<VerticalLayoutGroup>().spacing;

            _beatViews = new List<Image>();

            foreach (int b in track.beats) {
                GameObject g;
                switch (b)
                {
                    case 0: g = _left.gameObject; break;
                    case 1: g = _down.gameObject; break;
                    case 2: g = _up.gameObject; break;
                    case 3: g = _right.gameObject; break;
                    default: g = _empty.gameObject; break;
                }
                Image view = GameObject.Instantiate(g, transform).GetComponent<Image>();
                view.transform.SetAsFirstSibling();

                _beatViews.Add(view);
            }
        }

        public void Start()
        {
            Init(GamePlayController.Instance.track);
        }

        private void Update()
        {
            position -= (_beatViewSize+_spacing) * Time.deltaTime * GamePlayController.Instance.secondsPerBeat;
        }

        public void TriggerBeatView(int index,Trigger trigger) {
            switch (trigger)
            {
                case Trigger.Missed:
                    _beatViews[index].color = Color.gray;
                    //Debug.Break();
                    break;
                case Trigger.Right:
                    _beatViews[index].color = Color.yellow;
                    break;
                case Trigger.Wrong:
                    _beatViews[index].color = Color.cyan;
                    break;
            }
        }
    }
}