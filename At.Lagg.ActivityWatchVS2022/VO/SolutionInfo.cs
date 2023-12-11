namespace At.Lagg.ActivityWatchVS2022.VO
{
    public record struct SolutionInfo(
        string ActiveConfiguration,
        string BaseName,
        string Directory,
        string FileName,
        string Path
    );
}