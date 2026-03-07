using Bibliography.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bibliography.Lib.Formatters
{
    public class ChicagoBiblioFormatter : IBibliographyStyleFormatter
    {
        public string FormatBibliography(IEnumerable<BibliographyEntry> entries)
        {
            if (entries == null) throw new ArgumentNullException(nameof(entries));
            if (!entries.Any()) return "No entries provided.";

            var sortedEntries = entries
                .OrderBy(e => e.Contributors.FirstOrDefault(c => c.Role == ContributorRole.Author)?.LastName ?? e.Title)
                .ToList();

            var sb = new StringBuilder();

            for (int i = 0; i < sortedEntries.Count(); i++)
            {
                var entry = sortedEntries.ElementAt(i);
                string formattedEntry = FormatBibliographyEntry(entry);
                sb.AppendLine(formattedEntry);
                sb.AppendLine();
            }

            return sb.ToString().TrimEnd();
        }

        private string FormatBibliographyEntry(BibliographyEntry entry)
        {
            return entry.SourceType switch
            {
                SourceType.Book => FormatBook(entry),
                SourceType.Journal => FormatJournalArticle(entry),
                SourceType.Report => FormatTechnicalReport(entry),
                SourceType.Website => FormatWebsite(entry),
                _ => FormatBook(entry) // Default fallback
            };
        }

        #region Format Methods by Source Type

        private string FormatBook(BibliographyEntry entry)
        {
            var authors = entry.Contributors
                .Where(c => c.Role == ContributorRole.Author)
                .ToList();
            var authorString = FormatAuthors(authors);
            return $"{authorString} {entry.Title}. {entry.Publisher}, {entry.PublicationDate?.Year}.";
        }

        private string FormatJournalArticle(BibliographyEntry entry)
        {
            // Chicago style for journal articles:
            // Author(s). "Article Title." Journal Name, Volume(Issue): Pages, Year.
            var authors = entry.Contributors
                .Where(c => c.Role == ContributorRole.Author)
                .ToList();
            var authorString = FormatAuthors(authors);
            
            var volume = entry.Volume ?? "";
            var issue = entry.Issue ?? "";
            var volumeIssue = !string.IsNullOrEmpty(volume) 
                ? !string.IsNullOrEmpty(issue) 
                    ? $"{volume}({issue})" 
                    : $"{volume}"
                : "";
            
            var pages = !string.IsNullOrEmpty(entry.Pages) ? $": {entry.Pages}" : "";
            var containerTitle = entry.ContainerTitle ?? "Unknown Journal";
            var year = entry.PublicationDate?.Year==null ? "n.d.":entry.PublicationDate?.Year.ToString();

            return $"{authorString} \"{entry.Title}.\" {containerTitle}, {volumeIssue}{pages}, {year}.";
        }

        private string FormatTechnicalReport(BibliographyEntry entry)
        {
            // Chicago style for technical reports:
            // Author(s). "Report Title." Report Type, Report Number, Organization, Date.
            var authors = entry.Contributors
                .Where(c => c.Role == ContributorRole.Author)
                .ToList();
            var authorString = FormatAuthors(authors);
            
            var reportNumber = !string.IsNullOrEmpty(entry.Issue) ? $", {entry.Issue}" : "";
            var organization = entry.Publisher ?? "Unknown Organization";
            var year = entry.PublicationDate?.Year ==null? "n.d.":entry.PublicationDate?.Year.ToString();

            return $"{authorString} \"{entry.Title}.\" Technical Report {reportNumber}, {organization}, {year}.";
        }

        private string FormatWebsite(BibliographyEntry entry)
        {
            // Chicago style for websites:
            // Author(s). "Page Title." Website Name. Accessed Month Day, Year. URL.
            var authors = entry.Contributors
                .Where(c => c.Role == ContributorRole.Author)
                .ToList();
            var authorString = authors.Count > 0 ? FormatAuthors(authors) : "Unknown Author";
            
            var websiteName = entry.ContainerTitle ?? entry.Publisher ?? "Unknown Website";
            var accessDate = entry.AccessDate.HasValue 
                ? $" Accessed {entry.AccessDate.Value:MMMM d, yyyy}." 
                : "";
            var url = !string.IsNullOrEmpty(entry.Url) ? $" {entry.Url}" : "";

            return $"{authorString} \"{entry.Title}.\" {websiteName}.{accessDate}{url}";
        }

        #endregion

        #region Helper Methods

        private string FormatAuthors(List<Contributor> authors)
        {
            if (authors.Count == 0) return "Unknown Author";

            if (authors.Count >= 7)
            {
                var firstSeven = authors.Take(7)
                    .Select((a, i) => i == 0 ? $"{a.LastName}, {a.FirstName}" : $"{a.FirstName} {a.LastName}")
                    .ToList();
                return string.Join(", ", firstSeven) + ", et al.";
            }

            if (authors.Count == 1)
                return $"{authors[0].LastName}, {authors[0].FirstName}.";

            var result = new List<string> { $"{authors[0].LastName}, {authors[0].FirstName}" };
            
            for (int i = 1; i < authors.Count - 1; i++)
            {
                result.Add($"{authors[i].FirstName} {authors[i].LastName}");
            }

            return string.Join(", ", result) + ", and " + $"{authors.Last().FirstName} {authors.Last().LastName}.";
        }

        #endregion
    }
}
