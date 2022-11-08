namespace SuraSang
{
    public abstract class UIModelBase
    {
        public UIViewBase UIView {get; private set;}

        public abstract UIType UIType {get;}
        public abstract string PrefabPath {get;}

        public virtual void OnCreate(UIViewBase view)
        {
            UIView = view;
        }

        public virtual void SetState(UIState state)
        {}
        
        public virtual void Show()
        {
            UIView.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            UIView.gameObject.SetActive(false);
        }

        public virtual void ReleaseUI()
        {
            Global.Instance.UIManager.Release(this);
        }
    }
}