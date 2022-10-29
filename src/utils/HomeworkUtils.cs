using APIHomeworkBW.Models;
using System.Globalization;

namespace APIHomeworkBW.Utils;

public class HomeworkUtils
{
    public static bool ValidateAssignmentRequest(Assignment assignmentRequest)
    {
        if (String.IsNullOrEmpty(assignmentRequest.Course) || String.IsNullOrEmpty(assignmentRequest.Name))
            return false;
        
        return true;
    }

    public static bool ValidateCreateAssignmentRequest(Assignment assignmentRequest)
    {
        if (!ValidateAssignmentRequest(assignmentRequest)) return false;
        if (assignmentRequest.DueDate == null) return false;

        try
		{
            var dateString = assignmentRequest.DueDate;
			DateTime date = GetExactDueDate(dateString);
            if (date < DateTime.Now) {
                return false;
            }
		}
		catch
		{
            return false;
		}
        return true;
    }

    public static DateTime GetExactDueDate(string dateString) {
        var parts = dateString.Split('/');
        dateString = parts[0].PadLeft(2, '0') + parts[1].PadLeft(2, '0') + parts[2];
		var date = DateTime.ParseExact(dateString, "MMddyyyy", CultureInfo.InvariantCulture);
        var delta = new TimeSpan(0, 23, 59, 0);
		date = date.Add(delta);
        return date;
    }
}
