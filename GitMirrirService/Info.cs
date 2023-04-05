namespace GitMirrorService
{
    public class Info
    {
        public string DestinationUrl { get; set; }
        public string SourceUrl { get; set; }
        public string SaveDirectory { get; set; }
        public int WaitForDownload { get; set; }
    }
}
