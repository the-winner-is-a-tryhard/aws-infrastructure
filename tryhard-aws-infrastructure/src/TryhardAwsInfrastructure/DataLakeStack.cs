using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Constructs;

namespace TryhardAwsInfrastructure
{
    public class DataLakeStack : Stack
    {
        internal DataLakeStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // primary bucket
            Bucket dataLakeBucket = new Bucket(this, "DataLakeBucket", new BucketProps()
            {
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
                RemovalPolicy = RemovalPolicy.RETAIN
            });
            
            // recurring job to load NFL player data from Sleeper
            Role nflPlayerFunctionExecutionRole = new Role(this, "NFLPlayerLoadFunctionExecutionRole", new RoleProps {
                AssumedBy = new ServicePrincipal("lambda.amazonaws.com"),
                ManagedPolicies = new IManagedPolicy[]
                {
                    new ManagedPolicy(this, "NFLPlayerLoadS3Policy", new ManagedPolicyProps()
                    {
                        Document = new PolicyDocument(new PolicyDocumentProps()
                        {
                            Statements = new []
                            {
                                new PolicyStatement(new PolicyStatementProps()
                                {
                                    Actions = new [] { "s3:*" },
                                    Resources = new [] { dataLakeBucket.BucketArn, dataLakeBucket.ArnForObjects("*") }
                                })
                            }
                        })
                    }),
                    ManagedPolicy.FromAwsManagedPolicyName("AWSLambdaBasicExecutionRole"),  
                }
            });
            DockerImageCode nflPlayerLoadPythonCode = DockerImageCode.FromImageAsset("src/TryhardAwsInfrastructure/lambda/nfl-player-load");
            DockerImageFunction nflPlayerLoadFunction = new DockerImageFunction(this, "NFLPlayerLoadFunction",
                new DockerImageFunctionProps()
                {
                    Architecture = Architecture.X86_64,
                    Code = nflPlayerLoadPythonCode,
                    Description = "Lambda function for loading NFL player data from Sleeper",
                    Environment = new Dictionary<string, string>
                    {
                        {
                            "DATALAKE_S3_BUCKET_NAME", dataLakeBucket.BucketName
                        }
                    },
                    MemorySize = 1024,
                    Role = nflPlayerFunctionExecutionRole,
                    Timeout = Duration.Seconds(900) 
                });
            Rule nflPlayerLoadSchedule = new Rule(this, "NFLPlayerLoadSchedule", new RuleProps()
            {
                Schedule = Schedule.Cron(new CronOptions()
                {
                    Day = "*",
                    Hour = "0",
                    Minute = "0",
                })
            });
            nflPlayerLoadSchedule.AddTarget(new LambdaFunction(nflPlayerLoadFunction));
        }
    }
}