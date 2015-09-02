namespace Sporty.ViewModel.Reports
{
    public class DurationPerSportType
    {
        public string SportTypeName { get; private set; }
        public int Duration { get; set; }

        public DurationPerSportType(string sportTypeName)
        {
            SportTypeName = sportTypeName;
        }
    }
}