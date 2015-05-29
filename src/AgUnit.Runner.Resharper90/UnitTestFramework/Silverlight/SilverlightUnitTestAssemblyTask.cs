namespace AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight
{
    using System;
    using System.Xml;

    using JetBrains.ReSharper.TaskRunnerFramework;

    [Serializable]
    public class SilverlightUnitTestAssemblyTask : RemoteTask, IEquatable<SilverlightUnitTestAssemblyTask>
    {
        private const string SilverlightPlatformVersionKey = "SilverlightPlatformVersion";
        private const string XapPathKey = "XapPath";
        private const string DllPathKey = "DllPath";

        public Version SilverlightPlatformVersion { get; private set; }
        public string XapPath { get; private set; }
        public string DllPath { get; private set; }

        public override bool IsMeaningfulTask
        {
            get { return false; }
        }

        public SilverlightUnitTestAssemblyTask(XmlElement element)
            : base(element)
        {
            var silverlightPlatformVersion = GetXmlAttribute(element, SilverlightPlatformVersionKey);
            this.SilverlightPlatformVersion = !string.IsNullOrEmpty(silverlightPlatformVersion) ? new Version(silverlightPlatformVersion) : null;
            this.XapPath = GetXmlAttribute(element, XapPathKey);
            this.DllPath = GetXmlAttribute(element, DllPathKey);
        }

        public SilverlightUnitTestAssemblyTask(Version silverlightPlatformVersion, string xapPath, string dllPath)
            : base(SilverlightUnitTestProvider.RunnerId)
        {
            this.SilverlightPlatformVersion = silverlightPlatformVersion;
            this.XapPath = xapPath;
            this.DllPath = dllPath;
        }

        public override void SaveXml(XmlElement element)
        {
            base.SaveXml(element);
            SetXmlAttribute(element, SilverlightPlatformVersionKey, (this.SilverlightPlatformVersion ?? (object)string.Empty).ToString());
            SetXmlAttribute(element, XapPathKey, this.XapPath);
            SetXmlAttribute(element, DllPathKey, this.DllPath);
        }

        #region Equals

        public bool Equals(SilverlightUnitTestAssemblyTask other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.SilverlightPlatformVersion, this.SilverlightPlatformVersion) && Equals(other.XapPath, this.XapPath) && Equals(other.DllPath, this.DllPath);
        }

        public override bool Equals(RemoteTask other)
        {
            return this.Equals(other as object);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return this.Equals(obj as SilverlightUnitTestAssemblyTask);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 0;
                result = (result * 397) ^ (this.SilverlightPlatformVersion != null ? this.SilverlightPlatformVersion.GetHashCode() : 0);
                result = (result * 397) ^ (this.XapPath != null ? this.XapPath.GetHashCode() : 0);
                result = (result * 397) ^ (this.DllPath != null ? this.DllPath.GetHashCode() : 0);
                return result;
            }
        }

        #endregion
    }
}