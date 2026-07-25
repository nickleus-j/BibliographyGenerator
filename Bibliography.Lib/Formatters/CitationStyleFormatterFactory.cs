using Bibliography.Lib.Models;

namespace Bibliography.Lib.Formatters;

public static class CitationStyleFormatterFactory
{
    private static readonly Dictionary<CitationStyle, IBibliographyStyleFormatter> Formatters =
        new Dictionary<CitationStyle, IBibliographyStyleFormatter>
        {
            { CitationStyle.APA, new ApaBiblioFormatter() },
            { CitationStyle.MLA, new MlaBiblioFormatter() },
            { CitationStyle.Chicago, new ChicagoBiblioFormatter() },
            { CitationStyle.Harvard, new HarvardBiblioFormatter() },
            { CitationStyle.IEEE, new IeeeBiblioFormatter() },
        };

    public static IBibliographyStyleFormatter GetFormatter(CitationStyle style)
    {
        if (Formatters.TryGetValue(style, out var formatter))
        {
            return formatter;
        }

        throw new ArgumentException($"Unsupported citation style: {style}");
    }
}