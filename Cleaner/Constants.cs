using System;
using System.Linq;
using bws.Business;
using System.Configuration;
using System.IO;
using eB.Data;

namespace bws02.Common
{
    /// <summary>
    /// 常量、枚举、方法
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// 项目根路径
        /// </summary>
        //public static string base_path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public static string base_path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        /// <summary>
        /// 增量数据类型
        /// </summary>
        public enum APPENDENUM
        {
            POST_DESN_FILE_INFO = 0,//设计文件
            POST_EDCR_INFO = 1,//设计Edcr
            POST_EDCR_REF_INFO = 2,//设计Edcr关联文件清单
            POST_EFF_INFO = 3,//设计受影响设计文件清单
            POST_RFI_INFO = 4,//设计澄清RFI
            POST_RFI_REF_INFO = 5,//设计澄清RFI子表参考文件清单
            POST_RFI_EFF_INFO = 6,//设计澄清RFI子表受影响文件清单
            POST_DESN_OPEN_ITEMS_INFO = 7,//设计开口项
        }
        /// <summary>
        /// 全量数据类型
        /// </summary>
        public enum OVERRIDEENUM
        {
            PR_HEARD = 0,//采购PR信息
            PR_ITEMS = 1,//采购请购单物项明细
            PR_LOCATION = 2,//采购请购单所含物项定位码（废弃）
            PR_TDL = 3,//采购TDL
            PR_TDL_LOCATION = 4,//采购TDL物项定位码（废弃）
            POST_PO_INFO = 12,//采购PO
            POST_PO_BEF_ITEMS_INFO = 13,//采购PO子表合并前的物项清单
            POST_SUPPLIER_INFO = 14,//采购供应商名称代码
            POST_CAR_INFO = 5,//质量CAR
            POST_QOR_INFO = 6,//质量QOR
            POST_LOCALE_NCR_INFO = 19,//现场NCR
            POST_CONSTRUCT_NCR_INFO = 20,//施工NCR
            POST_APPROVE_NCR_INFO = 21,//采购NCR
            PROJECT_STAND_RISK = 7,//风险项目基准风险清单
            PTN_PROJECT_CHANGE_INFO = 8,//变更项目PTN
            PTN_PUR_CHANGE_INFO = 9,//变更采购PTN
            POST_RISK = 10,//风险
            POST_ABOVE_FRISK_INFO = 22,//风险四级以上风险
            PROJECT_PACKAGE_DATA = 11,//施工包
            POST_CONSTRUCT_INFO = 15,//施工文件
            POST_CONSTRUCT_SUBKEY_INFO = 16,//施工文件子表文件关键属性
            POST_CONSTRUCT_SUBWBS_INFO = 17,//施工文件子表工作包
            POST_CONSTRUCT_SUBDESN_INFO = 18,//施工文件子表相关设计文件
            POST_CIVILENGINEERING_INFO = 23,//图纸工程量
            POST_ICM_INFO = 24,//提资
        }
        /// <summary>
        /// eB Class Name对应的Class Id
        /// </summary>
        public enum EBCLASSENUM
        {
            VIG_DES_EDCR=1545,//Edcr
            PR = 1566,//PR
            PI_PHYSICALITEM = 1567,//请购单物项明细
            TAG_GOODSPOSCODE = 1569,//请购单所含物项定位码
            PI_TDL=0003,//采购TDL
            TAG_TDLGOODSPOSCODE=0004,//采购TDL物项定位码
            DOC_QUA_CAR = 1571,//质量CAR
            DOC_QUA_QOR = 1572,//质量QQR
            VI_RISKBASE=0007,//项目基准风险清单
            PTN_PRJ = 1574,//变更项目PTN
            PTN_PUR = 1575,//变更采购PTN
        }

       public static string[] keys = ConfigurationManager.AppSettings.AllKeys.ToArray();
        /// <summary>
        /// 根据键取webconfig配置的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetKeyValue(string key)
        {
            try
            {
                if (keys == null)
                {
                    keys = ConfigurationManager.AppSettings.AllKeys.ToArray();
                }
                foreach (string s in Constants.keys)
                {
                    string value = ConfigurationManager.AppSettings[key];
                    if (s == key)
                        return value;
                }
            }
            catch (System.Exception ex)
            {
                //LogWriter.WriteLog(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 全局eb session
        /// </summary>
        private static eB.Data.Session _se;

        public static eB.Data.Session Se
        {
            get {
                if (_se == null)
                {
                    _se = ebLogin();
                }
                return _se; 
            }
            set { _se = value; }
        }
        /// <summary>
        /// 获取登录后的eb session
        /// </summary>
        /// <returns></returns>
        public static eB.Data.Session ebLogin(){
            string ebUserName=GetKeyValue("ebUserName");
            string ebUserPassword=GetKeyValue("ebUserPassword");
            string[] ebDatabase = GetKeyValue("SNGCAP1400").Split(';');
            string server=ebDatabase[0];
            string community=ebDatabase[1];
            eB.Data.Session LoginSession = null;
            try
            {
                LoginSession = new Session();
                LoginSession.ConnectAndLogon(server, community, ebUserName, ebUserPassword);
                LoginSession.SetDefaultScope(1);
            }catch(Exception e)
            {
                //LogWriter.WriteLog("登陆eb失败：原因是" + e.Message, LogLevel.Error);
                throw;
            }
            return LoginSession;
        }

        #region eb模板、类型及实例配置
        #region eb类名
        /// <summary>
        /// 关系表
        /// </summary>
        public static string VIG_REL = GetKeyValue("VIG_REL");
        #endregion
        #endregion

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetMillsecondsFrom1970()
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            return Math.Ceiling(ts.TotalMilliseconds).ToString();
        }
    }
}