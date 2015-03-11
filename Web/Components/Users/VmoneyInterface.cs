using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

namespace Web.Components
{
    //消耗V币操作类
    public class VmoneyInterface:BasePage
    {


        DAL.DALVmoneyDetailNew DALVmoneyDetailNew1 = new DAL.DALVmoneyDetailNew();
        Model.VmoneyDetailNew ModelVmoneyDetailNew1 = new Model.VmoneyDetailNew();

        DBUtility.SqlManage SqlManage1 = new DBUtility.SqlManage();
        public  double _vmoney;
        private readonly string[] operationname = { "", "充值V币", "分发V币", "回收V币", "群发邮件扣除V币", "赠送V币", "发送邮件扣除V币" };
        /// <summary>
        /// 活动开始时间
        /// </summary>
        private readonly static DateTime YDBTime = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["YDBTime"]);
        /// <summary>
        /// 活动结束时间
        /// </summary>
        private readonly static DateTime YDETime = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["YDETime"]);
        /// <summary>
        /// 个人最大返点数
        /// </summary>
        private readonly static double PMaxReV = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["PMaxReV"]);
        /// <summary>
        /// 邀请人最大返点数
        /// </summary>
        private readonly static double TMaxReV = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["TMaxReV"]);

        public double Vmoney
        {
            get
            {
                return GetVmoney();
            }
            set { _vmoney = value; }
        }



        /// <summary>
        ///type( 发送邮件， 公司信息， 个人设置，  增加客户， 邮箱配置 ，打卡)
        ///活动赠送v币
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public string TaskCoin( string Type)
        {
         
            string where="";
            string json="";
            int type = -1;
            int strVmoney = 0;
                int taskid = 0;
                if (Type == "发送邮件")
                {
                    taskid = 5;
                    strVmoney = 1000;
                }

                if (Type == "公司信息")
                {
                    taskid = 1;
                    strVmoney = 1000;
                }
                if (Type == "个人设置")
                {
                    taskid = 2;
                    strVmoney = 1000;
                }
                if (Type == "增加客户")
                {
                    taskid = 4;
                    strVmoney = 1000;
                }
                if (Type == "邮箱配置")
                {
                    taskid = 3;
                    strVmoney = 1000;
                }
                 if (Type == "打卡")
                {
                    taskid = 0;
                    strVmoney = 50;
                    where = " and tasktype=1 and  DATEDIFF( DAY,Addtime,GETDATE())=0  ";
                }

                 string tkid = SqlManage1.One("select id from  iTradeCRM.dbo.VmoneyDetailNew where taskid=" + taskid + " and userid=" + UserId + where);


                
                if (Type == "发送邮件")
                {
                    if (tkid == null)
                    {

                        SqlManage1.Upd(" insert iTradeCRM.dbo.VmoneyDetailNew([Addtime],[taskid],[type],[Vmoney],[UserId],[ParentId],[DelState],FromUserId,ToUserId,tasktype) select  top 1 getdate(),5,'发送邮件',1000 , UserId ,  ParentId ,0,0," + UserId + ",0 from iTradeEM.dbo.Sentbox    WHERE      (DelState = '0') AND (SentStatu = '2') and UserId=" + UserId);
                        type = 0;
                    }
                }



                if (Type == "公司信息")
                {
                    if (tkid == null)
                    {
                        SqlManage1.Upd(" insert VmoneyDetailNew([Addtime],[taskid],[type],[Vmoney],[UserId],[ParentId],[DelState],FromUserId,ToUserId,tasktype) values(getdate(),1,'公司信息',1000 ," + UserId + "," + ParentId + ",0,0," + UserId + ",0 )");
                        type = 0;
                    }
                }

                if (Type == "个人设置")
                {
                    if (tkid == null)
                    {
                        SqlManage1.Upd(" insert VmoneyDetailNew([Addtime],[taskid],[type],[Vmoney],[UserId],[ParentId],[DelState],FromUserId,ToUserId,tasktype) values(getdate(),2,'个人设置',1000 ," + UserId + "," + ParentId + ",0,0," + UserId + ",0 )");
                        type = 0;
                    }
                }

                if (Type == "增加客户")
                {
                    if (tkid == null)
                    {
                        SqlManage1.Upd(" insert VmoneyDetailNew([Addtime],[taskid],[type],[Vmoney],[UserId],[ParentId],[DelState],FromUserId,ToUserId,tasktype) values(getdate(),4,'增加客户',1000 ," + UserId + "," + ParentId + ",0,0," + UserId + ",0 )");
                        type = 0;
                    }
                }

                if (Type == "邮箱配置")
                {
                    if (tkid == null)
                    {
                        SqlManage1.Upd(" insert VmoneyDetailNew([Addtime],[taskid],[type],[Vmoney],[UserId],[ParentId],[DelState],FromUserId,ToUserId,tasktype) select  top 1 getdate(),3,'邮箱配置',1000 , UserId ,  ParentId ,0 ,0," + UserId + " ,0 from EmailSetting where userid=" + UserId);
                        type = 0;
                    }
                }



                if (Type == "打卡")
                {
                    type = 1;
                    if (tkid == null)
                    {
                        SqlManage1.Upd(" insert VmoneyDetailNew([Addtime],[taskid],[type],[Vmoney],[UserId],[ParentId],[DelState],tasktype,FromUserId,ToUserId) values(getdate(),0,'打卡',50 ," + UserId + "," + ParentId + ",0,1,0," + UserId + ")");

                        json = "{\"Type\":0,\"Message\":\"我已打卡\"}"; 
                    }
                    else
                        json = "{\"Type\":-1,\"Message\":\"我已打卡\"}"; 
                }

                if (tkid == null)
                {
                    SqlManage1.Upd(" update iTradeCRM.dbo.Users set Vmoney=Vmoney+" + strVmoney + " where userid=" + UserId);
                }

               

            
            if (type == 0)
            {
                json = "{\"Type\":0,\"Message\":\"操作成功\"}";
            }
            else if (type==1)
                return json;
                else
                json = "{\"Type\":-1,\"Message\":\"已领取\"}";
            return json;

        }


        /// <summary>
        /// 返回未完成的任务数
        /// </summary>
        /// <returns></returns>
        public int ReturnTask()
        {

        int i= int.Parse(   SqlManage1.One("select 5-count(*) from VmoneyDetailNew where  tasktype=0 and userid=" + UserId  ));

            return i;
        }

        /// <summary>
        /// 增加或扣除V币方法
        /// </summary>
        /// <param name="TypeId">操作标识(2：分发，3：回收，4：群发邮件，5：注册赠送,6:发送邮件)</param>
        /// <param name="deductvmoney">扣除V币数</param>
        /// <param name="userid">用户Id</param>
        /// <param name="remarks">备注</param>
        /// <returns></returns>
        public string DeductVmoney(int FromUserId,int ToUserId,double Vmoney,string Type,string Description="")
        {
            if (Type == "")
            {
                return "{\"Type\":\"-1\",\"Message\":\"请求来源错误哦\"}";
            }
            string refer = ""; //邀请人
            double myvmoney = 0; 
            double pnum = 0.0; //已经返点V币数
            double totalnum = 0.0; //邀请人获得的总返点V币数
            if ((Type == "分发") || (Type == "群发邮件") || (Type == "发送邮件"))
            {
                myvmoney = GetVmoney();
                if ((Vmoney <= 0) || (myvmoney < Vmoney))
                {
                    return "{\"Type\":\"-1\",\"Message\":\"V币余额不足\",\"DetailId\":\"0\",\"NewMoney\":\"" + myvmoney + "\"}";
                }
                if (Type == "群发邮件" && DateTime.Now > YDBTime && DateTime.Now < YDETime)
                {
                    refer = SqlManage1.One("select referrer from Users where UserId=" + FromUserId);
                    if (refer != null && refer != "") {
                        DataTable dt = SqlManage1.Sel(@"select  isnull(SUM(Vmoney),0)as pnum ,(select isnull(SUM(Vmoney),0)  from  VmoneyDetailNew where type='返点' and ToUserId=" + refer + ")as totalnum from  VmoneyDetailNew where type='返点' and UserId=" + FromUserId + "");
                        if (dt.Rows.Count > 0) {
                            pnum = Convert.ToDouble(dt.Rows[0]["pnum"]);
                            totalnum = Convert.ToDouble(dt.Rows[0]["totalnum"]);
                        }
                    }
                   // DataTable dt = SqlManage1.Sel(@"select isnull(SUM(Vmoney),0)as pnum ,(select isnull(SUM(Vmoney),0)  from  VmoneyDetailNew where type='返点' and ToUserId=(select referrer from Users where UserId=" + FromUserId + "  ))as totalnum,(select referrer from Users where UserId=" + FromUserId + " )as refer  from  VmoneyDetailNew where type='返点' and UserId=" + FromUserId + "");
                   //if (int.Parse(dt.Rows[0][0].ToString()) <= 5000 && int.Parse(dt.Rows[0][1].ToString()) <= 500000)
                   //{
                   //    refer = dt.Rows[0][2].ToString();
                   //}
                }
            }



            if (Type == "好友注册")
            {
                string value = SqlManage1.One(" select id from VmoneyDetailNew where type='好友注册' and  ToUserId="+ToUserId);
                if(value!=null)
                    return "{\"Type\":\"-1\",\"Message\":\"超过上限\"}";
            }


            if (Type == "回收")
            {
                double Fromvmoney = GetVmoney(FromUserId);
                if ((Fromvmoney <= 0) || (Fromvmoney < Vmoney))
                {
                    return "{\"Type\":\"-1\",\"Message\":\"V币余额不足\",\"DetailId\":\"0\",\"NewMoney\":\"" + myvmoney + "\"}";
                }
            }


          

            Hashtable ht1 = new Hashtable();
            ht1["FromUserId"] = FromUserId;
            ht1["ToUserId"] = ToUserId;
            ht1["Vmoney"] = Vmoney;
            ht1["Type"] = Type;
            ht1["Description"] = Description;


            string sql1 = "",sql2="";
            string detailid = JsonHashtable1.GetNodeByKey(DALVmoneyDetailNew1.Add(ht1), "Id");
            if (detailid != null && !detailid.Equals(""))//插入操作成功
            {
                if ((Type == "群发邮件")||(Type == "发送邮件"))
                {
                    sql1 = "update users set Vmoney=Vmoney-" + Vmoney + " where id="+UserId;
                    myvmoney = myvmoney - Vmoney;

                    if (Type == "群发邮件")
                    {
                        if (refer != null && refer != "" && pnum < PMaxReV && totalnum < TMaxReV)
                        {
                            double ReV = Vmoney;//返点数
                            //若当前返点数量超出个人最大返点数量
                            if (pnum + Vmoney > PMaxReV)
                            {
                                ReV = PMaxReV - pnum;
                            }
                            //若当前返点数量超出返点人最大返点数量
                            if (totalnum + ReV > TMaxReV)
                            {
                                ReV = TMaxReV - totalnum;
                            }

                            Hashtable ht2 = new Hashtable();
                            ht2["FromUserId"] = FromUserId;
                            ht2["ToUserId"] = refer;
                            ht2["Vmoney"] = ReV;
                            ht2["Type"] = "返点";
                            ht2["Description"] = Description;
                            DALVmoneyDetailNew1.Add(ht2);

                            SqlManage1.Upd("   update users set Vmoney=Vmoney+" + ReV + "  where id=" + refer);
                            //SqlManage1.Upd("   update users set Vmoney=Vmoney+(select case when " + Vmoney + "+isnull(SUM(Vmoney),0)>5000 then 5000-isnull(SUM(Vmoney),0)else " + Vmoney + "  end from  VmoneyDetailNew where type='返点' and UserId=" + FromUserId + ")  where id=" + refer);


                        }

                    }


                }
                if ((Type == "充值") || (Type == "注册赠送") || (Type == "好友注册"))
                {
                    sql1 = "update users set Vmoney=Vmoney+" + Vmoney + " where id="+UserId;
                    myvmoney = myvmoney + Vmoney;

                }
                if ((Type == "分发")||(Type == "回收"))
                {
                    sql1 = "update users set Vmoney=Vmoney-" + Vmoney + " where id=" + FromUserId;
                    sql2 = "update users set Vmoney=Vmoney+" + Vmoney + " where id=" + ToUserId;
                    myvmoney = myvmoney - Vmoney;
                }

                if ((Type == "内部充值") )
                {
                    sql1 = "update users set Vmoney=Vmoney+" + Vmoney + " where id=" + UserId;
                    myvmoney = myvmoney + Vmoney;
                }
                //if (Type == "回收")
                //{
                //    sql1 = "update users set Vmoney=Vmoney+" + Vmoney + " where id=" + FromUserId;
                //    sql2 = "update users set Vmoney=Vmoney-" + Vmoney + " where id=" + ToUserId;
                //    myvmoney = myvmoney + Vmoney;
                //}

                SqlManage1.Upd(sql1);
                if (sql2 != "")
                {
                    SqlManage1.Upd(sql2);
                }

                return "{\"Type\":\"0\",\"Message\":\"操作成功\",\"DetailId\":\"" + detailid + "\",\"NewMoney\":\"" + myvmoney + "\"}";
            }
            else
            {
                return "{\"Type\":\"-1\",\"Message\":\"操作失败\",\"DetailId\":\"0\",\"NewMoney\":\"0\"}";
            }
            return "{\"Type\":\"-1\",\"Message\":\"操作失败\",\"DetailId\":\"0\",\"NewMoney\":\"0\"}";
        }





        /// <summary>
        /// V币充值方法
        /// </summary>
        /// <param name="deductvmoney">扣除V币数</param>
        /// <param name="userid">用户Id</param>
        /// <param name="remarks">备注</param>
        /// <returns></returns>
        public void DeductVmoneyZFBCZ(int userid, int ParentdId, double Vmoney, string Description)
        {
            //Description已经做了安全处理
            SqlManage1.Ins("insert into VmoneyDetailNew (UserId,ParentId,Type,Vmoney,FromUserId,ToUserId,Description)values(" + userid + "," + ParentdId + ",'充值'," + Vmoney + ",'0'," + userid + ",'" + Description + "')");
            SqlManage1.Upd("update users set Vmoney=Vmoney+" + Vmoney + " where id=" + userid);
        }



        /// <summary>
        /// 获取当前登录用户的剩余V币数
        /// </summary>
        /// <returns></returns>
        public double GetVmoney()
        {
            string a = Helper1.One("users","Vmoney",base.UserId);
            return Convert.ToDouble(a);
        }
        
       /// <summary>
        /// 获取传入的用户的剩余V币数
        /// </summary>
        /// <returns></returns>
        public double GetVmoney(int UserId1)
        {
            //string a = Helper1.One("users", "Vmoney", UserId1);
            //return Convert.ToDouble(a);

            string a = SqlManage1.One("select Vmoney from users where id=" + UserId1 + " and parentid=" + ParentId);
            return Convert.ToDouble(a);
        }

    }
}