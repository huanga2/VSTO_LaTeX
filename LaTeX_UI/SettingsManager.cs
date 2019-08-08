using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LaTeX_UI
{
    public class SettingsManager
    {
        private static SettingsClass settings;

        public SettingsClass Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = new SettingsClass();
                }

                return settings;
            }
        }

        [DataContract]
        public class SettingsClass
        {
            [DataMember]
            private int _DefaultImageType;
            public int DefaultImageType
            {
                get { return _DefaultImageType; }
                set { _DefaultImageType = value; }
            }

            protected internal SettingsClass() { }
        }
    }
}
