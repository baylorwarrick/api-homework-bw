using Amazon.DynamoDBv2.DataModel;
using APIHomeworkBW.Utils;

namespace APIHomeworkBW.Models
{
    [DynamoDBTable("APIHomeworkBWHackathonDB")]
    public class Assignment : IComparable<Assignment>
    {
        [DynamoDBHashKey("pk")]
        public string? Course { get; set; }

        [DynamoDBRangeKey("sk")]
        public string? Name { get; set; }

        // due date as "yyyy/mm/dd". Assumes 11:59pm
        [DynamoDBProperty("due_date")]
        public string? DueDate { get; set; }

        // total number of hours assignment will take to complete
        [DynamoDBProperty("size")]
        public double Size { get; set; } = 1;

        // from 0 to 100: percent completed
        [DynamoDBProperty("percent_done")]
        public int? PercentDone { get; set; }

        [DynamoDBIgnore]
        public double CurrentDensity
        {
            get
            {
                if (DueDate == null || PercentDone == null) return 0;
                var workLeft = (double) (Size * (100-PercentDone)/100);  // remaining work, in hours
                // time left before the due date
                var timeRemaining = HomeworkUtils.GetExactDueDate(DueDate) - DateTime.Now;
                var hoursRemaining = timeRemaining.TotalHours;
                return workLeft / hoursRemaining;
            }
        }

        public int CompareTo(Assignment other)
        {
            return CurrentDensity.CompareTo(other.CurrentDensity);
        }
    }
}
