using Amazon.DynamoDBv2.DataModel;

namespace APIHomeworkBW.Models
{
    [DynamoDBTable("APIHomeworkBWHackathonDB")]
    public class Assignment
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
        public float? Size { get; set; }

        // from 0 to 100: percent completed
        [DynamoDBProperty("percent_done")]
        public int? PercentDone { get; set; }

    }
}
