// <copyright file="ZoombracoMultiLingualAnalyzer.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Search
{
    using System.Collections;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.AR;
    using Lucene.Net.Analysis.BR;
    using Lucene.Net.Analysis.CJK;
    using Lucene.Net.Analysis.Cn;
    using Lucene.Net.Analysis.Cz;
    using Lucene.Net.Analysis.De;
    using Lucene.Net.Analysis.Fr;
    using Lucene.Net.Analysis.Nl;
    using Lucene.Net.Analysis.Ru;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Util;

    /// <summary>
    /// This analyzer is configured to use culturally specific analyzers per given content fields.
    /// </summary>
    public class ZoombracoMultiLingualAnalyzer : PerFieldAnalyzerWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoombracoMultiLingualAnalyzer"/> class.
        /// </summary>
        public ZoombracoMultiLingualAnalyzer()
            : this(new StandardAnalyzer(Version.LUCENE_29))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoombracoMultiLingualAnalyzer"/> class.
        /// </summary>
        /// <param name="defaultAnalyzer">Any fields not specifically
        /// defined to use a different analyzer will use the one provided here.
        /// </param>
        public ZoombracoMultiLingualAnalyzer(Analyzer defaultAnalyzer)
            : this(defaultAnalyzer, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoombracoMultiLingualAnalyzer"/> class.
        /// </summary>
        /// <param name="defaultAnalyzer">Any fields not specifically
        /// defined to use a different analyzer will use the one provided here.
        /// </param>
        /// <param name="fieldAnalyzers">a Map (String field name to the Analyzer) to be
        /// used for those fields
        /// </param>
        public ZoombracoMultiLingualAnalyzer(Analyzer defaultAnalyzer, IDictionary fieldAnalyzers)
            : base(defaultAnalyzer, fieldAnalyzers)
        {
            this.AddAnalyzers();
        }

        /// <summary>
        /// Adds the known specific analyzers to this analyzer
        /// </summary>
        private void AddAnalyzers()
        {
            // Arabic
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-001"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-AE"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-BH"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-DJ"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-DZ"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-EG"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-ER"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-IL"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-IQ"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-JO"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-KM"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-KW"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-LB"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-LY"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-MA"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-MR"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-OM"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-PS"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-QA"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-SA"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-SD"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-SO"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-SS"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-SY"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-TD"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-TN"), new ArabicAnalyzer(Version.LUCENE_29));
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ar-YE"), new ArabicAnalyzer(Version.LUCENE_29));

            // Czech
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "cs-CZ"), new CzechAnalyzer());

            // German
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "de"), new GermanAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "de-AT"), new GermanAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "de-BE"), new GermanAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "de-CH"), new GermanAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "de-DE"), new GermanAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "de-LI"), new GermanAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "de-LU"), new GermanAnalyzer());

            // French
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-029"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-BE"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-BF"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-BI"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-BJ"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-BL"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-CA"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-CD"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-CF"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-CG"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-CH"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-CI"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-CM"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-DJ"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-DZ"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-FR"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-GA"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-GF"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-GN"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-GP"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-GQ"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-HT"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-KM"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-LU"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-MA"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-MC"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-MF"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-MG"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-ML"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-MQ"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-MR"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-MU"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-NC"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-NE"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-PF"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-PM"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-RE"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-RW"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-SC"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-SN"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-SY"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-TD"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-TG"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-TN"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-VU"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-WF"), new FrenchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "fr-YT"), new FrenchAnalyzer());

            // Japan, Korea
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ja"), new CJKAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ja-JP"), new CJKAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ko"), new CJKAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ko-KP"), new CJKAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ko-KR"), new CJKAnalyzer());

            // Netherlands
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "nl"), new DutchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "nl-AW"), new DutchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "nl-BE"), new DutchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "nl-BQ"), new DutchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "nl-CW"), new DutchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "nl-NL"), new DutchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "nl-SR"), new DutchAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "nl-SX"), new DutchAnalyzer());

            // Brasil
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "pt-BR"), new BrazilianAnalyzer());

            // Russia
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ru"), new RussianAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ru-BY"), new RussianAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ru-KG"), new RussianAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ru-KZ"), new RussianAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ru-MD"), new RussianAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ru-RU"), new RussianAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "ru-UA"), new RussianAnalyzer());

            // China
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-CN"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-Hans"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-Hans-HK"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-Hans-MO"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-Hant"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-HK"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-MO"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-SG"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-TW"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-CHS"), new ChineseAnalyzer());
            this.AddAnalyzer(string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, "zh-CHT"), new ChineseAnalyzer());
        }
    }
}