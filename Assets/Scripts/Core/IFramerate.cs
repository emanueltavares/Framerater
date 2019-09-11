namespace Framerater.Core
{
    public interface IFramerate
    {
        float NumFrames { get; }
        float AverageDeltaTime { get; }
    }
}
