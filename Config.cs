using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustHintShow
{
    public class Config : IConfig
    {
        [Description("是否启动CustHintShow(CHS)")]
        public bool IsEnabled {  get; set; } = true;
        public bool Debug { get; set; } = true;
    }
}
