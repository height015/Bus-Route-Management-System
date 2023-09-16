using System.Net;

namespace Configuration;

public static class ApiResponse
{
    public static bool IsResponseValid(this HttpResponseMessage response, out string message)
    {
        try
        {
            if (response.StatusCode != HttpStatusCode.OK &&
                response.StatusCode != HttpStatusCode.Unauthorized &&
                response.StatusCode != HttpStatusCode.BadRequest &&
                response.StatusCode != HttpStatusCode.MethodNotAllowed &&
                response.StatusCode != HttpStatusCode.NotFound &&
                response.StatusCode != HttpStatusCode.InternalServerError)
            {
                message = "Unknow Request Status! This may be duw to Server Error";
                return false;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                using var respContent = response.Content;
                string json = respContent.ReadAsStringAsync().Result;

                message = json ?? "Invalid Request Source or Unauthorize User";
                return false;
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {

                message = "System Error! Invalid Request Point";
                return false;
            }
            if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
            {

                message = "System Error! Invalid Request End";
                return false;
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {

                message = "Remote Server Error! Please Try Again Later";
                return false;
            }

            message = "";
            return true;
        }

        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.Message);
            message = "Service Respose Error! Please Try again";
            return false;

        }
    }


    public static string ToUtilString(this object obj)
    {
        if (obj == null)
            return "";
        try
        {
            string str = obj.ToString();
            return string.IsNullOrEmpty(str) ? "" : str.Replace("_", " ");
        }
        catch (Exception ex)
        {
            ErrorUtilTools.LogErr(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            return "";
        }
    }

}

public class ApiStatusResponse
{

    public ApiStatusResponse(HttpStatusCode StatusCode, string message = null)
    {
        Status = StatusCode;
        Message = message ?? GetDefaultStatusMessage(StatusCode);
    }

    public HttpStatusCode Status { get; set; }
    public string Message { get; set; }

    private string GetDefaultStatusMessage(HttpStatusCode Status)
    {

        return Status switch
        {
            HttpStatusCode.Unauthorized => "Invalid Source or Unauthorize User",
            HttpStatusCode.NotFound => "System Error! Invalid Request Point",
            HttpStatusCode.MethodNotAllowed => "System Error! Invalid Request End",
            HttpStatusCode.InternalServerError => "Remote Server Error! Please Try Again Late",
            HttpStatusCode.BadRequest => "Bad Request",
            HttpStatusCode.OK => "Success",
            HttpStatusCode.NoContent => "Empty Result",
            HttpStatusCode.Created => "Created",
            _ => string.Empty

        };

    }
}


public class ResponseResObj
{
    public int Id { get; set; }
    public bool IsSuccessful { get; set; }
    public string Error { get; set; }

}
