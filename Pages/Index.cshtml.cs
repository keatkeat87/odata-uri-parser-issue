using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;

namespace test_odata.Pages
{
    public class UserSettingGroup
    {
        public int Id { get; set; }
        public string name { get; set; } = "";
    }

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            ILogger<IndexModel> logger
        )
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<UserSettingGroup>("UserSettingGroups");
            var model = builder.GetEdmModel();

            var filterValue1 = "freeland"; // Wrong url = http://localhost:46151/api/UserSettingGroups?$filter=name eq 'freel and '
            //var filterValue1 = "freelanz"; // Correct url = http://localhost:46151/api/UserSettingGroups?$filter=name eq 'freelanz' and Id eq 1

            var parser1 = new ODataUriParser(model, new Uri($"http://localhost:46151/api"), new Uri($"http://localhost:46151/api/UserSettingGroups?$filter=name eq '{filterValue1}'"))
            {
                Resolver = new ODataUriResolver { EnableCaseInsensitive = true }
            };
            var parser2 = new ODataUriParser(model, new Uri($"http://localhost:46151/api"), new Uri("http://localhost:46151/api/UserSettingGroups?$filter=Id eq 1"))
            {
                Resolver = new ODataUriResolver { EnableCaseInsensitive = true }
            };
            var filter1 = parser1.ParseFilter();
            var filter2 = parser2.ParseFilter();
            var filter3 = new FilterClause(new BinaryOperatorNode(BinaryOperatorKind.And, filter1.Expression, filter2.Expression), filter1.RangeVariable);
            var uri = parser1.ParseUri();
            uri.Filter = filter3;
            var final = uri.BuildUri(ODataUrlKeyDelimiter.Parentheses);
        }
    }
}
