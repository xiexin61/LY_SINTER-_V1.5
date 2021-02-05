using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSJ_MAIN_UC.Model
{
    public class LanguageConfig
    {
        private static LanaugeEnum lanaugeFlag = LanaugeEnum.CN;
        /// <summary>
        /// app语言
        /// </summary>
        public static LanaugeEnum LanaugeFlag
        {
            get
            {
                return lanaugeFlag;
            }

            set
            {
                lanaugeFlag = value;
            }
        }
    }
    public enum LanaugeEnum
    {
        /// <summary>
        /// 中文
        /// </summary>
        CN,
        /// <summary>
        /// 英文
        /// </summary>
        EN
    }
    public  class CONFIG_GRIDVIEWCOLS
    {
        public String NAME { get; set; }
        public int COLUMNINDEX { get; set; }
        public String FEILDNAME { get; set; }
        /// <summary>
        /// 处理字段
        /// </summary>
        public String DEALFEILD { get; set; }
        private LanaugeEnum lanaugeFlag = LanaugeEnum.CN;
        public LanaugeEnum LanaugeFlag
        {
            get
            {

                return LanguageConfig.LanaugeFlag;
            }

            set
            {
                lanaugeFlag = value;
            }
        }
        public string DESC_FLAG
        {

            get
            {
                if (this.LanaugeFlag == LanaugeEnum.CN)
                {
                    return DESC_CN;
                }
                else
                {
                    return DESC_EN;
                }
            }
            set
            {
                if (this.LanaugeFlag == LanaugeEnum.CN)
                {
                    DESC_CN = value;
                }
                else
                {
                    DESC_EN = value;
                }
            }
        }
        public String DESC_CN { get; set; }
        public String DESC_EN { get; set; }
        public int FORMATTYPE { get; set; }
        public string FORMATSTRING_FLAG
        {

            get
            {
                if (this.LanaugeFlag == LanaugeEnum.CN)
                {
                    return FORMATSTRING;
                }
                else
                {
                    return FORMATSTRING_EN;
                }
            }
            set
            {
                if (this.LanaugeFlag == LanaugeEnum.CN)
                {
                    FORMATSTRING = value;
                }
                else
                {
                    FORMATSTRING_EN = value;
                }
            }
        }
        public String FORMATSTRING { get; set; }
        public String FORMATSTRING_EN { get; set; }
        public bool ISFIXEDWIDTH { get; set; }
        public Double FIXEDWIDTH { get; set; }
        public bool READONLY { get; set; }
        public bool VISABLE { get; set; }
        /// <summary>
        /// 是否是分组列
        /// </summary>
        public int IsGroupColumn { get; set; }
    }
}
