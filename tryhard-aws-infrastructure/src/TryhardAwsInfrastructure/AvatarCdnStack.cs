using Amazon.CDK;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.CloudFront.Origins;
using Amazon.CDK.AWS.S3;
using Constructs;

namespace TryhardAwsInfrastructure
{
    public class AvatarCdnStack : Stack
    {
        internal AvatarCdnStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            Bucket avatarBucket = new Bucket(this, "AvatarBucket", new BucketProps()
            {
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
                RemovalPolicy = RemovalPolicy.RETAIN
            });
            OriginAccessIdentity avatarBucketOriginAccessIdentity = new OriginAccessIdentity(this, "AvatarBucketOriginAccessIdentity");
            Distribution avatarDistribution = new Distribution(this, "AvatarDistribution", new DistributionProps()
            {
                DefaultBehavior = new BehaviorOptions()
                {
                    Origin = new S3Origin(avatarBucket, new S3OriginProps()
                    {
                        OriginAccessIdentity = avatarBucketOriginAccessIdentity
                    })
                }
            });
        }
    }
}
