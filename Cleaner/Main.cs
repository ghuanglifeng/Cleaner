using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using eB.Data;
using bws.Business;
using bws02.Common;

namespace Cleaner
{
    public partial class Main : Form
    {
        eB.Data.Session Se = null;
        public Main()
        {
            InitializeComponent();
            Se = new eB.Data.Session();
            try
            {
                Se.ConnectAndLogon(Constants.GetKeyValue("server"), Constants.GetKeyValue("db"), Constants.GetKeyValue("usr"), Constants.GetKeyValue("pwd"));
            }
            catch (Exception e)
            {
                MessageBox.Show("登陆失败:"+e.Message);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string clsName = txtClassName.Text;
                switch (clsName)
                {
                    case "PR":
                    case "PTN_PRJ":
                    case "PTN_PUR":
                    case "VIG_REL":
                    case "PO":
                    case "ICM":
                        ClearAllVIG(clsName);
                        break;
                    case "DOC_DES_DRAW":
                    case "DOC_DES_EDCR":
                    case "DOC_QUA_CAR":
                    case "DOC_QUA_QOR":
                    case "DOC_PUR_TDL":
                    case "DOC_DES_RFI":
                    case "DOC_DES_KKX":
                    case "DOC_CON_DOC":
                    case "DOC_QUA_NCRPUR":
                    case "DOC_QUA_NCRCON":
                    case "DOC_QUA_NCRSIT":
                        ClearAllDOC(clsName);
                        break;
                    case "RISKBASE":
                        ClearAllGVI(Se.GetVIGId("VIG_RISKBASE_001"),clsName);
                        break;
                    case "WP":
                        ClearAllGVI(Se.GetVIGId("VIG_WORKPACKAGE_001"), clsName);
                        break;
                    case "RISKFOUR":
                        ClearAllGVI(Se.GetVIGId("VIG_CONSTRAINT_001"), clsName);
                        break;
                    case "WP_CONST":
                        ClearAllGVI(Se.GetVIGId("VIG_RISKABOVEF_001"), clsName);
                        break;
                    case "SNGMODULE":
                        ClearAllGVI(Se.GetVIGId("SNGMODULE"), "MODULE");
                        break;
                    case "SNGCCODE":
                        ClearAllGVI(Se.GetVIGId("SNGCCODE"), "MODULE");
                        break;
                    case "MODULE":
                        MessageBox.Show("删除模块或者定位码请填写vig编号");
                        return;
                    case "PR_ITEMS":
                        ClearAllGvisByClassName("PR", clsName);
                        break;
                    case "PO_ITEMS":
                        ClearAllGvisByClassName("PO", clsName);
                        break;
                    case "ORG_PROVIDER":
                        ClearAllORG(clsName);
                        break;
                    case "tj_tf"://土方
                    case "tj_sf"://石方
                    case "tj_ht"://回填
                    case "tj_zh"://支护
                    case "tj_zj"://桩基
                    case "tj_qt"://砌体(含砌体内钢筋)
                    case "tj_hnt"://混凝土(含灌浆)
                    case "tj_gj"://钢筋（含套筒、锚固块）
                    case "tj_ymls"://预埋螺栓
                    case "tj_tgymj"://碳钢预埋件
                    case "tj_bxgymj"://不锈钢埋件
                    case "tj_hzmj"://后置埋件(含油漆)
                    case "tj_ymtg"://预埋套管(含油漆)
                    case "tj_jsjg"://金属结构(含油漆)
                    case "tj_bxgjg"://不锈钢结构
                    case "tj_jsqb"://金属墙板
                    case "tj_yxgb"://压型钢板
                    case "tj_jcfs"://卷材防水
                    case "tj_wmpsg":////屋面排水管
                    case "tj_bxf"://变形缝
                    case "tj_bw"://保温
                    case "tj_mc"://门窗
                    case "tj_db"://地板
                    case "tj_dt"://地毯
                    case "tj_kldm"://块料地面
                    case "tj_klqm"://块料墙面
                    case "tj_tjx"://踢脚线
                    case "tj_gd"://隔断(石膏板墙)
                    case "tj_dd"://吊顶
                    case "tj_tjg"://砼结构和砌体涂层
                    case "tj_fd"://封堵(密封、嵌缝)
                    case "tj_snsj"://水泥砂浆
                    case "tj_jgmk"://结构模块
                    case "tj_dldc"://道路垫层、基层
                    case "tj_dlmc"://道路面层
                    case "tj_lcs"://路侧石
                    case "tj_lxwj"://零星物件
                        ClearAllTAG(clsName);
                        break;
                    default:
                        MessageBox.Show(string.Format("{0}不存在", clsName));
                        break;
                }
                MessageBox.Show("ok");
            }
            catch (Exception ex)
            {
                MessageBox.Show("fail:" + ex.Message);
            }
        }
        public void ClearAllVIG(string typeName)
        {
            string eql = string.Format(@"START WITH VirtualItemGroup
                            SELECT Id
                            WHERE Class.Code='{0}' AND IsTemplate='N'", typeName);
            try
            {
                var s = new eB.Data.Search(Se, eql);
                DataTable dt = s.Retrieve<DataTable>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        eB.Data.VirtualItemGroup vig = new eB.Data.VirtualItemGroup(Se, id);
                        vig.Retrieve("Header;Members");
                        if (vig.Members != null && vig.Members.Count > 0)
                        {
                            string membersTypeName = null;
                            if (typeName == "PR")
                            {
                                membersTypeName = "PR_ITEMS";
                                ClearAllGVI(vig.Id, membersTypeName);
                            }
                            else {
                                MessageBox.Show("删除失败：存在子项");
                            }
                        }
                        vig.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("清空类型编码为{0}的vig失败::原因是{1}", typeName, e.Message));
            }
        }
        public void ClearAllGVI(int vigId,string clsName)
        {
            eB.Data.VirtualItemGroup vig = new eB.Data.VirtualItemGroup(Se, vigId);
            vig.Retrieve("Header;Members");
            BaseCollection<GroupedVirtualItem> col = vig.Members;
            if (col != null && col.Count > 0)
            {
                foreach (GroupedVirtualItem gvi in col)
                {
                    gvi.VirtualItem.Retrieve("Header");
                    Class cl = gvi.VirtualItem.Class;
                    if (cl.Name == clsName)
                    {
                        try
                        {
                            Se.Writer.DelVitemGrpMember(gvi.Id);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        try
                        {
                            Se.Writer.DelVitem(gvi.VirtualItem.Id);
                        }
                        catch (Exception e)
                        {
                            //throw new Exception(string.Format("删除gvi{1}失败::原因是{2}", gvi.VirtualItem.Code, e.Message));
                        }
                    }
                }
            }
        }
        public void ClearAllGvisByClassName(string vigCls,string viCls) 
        {
            string eql = string.Format(@"START WITH VirtualItemGroup
                            SELECT Id
                            WHERE Class.Code='{0}' AND IsTemplate='N'", vigCls);
            try
            {
                var s = new eB.Data.Search(Se, eql);
                DataTable dt = s.Retrieve<DataTable>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        ClearAllGVI(id, viCls);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("失败："+e.Message);
            }
        }
        public void ClearAllPI(string typeName)
        {
            string eql = string.Format(@"START WITH PhysicalItem
                            SELECT Id
                            WHERE Class.Code='{0}' AND IsTemplate='N'", typeName);
            try
            {
                var s = new eB.Data.Search(Se, eql);
                DataTable dt = s.Retrieve<DataTable>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        eB.Data.PhysicalItem pI = new eB.Data.PhysicalItem(Se, id);
                        pI.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("清空类型编码为{0}的PI失败::原因是{1}", typeName, e.Message));
            }
        }
        public void ClearAllTAG(string typeName)
        {
            string eql = string.Format(@"START WITH Tag
                            SELECT Id
                            WHERE Class.Code='{0}' AND IsTemplate='N'", typeName);
            try
            {
                var s = new eB.Data.Search(Se, eql);
                DataTable dt = s.Retrieve<DataTable>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        eB.Data.Tag tag = new eB.Data.Tag(Se, id);
                        tag.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("清空类型编码为{0}的TAG失败::原因是{1}", typeName, e.Message));
            }
        }
        public void ClearAllDOC(string typeName)
        {
            string eql = string.Format(@"START WITH Document
                            SELECT Id
                            WHERE Class.Code='{0}' AND IsTemplate='N'", typeName);
            try
            {
                var s = new eB.Data.Search(Se, eql);
                DataTable dt = s.Retrieve<DataTable>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        eB.Data.Document doc = new eB.Data.Document(Se, id);
                        doc.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("清空类型编码为{0}的DOC失败::原因是{1}", typeName, e.Message));
            }
        }
        public void ClearAllVI(string typeName)
        {
            string eql = string.Format(@"START WITH VirtualItem
                            SELECT Id
                            WHERE IsUsed='N' AND Class.Code='{0}'", typeName);
            try
            {
                var s = new eB.Data.Search(Se, eql);
                DataTable dt = s.Retrieve<DataTable>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        eB.Data.VirtualItem vi = new eB.Data.VirtualItem(Se, id);
                        vi.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("清空类型编码为{0}的VI失败::原因是{1}", typeName, e.Message));
            }
        }
        public void ClearAllORG(string typeName)
        {
            string eql = string.Format(@"START WITH Organization
                            SELECT Id
                            WHERE Class.Code='{0}' AND IsTemplate='N'", typeName);
            try
            {
                var s = new eB.Data.Search(Se, eql);
                DataTable dt = s.Retrieve<DataTable>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        eB.Data.Organization org = new eB.Data.Organization(Se, id);
                        org.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("清空类型编码为{0}的org失败::原因是{1}", typeName, e.Message));
            }
        }


        private void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAllVIG("PR");
                ClearAllVIG("PTN_PRJ");
                ClearAllVIG("PTN_PUR");
                ClearAllDOC("DOC_DES_DRAW");
                ClearAllDOC("DOC_DES_EDCR");
                ClearAllDOC("DOC_QUA_CAR");
                ClearAllDOC("DOC_QUA_QOR");
                ClearAllDOC("DOC_PUR_TDL");
                ClearAllGVI(Se.GetVIGId("VIG_RISKBASE_001"), "RISKBASE");
                ClearAllGVI(Se.GetVIGId("VIG_WORKPACKAGE_001"), "WP");
                ClearAllGVI(Se.GetVIGId("VIG_CONSTRAINT_001"), "WP_CONST");

                MessageBox.Show("ok");
            }
            catch (Exception ex)
            {
                MessageBox.Show("fail:" + ex.Message);
            }
            
        }
    }
}
