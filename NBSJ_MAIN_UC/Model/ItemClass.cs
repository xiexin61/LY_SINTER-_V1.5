using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NBSJ_MAIN_UC.Model
{
    public class ItemClass
    {
        string keys;

        public string Keys
        {
            get { return keys; }
            set { keys = value; }
        }
        int value;

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        double dbValue;
        public double DbValue
        {
            get { return this.dbValue; }
            set { this.dbValue = value; }
        }


        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private object tag;

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public string Color
        {
            get
            {
                if (string.IsNullOrEmpty(color))
                {
                    return "#FFFFFF";
                }
                return color;
            }

            set
            {
                color = value;
            }
        }

        private string color = "#FFFFFF";

    }

}
