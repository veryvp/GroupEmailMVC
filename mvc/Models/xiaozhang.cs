using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{

    public class xiaozhang
    {
        public int id { get; set; }

        [Display(Name = "特殊数字")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}在{2}位至{1}位之间", MinimumLength = 1)]
        public string username { get; set; }


        public int Discipline { get; set; }
        public string mobile { get; set; }
        public string head { get; set; }


        public string email { get; set; }

        public string sex { get; set; }
        public string address { get; set; }
        public DateTime addtime { get; set; }
    }


    public class QueryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}