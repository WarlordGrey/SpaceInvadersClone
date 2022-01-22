using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace UI.Screens
{

    public class LoadingScreen : BaseScreen
    {

        [SerializeField]
        private Text loadingText = null;
        [SerializeField]
        private float textChangeTimeout = 0.5f;
        [SerializeField]
        private int maxDotsCnt = GlobalConstants.kMaxLoadingDotsCount;

        private bool _isAnimationWorking = false;

        private bool hasScreenManagerDuringOnShow = false;
        private int curDotsCnt = 0;
        private float curTimeout = 0;

        private void Update()
        {
            if (_isAnimationWorking)
            {
                if (curTimeout > 0)
                {
                    curTimeout -= Time.deltaTime;
                }
                else
                {
                    string result = null;
                    if (curDotsCnt >= maxDotsCnt)
                    {
                        curDotsCnt = 0;
                        result = GlobalConstants.kBaseLoadingTextString;
                    }
                    else
                    {
                        curDotsCnt++;
                        result = loadingText.text + '.';
                    }
                    loadingText.text = result;
                    curTimeout = textChangeTimeout;
                }
            }
        }

        public override void OnShow()
        {
            base.OnShow();
            _isAnimationWorking = true;
            ResetAnimation();
        }

        private void ResetAnimation()
        {
            loadingText.text = GlobalConstants.kBaseLoadingTextString;
            curDotsCnt = 0;
            curTimeout = textChangeTimeout;
        }

        public override void OnHide()
        {
            base.OnHide();
            _isAnimationWorking = false;
        }

        public void ForceWork()
        {
            _isAnimationWorking = true;
        }
    }

}