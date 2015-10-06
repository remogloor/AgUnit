namespace AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.TaskRunnerFramework;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.ReSharper.UnitTestFramework.Strategy;

    public class SilverlightUnitTestElement : IUnitTestElement
    {
        private readonly IUnitTestRunStrategy runStrategy;

        public SilverlightUnitTestElement(UnitTestElementId id, IUnitTestRunStrategy runStrategy)
        {
            this.runStrategy = runStrategy;
            this.Id = id;
            this.Children = new List<IUnitTestElement>();
        }

        public IList<UnitTestTask> GetTaskSequence(ICollection<IUnitTestElement> explicitElements, IUnitTestRun run)
        {
            return new List<UnitTestTask>();
        }

        public UnitTestElementId Id
        {
            get;
            private set;
        }

        public string Kind
        {
            get { return "Silverlight tests"; }
        }

        public IEnumerable<UnitTestElementCategory> Categories
        {
            get { return UnitTestElementCategory.Uncategorized; }
        }

        public string ExplicitReason
        {
            get { return null; }
        }

        private IUnitTestElement parent;
        public IUnitTestElement Parent
        {
            get { return this.parent; }
            set
            {
                if (!Equals(this.parent, value))
                {
                    if (this.parent != null)
                        this.parent.Children.Remove(this);

                    if (value != null)
                        value.Children.Add(this);

                    this.parent = value;
                }
            }
        }

        public ICollection<IUnitTestElement> Children { get; private set; }

        public string ShortName
        {
            get { return this.Id; }
        }

        public bool Explicit
        {
            get { return false; }
        }

        public UnitTestElementState State { get; set; }

        public bool Equals(IUnitTestElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!(other is SilverlightUnitTestElement)) return false;
            return Equals(other.Id, this.Id);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SilverlightUnitTestElement)) return false;
            return this.Equals((SilverlightUnitTestElement)obj);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public IProject GetProject()
        {
            return null;
        }

        public string GetPresentation(IUnitTestElement parent = null, bool full = false)
        {
            return this.Id;
        }

        public UnitTestElementNamespace GetNamespace()
        {
            return null;
        }

        public UnitTestElementDisposition GetDisposition()
        {
            return UnitTestElementDisposition.InvalidDisposition;
        }

        public IDeclaredElement GetDeclaredElement()
        {
            return null;
        }

        public IEnumerable<IProjectFile> GetProjectFiles()
        {
            return new List<IProjectFile>();
        }

        public IUnitTestRunStrategy GetRunStrategy(IHostProvider hostProvider)
        {
            return this.runStrategy;
        }
    }
}