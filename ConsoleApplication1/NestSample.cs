using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Nest;


namespace DecodeSample
{
    class NestSample
    {

        static void DebugPrint(string s)
        {
            Debug.WriteLine(s);
        }

        static void Main(string[] args)
        {
            var indexName = "sample_index";
            var typeName = "document";
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node)
                .DefaultIndex(indexName)
                .PrettyJson();
            var client = new ElasticClient(settings);

            //indexing
            var doc2 = new SampleDocument
            {
                Id = 2,
                Title = "はじめてのElasticsearch",
                Body = "NESTで始める、はじめてのElasticsearchクライアントアプリケーション"
            };
            client.Index<SampleDocument>(doc2, idx => idx.Index(indexName).Type(typeName));

            DebugPrint("--- indexing sample doc2");

            //Searching
            var searchDescriptor = new SearchDescriptor<SampleDocument>()
                .Query(q => q.MatchAll())
                .Index(indexName)
                .Type(typeName);
            DebugPrint("--- match_all start -------------------");

            var response = client.Search<SampleDocument>(searchDescriptor);
            DebugPrint("Hits : [" + response.Total + "]");
            DebugPrint("Docs : [" + response.Documents.Count() + "]");
            foreach (var doc in response.Documents)
            {
                DebugPrint(doc.ToString());
            }
            DebugPrint("--- match_all end -------------------");

            DebugPrint("---  query start ------------------ - ");
            searchDescriptor.Query(
                q => q.QueryString(
                    qs => qs.Query(
                        "body:NEST"
                    )
                )
            );
            response = client.Search<SampleDocument>(searchDescriptor);
            DebugPrint("Hits : [" + response.Total + "]");
            DebugPrint("Docs : [" + response.Documents.Count() + "]");
            foreach (var doc in response.Documents)
            {
                DebugPrint(doc.ToString());
            }
            DebugPrint("--- query_string query end -------------------");


        }
    }

    class SampleDocument
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public override string ToString()
        {
            return base.ToString() + "{ id : " + Id + ", title : " + Title + ", body : " + Body + " }";
        }

    }
}
