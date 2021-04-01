using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Serialization;
using Amazon.DynamoDBv2.DocumentModel;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Assignment7GetStatsForType
{
    public class ItemsToGet
    {
        public string type { get; set; }
        public double count { get; set; }
        public double averageRating { get; set; }
    }
    public class Function
    {

        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private string tableName = "RatingsByType";

        public async Task<ItemsToGet> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            string type = "";
            Dictionary<string, string> dict = (Dictionary<string, string>)input.QueryStringParameters;
            dict.TryGetValue("type", out type);
            GetItemResponse res = await client.GetItemAsync(tableName, new Dictionary<string, AttributeValue>
            {
                {"type", new AttributeValue {S = type} }
            });
            Document doc = Document.FromAttributeMap(res.Item);
            ItemsToGet myItems = JsonConvert.DeserializeObject<ItemsToGet>(doc.ToJson());
            return myItems;
        }

    }
}
