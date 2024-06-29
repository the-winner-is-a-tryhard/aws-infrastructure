## Environment
There are two linked accounts underneath my payer account. These accounts are managed by Control Tower and belong to the same Organization Unit. One account is for pre-production, and one is for production. This CDK application manages all infrastructure for these accounts two accounts. An Azure Pipeline deploys the infrastructure.

## Application Structure
This C# application was created by the CDK command line tool and uses CloudFormation stacks behind the scenes. The application requires .NET 8. Multiple stacks separate logical modules of infrastructure, which reside in `tryhard-aws-infrastructure/src/TryhardAwsInfrastructure/Program.cs`.

## Development Flow and Pipeline
Only changes to the `main` branch are deployed to pre-production and production. Production changes require manual approval. All branches trigger the `Build` job which, runs unit tests and `cdk synth`.