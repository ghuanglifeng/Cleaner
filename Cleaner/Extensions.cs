using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using eB.Common.Enum;
using eB.Data;
using System.Xml;


namespace bws.Business
{
    public static class SessionExtensions
    {

        public static int GetProjectId(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Project SELECT Id WHERE Code = '{0}'", code);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;

        }

        public static int GetSkillId(this Session se, string name)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Skill SELECT Id WHERE Name = '{0}'", name);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int GetTemplateId(this Session se, string name)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Template SELECT Id WHERE Name = '{0}'", name);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }
        public static int GetTemplateId(this Session se, string templatename, string objecttype)
        {
            int id = 0;
            try
            {
                int modeltype = getObjectTypeId(se, objecttype);
                string eql = string.Format("START WITH Template SELECT Id WHERE Name = '{0}' AND Model.Type={1}", templatename, modeltype);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int getSubjectDocCode(this Session se, string wOrderCode)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH WorkOrder SELECT Documents.Document.Id WHERE Code = '{0}' AND Documents.IsSubjectData ='Y'", wOrderCode);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("DocumentsDocumentId");
            }
            catch { }
            return id;
        }

        public static int GetTagId(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Tag SELECT Id WHERE Code = '{0}'", code);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int GetPIId(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH PhysicalItem SELECT Id WHERE Code = '{0}'", code);
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int GetCfgDOCId(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Document SELECT Documents.Right.Id WHERE Code = '{0}' AND Documents.RelationshipType.RightName = '配置文件'", code);
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("DocumentsRightId");
            }
            catch { }
            return id;
        }


        public static int GetDOCId(this Session se, string code, string version=null,string type=null)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Document SELECT Id WHERE Code = '{0}' AND Revision ='{1}'", code, version);
                if (string.IsNullOrEmpty(version))
                {
                    eql = string.Format("START WITH Document SELECT Id WHERE Code = '{0}' AND Revision IS NULL", code);
                }
                if (!string.IsNullOrEmpty(type))
                {
                    eql += string.Format(" AND Class.Code='{0}'", type);
                }
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }
        public static int GetObjectId(this Session se, string code, int objectTypeId)
        {
            int id = 0;
            try
            {
                if (objectTypeId == 123)
                {
                    string eql = string.Format("START WITH GroupedVirtualItem SELECT Id WHERE VirtualItem.Code = '{0}'", code);
                    var s = new eB.Data.Search(se, eql);
                    id = s.RetrieveScalar<int>("Id");
                }
                else
                {
                    string eql = string.Format("START WITH Object SELECT Id WHERE Type = {0} AND Code = '{1}'", objectTypeId, code);
                    var s = new eB.Data.Search(se, eql);
                    id = s.RetrieveScalar<int>("Id");
                }
            }
            catch { }
            return id;
        }

        //public static int GetClassIdByTemplateName(this Session se, string tempName, int objectTypeId)
        public static int GetClassIdByTemplateName(this Session se, string tempName)
        {
            int id = 0;
            try
            {
                //string eql = string.Format("START WITH Class SELECT Id WHERE ClassGroup.ObjectType = {0} AND Templates.Name = '{1}'", objectTypeId, tempName);
                string eql = string.Format("START WITH Template SELECT Class.Id WHERE Name = '{0}'", tempName);
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("ClassId");
            }
            catch { }
            return id;
        }

        public static int GetCharId(this Session se, string attrName, int objectTypeId)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH AttributeDef SELECT Id WHERE ObjectType = {0} AND Name = '{1}'", objectTypeId, attrName);
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }


        public static int GetClassId(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Class SELECT Id WHERE Code = '{0}' ", code);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int GetVIGId(this Session se, string code,string version=null,string type=null)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH VirtualItemGroup SELECT Id WHERE Code = '{0}' AND Revision ='{1}'", code, version);
                if (string.IsNullOrEmpty(version))
                {
                    eql = string.Format("START WITH VirtualItemGroup SELECT Id WHERE Code = '{0}' AND Revision IS NULL", code);
                }
                if (!string.IsNullOrEmpty(type))
                {
                    eql += string.Format(" AND Class.Code='{0}'", type);
                }
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int GetORGId(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Organization SELECT Id WHERE Code = '{0}' ", code);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int GetLOCId(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Location SELECT Id WHERE Code = '{0}' ", code);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int PiHasManu(this Session se, string piCode, string manuCode)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH PhysicalItem SELECT Manufacturers.Organization.Id WHERE Manufacturers.Organization.Code = '{0}' AND Code = '{1}' ", manuCode, piCode);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("ManufacturersOrganizationId");
            }
            catch { }
            return id;
        }

        public static int GetGViId(this Session se, string code,string type=null)
        {
            int id = 0;
            try
            {
                string eql=null;
                if (!string.IsNullOrEmpty(type))
                {
                    eql = string.Format("START WITH GroupedVirtualItem SELECT Id WHERE VirtualItem.Code = '{0}' AND VirtualItem.Class.Code='{1}'", code, type);
                }
                else {
                    eql = string.Format("START WITH GroupedVirtualItem SELECT Id WHERE VirtualItem.Code = '{0}' ", code);
                }
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int GetVigIdByGviId(this Session se, int gviId)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH GroupedVirtualItem SELECT VirtualItemGroup.Id WHERE Id = {0}", gviId);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("VirtualItemGroupId");
            }
            catch { }
            return id;
        }

        public static int GetViId(this Session se, string code,string type=null)
        {
            int id = 0;
            try
            {
                string eql = null;
                if (!string.IsNullOrEmpty(type))
                {
                    eql = string.Format("START WITH VirtualItem SELECT Id WHERE Code = '{0}' AND Class.Code= '{1}'", code,type);
                }
                else {
                    eql = string.Format("START WITH VirtualItem SELECT Id WHERE Code = '{0}' ", code);
                }
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int classHasAttr(this Session se, int clsId, int attId)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Class SELECT Id WHERE Id = {0} AND Attributes.AttributeDef.Id = {1}", clsId, attId);
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int getViMaxId(this Session se, int clsId)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH VirtualItem SELECT Id WHERE Class.Id = {0} ORDER BY Id DESC", clsId);
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int getObjectTypeId(this Session se, string objectType)
        {
            int id = 0;
            switch (objectType)
            {
                case "VirtualItem": id = 2; break;
                case "Document": id = 3; break;
                case "Location": id = 12; break;
                case "Tag": id = 212; break;
                case "Item": id = 1; break;
                case "GroupedVitem": id = 123; break;
                case "VirtualItemGroup": id = 182; break;
                case "Organization": id = 5; break;
            }
            return id;
        }

        public static int getClassGroupId(this Session se, string classGroupName)
        {
            int id = 0;
            switch (classGroupName)
            {
                case "VirtualItem": id = 2; break;
                case "Document": id = 1; break;
                case "Location": id = 5; break;
                case "Tag": id = 17; break;
                case "Item": id = 3; break;
                case "Organization": id = 8; break;
                case "Person": id = 8; break;
            }
            return id;
        }

        public static void attachAttrToClass(this Session se, XElement el, string templateName)
        {
            if (el.Attribute("ModifyClass") != null && el.Attribute("ModifyClass").Value == "Y" && el.Element("Attribute") != null)
            {
                XElement atEl = el.Element("Attribute");
                foreach (XAttribute xatt in atEl.Attributes())
                {
                    int otId = se.getObjectTypeId(el.Name.ToString());
                    if (otId == 182) otId = 3;
                    string xattName = xatt.Name.ToString().Replace("_", " ");
                    int aId = se.GetCharId(xattName, otId);
                    int clsId = se.GetClassIdByTemplateName(templateName);

                    //Y - Inherit 
                    //N - Don't Inherit 

                    //1 : Cascaded the association to all existing subclasses. 
                    if (se.classHasAttr(clsId, aId) == 0 && aId != 0 && clsId != 0)
                        se.Writer.AddClassAttribute(clsId, aId, false, 0);

                }
            }
        }

        public static void attachAttrToClass(this Session se, XElement el, int clsId)
        {
            if (el.Attribute("ModifyClass") != null && el.Attribute("ModifyClass").Value == "Y" && el.Element("Attribute") != null)
            {
                XElement atEl = el.Element("Attribute");
                foreach (XAttribute xatt in atEl.Attributes())
                {
                    int otId = se.getObjectTypeId(el.Name.ToString());
                    string xattName = xatt.Name.ToString().Replace("_", " ");
                    if (otId == 2)
                        otId = 123;
                    int aId = se.GetCharId(xattName, otId);
                    //clsId = se.GetClassIdByTemplateName(templateName, otId);

                    //Y - Inherit 
                    //N - Don't Inherit 
                    //0 : Associate the attribute to this class only. 
                    //1 : Cascaded the association to all existing subclasses. 
                    if (se.classHasAttr(clsId, aId) == 0 && aId != 0 && clsId != 0)
                        se.Writer.AddClassAttribute(clsId, aId, false, 0);

                }
            }
        }

        public static string getAttType(this Session se, string att)
        {
            string dataType = "";
            switch (att)
            {
                case "Date":
                    dataType = "DA";
                    break;
                case "Number":
                    dataType = "NU";
                    break;
                case "Memo":
                    dataType = "ME";
                    break;
                case "Character":
                    dataType = "CH";
                    break;
                case "Fixed List":
                    dataType = "PD";
                    break;
                default:
                    dataType = "CH";
                    break;
            }
            return dataType;
        }

        public static void createAttrForClass(this Session se, XElement el, int clsId, int otId)
        {
            if (el.Element("AttributeType") != null)
            {
                XElement atEl = el.Element("AttributeType");
                foreach (XAttribute xatt in atEl.Attributes())
                {
                    string xattName = xatt.Name.ToString().Replace("_", " ");
                    if (otId == 2)
                        otId = 123;
                    int aId = se.GetCharId(xattName, otId);
                    if (aId == 0)
                    {
                        string[] strT = xatt.Value.Split(':');
                        string dataType = se.getAttType(strT[0]);
                        string unit = "";
                        if (strT.Count() == 2)
                            unit = strT[1];
                        aId = se.Writer.AddCharacteristic(otId, xattName, dataType, 0, 0, unit, "N", "N", "N", "", "N", "", "", "", "N");
                    }
                    //clsId = se.GetClassIdByTemplateName(templateName, otId);

                    //Y - Inherit 
                    //N - Don't Inherit 
                    //0 : Associate the attribute to this class only. 
                    //1 : Cascaded the association to all existing subclasses. 
                    if (se.classHasAttr(clsId, aId) == 0 && aId != 0 && clsId != 0)
                        se.Writer.AddClassAttribute(clsId, aId, false, 0);
                }
            }
        }

        public static int getDocClassAttrId(this Session se, eB.Data.Document obj, string attName)
        {
            int intR = 0;
            obj.Class.Retrieve("Attributes");
            BaseCollection<eB.Data.ClassAttribute> acCls = obj.Class.Attributes;
            string xatt = attName.Replace("_", " ");
            eB.Data.ClassAttribute b = acCls.FirstOrDefault(a => a.AttributeDef.Name == xatt);
            if (b != null)
            {
                intR = b.AttributeDef.Id;
            }
            return intR;
        }

        public static int getTagClassAttrId(this Session se, eB.Data.Tag obj, string attName)
        {
            int intR = 0;
            obj.Class.Retrieve("Attributes");
            BaseCollection<eB.Data.ClassAttribute> acCls = obj.Class.Attributes;
            string xatt = attName.Replace("_", " ");
            eB.Data.ClassAttribute b = acCls.FirstOrDefault(a => a.AttributeDef.Name == xatt);
            if (b != null)
            {
                intR = b.AttributeDef.Id;
            }
            return intR;
        }

        public static int getPiClassAttrId(this Session se, eB.Data.PhysicalItem obj, string attName)
        {
            int intR = 0;
            obj.Class.Retrieve("Attributes");
            BaseCollection<eB.Data.ClassAttribute> acCls = obj.Class.Attributes;
            string xatt = attName.Replace("_", " ");
            eB.Data.ClassAttribute b = acCls.FirstOrDefault(a => a.AttributeDef.Name == attName);
            if (b != null)
            {
                intR = b.AttributeDef.Id;
            }
            return intR;
        }

        public static int getPBSId(this Session se, string name)
        {
            int intR = 0;

            string sql = "select pbs_type_id from pbs_types where pbs_type = '" + name + "'";
            string queryStr = se.ProtoProxy.Query(se.ReaderSessionString, sql);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(queryStr);
            XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
            if (nodeList.Count > 0)
            {
                string clsIDStr = nodeList.Item(0).SelectSingleNode("pbs_type_id").InnerText;
                intR = int.Parse(clsIDStr);
            }
            return intR;
        }

        public static int getPPIlistId(this Session se, int ppiId, int pbsId)
        {
            int intR = 0;

            string sql = string.Format(@"select control_id from product_structures
                                        where parent_id = {0} and pbs_type_id = {1} 
                                        ", ppiId, pbsId);
            string queryStr = se.ProtoProxy.Query(se.ReaderSessionString, sql);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(queryStr);
            XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
            if (nodeList.Count > 0)
            {
                string clsIDStr = nodeList.Item(0).SelectSingleNode("control_id").InnerText;
                intR = int.Parse(clsIDStr);
            }
            return intR;
        }

        public static int PpiHasPi(this Session se, int ppiId, int pId)
        {
            int intR = 0;

            string sql = string.Format(@"select component_id from product_structures
                                        where parent_id = {0} and component_id = {1} 
                                        ", ppiId, pId);
            string queryStr = se.ProtoProxy.Query(se.ReaderSessionString, sql);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(queryStr);
            XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
            if (nodeList.Count > 0)
            {
                string clsIDStr = nodeList.Item(0).SelectSingleNode("component_id").InnerText;
                intR = int.Parse(clsIDStr);
            }
            return intR;
        }

        public static int getMaxSqeNo(this Session se, int ppiId, int pbsId)
        {
            int intR = 0;

            string sql = string.Format(@"select MAX(seq_number) as maxId from product_structures
                                        where parent_id = {0} and pbs_type_id = {1} 
                                        ", ppiId, pbsId);
            string queryStr = se.ProtoProxy.Query(se.ReaderSessionString, sql);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(queryStr);
            XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
            if (nodeList.Count > 0)
            {
                string clsIDStr = nodeList.Item(0).SelectSingleNode("maxid").InnerText;
                intR = int.Parse(clsIDStr);
            }
            return intR;
        }

        public static int TagHasPi(this Session se, int piId, int tagId)
        {
            int intR = 0;

            string sql = string.Format(@"select item_id from tag_items
                                        where item_id = {0} and tag_id = {1} 
                                        ", piId, tagId);
            string queryStr = se.ProtoProxy.Query(se.ReaderSessionString, sql);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(queryStr);
            XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
            if (nodeList.Count > 0)
            {
                string clsIDStr = nodeList.Item(0).SelectSingleNode("item_id").InnerText;
                intR = int.Parse(clsIDStr);
            }
            return intR;
        }

        public static int getRelationshipId(this Session se, int leftId, int rightId, int leftType, int rightType)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Relationship SELECT Id WHERE Left.Id = {0} AND Right.Id = {1} AND Left.Type ={2} AND Right.Type ={3}", leftId, rightId, leftType, rightType);
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int getRelationshipId(this Session se, int clsId, int leftId, int rightId, int leftType, int rightType)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Relationship SELECT Id WHERE Left.Id = {0} AND Right.Id = {1} AND Left.Type ={2} AND Right.Type ={3} AND RelationshipType.DerivedFrom.Id = {4}", leftId, rightId, leftType, rightType, clsId);
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int getRelationshipId(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Relationship SELECT Id WHERE RelationshipType.DerivedFrom.Code = '{0}'", code);
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static TagInstallableItem FindInstallableItem(this Session session, Tag tag, PhysicalItem physicalItem)
        {
            var installableItem = new TagInstallableItem();
            tag.Retrieve("InstallableItems");
            if (tag.InstallableItems != null)
            {
                installableItem = tag.InstallableItems.SingleOrDefault(ii => ii.PhysicalItem.Id == physicalItem.Id);
            }
            if (installableItem == null)
            {
                throw new Exception("Provided Physical Item does not match any of Installable Items on this Tag");
            }
            return installableItem;
        }
        public static int getScopeId(this Session se, string strName)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Scope SELECT Id WHERE Name = '{0}'", strName);
                var s = new eB.Data.Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }
        public static string getClassDesc(this Session se, string className)
        {
            string tmp = "";
            try
            {
                string eql = string.Format("START WITH Class SELECT Description WHERE Code = '{0}'", className);
                var s = new eB.Data.Search(se, eql);
                tmp = s.RetrieveScalar<string>("Description");
            }
            catch { }
            return tmp;
        }

        public static string generateCode(this Session se, string strOrgCode, string className)
        {
            string clsDesc = getClassDesc(se, className);
            string code = strOrgCode.Substring(0, strOrgCode.IndexOf("#"));
            code = code + clsDesc + "-";
            try
            {
                string eql = string.Format("START WITH Tag SELECT Code WHERE Code LIKE '{0}' AND Class.Code = '{1}' AND AuditDetails.AuditDef.Name = 'Created' ORDER BY AuditDetails.AuditDate DESC", code + '%', className);
                var s = new eB.Data.Search(se, eql);
                s.RetrieveScalar<string>("Code");
                if (s.HitCount > 0)
                {
                    string tmp = s.RetrieveScalar<string>("Code");
                    tmp = tmp.Substring(code.Length, tmp.Length - code.Length);
                    int t = Convert.ToInt16(tmp);
                    code = code + (t + 1).ToString();
                }
                else
                    code = code + "1";
            }
            catch { }
            return code;
        }

        public static int GetPersonId(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Person SELECT Id WHERE Code = '{0}' ", code);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int GetPersonIdByUserName(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Person SELECT Id WHERE UserAccounts.UserName = '{0}' ", code);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }

        public static int GetFolderId(this Session se, string code)
        {
            int id = 0;
            try
            {
                string eql = string.Format("START WITH Folder SELECT Id WHERE Name = '{0}' ", code);
                var s = new Search(se, eql);
                id = s.RetrieveScalar<int>("Id");
            }
            catch { }
            return id;
        }
    }
}
