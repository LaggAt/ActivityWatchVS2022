using At.Lagg.ActivityWatchVS2022.VO;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.ProjectSystem.Query;
using StreamJsonRpc;

namespace At.Lagg.ActivityWatchVS2022.Services
{
    public class SolutionInfoService : ExtensionPart
    {
        #region CTor

        public SolutionInfoService(ExtensionCore container, VisualStudioExtensibility extensibility)
            : base(container, extensibility)
        {
        }

        #endregion CTor

        #region Methods

        public async Task<SolutionInfo?> GetSolutionInfoAsync()
        {
            ISolutionSnapshot? solution = null;
            try
            {
                solution = (
                    await this.Extensibility.Workspaces().QuerySolutionAsync<ISolutionSnapshot>(
                        query => query
                            .With(s => s.ActiveConfiguration)
                            .With(s => s.BaseName)
                            .With(s => s.Directory)
                            .With(s => s.FileName)
                            .With(s => s.Path)
                        ,
                        default
                    ).ConfigureAwait(false)
                ).FirstOrDefault();
            }
            catch (ConnectionLostException)
            {
                // no solution info is available (Visual Studio exited?)
            }
            if (solution == null)
            {
                return null;
            }
            return new SolutionInfo(solution.ActiveConfiguration, solution.BaseName, solution.Directory, solution.FileName, solution.Path);
        }

        #endregion Methods
    }
}