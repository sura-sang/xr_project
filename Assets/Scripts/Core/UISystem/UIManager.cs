using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public enum UIType
    {
        Panel,
        Popup,
        Page
    }

    public enum UIState
    {
        None,

        Normal,
        AddFurniture,
    }

    public class UIManager : MonoBehaviour
    {
        private const string PanelPrefabPath = "UI/Panel/";
        private const string PagePrefabPath = "UI/Page/";
        private const string PopupPrefabPath = "UI/Popup/";

        [SerializeField] private Transform panelParent;
        [SerializeField] private Transform popupParent;
        [SerializeField] private Transform pageParent;
        [SerializeField] private Transform deActiveParent;

        private Dictionary<Type, UIModelBase> _allUI = new Dictionary<Type, UIModelBase>();

        private Stack<UIModelBase> _popupStack = new Stack<UIModelBase>();

        public UIState State { get; private set; } = UIState.None;

        #region Get

        public T Get<T>() where T : UIModelBase
        {
            if (_allUI.TryGetValue(typeof(T), out var model))
            {
                switch (model.UIType)
                {
                    case UIType.Panel:
                        model.UIView.transform.SetParent(panelParent);
                        break;
                    case UIType.Popup:

                        if(model.UIView.transform.parent == popupParent)
                        {
                            ReleasePopup(model);
                        }

                        model.UIView.transform.SetParent(popupParent);
                        model.UIView.transform.SetAsLastSibling();

                        _popupStack.Push(model);
                        break;
                    case UIType.Page:
                        model.UIView.transform.SetParent(pageParent);
                        break;
                }

                return (T) model;
            }

            model = (T) Activator.CreateInstance(typeof(T));
            UIViewBase view = null;
            switch (model.UIType)
            {
                case UIType.Panel:
                    view = OpenPanel(model);
                    break;
                case UIType.Popup:
                    view = OpenPopup(model);
                    view.transform.SetAsLastSibling();

                    _popupStack.Push(model);
                    break;
                case UIType.Page:
                    view = OpenPage(model);
                    break;
            }

            model.OnCreate(view);

            if (view.UsePooling)
            {
                _allUI.Add(typeof(T), model);
            }

            return (T) model;
        }

        private UIViewBase OpenPanel(UIModelBase model)
        {
            var go = GameObject.Instantiate(Resources.Load<GameObject>(PanelPrefabPath + model.PrefabPath), panelParent);
            var view = go.GetComponent<UIPanelBase>();
            return view;
        }

        private UIViewBase OpenPopup(UIModelBase model)
        {
            var go = GameObject.Instantiate(Resources.Load<GameObject>(PopupPrefabPath + model.PrefabPath), popupParent);
            var view = go.GetComponent<UIPopupBase>();

            return view;
        }

        private UIViewBase OpenPage(UIModelBase model)
        {
            var go = GameObject.Instantiate(Resources.Load<GameObject>(PagePrefabPath + model.PrefabPath), pageParent);
            var view = go.GetComponent<UIPageBase>();
            return view;
        }
        
        #endregion

        #region Release

        public void Release(UIModelBase model)
        {
            switch (model.UIType)
            {
                case UIType.Panel:
                    model.UIView.transform.SetParent(deActiveParent);
                    break;
                case UIType.Popup:
                    ReleasePopup(model);
                    if (model.UIView && !model.UIView.UsePooling)
                    {
                        GameObject.Destroy(model.UIView);
                    }
                    break;
                case UIType.Page:
                    model.UIView.transform.SetParent(deActiveParent);
                    break;
            }
        }

        private void ReleasePopup(UIModelBase model)
        {
            if (_popupStack.Count != 0 && _popupStack.Peek().Equals(model))
            {
                _popupStack.Pop();
                model.UIView.transform.SetParent(deActiveParent);
            }
            else if (_popupStack.Contains(model))
            {
                while (_popupStack.Count > 0)
                {
                    var last = _popupStack.Pop();
                    last.UIView.transform.SetParent(deActiveParent);

                    if (last.Equals(model))
                    {
                        return;
                    }
                }
            }
        }

        public void ReleaseAllPopups()
        {
            while (_popupStack.Count > 0)
            {
                var last = _popupStack.Pop();
                last.UIView.transform.SetParent(deActiveParent);
            }
        }

        #endregion

        public void SetState(UIState state)
        {
            State = state;

            foreach (var ui in _allUI)
            {
                ui.Value.SetState(state);
            }
        }
    }
}