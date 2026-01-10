/******************************************************************************************
*                                                                                         *    
*                              Developed by Bruno Pires                                   *
*                                                                                         *
*                             email: bruno@blastersystems.com                             *
*                               web: www.blastersystems.com                               *
*                              blog: www.blastersystems.com/blog                          *
*                           twitter: brunoacpires                                         *
*                                                                                         *
*                      Software available under GNU LGPL v3 License                       *
*                                                                                         *    
* *****************************************************************************************/  

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace jQueryMessageBox.WebUserControl
{
    public partial class MessageBoxControl : System.Web.UI.UserControl
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string NameBtn { get; set; }
        public string NameBtnMessageBox { get; set; }
        public typeMessageBox tipo { get; set; }

        public enum typeMessageBox
        {
            centerMessageBox,
            growlMessageBox
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (NameBtn != null)
            {
                Button btn = new Button();
                btn.ID = "btn";
                btn.Attributes.Add("class", "fg-button ui-state-default");
                btn.Click += new EventHandler(btn_Click);
                btn.Text = NameBtn;
                messageBoxBtn.Controls.Add(btn);
            }
        }

        public void btn_Click(object sender, EventArgs e)
        {
            string javascript;
            if (tipo == typeMessageBox.centerMessageBox)
            {
                javascript = "javascript: messagebox('" + Text + "','" + Title + "', '" + NameBtnMessageBox + "'); ";
            }
            else
            {
                javascript = "javascript: growlMessagebox('" + Title + "', '" + Text + "'); ";
            }

            //Page.ClientScript.RegisterStartupScript(GetType(), "Javascript", javascript, true);

            ScriptManager Smgr = ScriptManager.GetCurrent(Page);
            if (Smgr == null) throw new Exception("ScriptManager não encontrado!");

            bool registered = false; if (Smgr.IsInAsyncPostBack)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), String.Format("ScriptStatic_{0}", this.ClientID), javascript, true);

                registered = true;
            }

            if (!registered)
            {

                Page.ClientScript.RegisterStartupScript(this.GetType(), String.Format("ScriptStatic_{0}", this.ClientID), javascript, true);
            }
        }
    }
}