namespace At.Lagg.ActivityWatchVS2022.VO
{
    public record struct VsEventInfo(
        DateTime UtcEventDateTime,
        string ChangedFile,
        string Caller,
        SolutionInfo? SolutionInfo
    );
}