using UnityEngine;

namespace FusionGame.Core.LayoutManagement
{
    internal abstract class FadeoutFadeinUITransitionController : MonoBehaviour, IUITransition
    {
        [SerializeField] private float uiTransitionStartAt = 0.2f;
        [SerializeField] private float uiTransitionFinishAt = 0.8f;

        private bool fadeoutFinished = false;
        private bool uiTransitionFinished = false;

        protected abstract void OnStartFadeout();
        protected abstract void OnFadeout(float progress);
        protected abstract void OnEndFadeout();
        protected abstract void OnStartUITransition();
        protected abstract void OnUITransition(float progress);
        protected abstract void OnEndUITransition();
        protected abstract void OnStartFadein();
        protected abstract void OnFadein(float progress);
        protected abstract void OnEndFadein();

        public void OnStartTransition()
        {
            OnStartFadeout();
        }

        public void OnTransition(float progress)
        {
            // if the progress is within the range [0, uiTransitionStartAt], do fadeout
            if (progress <= uiTransitionStartAt)
            {
                OnFadeout(progress / uiTransitionStartAt);
            }
            else if (progress <= uiTransitionFinishAt)
            {
                if (!fadeoutFinished)
                {
                    fadeoutFinished = true;
                    OnEndFadeout();
                    OnStartUITransition();
                }

                OnUITransition((progress - uiTransitionStartAt) / (uiTransitionFinishAt - uiTransitionStartAt));
            }
            else
            {
                if (!uiTransitionFinished)
                {
                    uiTransitionFinished = true;
                    OnEndUITransition();
                    OnStartFadein();
                }

                OnFadein((progress - uiTransitionFinishAt) / (1 - uiTransitionFinishAt));
            }
        }

        public void OnEndTransition()
        {
            OnEndFadein();
        }
    }
}
