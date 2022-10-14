using Amazon.CDK;
using System.Linq;

namespace Cloudfrontmultiregs3;

sealed class Program
{
    static string ACCOUNT => System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT");
    public static void Main(string[] args)
    {
        var secondaryBucketArnParameterName = "seconday-BucketArn";
        var secondaryRegion = "us-east-2";
        var pathToAssets = "./public";

        var app = new App();
        var stackPrimary = new PrimaryRegionStack(app, "PrimaryRegionStack", new PrimaryRegionStackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = ACCOUNT,
                Region = "us-east-1",
            },
            PathToAssets = pathToAssets
        });

        var stackSecondary = new SecondaryRegionStack(app, "SecondaryRegionStack", new SecondaryRegionStackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = ACCOUNT,
                Region = secondaryRegion,
            },
            PathToAssets = pathToAssets,
            BucketArnParameterName = secondaryBucketArnParameterName
        });

        var stackGlobal = new GlobalStack(app, "GlobalStack", new GlobalStackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = ACCOUNT,
                Region = "us-east-1",
            },
            PrimaryBucket = stackPrimary.Bucket,
            SecondayBucketArnParameterName = secondaryBucketArnParameterName,
            SecondaryRegion = secondaryRegion
        });

        stackGlobal.AddDependency(stackSecondary);

        app.Synth();
    }
}
