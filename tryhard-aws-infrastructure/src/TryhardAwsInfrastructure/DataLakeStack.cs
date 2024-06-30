using Amazon.CDK;
using Amazon.CDK.AWS.S3;
using Constructs;

namespace TryhardAwsInfrastructure
{
    public class DataLakeStack : Stack
    {
        internal DataLakeStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            Bucket dataLakeBucket = new Bucket(this, "DataLakeBucket", new BucketProps()
            {
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
                RemovalPolicy = RemovalPolicy.RETAIN
            });
        }
    }
}