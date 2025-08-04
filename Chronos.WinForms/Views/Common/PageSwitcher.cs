namespace Chronos.WinForms.Views.Common
{
    internal class PageSwitcher<T> where T : notnull
    {
        private readonly Control _contentHolderControl;
        private readonly Action<Button>? _highlightButtonAction;
        private readonly Action<Button>? _nonHighlightButtonAction;
        private readonly Dictionary<T, Control> _pageControlByPageId = new();
        private readonly Dictionary<T, Button> _buttonByPageId = new();

        public PageSwitcher(Control contentHolderControl, Action<Button>? highlightButtonAction = null, Action<Button>? nonHighlightButtonAction = null)
        {
            _contentHolderControl = contentHolderControl;
            _highlightButtonAction = highlightButtonAction;
            _nonHighlightButtonAction = nonHighlightButtonAction;
        }

        internal void RegisterPage(T pageType, Control pageControl, Button button)
        {
            pageControl.Visible = false;
            pageControl.Dock = DockStyle.Fill;
            _pageControlByPageId.Add(pageType, pageControl);
            _buttonByPageId.Add(pageType, button);
            _contentHolderControl.Controls.Add(pageControl);
        }

        internal void SetActivePage(T pageType)
        {
            foreach (var registeredControl in _pageControlByPageId.Values)
            {
                registeredControl.Visible = false;
            }

            var control = _pageControlByPageId[pageType];
            control.Visible = true;

            if(_highlightButtonAction != null)
            {
                _highlightButtonAction(_buttonByPageId[pageType]);
            }

            if(_nonHighlightButtonAction == null)
            {
                return;
            }

            foreach (var (pageId, button) in _buttonByPageId)
            {
                if(pageId.Equals(pageType))
                {
                    continue;
                }

                _nonHighlightButtonAction(button);
            }
        }
    }     
}
