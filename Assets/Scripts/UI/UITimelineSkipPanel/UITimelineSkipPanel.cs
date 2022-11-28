using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Threading.Tasks;

namespace SuraSang
{
    public class UITimelineSkipPanel : UIPanelBase
    {
        public UnityAction _onSkip { get; set; }

        public void OnClickSkipButton()
        {
            _onSkip?.Invoke();
        }
    }

    public class UITimelineSkipPanelModel : UIModelBase
    {
        private UnityAction _onSkip;

        public override UIType UIType => UIType.Panel;
        public override string PrefabPath => "TimelineSkipPanel";
        public override void SetState(UIState state)
        {
        }

        public override void OnCreate(UIViewBase view)
        {
            base.OnCreate(view);

            (view as UITimelineSkipPanel)._onSkip = OnSkip;
        }

        public void Init(UnityAction onSkip)
        {
            _onSkip = onSkip;
        }

        private void OnSkip()
        {
            _onSkip?.Invoke();
        }
        
    }
}