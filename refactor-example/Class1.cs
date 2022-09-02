using System;
using System.Text;

class HtmlUtil {
    public static string GetTestableHtml (PageData pageData, bool includeSuiteSetup) {
        WikiPage wikiPage = pageData.GetWikiPage();
        StringBuilder builder = new StringBuilder();

        if (pageData.HasAttribute("Test")) {
            if (includeSuiteSetup) {
                WikiPage suiteSetup = PageCrawlerImpl.GetInheritedPage(SuiteResponder.SUITE_SETUP_NAME, wikiPage);
                if (suiteSetup != null) {
                    WikiPagePath pagePath = suiteSetup.GetPageCrawler().GetFullPath(suiteSetup);
                    String pagePathName = PathParser.Render(pagePath);
                    builder.Append("!include -setup .").Append(pagePathName).Append('\n');
                }
            }
            
            WikiPage setup = PageCrawlerImpl.GetInheritedPage(SuiteResponder.SET_UP, wikiPage);
            if (setup != null) {
                WikiPagePath setupPath = wikiPage.GetPageCrawler().GetFullPath(setup);
                String setupPathName = PathParser.Render(setupPath);
                builder.Append("!include -setup .").Append(setupPathName).Append('\n');
            }
        }

        builder.Append(pageData.GetContent());
        if (pageData.HasAttribute("Test")) {
            WikiPage teardown = PageCrawlerImpl.GetInheritedPage(SuiteResponder.TEARDOWN, wikiPage);
            if (teardown != null) {
                WikiPagePath tearDownPath = wikiPage.GetPageCrawler().GetFullPath(teardown);
                String tearDownPathName = PathParser.Render(tearDownPath);
                builder.Append("\n").Append("!include -teardown .").Append(tearDownPathName).Append('\n');
            }
            
            if (includeSuiteSetup) {
                WikiPage suiteTeardown = PageCrawlerImpl.GetInheritedPage(SuiteResponder.SUITE_TEARDOWN_NAME, wikiPage);
                if (suiteTeardown != null) {
                    WikiPagePath pagePath = suiteTeardown.GetPageCrawler().GetFullPath(suiteTeardown);
                    String pagePathName = PathParser.Render(pagePath);
                    builder.Append("!include -teardown .").Append(pagePathName).Append("\n");
                }
            }
        }

        pageData.SetContent(builder.ToString());
        return pageData.GetHtml();
    }
}

#region BOILERPLATE
class PageCrawler
{
    public WikiPagePath GetFullPath(WikiPage wp) => new();
}

class WikiPage
{
    public PageCrawler GetPageCrawler() => new ();
}

abstract class PageData
{
    public abstract WikiPage GetWikiPage();
    public abstract bool HasAttribute(string attr);
    public abstract string GetContent();
    public abstract void SetContent(string content);
    public abstract string GetHtml();
}

static class PageCrawlerImpl
{
    public static WikiPage GetInheritedPage(SuiteResponder sr, WikiPage page) => new ();
}

public class WikiPagePath
{
    
}

enum SuiteResponder
{
    SUITE_SETUP_NAME,
    SET_UP,
    TEARDOWN,
    SUITE_TEARDOWN_NAME
}

static class PathParser
{
    public static string Render(WikiPagePath path) => "";
}
#endregion
