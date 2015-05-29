namespace AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform
{
    using System;
    using System.Linq;

    using JetBrains.Metadata.Reader.API;
    using JetBrains.ProjectModel;
    using JetBrains.VsIntegration.ProjectDocuments.Projects.Builder;
    
    public static class ProjectExtensions
    {
        private const string SilverlightMsTestAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTesting.Silverlight";

        public static string GetXapPath(this IProject silverlightProject)
        {
            try
            {
                var projectModelSynchronizer = silverlightProject.GetSolution().GetComponent<ProjectModelSynchronizer>();
                var vsProjectInfo = projectModelSynchronizer.GetProjectInfoByProject(silverlightProject);

                if (vsProjectInfo != null)
                {
                    var project = vsProjectInfo.GetExtProject();
                    var xapOutputs = (bool)project.Properties.Item("SilverlightProject.XapOutputs").Value;

                    if (xapOutputs)
                    {
                        var xapFileName = (string)project.Properties.Item("SilverlightProject.XapFilename").Value;
                        return silverlightProject.GetOutputDirectory().Combine(xapFileName).FullPath;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetDllPath(this IProject silverlightProject)
        {
            return silverlightProject.GetOutputFilePath().FullPath;
        }

        public static bool IsSilverlight(this IProject project)
        {
            return project.GetAssemblyReferences(project.TargetFrameworkIds.First()).Any(r => r.Name == SilverlightMsTestAssemblyName);
        }
    }
}