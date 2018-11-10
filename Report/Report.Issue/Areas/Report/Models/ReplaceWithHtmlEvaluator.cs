using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Report.Issue.Areas.Report.Models
{
    public class ReplaceWithHtmlEvaluator : IReplacingCallback
    {
        private readonly string _value;

        public ReplaceWithHtmlEvaluator(string value)
        {
            _value = value;
        }

        ReplaceAction IReplacingCallback.Replacing(ReplacingArgs e)
        {
            DocumentBuilder builder = new DocumentBuilder((Document)e.MatchNode.Document);
            builder.MoveTo(e.MatchNode);
            builder.InsertHtml(_value);
            e.Replacement = "";
            return ReplaceAction.Replace;
        }
    }
}