using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.ProjectSystem.Query.ProjectModel;
using Microsoft.VisualStudio.ProjectSystem.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using At.Lagg.ActivityWatchVS2022.VO;
using StreamJsonRpc;

namespace At.Lagg.ActivityWatchVS2022.Services
{
    public class SolutionInfoService : ExtensionPart
    {
        public SolutionInfoService(ExtensionCore container, VisualStudioExtensibility extensibility)
            : base(container, extensibility)
        {
        }

        public async Task<SolutionInfo?> GetSolutionInfoAsync()
        {
            ISolution? solution = null;
            try
            {
                solution = (
                    await this.Extensibility.Workspaces().QuerySolutionAsync<ISolution>(
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
    }
}
