using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SessionUser
    {
        // Fields
        public int _groupid;
        private int _subclassid;
        public int _typeid;
        public string _username;
        public int _userId;
        private string _userno;
        // Properties
        private int _parentid;
        private int _companyid;
        private string _companyname;
        private string _email;
        private string _englishname;
        private string _nickname;
        private string _realname;
        private int _ownerid;
        private int _status;
        private string _HeadPortrait;

        /// <summary>
        /// 中文名
        /// </summary>
        public string Realname
        {
            get { return _realname; }
            set { _realname = value; }
        }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName
        {
            set { _nickname = value; }
            get { return _nickname; }
        }
        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName
        {
            set { _englishname = value; }
            get { return _englishname; }
        }
        /// <summary>
        /// 邮件
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }

        /// <summary>
        /// 所属公司名称
        /// </summary>
        public int CompanyId
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string CompanyName
        {
            set { _companyname = value; }
            get { return _companyname; }
        }
        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 拥有者
        /// </summary>
        public int OwnerId
        {
            set { _ownerid = value; }
            get { return _ownerid; }
        }

        public int UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                this._userId = value;
            }
        }

        public string UserNo
        {
            get { return _userno; }
            set { _userno = value; }
        }

        public int GroupId
        {
            get
            {
                return this._groupid;
            }
            set
            {
                this._groupid = value;
            }
        }

        public int SubClassId
        {
            get
            {
                return this._subclassid;
            }
            set
            {
                this._subclassid = value;
            }
        }

        public int TypeId
        {
            get
            {
                return this._typeid;
            }
            set
            {
                this._typeid = value;
            }
        }

        public string UserName
        {
            get
            {
                return this._username;
            }
            set
            {
                this._username = value;
            }
        }

        public int Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }


        public string HeadPortrait
        {
            get
            {
                return this._HeadPortrait;
            }
            set
            {
                this._HeadPortrait = value;
            }
        }


        
    }
}
