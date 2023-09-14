using Microsoft.AspNetCore.Mvc;
using API.Models;
using Octokit;
using System.Reactive.Linq; 


namespace API.Controllers;
public class SearchResultController : Controller
{

    public async Task<IActionResult> Result(string owner, string repoName)
    {

        if (string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(repoName))
        {
            return RedirectToAction("Error", new { message = "Invalid parameters: Owner and Repository Name must be provided." });
        }

        ViewData["repoName"] = repoName;

        // Introduce a 2-second delay without blocking the server
        await Task.Delay(2000);

        try
        {
            // Call the GetIssues method with the provided owner and repoName
            var issueViewModel = await GetIssues(owner, repoName);

            // Pass the issueViewModel to the view
            return View(issueViewModel);
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur
            return StatusCode(500, "Internal Server Error: " + ex.Message);
        }
    }

    public async Task<List<IssueViewModel>> GetIssues(string owner, string repoName)
    {
        try
        {
            var client = new GitHubClient(new ProductHeaderValue("Github-Searcher"));

            // Authenticate with a Personal Access Token for more api requests
            //var tokenAuth = new Credentials("YOUR_PERSONAL_ACCESS_TOKEN"); // Replace with your PAT
            //client.Credentials = tokenAuth;

            User githubUser = await client.User.Get(owner);

            // Construct the repository reference
            Repository repository = await client.Repository.Get(owner, repoName);

            // Get the list of issues for the given repository
            IReadOnlyList<Issue> issues = await client.Issue.GetAllForRepository(repository.Id);

            // Transform the issues into IssueViewModel objects
            var issueViewModel = issues.Select(issue => new IssueViewModel
            {
                Number = issue.Number,
                Title = issue.Title,
                CreationDate = issue.CreatedAt.DateTime,
                Url = issue.HtmlUrl,
                AuthorUsername = issue.User?.Login ?? "[deleted]",
                AuthorProfileUrl = issue.User?.HtmlUrl ?? null,
                AuthorAvatarUrl = issue.User?.AvatarUrl ?? "/wwwroot/user.png"
            }).ToList();
            return issueViewModel;
        }
        catch (ApiException ex)
        {
            // Handle any GitHub API exceptions here
            throw new Exception("Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            // Handle any other exceptions that may occur
            throw new Exception("Internal Server Error: " + ex.Message);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string message)
    {
        ViewData["ErrorMessage"] = message;
        return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
