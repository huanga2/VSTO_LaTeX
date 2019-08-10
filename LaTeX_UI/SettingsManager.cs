using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
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
                    LoadSettings();
                }

                return settings;
            }
        }

        ~SettingsManager()
        {
            SaveSettings();
        }

        public void SaveSettings()
        {
            var folderPath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "VSTO_LaTeX");
            Directory.CreateDirectory(folderPath);

            var settingsFile = Path.Combine(folderPath, "settings.ini");

            using (FileStream fs = File.Create(settingsFile))
            {
                var ser = new DataContractJsonSerializer(typeof(SettingsClass));
                ser.WriteObject(fs, settings);
            }
        }

        public void LoadSettings()
        {
            var folderPath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "VSTO_LaTeX");
            var settingsFile = Path.Combine(folderPath, "settings.ini");

            using (FileStream fs = File.Open(settingsFile, FileMode.Open, FileAccess.Read))
            {
                var ser = new DataContractJsonSerializer(typeof(SettingsClass));

                settings = ser.ReadObject(fs) as SettingsClass;
            }
        }

        [DataContract]
        public class SettingsClass
        {
            [DataMember(Name ="DefaultImageType")]
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
