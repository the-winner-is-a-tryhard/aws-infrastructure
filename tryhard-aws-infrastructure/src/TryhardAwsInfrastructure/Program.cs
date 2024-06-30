using Amazon.CDK;

namespace TryhardAwsInfrastructure
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new AvatarCdnStack(app, "AvatarCdnStack");
            new DataLakeStack(app, "DataLakeStack");
            app.Synth();
        }
    }
}
