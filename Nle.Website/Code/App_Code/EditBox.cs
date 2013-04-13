using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for EditBox
/// </summary>
public class EditBox : WebControl
{
    const string PARAM_TAG = "EditBoxTag";

    TextBox _textbox;

    public event EventHandler TextChanged;

    public int Tag
    {
        get { return (int)ViewState[PARAM_TAG]; }
        set { ViewState[PARAM_TAG] = value; }
    }

    public int MaxLength
    {
        get { return _textbox.MaxLength; }
        set { _textbox.MaxLength = value; }
    }

    public string Text
    {
        get { return _textbox.Text; }
        set { _textbox.Text = value; }
    }

    public override Unit Width
    {
        get { return _textbox.Width; }
        set { _textbox.Width = value; }
    }

    public string ClientOnChange
    {
        set { _textbox.Attributes["onchange"] = value; }
    }

    public string ClientOnKeyPress
    {
        set { _textbox.Attributes["onkeypress"] = value; }
    }

    public string ClientOnKeyUp
    {
        set { _textbox.Attributes["onkeyup"] = value; }
    }

    public string ClientOnKeyDown
    {
        set { _textbox.Attributes["onkeydown"] = value; }
    }

    public override string ClientID
    {
        get
        {
            return _textbox.ClientID;
        }
    }

    public EditBox()
    {
        _textbox = new TextBox();
        Controls.Add(_textbox);

        _textbox.TextChanged += new EventHandler(_textbox_TextChanged);
    }

    void _textbox_TextChanged(object sender, EventArgs e)
    {
        TextChanged(this, new EventArgs());
    }
}
