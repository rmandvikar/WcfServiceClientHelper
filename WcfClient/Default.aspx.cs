using System;
using WcfClientHelper;
using WcfClientHelper.WcfServiceReference;

namespace WcfClient
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //Wrong();
            //Normal();
            Radical();
        }

        #region ways
        private void Wrong()
        {
            string data;
            using (var client = new ServiceClient().GetProxy())
            {
                data = client.GetData(int.Parse(txtData.Text));
            }
            ltlData.Text = data;
        }
        private void Correct()
        {
            string data;
            bool success = false;
            ServiceClient client = null;
            try
            {
                client = new ServiceClient().GetProxy();
                data = client.GetData(int.Parse(txtData.Text));
                client.Close();
                success = true;
            }
            finally
            {
                if (!success && client != null)
                {
                    client.Abort();
                }
            }
            ltlData.Text = data;
        }
        private void Radical()
        {
            string data = Service<ServiceClient>.Use(client => client.GetData(int.Parse(txtData.Text)));
            ltlData.Text = data;
        }
        #endregion
    }
}
