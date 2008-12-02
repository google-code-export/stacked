using System;
using Entities;
using System.Collections.Generic;
using Ra.Widgets;

public partial class UserControls_ItemGrid : System.Web.UI.UserControl
{
    protected string GetCssClass(int count)
    {
        if (count < -10)
            return "really-bad";
        if (count < 0)
            return "bad";
        if (count > 9)
            return "great";
        if (count > 0)
            return "good";
        return "neutral"; // 0 items... (only ones left)
    }

    protected string GetTime(DateTime time)
    {
        return TimeFormatter.Format(time);
    }

    public bool IsDataBound
    {
        get { return rep.DataSource != null; }
    }

    public void DataBindGrid(IEnumerable<QuizItem> items)
    {
        rep.DataSource = items;
        rep.DataBind();
        wrap.ReRender();
    }
}
