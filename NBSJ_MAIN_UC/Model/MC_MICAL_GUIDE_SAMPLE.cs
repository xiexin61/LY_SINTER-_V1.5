using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSJ_MAIN_UC.Model
{
    public class MC_MICAL_GUIDE_SAMPLE
    {

        /// <summary>
        /// Desc:换堆开始时间

        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? START_TIME { get; set; }

        /// <summary>
        /// Desc:指导取样时间1 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? SAMPLE_TIME_1 { get; set; }
        /// <summary>
        /// Desc:指导取样时间0 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? SAMPLE_TIME_0{ get; set; }

    }
}

