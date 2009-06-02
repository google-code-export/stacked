using System;
using ASP = System.Web.UI;
using Ra.Selector;
using Ra.Widgets;

namespace Utilities
{
    public static class SelectorHelpers
    {
        public static T FindFirstByCssClass<T>(ASP.Control from, string cssClass) where T : ASP.Control
        {
            return Selector.SelectFirst<T>(from,
                delegate(ASP.Control idx)
                {
                    if (idx is RaWebControl)
                        return (idx as RaWebControl).CssClass == cssClass;
                    return false;
                });
        }
    }
}