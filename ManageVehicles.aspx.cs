using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageVehicles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        String desc = TextBox1.Text;
        String num = TextBox2.Text;
        Vehicle vh = new Vehicle();
        vh.Description =desc;
        vh.TrackingNumber = num;

        DatabaseEntities de = new DatabaseEntities();
        de.Vehicles.Add(vh);
        de.SaveChanges();
        GridView1.DataBind();


    }
}