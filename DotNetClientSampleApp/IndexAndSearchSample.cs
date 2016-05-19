using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Elasticsearch.Net;

namespace DecodeSample
{
    class IndexAndSearchSample
    {
        static void Main(string[] args)
        {
            var node = new Uri("http://localhost:9200");
            var config = new ConnectionConfiguration(node).PrettyJson();
            var client = new ElasticLowLevelClient(config);

            //Indexing
            var doc1 = new {
                id = 1,
                title = "はじめてのElasticsearch", 
                body = ".NETで始める、はじめてのElasticsearchクライアントアプリケーションの説明"
            };
            
            client.Index<String>("sample_index", "document", "1", doc1);
            client.IndicesFlush<String>("sample_index");

            //Searching
            var queryJson = @"{ ""query"" : { ""match_all"": {}} }";
            DebugPrint("--- match_all start -------------------");
            ElasticsearchResponse<String> res = client.Search<String>("sample_index", queryJson);

            DebugPrint(res.Body);
            DebugPrint("--- match_all end -------------------");

            DebugPrint("--- query_string query start -------------------");
            queryJson = @"{ ""query"" : { ""query_string"": { ""query"" : ""body:.NET"" } } }";

            res = client.Search<String>("sample_index", queryJson);
            DebugPrint(res.Body);
            DebugPrint("--- query_string query end -------------------");

        }

        static void DebugPrint(string s)
        {
            Debug.WriteLine(s);
        }
    }
}
