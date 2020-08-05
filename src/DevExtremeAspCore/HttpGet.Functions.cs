using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.DevExtremeAspCore
{
    public partial class HttpGet : ExpansionBase
    {
        private string _table;
        private void MainFunction()
        {
            _table = GetTable(Input);
            Output = Get();
        }


        private string Get()
        {
            BuildSnippet(null);
            BuildSnippet("[HttpGet]",8);
            BuildSnippet("public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {",8);
            BuildSnippet("    var "+ Pluralize(_table).ToLower()+ " = _context."+_table+".Select(i => new {",8);

            var columns = GetColumns(_table, false); //.Where( t=> t.IsPrimaryKey).ToList();
            var columnsCount = 0;
            foreach (var column in columns)
            {
                if (!General.DisplayColumn(column)) continue;

                columnsCount++;
                BuildSnippet("         i."+ column.ColumnName+Comma(columnsCount, columns.Count),8);
            }
            BuildSnippet("  });",8);
            BuildSnippet(" ",8);
            BuildSnippet("// If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.",8);
            BuildSnippet("// In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.",8);
            BuildSnippet("// Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.",8);
            BuildSnippet("// loadOptions.PrimaryKey = new[] { "+"Id".AddQuotes()+" };",8);
            BuildSnippet("// loadOptions.PaginateViaPrimaryKey = true;",8);
            BuildSnippet(" ",8);
            BuildSnippet("  return Json(await DataSourceLoader.LoadAsync("+Pluralize(_table).ToLower()+", loadOptions));",8);
            BuildSnippet("}",8);
            return BuildSnippet();
        }

        private static string Comma(int current, int count) => current < count ? "," : string.Empty;


    }
}


