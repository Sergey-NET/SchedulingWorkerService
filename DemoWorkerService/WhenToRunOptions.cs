namespace DemoWorkerService
{
    public class WhenToRunOptions
    {
        public const string WhenToRun = "WhenToRun";

        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }
}